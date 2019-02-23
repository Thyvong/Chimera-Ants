// This class represent all kind of living species animals, vegetals, bacterium, mushroom

public abstract class Species : Element, SpeciesManager{
    
    protected float longevity; //A species life expenctancy 
    protected float weight; 
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death

    protected float baseLifePoint;
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle

    protected float feedInspector = 0f;//time indicator which mesure the time spent without eating


    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;

    protected Species(){
        longevity = 5000;
        lifePoint = 100;
        baseLifePoint = 100;
    }

    protected Species detection(){
        //si le game object d'une espece se trouve dans le champs de detection d'une fourmi (=15 cm)
            //retourner l'espece (= en recupérant son game object et le type de son component)

        //retourner null
        return null;

    }

    //public abstract void deplacement();
    public abstract Species reproduction(Species species);
    
    public abstract void drink();

    protected void feed(Species species){
        if(lifePoint <= baseLifePoint-10){
            restoreLifePoint();
            species.death();
        }
        
    }

    protected void restoreLifePoint(){
        setLifePoint(baseLifePoint);
        feedInspector = 0f;
    }

    protected void deplacement(float x, float y, float z){
        this.transform.Translate(x,y,z);
    }

    public void death(){
        if(lifePoint <= 0 || longevity <= 0 || baseLifePoint <= 20){
            print("death");
            //Destroy(this.model);
            Destroy(this);
		} 
    }
    public float getLongevity(){
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

    public void developpement(){
        death();

        feedInspector ++;

        //if the species didn't eat for too long
		if(feedInspector >= 500f){
            //it weakens
			lifePoint --;
			baseLifePoint --;
            resistance -= 0.1f;
		}

        //if the spieces eats regulary
		if(feedInspector <= 0.00000000001f){
            //it grows well
			baseLifePoint += 0.1f;
            resistance += 1f;
		}

        print("base life point " + baseLifePoint);
        print("life point " + lifePoint);
        print("feedInspector " + feedInspector);
    }

    //protected setSpiecesBoidReference    

}