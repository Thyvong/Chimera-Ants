// This class represent all kind of living species animals, vegetals, bacterium, mushroom

public abstract class Species : Element, SpeciesManager{
    
    protected int longevity; //A species life expenctancy 
    protected float weight; 
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death

    protected float baseLifePoint;
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle

    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;

    //public abstract void deplacement();
    public abstract void developpement();
    public abstract Species reproduction(Species species);
    
    public abstract void drink();
    

    protected void feed(Species species){
        restoreLifePoint();
        species.death();
    }

    protected void restoreLifePoint(){
        setLifePoint(baseLifePoint);
    }

    protected void deplacement(float x, float y, float z){
        this.model.transform.Translate(x,y,z);
    }

    public void death(){
        if(lifePoint <= 0){
            Destroy(this.getModel());
		} 
    }
    public int getLongevity(){
        return longevity;
    }

    public float getWeight(){
        return weight;
    }

    public float getStrenght(){
        return strength;
    }

    public float getLifePoint(){
        return lifePoint;
    }

    public void setLifePoint(float lifePoint){
        this.lifePoint = lifePoint;
    }

    
    public float getResistance(){
        return resistance;
    }

    public LifeStyle getLifeStyle(){
        return lifeStyle;
    }

    //protected setSpiecesBoidReference    

}