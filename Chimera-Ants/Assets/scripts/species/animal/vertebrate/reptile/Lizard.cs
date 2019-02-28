//This class represents lizards behaviour and features

public class Lizard : Reptile{
    // Species method
	private void developpement(){}
    public override Species Reproduction(Species species){
    	Lizard lizard = new Lizard();
    	return lizard;
    }
    public void feed(Species species){}
    public override void Drink(){}
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
   	public override void Attack(Species species){}
   	public override bool RunAway(Animal animal){
   		return false;
   	}
   	public override void other(){}
}