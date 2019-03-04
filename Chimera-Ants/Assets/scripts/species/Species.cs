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

    protected Movement move;
    

    /* Wandering parameters */
    private bool canmove ;
    private bool groundedSpecies = true;
    private float waittimer = 0;
    private float walktimer = 0;
    private float turntimer = 0;
    private Vector3 randomRotation = Vector3.zero;
    private Vector3 randomDirection = Vector3.zero;

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
        longevity = 5000;
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
    
    public abstract Species Reproduction(Species species);

    public virtual void Drink(){}

    public virtual void Feed(Species species){
        if(lifePoint <= baseLifePoint-10){
            RestoreLifePoints();
        }
        hunger = 0;
        species.Death();
    }

    protected void RestoreLifePoints(){
        lifePoint=baseLifePoint;
        
    }
    public float TakeDamage(float damage)
    {
        float totalDamage = damage / resistance * weight;
        lifePoint -= totalDamage;
        return totalDamage;
    }

    protected virtual void Deplacement(Vector3 direction){
        transform.LookAt(transform.position + direction);
        _rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
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

        print("base life point " + baseLifePoint);
        print("life point " + lifePoint);
        print("hunger " + hunger);
    }

    protected void Death()
    {
        if (lifePoint <= 0 || longevity <= 0 || baseLifePoint <= 20)
        {
            print("death");
            Destroy(gameObject);
        }
    }
    //protected setSpiecesBoidReference    

}