﻿//This class represents man behaviour and features


public class Human : Primate, HumanManager{
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
    	Human man = new Human();
    	return man;
    }
    public void feed(Species species){}
    public override void drink(){}
    public void death(){}
	// Animal method
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}
   	public override void stateBehaviour(){
   		//return State.Leader;
   	}
	public override void dangerEvaluation(Species species){
		//return ;
	}
   	public override void kill(Species species){}
   	public override bool runAway(Animal animal){
   		return false;
   	}
   	public override void other(){}
}