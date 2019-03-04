//This class represent all kind of animals and their behaviour
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : Species, AnimalManager{
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
    private bool trigger = false;
    private GameObject target;
    private bool fleeing = false, attacking = false;
    bool withinreach = false;

    /* Wandering parameters */
    private bool canmove;
    private bool groundedSpecies = true;
    private float waittimer = 0;
    private float walktimer = 0;
    private float turntimer = 0;
    private Vector3 randomRotation = Vector3.zero;
    private Vector3 randomDirection = Vector3.zero;


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

        if(species.GetType() == typeof(Animal)){
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
        if(species.lifePoint > 0){
			species.TakeDamage( strength * weight );
		}
    }

    

    

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
            walktimer = Random.Range(0, 10);
            randomDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
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
            turntimer = Random.Range(0, 10);
            if (groundedSpecies)
                randomRotation = new Vector3(0, Random.Range(-1, 1), 0);
            else
                randomRotation = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
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
            waittimer = Random.Range(0, 10);
            canmove = true;
        }
    }
    

    protected void OnTriggerEnter(Collider other)
    {
        
        Species species = other.GetComponent<Species>();
        if (species)
        {
            print(name + " : Theres something ...");
            if (species.GetType().IsSubclassOf(typeof(Vegetal)))
            {
                if (!fleeing && !attacking) // trigger only if in normal state
                {
                    attacking = true;
                    move.direction = Vector3.Normalize(species.transform.position - transform.position);
                    target = species.gameObject;
                }
            }
            if (species.GetType().IsSubclassOf(typeof(Animal)))
            {
                Animal ani = species as Animal;
                if (ani.GetType() == GetType()) // same species ?
                {
                    // group behaviours ?
                    // target assignement not needed ?
                }
                else
                {
                    print(name + ": Oh thats a " + ani.GetType());
                    DangerEvaluation(ani);
                    if (RunAway(ani)) // Jamais vu fuir pour l'instant
                    {
                        print(name + ": NIGEROOOOO ");
                        attacking = false;
                        fleeing = true;
                        move.direction = Vector3.Normalize(transform.position - ani.transform.position ); // sens opposé
                    }
                    else
                    {
                        print(name + ": TATAKAI");
                        attacking = true;
                        fleeing = false;
                        move.direction = Vector3.Normalize(ani.transform.position - transform.position); 

                    }
                    target = species.gameObject;
                    print(name + ": target acquired -> " + target);
                }
                
            }
            
        }

    }
    protected void OnTriggerExit(Collider other)
    {
        print(name + "wesssssssssshhhh2 " + other.name);
        if (other.gameObject == target) // predator or prey out of range
        {
            target = null;
            attacking = false;
            fleeing = false;
        }

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == target)
        {
            if (attacking)
            {
                withinreach = true;
                Attack(collision.gameObject.GetComponent<Species>());
            }
            
        }
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == target)
        {
            if (attacking)
            {
                withinreach = false;
            }

        }
    }

    
    public abstract bool RunAway(Animal animal);
    public abstract void other();

    protected virtual void Update()
    {
        if(target== null)
        {
            Wander();
        }
        else
        {
            // refresh direction
            if (fleeing)
            {
                move.direction = Vector3.Normalize(transform.position - target.transform.position); // sens opposé
            }
            if (attacking)
            {
                move.direction = Vector3.Normalize(target.transform.position - transform.position);
            }
            if(!withinreach)
                move.Apply(move.direction);
        }
        
        
    }

}

