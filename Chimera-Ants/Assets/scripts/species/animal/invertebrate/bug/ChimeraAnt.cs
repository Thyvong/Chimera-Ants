//This class represent Chimera ants features and behaviour
using System;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;
    private Species[] speciesGenomes;
	
	private static int familyBoidIdReference = 0;
    

	//Constructor

	private ChimeraAnt() : base()
	{
		
		this.setAnimalBoidId(0);

		this.status = status;

		this.familyBoidId = familyBoidIdReference;
		familyBoidIdReference++;

		this.sex = Sex.Female;

		resistance = 1000;

		longevity = 5000f * 15f;

		
	}

	private ChimeraAnt(ChimeraAntClass status, int familyBoidId, Sex sex) : base()
	{
		
		this.setAnimalBoidId(0);

		this.status = status;

		if(this.status == ChimeraAntClass.Queen){
			//Resources.Load("Prefabs/AntQueen.prefab");
			//Charge le prefab de reine
		}

		if(this.status == ChimeraAntClass.King){
			//Resources.Load("Prefabs/AntKing.prefab");
			//Charge le prefab de roi
		}

		if(this.status == ChimeraAntClass.KingGuard){
			//Resources.Load("Prefabs/AntKingGuard.prefab");
			//Charge le prefab de garde royal
		}

		if(this.status == ChimeraAntClass.Soldier){
			//Resources.Load("Prefabs/AntSoldier.prefab");
			//Charge le prefab de soldat
		}

		if(this.status == ChimeraAntClass.Worker){
			//Resources.Load("Prefabs/AntWorker.prefab");
			//Charge le prefab de ouvrier
		}

		this.familyBoidId = familyBoidId;

		this.sex = sex;

		resistance = 1000;

		longevity = longevity * 15f;	
	}

    
    // Species method

	//A faire remonter dans Species
	public void deplacement(float x, float z){
		base.deplacement(x,0,z);
	}

	private void developpement(){
		base.developpement();
		
		//longevity equivalent of 15 years
		longevity --;

		Species species = detection();

		//1st part of queen's life
		if(longevity >= 4000){
			if(species.GetType() == typeof(ChimeraAnt)){
				reproduction( (ChimeraAnt) species );
			}
		}

		//2nd part of queen's life
		if(longevity >= 3000){
			if(species.GetType() == typeof(ChimeraAnt)){
				KingGuardBirth( (ChimeraAnt) species );
			}
		}

		//last part of queen's life
		if(longevity >= 2000){
			if(species.GetType() == typeof(ChimeraAnt)){
				KingBirth( (ChimeraAnt) species );
			}
		}


	}

	//Fait
    public override Species reproduction(Species species){
		int rand = -1;
		print("Appel");
		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt ant = (ChimeraAnt) species;
			Sex sex;
			ChimeraAntClass status; 
			int familyBoidId;

			print("status 1 " + this.status);
			print("status 2 " + ant.status);
			if( (this.status == ChimeraAntClass.Queen && ant.status == ChimeraAntClass.King) ){
				print("its a chimera ant");

				familyBoidId = ant.familyBoidId;

				System.Random random = new System.Random();
				rand = random.Next();

				print("rand " + rand);

				if(rand%2 == 0){
					sex = Sex.Male;
				}
				else{
					sex = Sex.Female;
				}

				rand = random.Next();
				
				if(rand%2 == 0){
					status = ChimeraAntClass.Worker;
				}
				else{
					status = ChimeraAntClass.Soldier;
				}

				return new ChimeraAnt(status, familyBoidId, sex);
			}
		}
		return null;

    }
    

	public ChimeraAnt KingGuardBirth(ChimeraAnt king){
		
		int rand = -1;

		if(status == ChimeraAntClass.Queen){
			ChimeraAnt cAntChild = Instantiate<ChimeraAnt>(this);
			cAntChild.familyBoidId = king.familyBoidId;

			System.Random random = new System.Random();
			rand = random.Next();

			if(rand%2 == 0){
				cAntChild.sex = Sex.Male;
			}
			else{
				cAntChild.sex = Sex.Female;		
			}

			cAntChild.status = ChimeraAntClass.KingGuard;

			return cAntChild;
		}

		return null;
	}

	public ChimeraAnt KingBirth(ChimeraAnt king){
		
		int rand = -1;

		if(status == ChimeraAntClass.Queen){
			ChimeraAnt cAntChild = Instantiate<ChimeraAnt>(this);
			cAntChild.familyBoidId = king.familyBoidId;

			System.Random random = new System.Random();
			rand = random.Next();

			if(rand%2 == 0){
				cAntChild.sex = Sex.Male;
			}
			else{
				cAntChild.sex = Sex.Female;		
			}

			cAntChild.status = ChimeraAntClass.King;

			return cAntChild;
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

	
	// Animal method

    public override void groupBehaviour(){

	}
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

	//A faire remonter dans Animal
	public void dangerEvaluation(Species species){
		
		base.dangerEvaluation(species);

		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt cAnt = (ChimeraAnt) species;

			if(cAnt.familyBoidId != this.familyBoidId){
				if(cAnt.status == ChimeraAntClass.King){
					increaseDangerLvl(3);
				}
					
				if(cAnt.status == ChimeraAntClass.KingGuard){
					increaseDangerLvl(2);;
				}
			}
		}
	}
   	
	//A faire dans Animal
	public override void kill(Species species){
		
		while(species.getLifePoint() > 0){
			species.setLifePoint( species.getLifePoint() - (getStrenght() * getWeight()) / ( species.getResistance() * species.getWeight() ) );

			species.death();
			
		}
	}
	
	/* public Species feedQueen(Species spieces){

	}*/

	//A faire dans Animal
	public override bool runAway(Animal animal){

		if(dangerLvl >= 3){
			int rand = -1;
			System.Random random = new System.Random();
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
		print("longevity " + longevity);
		print("lifePoint " + lifePoint);
		print("baseLifePoint " + baseLifePoint);

		//ChimeraAnt cAnt = new ChimeraAnt();
		//cAnt.status = ChimeraAntClass.King;

		if(status == ChimeraAntClass.Queen){
			GameObject king = GameObject.Find("AntKing");

			if(!king.GetComponent<Species>()){
				print("c'est nul");
			}
			else{
				print("c'est pas nul");
				ChimeraAnt cAntChild = (ChimeraAnt)reproduction(king.GetComponent<Species>());

				if(cAntChild != null){
					print("c'est pas null");
					cAntChild.deplacement(10,0,0);
				}
				else{
					print("null");
				}
					
			}
			
		}
		

	}

	public void Update(){
		
		/*/longevity--;
		print("longevity = " + longevity);
		death();*/

		//developpement();

		if(longevity % 500 == 0){
			//reproduction(cAnt);
		}

		/* if(model.gameObject.tag == "AntKing"){

		}*/

	}
}