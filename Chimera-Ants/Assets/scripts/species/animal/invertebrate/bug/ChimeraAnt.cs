//This class represent Chimera ants features and behaviour
using System;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;
    private Species[] speciesGenomes;
	
	private new static int familyBoidIdReference = 0;
    

	//Constructor
	private void Start()
	{
        System.Random random = new System.Random();
        if (random.Next() % 2 == 0)
        {
            sex = Sex.Male;
        }
        else
        {
            sex = Sex.Female;
        }
        setAnimalBoidId(0);
		resistance = 1000;
		longevity = longevity * 15f;
	}

    
    // Species method

	//A faire remonter dans Species
	public void deplacement(float x, float z){
		base.Deplacement(x,0,z);
	}

    private void SpawnChildren()
    {
        //1st part of queen's life
        if (longevity >= 4000)
        {
            Instantiate(Resources.Load("Prefab/AntWorker.prefab"),transform.position,new Quaternion());

        }
        else
        {
            //2nd part of queen's life
            if (longevity >= 3000)
            {
                Instantiate(Resources.Load("Prefab/AntKingGuard.prefab"), transform.position, new Quaternion());

            }
            else
            {
                //last part of queen's life
                if (longevity >= 2000)
                {
                    Instantiate(Resources.Load("Prefab/AntKing.prefab"), transform.position, new Quaternion());

                }
            }
        }
    }

	private new void Developpement(){
		base.Developpement();
		//longevity equivalent of 15 years
		longevity --;

	}

	//Fait
    public override Species Reproduction(Species species){
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
			base.Feed(species);
		}
	}
	
	//A faire remonter dans Spieces
    public override void Drink(){}

	
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
			species.SetLifePoints( species.getLifePoint() - (getStrenght() * getWeight()) / ( species.getResistance() * species.getWeight() ) );

			species.Death();
			
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


	public void Update(){
		
		

	}
}