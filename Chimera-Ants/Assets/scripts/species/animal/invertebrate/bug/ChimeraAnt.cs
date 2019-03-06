//This class represent Chimera ants features and behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;
    public Species foodForQueen;
    public ChimeraAnt queen;

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
        go.queen = this;
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

    private void Absorb(Species species)
    {
        if(status == ChimeraAntClass.Queen)
        {
            longevity += species.longevity /5;
            weight += species.weight/5;
            baseLifePoint += species.baseLifePoint / 5;
            resistance += species.resistance / 5;
            
            visionRange += species.visionRange / 5;
            Animal animal = species as Animal;
            if (animal)
            {
                strength += animal.strength / 5;
                attackSpeed += animal.attackSpeed / 5;
                ((ChimeraAntMove)move).AddMovement(animal.move);
            }
            
        }
    }
    public override void Feed(Species species)
    {
        if(status == ChimeraAntClass.Queen)
        {
            if (lifePoint <= baseLifePoint - 10)
            {
                RestoreLifePoints();
                Absorb(species);
                species.Eaten();
                print("EAT !! ");
            }
            hunger = 0;

        }
        if(status == ChimeraAntClass.Worker)
        {
            if (hunger > 70 )
            {
                print("WTFFFFFF");
                base.Feed(species);
            }
            else
            {
                foodForQueen = species;
                species.GetComponent<BoxCollider>().enabled = false;
                species.GetComponent<SphereCollider>().enabled = false;
                species.GetComponent<Rigidbody>().freezeRotation = true;
                species.transform.position = transform.position + transform.up;
                species.transform.parent = transform;

            }
        }
        if(status != ChimeraAntClass.Worker && status != ChimeraAntClass.Queen)
        {
            print("ihnooooooooooooo");
            base.Feed(species);
        }
        

    }
    public void FeedQueen(ChimeraAnt queen){
        if (queen.status != ChimeraAntClass.Queen) return;
        if (foodForQueen == null) return;
        queen.Feed(foodForQueen);
        foodForQueen = null;
	}
    
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
        if (target)
        {
            if (target.transform.parent)
            {
                target = null;
                feeding = false;
            }
        }
        if (foodForQueen)
        {
            Vector3 dir = queen.transform.position - transform.position;
            move.direction = Vector3.Normalize(dir);
            move.Apply(move.direction);
            foodForQueen.transform.localPosition = new Vector3(0, 1, 0);
            if (Vector3.Distance(queen.transform.position, transform.position) < 2)
            {
                FeedQueen(queen);
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (status == ChimeraAntClass.Queen)
                SpawnChildren();
            
        }
    }
}