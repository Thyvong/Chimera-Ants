//This class represents owls behaviour and features

public class Owl : Bird{
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
    	Owl owl = new Owl();
    	return owl;
    }
    public override void feed(Species species){}
    public override void drink(){}
    public override void death(){}
	// Animal method
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}
   	public override void stateBehaviour(){
   		//return State.Leader;
   	}
	public override int dangerEvaluation(){
		return 0;
	}
   	public override void kill(Species species){}
   	public override bool runAway(){
   		return false;
   	}
   	public override void other(){}
}