//This class represents eagles behaviour and features

public class Eagle : Bird{

	// Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
    	Eagle eagle = new Eagle();
    	return eagle;
    }
    public void feed(Species species){}
    public override void drink(){}
    public override void death(){}
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