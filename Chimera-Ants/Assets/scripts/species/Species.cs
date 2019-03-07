// This class represent all kind of living species animals, vegetals, bacterium, mushroom

using UnityEngine;

public abstract class Species : Element, SpeciesManager{
    
    public float longevity{  get;  protected set;} //A species life expenctancy 
    public float weight { get; protected set; }
    public float strength { get; protected set; } //the strength value of a spieces
    public float lifePoint { get; protected set; } //Life point -> if lifePoint = 0 -> death
    public float baseLifePoint { get; protected set; }
    public float resistance { get; protected set; } //value between 0 and 1 more the value is high more the spieces is resistant
    public float speed { get; protected set; }
    public LifeStyle lifeStyle { get; protected set; } //Species lifestyle

    public int hunger { get; protected set; } //time indicator which mesure the time spent without eating

    public float visionRange { get; protected set; }
    protected Rigidbody _rb;

    //À modifier dans classe fille
    protected Species(){

        // initialisé ici, mais dans le futur, fait cas par cas
        longevity = 500;
        strength = 1;
        weight = 10;
        lifePoint = 100;
        baseLifePoint = 100;
        resistance = 1;
        lifeStyle = LifeStyle.Settled;
        speed = 1;
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
            hunger = 0;
            Destroy(species);
            transform.localScale *= 1.00001f;
        } 
    }


    //Fait
    protected void RestoreLifePoints(){
        lifePoint=baseLifePoint;    
    }

    //Fait
    public float TakeDamage(float damage)
    {
        float totalDamage = damage / resistance * weight;
        lifePoint -= totalDamage;
        return totalDamage;
    }

    //Fait
    protected virtual void Deplacement(Vector3 direction){
        transform.LookAt(transform.position + direction);
        _rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
    

    //Fait
    public virtual void Developpement()
    {
        Death();
        hunger ++;
        //if the species didn't eat for too long
		if(hunger >= 500){
            //it weakens
			lifePoint --;
			baseLifePoint --;
            resistance -= 0.1f;
		}

        //if the spieces eats regulary
		if(hunger <= 10){
            //it grows well
			baseLifePoint += 0.1f;
            resistance += 1f;
		}
    }

    //Fait
    protected void Death()
    {
        if (/* lifePoint <= 0 || */ longevity <= 0 /* || baseLifePoint <= 20*/)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Update(){
        Developpement();        
    }
}