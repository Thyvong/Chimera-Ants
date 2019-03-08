//This class represent Chimera ants features and behaviour

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;

    private List<Genomes> speciesGenomes;

    public Species foodForQueen;
    public ChimeraAnt queen;

    protected new static System.Random rng;
    public new static int  familyBoidIdReference = 0;


    // à mettre dans chimeraantmanager
    private static Dictionary<string, int> _geneticalProgress;
	
	
    private bool _isKingBorn = false;


    //Constructor
    protected override void Awake()
    {
        base.Awake();
    
        print("CHIMERA ANT " + name + "has started");
        if (rng == null)
        {
            rng = new System.Random();
        }
        
        if (rng.Next() % 2 == 0)
        {
            sex = Sex.Male;
        }
        else
        {
            sex = Sex.Female;
        }
        
        if (status == ChimeraAntClass.King)
        {
            sex = Sex.Male;
            state = State.Leader;
        }
        if (status == ChimeraAntClass.Queen)
        {
            sex = Sex.Female;
            state = State.Leader;
            familyBoidIdReference++;
        }
        if (status != ChimeraAntClass.Queen && status != ChimeraAntClass.King)
        {
            state = State.Follower;
        }
        familyBoidId = familyBoidIdReference;

        //Chimera Ant Id
        SetAnimalBoidId(0);

		speciesGenomes = new List<Genomes>();
		speciesGenomes.Add(Genomes.Tree);
		speciesGenomes.Add(Genomes.Wolf);


		longevity = longevity * 15f;

        strength = 1000;
        dietaryRegime = DietaryRegime.Omnivorus;

        move = new ChimeraAntMove(_rb);

        GetComponent<SphereCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = true;

	}
    
    private ChimeraAnt SpawnChildren()
    {	
		int familyId = familyBoidId;

        if (status != ChimeraAntClass.Queen) return null;

        string source = "Prefabs/";

        // Last part of Queen's life : Give birth to the future King
        if (longevity <= 2000 && !_isKingBorn)
        {
			//if it's a king we set a new family boid id
            source += "AntKing";
            _isKingBorn = true;
        }
        else
        {
            // Middle part of Queen's life : Give birth to the future King Guard
            if (longevity <= 3750 && !_isKingBorn)
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
        go.familyBoidId = familyBoidId; // after init/awake, reassign newborn to this queen's family
        go.transform.parent = transform.parent;
        return go;
    }

	//FAIT
    public override void Developpement(){
		base.Developpement();
		//speciesGenomes[0].Developpement();
		//longevity equivalent of 15 years
		

		if(status == ChimeraAntClass.King){
			longevity -= 100;
		}else{
			longevity --;
		}
	}

	//Fait -Méthode à supprimer 
    public override Species Reproduction(Species species){
        return ChimeraReproduction(speciesGenomes[Random.Range(0,speciesGenomes.Count)]);

    }


    //Factory en fonction des différent animaux
    public Species ChimeraReproduction(Genomes gene)
    {
        ChimeraAnt child = null;
        switch (gene){
            case Genomes.Tree:{
                    child = SpawnChildren();
                    int randX = Random.Range(-10, 10);
                    int randY = Random.Range(-10, 10);
                    print("nAISSANCE mode arbre");
                    child.transform.position = transform.position + new Vector3(randX, 0, randY);
                }
                break;
            case Genomes.Wolf:
                {
                    if (state != State.Leader && isInReproductionTime == false) return null;

                    child = SpawnChildren();
                }
                break;
            default:
                {
                    child = SpawnChildren();
                }break;
        }
		
        return child;
	}

	//FAIT
	private void QueenModeActivation(ChimeraAnt cAnt){

		if(status == ChimeraAntClass.King){
			
			if(cAnt.sex == Sex.Female){
				cAnt.status = ChimeraAntClass.Queen;
				//change material to red
			}
		}
	}


    public override void Drink(){}
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){
		base.familyBehaviour();

		if(status == ChimeraAntClass.Worker){
			//Deplacement(new Vector3(-3f,0f,0f));
		}

		if(status == ChimeraAntClass.Soldier){
			//Deplacement(new Vector3(3f,0f,0f));
		}

		if(status == ChimeraAntClass.KingGuard){
			//Deplacement(new Vector3(0f,2f,0f));
		}

		//wolf behaviour
		if(longevity > 1000 && longevity %600 == 0 && longevity < 7000){
			isInReproductionTime = true;
		} 
		else{
			   isInReproductionTime = false;
		}
	}

   	public override void stateBehaviour(){
		if(status == ChimeraAntClass.Queen || status == ChimeraAntClass.King){
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

/*
	public override void Update(){
		base.Update();
		

        if (longevity%300 == 0){
			SpawnChildren();			
		}		

		foreach(Genomes g in speciesGenomes){
			if(g != null){
				ChimeraReproduction(g);
				print("Pas null");
			}
			else{
				print("c'est null");
			}
			
		}
	}
*/
	


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
            if (species.GetType() == typeof(Tree) && speciesGenomes.Contains(Genomes.Tree))
            {
                speciesGenomes.Add(Genomes.Tree);
            }
            if (species.GetType() == typeof(Wolf) && speciesGenomes.Contains(Genomes.Wolf))
            {
                speciesGenomes.Add(Genomes.Tree);
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
            if (hunger > 50 || queen == null || queen.dead)
            {
                base.Feed(species);
                species = null;
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
            base.Feed(species);
            species = null;
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

		if(dangerLvl >= 30){
            // ca rpz quoi ?
			System.Random random = new System.Random();
            int rand = random.Next(0,10);

			if(animal.dangerLvl > 30){
				rand -= random.Next(0,animal.dangerLvl);
			}

			if(animal.dangerLvl <= 20){
				rand += random.Next(0,animal.dangerLvl);
			}

			if(rand >= 50){
				return true;
			}
			
		}

   		return false;
   	}
   	public override void other(){}
    //Chimera-ants special method
    public void geneticalEvolution(){}



	protected override void Update(){
        base.Update();
        if (target!= null)
        {
            if (target.transform.parent && target.transform.parent.GetComponent<ChimeraAnt>())
            {
                target = null;
                feeding = false;
            }
        }
        if (foodForQueen != null)
        {
            if (queen != null && !queen.dead)
            {
                Vector3 dir = queen.transform.position - transform.position;
                move.direction = Vector3.Normalize(dir);
                move.Apply(move.direction);
                foodForQueen.transform.localPosition = new Vector3(0, 1, 0);
                if (Vector3.Distance(queen.transform.position, transform.position) < 3)
                {
                    FeedQueen(queen);
                }
            }
            else
            {
                Feed(foodForQueen);
            }
            
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (status == ChimeraAntClass.Queen)
                SpawnChildren();
            
        }
        if (longevity % 300 == 0)
        {
            if (status == ChimeraAntClass.Queen)
                SpawnChildren();
        }
    }
}