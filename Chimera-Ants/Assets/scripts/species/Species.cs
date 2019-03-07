// This class represent all kind of living species animals, vegetals, bacterium, mushroom

using System.Collections.Generic;
using UnityEngine;

public abstract class Species : Element, SpeciesManager{

    public float age { get; protected set; } //A species life expenctancy 
    public float longevity{  get;  protected set;} //A species life expenctancy 
    
    public float lifePoint { get; protected set; } //Life point -> if lifePoint = 0 -> death
    public float baseLifePoint { get; protected set; }

    public float resistance { get; protected set; } //higher value => the spieces is  more resistant
    public float weight { get; protected set; }

    public bool dead = false;

    public float hunger;// { get; protected set; } //time indicator which mesure the time spent without eating (0 = not hungry)

    public float visionRange { get; protected set; }
    protected Rigidbody _rb;

    

    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;
    protected virtual void Awake()
    {
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rb.mass = weight;
        _rb.drag = 5;
        _rb.angularDrag = weight / 10.0f;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        
    }
    protected Species(){

        // initialisé ici, mais dans le futur, fait cas par cas
        age = 0;
        longevity = 5000;

        weight = 10;
        lifePoint = 100;
        baseLifePoint = 100;
        resistance = 1;

        hunger = 0;
        visionRange = 5f;
    }
    
    //Méthode ABSTRAITE---------
    public abstract Species Reproduction(Species species);

    //Fait
    public virtual void Drink(){}


    //Méthode concrete

    //Fait
    protected virtual void Feed(Species species){
        if(lifePoint <= baseLifePoint-10){
            RestoreLifePoints();
            species.Eaten();
            print("EAT !! ");
        }
        hunger = 0;
        
    }


    //Fait
    protected void RestoreLifePoints(){
        lifePoint=baseLifePoint;    
    }

    //Fait
    public float TakeDamage(float damage)
    {
        float totalDamage = damage / (resistance * weight);
        lifePoint -= totalDamage;
        return totalDamage;
    }

    public virtual void Developpement()
    {
        hunger += Time.deltaTime;
        //if the species didn't eat for too long
		if(hunger >= 70){
            //it weakens
			lifePoint --;
			baseLifePoint --;
            resistance -= 0.1f;
		}

        //if the spieces eats regulary
		if(hunger <= 10){
            //it grows well
			baseLifePoint += 0.01f;
            resistance += 0.01f;
		}

        
    }

    protected virtual void Death()
    {
        if ( lifePoint <= 0 ||  age >= longevity - 100  || baseLifePoint <= 20) // 0hp OR too old OR too weak 
        {
            print("death");
            dead = true;
            
        }
    }
    // call when predator eat species
    public void Eaten()
    {
        Destroy(gameObject);
    }

}