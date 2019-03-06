//This class represent Chimera ants features and behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;
    private Species[] speciesGenomes;

    // à mettre dans chimeraantmanager
    private static Dictionary<string, int> _geneticalProgress;
	
	private new static int familyBoidIdReference = 0;
    private bool _isKingBorn = false;

    //Constructor
    protected override void Awake()
    {
        base.Awake();
    
        print("CHIMERA ANT " + name + "has started");

        System.Random random = new System.Random();
        if (random.Next() % 2 == 0)
        {
            sex = Sex.Male;
        }
        else
        {
            sex = Sex.Female;
        }
		if(status == ChimeraAntClass.King){
			sex = Sex.Male;
		}
        SetAnimalBoidId(0);
		resistance = 1000;
		longevity = longevity * 15f;
        strength = 100;
        dietaryRegime = DietaryRegime.Omnivorus;
        move = new ChimeraAntMove(_rb);

	}
    
    
	
    private ChimeraAnt SpawnChildren()
    {
        if (status != ChimeraAntClass.Queen) return null;

        string source = "Prefabs/";

        // Last part of Queen's life : Give birth to the future King
        if (longevity <= 20000 && !_isKingBorn)
        {
            source += "AntKing";
            _isKingBorn = true;
        }
        else
        {
            // Middle part of Queen's life : Give birth to the future King Guard
            if (longevity <= 37500 && !_isKingBorn)
            {
                source += "AntKingGuard";
            }
            else
            {
                // First and default part of Queen's life : Give birth to the colony
                
                System.Random random = new System.Random();
                    
                if (random.Next() % 2 == 0)
                {
                    source += "AntWorker";
                }
                else
                {
                    source += "AntSoldier";
                }
                    
                
            }
        }
        print("Instatiating " + source);

        ChimeraAnt go =( (GameObject)Instantiate(Resources.Load(source), transform.position - transform.forward, new Quaternion())).GetComponent< ChimeraAnt>();
        print("Transmitting movement ");
        ((ChimeraAntMove)move).InheritMovement((ChimeraAntMove)go.move); // transmitting queen's movement qualities to newborn
        
        return go;
    }

	//FAIT
    private new void Developpement(){
		base.Developpement();
		//longevity equivalent of 15 years
		longevity --;

	}

	//Fait
    public override Species Reproduction(Species species){
		print("Appel reproduction chimera ant");

        /* La reine n'a pas besoin de roi pour spawner des enfants, alors pourquoi cette méthode ? */

		if(species.GetType() == typeof(ChimeraAnt)){

            return SpawnChildren();
            
		}
		return null;
    }

	//FAIT
	private void QueenModeActivation(ChimeraAnt cAnt){

		if(status == ChimeraAntClass.King){
			
			if(cAnt.sex == Sex.Female){
				cAnt.status = ChimeraAntClass.Queen;
				print("PONDEUSE !!!");
			}
		}
	}
    
	public override void Feed(Species species){
	    base.Feed(species);
		
	}
    public override void Drink(){}
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}

   	public override void stateBehaviour(){
		if(this.status == ChimeraAntClass.Queen || this.status == ChimeraAntClass.King){
			state = State.Leader;
			//print("Leader");
		}
		else{
			state  = State.Follower;
			//print("Follower");
		}
   	}

	//A faire remonter dans Animal FAIT
	public override void DangerEvaluation(Species species){
		
		base.DangerEvaluation(species);

		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt cAnt = (ChimeraAnt) species;

			if(cAnt.familyBoidId != familyBoidId){
				if(cAnt.status == ChimeraAntClass.King){
					increaseDangerLvl(3);
				}
					
				if(cAnt.status == ChimeraAntClass.KingGuard){
					increaseDangerLvl(2);
				}

                if (cAnt.status == ChimeraAntClass.Worker || cAnt.status == ChimeraAntClass.Soldier)
                {
                    increaseDangerLvl(1);
                }
            }
		}
	}
   	
	/* public Species feedQueen(Species spieces){

	}*/
    
    // Qui runaway ? la chimera ant ou l'animal ? FAIT
	public override bool RunAway(Animal animal){

		if(dangerLvl >= 3){
            // ca rpz quoi ?
			System.Random random = new System.Random();
            int rand = random.Next(0,10);

			if(animal.dangerLvl > 3){
				rand -= random.Next(0,animal.dangerLvl);
			}

			if(animal.dangerLvl <= 3){
				rand += random.Next(0,animal.dangerLvl);
			}

			if(rand >= 5){
				print("DANGER");
				return true;
			}
			
		}
		print("NO DANGER");
   		return false;
   	}
   	public override void other(){}
    //Chimera-ants special method
    public void geneticalEvolution(){}



	protected override void Update(){
        base.Update();
        if(Input.GetMouseButtonDown(0))
        {
            if (status == ChimeraAntClass.Queen)
                SpawnChildren();
            
        }
    }
}