//This class represents wolves behaviour and features

public class Wolf : Canid{
    // Species method
	private void developpement(){}
    public override Species reproduction(Species species){
    	Wolf wolf = new Wolf();
    	return wolf;
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
	public void dangerEvaluation(Species species){
		//return 0;
	}
   	public override void kill(Species species){}
   	public override bool runAway(Animal animal){
   		return false;
   	}
   	public override void other(){}
}