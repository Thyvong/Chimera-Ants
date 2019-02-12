//This class represent Chimera ants features and behaviour
using System;
//using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{
    private ChimeraAntClass status;
    private Species[] speciesGenomes;
	
	private static int antBoidIdReference = 0;
    private int antBoidId;

	//public GameObject antModel;

	//Constructor
	private ChimeraAnt(){
		
		this.setAnimalBoidId(0);

		this.status = ChimeraAntClass.Queen;

		this.antBoidId = antBoidIdReference;
		
		antBoidIdReference++;
	}
    
    // Species method
	public void deplacement(float x, float y, float z){
		this.model.transform.Translate(x,y,z);
	}

	public override void developpement(){}

    public override Species reproduction(Species species){
		int rand = -1;
		if(species.GetType() == typeof(ChimeraAnt)){
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
				
			//print("OK");
			print("reproduction !");

			print("/n/n");

			cAntChild.model.transform.Translate(150.0f,203.0f,100.0f);
			return cAntChild;
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
			//print("Leader");
		}
		else{
			this.setState(State.Follower);
			//print("Follower");
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
		//stateBehaviour();

		reproduction(new ChimeraAnt());

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
		//float update = 0.0f;

		//update += ;

		//if (update > 5.0f){
		//reproduction(new ChimeraAnt());
		//update = 0.0f;
		//print("it's update");
		//}
		
	}
}