//This class represent Chimera ants features and behaviour
using System;
//using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    private ChimeraAntClass status;
    private Species[] speciesGenomes;
	
	private static int antBoidIdReference = 0;
    private int antBoidId;

	

	//Constructor
	private ChimeraAnt(){
		
		this.setAnimalBoidId(0);

		this.status = ChimeraAntClass.Queen;

		this.antBoidId = antBoidIdReference;
		
		antBoidIdReference++;

		this.sex = Sex.Female;
	}
    
    // Species method

	//A faire remonter dans Species
	public void deplacement(float x, float z){
		base.deplacement(x,0,z);
	}

	public override void developpement(){}

	//Fait
    public override Species reproduction(Species species){
		int rand = -1;
		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt ant = (ChimeraAnt) species;
			if(status == ChimeraAntClass.Queen && ant.status == ChimeraAntClass.King){
				print("its a chimera ant");
				
				ChimeraAnt cAntChild = Instantiate<ChimeraAnt>(this);
				cAntChild.antBoidId = this.antBoidId;
				print("cAnt Child boid number = "+ cAntChild.antBoidId);

				Random random = new Random();
				rand = random.Next();

				if(rand%2 == 0){
					cAntChild.status = ChimeraAntClass.Worker;
				}
				else{
					cAntChild.status = ChimeraAntClass.Soldier;
				}
				print("status enfant = " + cAntChild.status);
					
				print("reproduction !");

				print("/n/n");

				return cAntChild;
			}
		}
		return null;

    }
    
	//Fait
	public void feed(Species species){
		if(species.GetType() == typeof(Animal) || species.GetType() == typeof(Vegetal)){
			base.feed(species);
		}
	}
	
	//A faire remonter dans Spieces
    public override void drink(){}

	//A faire remonter dans Spieces 
    public override void death(){
		if(lifePoint <= 0){
			death()
		}
	}
	
	// Animal method

    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}

	//Fait
   	public override void stateBehaviour(){
		if(this.status == ChimeraAntClass.Queen || this.status == ChimeraAntClass.King){
			this.setState(State.Leader);
			//print("Leader");
		}
		else{
			this.setState(State.Follower);
			//print("Follower");
		}
   	}

	//Afaire remonter dans Animal
	public override void dangerEvaluation(Species species){

		if(species.GetType() == typeof(Animal)){
			Animal animal = (Animal) species;
			if(animal.getAnimalBoidId() != this.getAnimalBoidId()){
				increaseDangerLvl(1);
			}
			if(animal.getDietaryRegime() != DietaryRegime.Vegetarian){
				increaseDangerLvl(2);;
			}
			if(animal.getState() == State.Leader){
				increaseDangerLvl(2);;
			}
			if(species.GetType() == typeof(ChimeraAnt)){
				ChimeraAnt cAnt = (ChimeraAnt) species;

				if(cAnt.antBoidId != this.antBoidId){
					if(cAnt.status == ChimeraAntClass.King){
						increaseDangerLvl(3);;
					}
					
					if(cAnt.status == ChimeraAntClass.KingGuard){
						increaseDangerLvl(2);;
					}
				}
			}
		}
	}
   	
	//A faire dans Animal
	public override void kill(Species species){
		
		while(species.getLifePoint() < 0){
			species.setLifePoint( (getStrenght() * getWeight()) / ( species.getResistance() * species.getWeight() ) );

			if(species.getLifePoint() <= 0){
				death()
			}
		}
	}
   	
	//A faire dans Animal
	public override bool runAway(Animal animal){

		if(dangerLvl >= 3){
			int rand = -1;
			Random random = new Random();
			rand = random.Next(0,10);

			if(animal.getDangerLvl() > 3){
				rand -= random.Next(1,animal.getDangerLvl());
			}

			if(animal.getDangerLvl() <= 3){
				rand += random.Next(1,animal.getDangerLvl());
			}

			if(rand >= 5){
				return true;
			}
			
		}
   		return false;
   	}
   	public override void other(){}
    //Chimera-ants special method
    public void geneticalEvolution(){}

	public void Start(){ 
		//stateBehaviour();

		//reproduction(new ChimeraAnt());

		//print("model1 " + cAnt.model );
		//cAnt.createModel("ChimeraAnt");
		//print("model2 " + cAnt.model );
		//print("position1 " + cAnt.model.transform.position );
		//cAnt.deplacement(0,0,0);
		//print("position2 " + cAnt.model.transform.position );

		print("it's starting");
	}

	public void Update(){

		//cAnt.deplacement(30,0,40);
		//print("update");
	}
}