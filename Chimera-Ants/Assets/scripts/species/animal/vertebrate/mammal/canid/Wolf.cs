//This class represents wolves behaviour and features

public class Wolf : Canid{
    // Species method
	private void developpement(){}
    public override Species Reproduction(Species species){
    	
		if(species.GetType() == typeof(Wolf)){
			Wolf w = (Wolf) species;
			if( (sex == Sex.Female && w.sex == Sex.Male) || (sex == Sex.Male && w.sex == Sex.Female) ){
				Wolf wolf = new Wolf();
				return wolf;
			}
		}
		return null;
    	
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
   	//public override void Attack(Species species){}
   	/*public override bool RunAway(Animal animal){
   		return false;
   	}*/
   	public override void other(){}

	public override void Start(){
		resistance = 2;
		longevity = longevity * 8f;
		weight = 60;
	}
}