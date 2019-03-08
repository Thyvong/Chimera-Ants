//This class represent all kind of animals and their behaviour
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Animal : Species, AnimalManager{

    protected static System.Random rng;
    /* Status */
    public float strength { get; protected set; } //the strength value of an animal
    public float attackSpeed { get; protected set; }
    public float attackCD=0;
    public LifeStyle lifeStyle { get; protected set; } // lifestyle as nomad or settler
    public NutritionStyle[] nutritionStyle { get; protected set; }
    public GroupStyle[] groupStyle { get; protected set; }
    public DietaryRegime dietaryRegime { get; protected set; }
    public Sex sex; // { get; set; }

    protected bool isInReproductionTime = false;
    public State state { get; protected set; }
    public int dangerLvl { get; protected set; }

    /* Boid parameters */
    public int animalBoidId { get; protected set; } // personal id
    public int familyBoidId { get;  set; } // group id
    public static int familyBoidIdReference = 0;
    public bool isInBoid = false; // activate if not targeting AND boids list not empty (aka theres a group to copy)
    public List<GameObject> animalInBoids;
    public float boidupdate = 0;
    
    public Movement move;
    
    /* Detection */
    protected SphereCollider FOV; // périmètre de détection, must be IsTrigger
    protected List<GameObject> detected; // espèces dans le périmètre de détection
    protected Species target;
    public bool fleeing = false, attacking = false, feeding = false;
    public bool withinreach = false;

    /* Wandering parameters */
    private bool canmove;
    private bool groundedSpecies = true;
    private float waittimer = 0;
    private float walktimer = 0;
    private float turntimer = 0;
    private Vector3 randomRotation = Vector3.zero;
    private Vector3 randomDirection = Vector3.zero;



    /* Constructor */
    protected Animal() : base()
    {
        strength = 1000;
        lifeStyle = LifeStyle.Settled;
        attackSpeed = 1;
        detected = new List<GameObject>();
    }
    protected override void Awake()
    {
        base.Awake();
        FOV = GetComponent<SphereCollider>();
        move = new DefaultMove(_rb);
        if (rng == null)
        {
            rng = new System.Random();
        }

    }

    /* Setters */
    public void SetAnimalBoidId(int id){
        animalBoidId = id;
    }
    public void SetSex(Sex genre){
        sex = genre;
    }
    public void SetState(State rang)
	{
        state = rang;
    }

    /* Behaviours */
    public virtual void DangerEvaluation(Species species){


        if(species.GetType().IsSubclassOf(typeof(Animal))){

			Animal animal = (Animal) species;

            //If it's not the same animal species
			if(animal.animalBoidId != animalBoidId){
				increaseDangerLvl(1);
			}
			if(animal.dietaryRegime != dietaryRegime){
				increaseDangerLvl(2);
			}
			if(animal.state == State.Leader){
				increaseDangerLvl(2);
			}
        }
    }
    // Increase danger level by nblvl
    protected void increaseDangerLvl(int nbLvl){
        dangerLvl = dangerLvl + nbLvl;
    }
    public void resetDangerLvl(){
        dangerLvl = 0;
    }
    

    /*
        
        public virtual void Update(){
            
            if(animalInBoids != null){
                isInBoid = true;
            }
            else{
                isInBoid = false;
            }
            familyBehaviour();

           
        
            if(animalInBoids.Count > 3){
                    System.Random random = new System.Random();
                    if(state == State.Leader){
                        direction = new Vector3( random.Next(-200,200), 0, random.Next(-200,200));
                        Deplacement(direction);
                        print("nouvelle direction = " + direction);
                    }
                }

        }
        */

    public virtual void familyBehaviour()
    {
        boidBehaviour();
        stateBehaviour();
    }
    public void boidBehaviour()
    {
        if (!isInBoid) return;
        Vector3 mainDirection = Vector3.zero;
        Vector3 groupCenter = Vector3.zero;
        Vector3 newDirection = transform.forward;
        float minDistance  = 99999.0f;
        GameObject nearestNeighbour = null;
        foreach (GameObject animal in animalInBoids)
        {
            mainDirection += animal.transform.forward;
            groupCenter += animal.transform.position;
            // Selection of the nearest neighbour
            if (Vector3.Distance(transform.position, animal.transform.position) < minDistance // plus proche de nous qu'un autre
                // && animal.familyBoidId == familyBoidId  <=== à trier au moment de l'ajout à la list
                // && minDistance > 3.5f  <=== on se rapproche frame par frame, pas plusieurs fois en une frame
                )
            {
                nearestNeighbour = animal;
                minDistance = Vector3.Distance(transform.position, animal.transform.position);
                
            }
        }
        // moyenne de la direction du groupe et calcul de son centre
        mainDirection = mainDirection / animalInBoids.Count;
        groupCenter =  groupCenter / animalInBoids.Count;
        if (Vector3.Distance(transform.position, groupCenter) < 3.5f)
        {
            //We move away
            newDirection = transform.position - nearestNeighbour.transform.position;
            

        }
        else
        {
            //We come closer
            newDirection = nearestNeighbour.transform.position - transform.position  ;
            
        }

        //We center
        newDirection = Vector3.Normalize( nearestNeighbour.transform.forward+mainDirection);
        newDirection.y = transform.position.y;
        Deplacement(newDirection);
        

    }
    public abstract void groupBehaviour();
    public abstract void stateBehaviour();

    public void Attack(Species species){
        if(attackCD <= 0)
        {
            print(name + " : Attack ! " + strength * weight);
            if (species.lifePoint > 0)
            {
                print(name + " : " + species.name + " took " + species.TakeDamage(strength * weight) + " damages from my attack ");
                if(species.lifePoint <= 0)
                {
                    target = null;
                    attacking = false;
                    fleeing = false;
                    withinreach = false;
                    AssessSituation();

                    print(target);
                    
                }
            }
            attackCD = attackSpeed;
        }
        
    }

    /* Deplacement */
    protected virtual void Deplacement(Vector3 direction)
    {
        move.Apply(direction);

    }
    public void Wander()
    {
        if (canmove)
        {
            WanderTurn();
            WanderWalk();
        }
        else
        {
            WanderWait();
        }



    }
    public virtual void WanderWalk()
    {
        if (walktimer > 0)
        {
            move.Apply(randomDirection);
            walktimer -= Time.fixedDeltaTime;
        }
        else
        {
            walktimer = UnityEngine.Random.Range(0, 10);
            randomDirection = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
            if (groundedSpecies) randomDirection.y = 0;
            canmove = false;
        }
    }
    public virtual void WanderTurn()
    {
        if (turntimer > 0)
        {
            transform.Rotate(randomRotation);
            turntimer -= Time.fixedDeltaTime;
        }
        else
        {
            turntimer = UnityEngine.Random.Range(0, 10);
            if (groundedSpecies)
                randomRotation = new Vector3(0, UnityEngine.Random.Range(-1, 1), 0);
            else
                randomRotation = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        }
    }
    public virtual void WanderWait()
    {
        if (waittimer > 0)
        {

            waittimer -= Time.fixedDeltaTime;
        }
        else
        {
            waittimer = UnityEngine.Random.Range(0, 10);
            canmove = true;
        }
    }

    /* Detection */
    protected List<Animal> AlliesNearBy()
    {
        List<Animal> animal = new List<Animal>();
        foreach (GameObject obj in detected)
        {
            Species species = obj.GetComponent<Species>();
            if (species.GetType().IsSubclassOf(typeof(Animal)))
            {
                Animal ani = species as Animal;
                
                if (ani.GetType() == GetType()) // same species ?
                {

                    animal.Add(ani);

                }

            }
        }
        return animal;
    }
    protected List<Animal> EnemiesNearBy()
    {
        List<Animal> animal = new List<Animal>();
        foreach (GameObject obj in detected)
        {
            Species species = obj.GetComponent<Species>();
            if (species.GetType().IsSubclassOf(typeof(Animal)))
            {
                Animal ani = species as Animal;
                if (ani.GetType() != GetType() && !ani.dead) // same species ?
                {
                    if (dietaryRegime != DietaryRegime.Vegetarian)
                    {

                        animal.Add(ani);

                    }

                }

            }
        }
        return animal;
    }
    protected Animal ClosestEnemy()
    {
        List<Animal> list = EnemiesNearBy();
        if (list == null) return null;
        if (list.Count == 0) return null;
        Animal closest = list[0];
        float distance = 99999.0f;
        float calcul = 0;
        foreach(Animal animal in list)
        {
            calcul = Vector3.Distance(transform.position, animal.transform.position);
            if ( calcul < distance)
            {
                distance = calcul;
                closest = animal;
            }
        }
        return closest;
    }
    protected List<Species> FoodNearBy()
    {
        List<Species> food = new List<Species>();
        foreach (GameObject obj in detected)
        {
            Species species = obj.GetComponent<Species>();
            if (species.GetType().IsSubclassOf(typeof(Vegetal)))
            {
                if (dietaryRegime == DietaryRegime.Vegetarian || dietaryRegime == DietaryRegime.Omnivorus)
                {
                    food.Add(species);

                }
            }
            if (species.GetType().IsSubclassOf(typeof(Animal)))
            {
                Animal ani = species as Animal;
                if (ani.GetType() != GetType()) // same species ?
                {
                    if ((ani.dietaryRegime != DietaryRegime.Carnivorus || dietaryRegime == DietaryRegime.Omnivorus) && ani.dead)
                    {
                        food.Add(species);

                    }

                }

            }
        }
        return food;
    }
    protected void ReactToEnemy(Animal ani)
    {
        DangerEvaluation(ani);
        if ( RunAway(ani))
        {

            attacking = false;
            fleeing = true;
            feeding = false;
            move.direction = Vector3.Normalize(transform.position - ani.transform.position); // sens opposé

        }
        else
        {

            attacking = true;
            fleeing = false;
            feeding = false;
            move.direction = Vector3.Normalize(ani.transform.position - transform.position);


        }
        target = ani;

    }
    protected void UpdateDetected()
    {
        List<GameObject> cleanup = new List<GameObject>();
        foreach (GameObject obj in detected)
        {
            if (obj != null && obj.transform.parent.GetComponent<ChimeraAnt>() == null)
            {
                cleanup.Add(obj);
            }
        }
        detected = cleanup;
    }
    
    void AssessSituation()
    {
        UpdateDetected();
        Animal enemy = ClosestEnemy();
        List<Species> food = FoodNearBy();
        if (enemy != null)
        {
            ReactToEnemy(enemy);
        }
        else
        {
            if(food.Count > 0 && hunger > 20)
            {
                target = food[0];
                attacking = false;
                fleeing = false;
                feeding = true;
                move.direction = Vector3.Normalize(target.transform.position - transform.position); 

            }
            else{
                target = null;
            }
        }
        
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (dead) return;
        if (other.transform.parent)
        {
            if (other.transform.parent.GetComponent<Animal>() && GetType()==typeof(ChimeraAnt)) return;
        }
        
        if (other is SphereCollider) return;
        Species species = other.gameObject.GetComponent<Species>();
        if (species)
        {
            if(!animalInBoids.Contains(other.gameObject) 
                && species.GetType() == GetType() 
                && ((Animal)species).familyBoidId == familyBoidId)
            {
                animalInBoids.Add(species.gameObject);
            }
            if (!detected.Contains(other.gameObject))// predator or prey out of range
            {

                detected.Add(other.gameObject);
                AssessSituation();
            }
        }

    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (dead) return;
        if (other is SphereCollider) return;
        Species species = other.gameObject.GetComponent<Species>();
        if (species)
        {
            if (animalInBoids.Contains(other.gameObject))
            {
                animalInBoids.Remove(species.gameObject);
            }
            if (species.GetType() != GetType())
            {
                resetDangerLvl();

            }
            if (detected.Contains(other.gameObject) )// predator or prey out of range
            {
                detected.Remove(other.gameObject);
                AssessSituation();
            }
            
        }
        

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (dead) return;
        if(collision.gameObject.tag == tag) Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>(),true);
        if (target == null) return;
        if (collision.gameObject == target.gameObject)
        {
            
            withinreach = true;
            if (feeding)
            {
                print(name + " : LETS EAT " + target.name);
                detected.Remove(target.gameObject);
                Feed(target);
                
                feeding = false;
                target = null;
            }
            if (attacking)
            {
                Attack(target);
            }


        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (dead) return;
        if (target == null) return;
        if (collision.gameObject == target.gameObject)
        {
            withinreach = false;

        }
    }
    
    public abstract bool RunAway(Animal animal);
    public abstract void other();

    protected override void Death()
    {
        base.Death();
        transform.Rotate(0,0,90);
        _rb.isKinematic = true;
        FOV.enabled = false; // désactive la trigger zone
    }

    protected virtual void Update()
    {
        /* Death */
        if (dead) return;
        if ((lifePoint <= 0 || longevity <= 0 || baseLifePoint <= 20) && !dead)
        {
            Death();
            return;
        }
        
        // Les paramètres de status évoluent
        Developpement();

        if (target== null) // aucune cible
        {
            attacking = false;
            fleeing = false;
            feeding = false;
            withinreach = false;
            
            UpdateDetected();
            AssessSituation();
            
            if (target == null && (animalInBoids.Count != 0 && state != State.Leader ))
            {
                isInBoid = true;
                familyBehaviour();
            }
            
            else
            {
                isInBoid = false;
                
            }
            Wander();

        }
        else // Possède une cible
        {
            // refresh direction
            if (fleeing) // fuir la cible
            {
                if (target.dead) // morte entre temps
                {
                    target = null;
                    fleeing = false;
                    return;
                }
                // fuir en sens opposé
                move.direction = Vector3.Normalize(transform.position - target.transform.position); 
                move.Apply(move.direction);
            }
            else
            {
                if (attacking) // attaquer la cible
                {
                    if (target.dead) // morte entre temps
                    {
                        target = null;
                        attacking = false;
                        return;
                    }
                    if (!withinreach) // se rapprocher
                    {
                        move.direction = Vector3.Normalize(target.transform.position - transform.position);
                        move.Apply(move.direction);
                    }
                    else // à portée
                    {
                        // continuous action
                        Attack(target);
                    }
                    
                }
                else
                {
                    if (feeding) // manger la cible
                    {
                        if (!withinreach) // se rapprocher
                        {
                            move.direction = Vector3.Normalize(target.transform.position - transform.position);
                            move.Apply(move.direction);
                        }
                    }
                }
            }
                
        }
        // dans tous les cas, le délai entre chaque attaque diminue
        if(attackCD>0)
            attackCD -= Time.deltaTime;
        
        boidupdate += Time.deltaTime;
        
    }

    
}

