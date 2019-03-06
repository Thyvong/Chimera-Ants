//This class represent all kind of animals and their behaviour
using UnityEngine;

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
    public Sex sex { get; protected set; }
    public State state { get; protected set; }
    public int dangerLvl { get; protected set; }
    
    public int animalBoidId { get; protected set; } // personal id
    public int familyBoidId { get; protected set; } // group id
    public static int familyBoidIdReference = 0;

    

    protected Movement move;
    
    /* Detection */
    protected SphereCollider FOV; // périmètre de détection, must be IsTrigger
    private List<GameObject> detected; // espèces dans le périmètre de détection
    private Species target;
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
        strength = 100;
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
    
    public void SetSex(Sex genre){
        sex = genre;
    }
   
    protected void SetState(State rang){
        state = rang;
    }

    public virtual void DangerEvaluation(Species species){

        if(species.GetType().IsSubclassOf(typeof(Animal))){
			Animal animal = (Animal) species;
			if(animal.animalBoidId != animalBoidId){ // ?? si animal is not this ??
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

   public abstract void groupBehaviour();
   public abstract void familyBehaviour();
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
        print(name + ": Oh thats a " + ani.GetType());
        DangerEvaluation(ani);
        print(name + ": dangerlvl = " + dangerLvl);
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
        
        UpdateDetected();
        
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





    // ERREUR PROBLABLE DE DETECTED : l'opération d'égalité de list.contains() ne marche pas avec la classe species
    // suggestion : detected devient une liste de gameobjct




    // 
    protected void OnTriggerExit(Collider other)
    {
        if (dead) return;
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
            if(obj != null)
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
        
        
        if (target== null) // aucune cible
        {
            
            attacking = false;
            fleeing = false;
            feeding = false;
            withinreach = false;
            Developpement();
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

