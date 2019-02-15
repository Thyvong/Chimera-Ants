//This class represents white bear behaviour and features

public class WhiteBear : Bear{
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
    	WhiteBear bear = new WhiteBear();
    	return bear;
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
		//return 0;
	}
   	public override void kill(Species species){}
   	public override bool runAway(Animal animal){
   		return false;
   	}
   	public override void other(){}
}