// This class represent all kind of living species animals, vegetals, bacterium, mushroom

using UnityEngine;

public abstract class Species : Element, SpeciesManager{
    
    protected float longevity; //A species life expenctancy 
    protected float weight;
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death
    protected float baseLifePoint;
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle

    protected int hunger = 0;//time indicator which mesure the time spent without eating

    protected float visionRange = 5f;

    //private static int speciesBoidIdReference = 0;
    //protected int spiecesBoidId;

    protected Species(){
        longevity = 5000;
        lifePoint = 100;
        baseLifePoint = 100;
    }
    
    /*
    protected bool Detection(Species species){
        print("Detection " + GetType().ToString() + " vs " + species.GetType().ToString());
        if (GetType().ToString() == species.GetType().ToString())
        {
            print("Detection true");
            return true;
        }
            
        return false;

    }
    */
    //public abstract void deplacement();
    public abstract Species Reproduction(Species species);
    
    public abstract void Drink();

    protected void Feed(Species species){
        if(lifePoint <= baseLifePoint-10){
            RestoreLifePoints();
            species.Death();
        }
        
    }

    protected void RestoreLifePoints(){
        SetLifePoints(baseLifePoint);
        hunger = 0;
    }

    protected void Deplacement(float x, float y, float z){
        this.transform.Translate(x,y,z);
    }

    public void Death(){
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

    public void SetLifePoints(float lifePoint){
        this.lifePoint = lifePoint;
    }

    
    public float getResistance(){
        return resistance;
    }

    public LifeStyle getLifeStyle(){
        return lifeStyle;
    }

    public void Developpement(){
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

    //protected setSpiecesBoidReference    

}