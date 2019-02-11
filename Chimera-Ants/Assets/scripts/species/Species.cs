// This class represent all kind of living species animals, vegetals, bacterium, mushroom

public abstract class Species : Element, SpeciesManager{
    
    protected int longevity; //A species life expenctancy 
    protected float weight; 
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle

    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;

    //public abstract void deplacement();
    public abstract void developpement();
    public abstract Species reproduction(Species species);
    public abstract void feed(Species species);
    public abstract void drink();
    public abstract void death();

<<<<<<< HEAD
    //protected setSpiecesBoidReference    
=======
    //Constructor

    /* protected Spieces(){
        base();
    }*/
>>>>>>> fb551fe5bb3aa1c851881da8de5dde07ef8b54cd

}