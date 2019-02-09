//This class represent Chimera ants features and behaviour
using System;

public class ChimeraAnt : Bug, ChimeraAntManager{
    private ChimeraAntClass status;
    private Species[] speciesGenomes;
	
	private static int antBoidIdReference = 0;
    private int antBoidId;

	//public GameObject antModel;

	//Constructor
	private ChimeraAnt(){
		this.setAnimalBoidId(0);

		print("ANIMAL BOID = " + getAnimalBoidId());

		this.status = ChimeraAntClass.Queen;
		//print("id = " + this.id);
		this.antBoidId = antBoidIdReference;
		//print("ant boid id = " + antBoidId);
		antBoidIdReference++;
		//print("status = " + this.status);
	}
    
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
		int rand = -1;
		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt cAnt = new ChimeraAnt();
			cAnt.antBoidId = this.antBoidId;
			print("cAnt boid number = "+ cAnt.antBoidId);

			Random random = new Random();
			rand = random.Next();
			if(rand%2 == 0){
				cAnt.status = ChimeraAntClass.Worker;
			}
			else{
				cAnt.status = ChimeraAntClass.Soldier;
			}
			print("status enfant = " + cAnt.status);
			return cAnt;	
			//print("OK");
		}
		//print("NO");
		return null;

    }
    public override void feed(Species species){}
    public override void drink(){}
    public override void death(){}
	
	// Animal method
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}
   	public override void stateBehaviour(){
		if(this.status == ChimeraAntClass.Queen || this.status == ChimeraAntClass.King){
			this.setState(State.Leader);
			print("Leader");
		}
		else{
			this.setState(State.Follower);
			print("Follower");
		}
   	}
	public override int dangerEvaluation(Species species){
		int dangerLvl = 0;

		if(species.GetType() == typeof(Animal)){
			Animal animal = (Animal) species;
			if(animal.getAnimalBoidId() != this.getAnimalBoidId()){
				dangerLvl++;
			}
			if(animal.getDietaryRegime() != DietaryRegime.Vegetarian){
				dangerLvl++;
			}
			if(animal.getState() == State.Leader){
				dangerLvl++;
			}
			if(species.GetType() == typeof(ChimeraAnt)){
				ChimeraAnt cAnt = (ChimeraAnt) species;

				if(cAnt.antBoidId != this.antBoidId){
					if(cAnt.status == ChimeraAntClass.King){
						dangerLvl = dangerLvl+2;
					}
					
					if(cAnt.status == ChimeraAntClass.KingGuard){
						dangerLvl++;
					}
				}
			}
		}

		return dangerLvl;
	}
   	public override void kill(Species species){}
   	public override bool runAway(){
   		return false;
   	}
   	public override void other(){}
    //Chimera-ants special method
    public void geneticalEvolution(){}

	public void Start(){
		ChimeraAnt cAnt = new ChimeraAnt(); 
		cAnt.stateBehaviour();

		ChimeraAnt cAnt1 = new ChimeraAnt();
		cAnt1.stateBehaviour();

		cAnt.reproduction(cAnt1);
	}
}