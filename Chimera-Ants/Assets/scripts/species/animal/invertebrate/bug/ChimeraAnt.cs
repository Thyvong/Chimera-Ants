//This class represent Chimera ants features and behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;
    private Species[] speciesGenomes;

    // à mettre dans chimeraantmanager
    private static Dictionary<string, int> _geneticalProgress;
	
	
    private bool _isKingBorn = false;

    //Constructor FAIT 
    public override void Start()
	{
		base.Start();

		if(status == ChimeraAntClass.King){
			sex = Sex.Male;
		}

		if(familyBoidIdReference == 0){
			familyBoidId = 0;
		}
		//Chimera Ant Id
        SetAnimalBoidId(0);

		speciesGenomes = new Species[10];
		speciesGenomes[0] = new Tree();


		longevity = longevity * 15f;
        strength = 1;
        weight = 150 * 0.000000001f;

        lifePoint = 100;
        baseLifePoint = 100;

        resistance = 1000;
        lifeStyle = LifeStyle.Settled;
        speed = 1;
        hunger = 0;
        visionRange = 5f;

		print("status " + status + " family boid id" + familyBoidId);
	}
    
    // Species method

	//A faire remonter dans Species FAIT
	protected override void Deplacement(Vector3 direction){
		base.Deplacement(direction);
	}

	//FAIT
    private ChimeraAnt SpawnChildren()
    {	
		ChimeraAntClass rang = ChimeraAntClass.Worker;
		int familyId = familyBoidId;

        if (status != ChimeraAntClass.Queen) return null;

        string source = "Prefabs/";

        // Last part of Queen's life : Give birth to the future King
        if (longevity <= 2000 && !_isKingBorn)
        {
			//if it's a king we set a new family boid id
            source += "AntKing";
            _isKingBorn = true;
			Animal.familyBoidIdReference++;
			familyId = Animal.familyBoidIdReference;
			rang = ChimeraAntClass.King;
        }
        else
        {
            // Middle part of Queen's life : Give birth to the future King Guard
            if (longevity <= 3750 && !_isKingBorn)
            {
                source += "AntKingGuard";
				rang = ChimeraAntClass.KingGuard;
            }
            else
            {
                // First and default part of Queen's life : Give birth to the colony
                
                System.Random random = new System.Random();
                if (random.Next() % 2 == 0)
                {
                    source += "AntWorker";
					rang = ChimeraAntClass.Worker;
                }
                else
                {
                    source += "AntSoldier";
					rang = ChimeraAntClass.Soldier;
                }        
            }
        }
		
        ChimeraAnt go =( (GameObject)Instantiate(Resources.Load(source), transform.position - transform.forward, new Quaternion())).GetComponent<ChimeraAnt>();
		go.familyBoidId = familyId;
		go.status = rang;
        return go;
    }

	//FAIT
    public override void Developpement(){
		base.Developpement();
		//speciesGenomes[0].Developpement();
		//longevity equivalent of 15 years
		longevity --;
	}

	//Fait -Méthode à supprimer 
    public override Species Reproduction(Species species){
		//print("Appel reproduction chimera ant");
        /* La reine n'a pas besoin de roi pour spawner des enfants, alors pourquoi cette méthode ? */
		if(species.GetType() == typeof(ChimeraAnt)){
			//speciesGenomesReproduction();
			print("Nous somme ici ?");
            return SpawnChildren();
		}
		return null;
    }


	//Factory en fonction des différent animaux
	public void ChimeraReproduction(Species species){
		if(species.GetType() == typeof(Tree)){
			
			if(longevity%255 != 0) return ;
		
			System.Random random = new System.Random();
			string source = "Prefabs/";
			int rand = random.Next();

			ChimeraAntClass rang = ChimeraAntClass.Worker;
			int familyId = familyBoidId;
			
			if(rand%7 == 0){
				source += "AntWorker";
			}
			else{
				source += "AntSoldier";
			}

			
			int randX = random.Next(-25,25);
			int randY = random.Next(-25,25);


			print("Naissance mode Arbre ");
			ChimeraAnt antChild = ( (GameObject)Instantiate(Resources.Load(source), transform.position + new Vector3(randX,0,randY), new Quaternion())).GetComponent<ChimeraAnt>();
			print("Naissance mode arbre - " + antChild.gameObject.name);
		}
		
	}

	//FAIT
	private void QueenModeActivation(ChimeraAnt cAnt){

		if(status == ChimeraAntClass.King){
			
			if(cAnt.sex == Sex.Female){
				cAnt.status = ChimeraAntClass.Queen;
				print("PONDEUSE !!!");
				//Destroy(gameObject);
			}
		}
	}
    
	public override void Feed(Species species){
		if(species.GetType() == typeof(Animal) || species.GetType() == typeof(Vegetal)){
			base.Feed(species);
		}
	}
    public override void Drink(){}
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){
		base.familyBehaviour();

		if(status == ChimeraAntClass.Worker){
			Deplacement(new Vector3(-3f,0f,0f));
		}

		if(status == ChimeraAntClass.Soldier){
			Deplacement(new Vector3(3f,0f,0f));
		}

		if(status == ChimeraAntClass.KingGuard){
			Deplacement(new Vector3(0f,2f,0f));
		}

		
	}

   	public override void stateBehaviour(){
		if(this.status == ChimeraAntClass.Queen || this.status == ChimeraAntClass.King){
			state = State.Leader;
		}
		else{
			state  = State.Follower;
		}
		//print("Status - State = " + status + " " + state);
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
	   
   	public override void other(){}

	//Mettre comportement évolution génétique
    public void geneticalEvolution(){}

	public override void Update(){
		base.Update();
		//Developpement();
		//print("Longev = "  + longevity);
		

        if (longevity%300 == 0){
			SpawnChildren();

			if(status == ChimeraAntClass.King){
				print("family Boid Id =" + familyBoidId);
			}
				//ChimeraReproduction(speciesGenomes[0]);
			
		}
		

		Vector3 randDirection;
		System.Random random = new System.Random();
		randDirection = new Vector3( -13,0,7 ) ;
		if(longevity%200 == 0){
			
			if(state == State.Leader){
				randDirection = new Vector3( random.Next(-20,20), 0, random.Next(-20,20));
				//print("Changement DIRECTION");
			}
		}
		Deplacement(randDirection);
		stateBehaviour();
		
	}

	private void OnTriggerEnter(Collider other){

		if(other.gameObject.GetComponent<ChimeraAnt>() != null){
			QueenModeActivation( other.gameObject.GetComponent<ChimeraAnt>() );
		}
	}
}