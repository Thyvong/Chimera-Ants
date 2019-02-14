// This class represent all kind of living species animals, vegetals, bacterium, mushroom

using UnityEngine;

public abstract class Species : Element, SpeciesManager{

    protected Rigidbody rb;
    protected Animator animator;
    protected int longevity; //A species life expenctancy 
    protected float weight; 
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle

    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;
    public abstract void developpement();
    public abstract Species reproduction(Species species);
    public abstract void feed(Species species);
    public abstract void drink();
    public abstract void death();

    //protected setSpiecesBoidReference    

}