﻿//This class represent Chimera ants features and behaviour
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
        SetAnimalBoidId(0);
		resistance = 1000;
		longevity = longevity * 15f;
        print(longevity);
	}

    
    // Species method

	//A faire remonter dans Species
	protected override void Deplacement(Vector3 direction){
		base.Deplacement(direction);
	}

    private ChimeraAnt SpawnChildren()
    {
        if (status != ChimeraAntClass.Queen) return null;

        string source = "Prefabs/";

        // Last part of Queen's life : Give birth to the future King
        if (longevity <= 2000 && !_isKingBorn)
        {
            source += "AntKing";
            _isKingBorn = true;
        }
        else
        {
            // Middle part of Queen's life : Give birth to the future King Guard
            if (longevity <= 3000 && !_isKingBorn)
            {
                source += "AntKingGuard";
            }
            else
            {
                //First part of Queen's life : Give birth to the colony
                if (longevity <= 4000 || _isKingBorn)
                {
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
        }
        ChimeraAnt go = (ChimeraAnt)Instantiate(Resources.Load(source), transform.position - transform.forward, new Quaternion());
        return go;
    }

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
    

	private ChimeraAnt KingGuardBirth(ChimeraAnt king){
		
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

    private ChimeraAnt KingBirth(ChimeraAnt king){
		
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
    


	public override void Feed(Species species){
		if(species.GetType() == typeof(Animal) || species.GetType() == typeof(Vegetal)){
			base.Feed(species);
		}
	}
    public override void Drink(){}
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

	//A faire remonter dans Animal
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
   	
	//A faire dans Animal
	public override void Attack(Species species)
    {
		if(species.lifePoint > 0){
			species.TakeDamage( strength * weight );
		}
	}
	
	/* public Species feedQueen(Species spieces){

	}*/

	//A faire dans Animal
	public override bool RunAway(Animal animal){

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


	private void Update(){
        if(Input.GetMouseButtonDown(0))
        {
            if (status == ChimeraAntClass.Queen)
                SpawnChildren();
        }
        

	}
}