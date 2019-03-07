//This class represent all kind of animals and their behaviour
using UnityEngine;
using System.Collections.Generic;



using System.Collections.Generic;
using System;


public abstract class Animal : Species, AnimalManager{

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
    public int animalBoidId { get; protected set; } // personal id
    public int familyBoidId { get;  set; } // group id
    public static int familyBoidIdReference = 0;
    public bool isInBoid = false;
    public List<Animal> animalInBoids;

    public Vector3 direction;

    //Méthode abstraite
    public abstract void groupBehaviour();
    public abstract void stateBehaviour();
    public abstract void other();

    

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

    }


    public void SetAnimalBoidId(int id){
        animalBoidId = id;
    }
    
    //Fait
    public void SetSex(Sex genre){
        sex = genre;
    }
   

    public void SetState(State rang)
	{
        state = rang;
    }

    //Fait
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
    //Fait
    protected void increaseDangerLvl(int nbLvl){
        dangerLvl = dangerLvl + nbLvl;
    }

    //Fait
    public void resetDangerLvl(){
        dangerLvl = 0;
    }

   
    //Fait
    public virtual void familyBehaviour(){
        boidBehaviour();
        stateBehaviour();
    }

    //Fait
    public void boidBehaviour(){

        if(isInBoid == true){
            
            Animal nearestNeighbour = null;
            float minDistance = 999999999f;

            foreach(Animal animal in animalInBoids){
                
                // Selection of the nearest neighbour
                if( Vector3.Distance(transform.position, animal.transform.position) < minDistance && animal.familyBoidId == familyBoidId && minDistance > 3.5f ){
                    nearestNeighbour = animal;
                    minDistance = Vector3.Distance(transform.position, animal.transform.position);

                    //We come closer
                    transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position,0.1f ) ;
                    transform.LookAt(nearestNeighbour.transform.position);
                    print("On s'approche");
                }
            }
            
            if( minDistance < 3.5f){
                //We move away
                transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position*(-1),0.1f ) ;
                transform.LookAt(nearestNeighbour.transform.position*(-1));
                print("On s'éloigne");
            }

            //We center
            Vector3 direction = new Vector3(0,0,0);
            foreach(Animal animal in animalInBoids){
                direction += animal.transform.forward; 
            }
            transform.LookAt(direction);
            Deplacement(direction);
        }  

    }
    

/*
    //Fait
    public virtual void Start(){
        System.Random random = new System.Random();
        if (random.Next() % 2 == 0){
            sex = Sex.Male;
            
        }
        else{
            sex = Sex.Female;
        }

        if(state == State.Leader){
            System.Random rand = new System.Random();
            direction = new Vector3( rand.Next(-200,200), 0, rand.Next(-200,200));
        }
        print("type" + GetType() + " sex1 " + sex);
        
    }

    //Fait
    public void OnTriggerEnter(Collider other){
        print("On trigger enter");
        //If the gameObject is an animal
        if(other.gameObject.GetComponent<Animal>() != null){

            //If the animals belongs to the same family
            if(other.gameObject.GetComponent<Animal>().familyBoidId == familyBoidId){

                if( animalInBoids.Contains(other.gameObject.GetComponent<Animal>()) == false){
                        //Add to the list
                        print("add");
                        animalInBoids.Add(other.gameObject.GetComponent<Animal>());
                }                
            }
        }
	}
	
	public void OnTriggerExit(Collider other){
        
        //If the animal leaves the trigger
        if(other.gameObject.GetComponent<Animal>() != null){
            //Remove the animal to the list
            print("remove");
            animalInBoids.Remove(other.gameObject.GetComponent<Animal>());

            
        }
	}

    public virtual void Update(){
        base.Update();
        //Developpement();
        //If isInBoid = true boidBehaviour is active 
        if(animalInBoids != null){
            isInBoid = true;
        }
        else{
            isInBoid = false;
        }
        familyBehaviour();

        if(longevity%500 == 0){
            //print( GetType() + " family Boid Id =" + familyBoidId + "isInBOID " + isInBoid );  
        }

        //print("animal count " + animalInBoids.Count);
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
                    print(name + " : I killed " + species.name);
                    print(target);
                    
                }
            }
            attackCD = attackSpeed;
        }
        
    }

    protected void ReactToEnemy(Animal ani)
    {
        DangerEvaluation(ani);
        if (RunAway(ani) || ani.strength > strength) 
        {
            print(name + ": NIGEROOOOO ");

            attacking = false;
            fleeing = true;
            feeding = false;
            move.direction = Vector3.Normalize(transform.position - ani.transform.position); // sens opposé
            
        }
        else
        {
            print(name + ": TATAKAI");
            attacking = true;
            fleeing = false;
            feeding = false;
            move.direction = Vector3.Normalize(ani.transform.position - transform.position);
            

        }
        target = ani;
        
    }

    
    /*
    protected void OnCollisionStay(Collision other){
        if( familyBoidId != other.gameObject.GetComponent<Animal>().familyBoidId ){
            Attack( other.gameObject.GetComponent<Animal>() );
            if( other.gameObject.GetComponent<Animal>().lifePoint <= 0 ){
                Feed( other.gameObject.GetComponent<Animal>() );
                print("Collision familiale");
                return;
            }
            Feed( other.gameObject.GetComponent<Species>() );
            print("Collision manger");
        }
    }
    */

    protected virtual void Deplacement(Vector3 direction)
    {
        move.Apply(direction);
        print(name + " Moving at " + move.speed);
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

    void AssessSituation()
    {
        
        //UpdateDetected();
        
        List<Animal> enemy = EnemiesNearBy();
        List<Animal> allies = AlliesNearBy();
        List<Species> food = FoodNearBy();
        if (enemy.Count > 0)
        {
            ReactToEnemy(enemy[0]);
        }
        else
        {
            if(food.Count > 0 && hunger > 20)
            {
                target = food[0];
                attacking = false;
                fleeing = false;
                feeding = true;
                move.direction = Vector3.Normalize(target.transform.position - transform.position); // sens opposé

            }
            else{
                target = null;
            }
        }
        
    }


    protected void OnTriggerEnter(Collider other)
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
            if (!detected.Contains(other.gameObject))// predator or prey out of range
            {
                detected.Add(other.gameObject);
                AssessSituation();
            }
        }

    }
    //




    // 
    protected void OnTriggerExit(Collider other)
    {
        if (dead) return;
        if (other is SphereCollider) return;
        Species species = other.gameObject.GetComponent<Species>();
        if (species)
        {
            if (detected.Contains(other.gameObject) )// predator or prey out of range
            {
                detected.Remove(other.gameObject);
                print(name + " : detection list is -" + detected.Count);
                print(name + " : removing " + species.name + " from detection");
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
    protected void UpdateDetected()
    {
        List<GameObject> cleanup = new List<GameObject>();
        foreach(GameObject obj in detected)
        {
            if(obj != null && obj.transform.parent == null)
            {
                cleanup.Add(obj);
            }
        }
        detected = cleanup;
    }

    public abstract bool RunAway(Animal animal);
    public abstract void other();
    protected override void Death()
    {
        base.Death();
        transform.Rotate(0,0,90);
        _rb.isKinematic = true;
        FOV.enabled = false;
    }

    protected virtual void Update()
    {
        if (dead) return;
        if ((lifePoint <= 0 || longevity <= 0 || baseLifePoint <= 20) && !dead)
        {
            Death();
            return;
        }

        // when gaining hunger, force an assess

        Developpement();
        if (target== null) // aucune cible
        {
            
            attacking = false;
            fleeing = false;
            feeding = false;
            withinreach = false;
            
            if (hunger > 30)
            {
                
                UpdateDetected();
                
                List<Species> food = FoodNearBy();
                if (food.Count != 0)
                {
                    if (food[0])
                    {
                        print(name + " : found " + food[0]);
                        target = food[0];
                        feeding = true;
                        move.direction = target.transform.position - transform.position;
                        print(name + " : Oh i need food, here is some - " + target.name);
                    }
                    
                }
            }
            Wander();
        }
        else
        {
            
            // refresh direction
            if (fleeing) // fuir la cible
            {
                if (target.dead)
                {
                    target = null;
                    fleeing = false;
                    return;
                }
                
                move.direction = Vector3.Normalize(transform.position - target.transform.position); // sens opposé
                move.Apply(move.direction);
                
                
            }
            else
            {
                if (attacking) // attaquer la cible
                {
                    if (target.dead)
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
                else // 
                {
                    if (feeding) // 
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
        if(attackCD>0)
            attackCD -= Time.deltaTime;

    }

    
}

