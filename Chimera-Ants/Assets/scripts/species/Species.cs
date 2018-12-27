﻿// This class represent all kind of living species animals, vegetals, bacterium, mushroom

public abstract class Species : Object, SpeciesManager{
    
    protected int longevity; //A species life expenctancy 
    protected float weight; 
    protected float strength; //the strength value of a spieces
    protected float lifePoint; //Life point -> if lifePoint = 0 -> death
    protected float resistance; //value between 0 and 1 more the value is high more the spieces is resistant

    protected LifeStyle lifeStyle; //Species lifestyle
   
    public abstract void developpement();
    public abstract Species reproduction(Species species);
    public abstract void feed(Species species);
    public abstract void drink();
    public abstract void death();

}