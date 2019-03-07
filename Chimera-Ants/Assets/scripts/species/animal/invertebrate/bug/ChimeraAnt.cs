//This class represent Chimera ants features and behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public ChimeraAntClass status;

    private List<Genomes> speciesGenomes;

    public Species foodForQueen;
    public ChimeraAnt queen;


    // à mettre dans chimeraantmanager
    private static Dictionary<string, int> _geneticalProgress;
	
	
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

		if(familyBoidIdReference == 0){
			familyBoidId = 0;
			familyBoidIdReference++;
		}
		//Chimera Ant Id
        SetAnimalBoidId(0);

		speciesGenomes = new List<Genomes>();
		speciesGenomes.Add(Genomes.Tree);
		speciesGenomes.Add(Genomes.Wolf);
		print("Hello c'est moi");


		longevity = longevity * 15f;

        strength = 1000;
        dietaryRegime = DietaryRegime.Omnivorus;
        move = new ChimeraAntMove(_rb);


	}
    
    
	
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

        print("Instatiating " + source);

        ChimeraAnt go =( (GameObject)Instantiate(Resources.Load(source), transform.position - transform.forward, new Quaternion())).GetComponent< ChimeraAnt>();
        print("Transmitting movement ");
        ((ChimeraAntMove)move).InheritMovement((ChimeraAntMove)go.move); // transmitting queen's movement qualities to newborn
        go.queen = this;

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
		//print("Appel reproduction chimera ant");
        /* La reine n'a pas besoin de roi pour spawner des enfants, alors pourquoi cette méthode ? */
		if(species.GetType() == typeof(ChimeraAnt)){
			//speciesGenomesReproduction();
			//print("Nous somme ici ?");
            return SpawnChildren();
		}
		return null;
    }


	//Factory en fonction des différent animaux
	public void ChimeraReproduction(Genomes gene){
		
		string source = "Prefabs/";

		System.Random random = new System.Random();
		int rand = random.Next();

		if(rand%2 == 0){
			source += "AntWorker";
			ChimeraAntClass rang = ChimeraAntClass.Worker;
		}
		else{
			source += "AntSoldier";
			ChimeraAntClass rang = ChimeraAntClass.Soldier;
		}

		
		if(gene == Genomes.Tree){
			
			if(longevity%977 != 0) return ;

			int familyId = familyBoidId;
			
			int randX = random.Next(-10,10);
			int randY = random.Next(-10,10);
			print("nAISSANCE mode arbre");
			ChimeraAnt antChild = ( (GameObject)Instantiate(Resources.Load(source), transform.position + new Vector3(randX,0,randY), new Quaternion())).GetComponent<ChimeraAnt>();
		}
		
		if(gene == Genomes.Wolf){

			if (state != State.Leader) return ;

			if (isInReproductionTime == false) return ;

			int familyId = familyBoidId;

			print("Naissance mode Loup");
			ChimeraAnt antChild =( (GameObject)Instantiate(Resources.Load(source), transform.position - transform.forward*(-1.5f), new Quaternion())).GetComponent<ChimeraAnt>();
			antChild.familyBoidId = familyId;
		}
	}

	//FAIT
	private void QueenModeActivation(ChimeraAnt cAnt){

		if(status == ChimeraAntClass.King){
			
			if(cAnt.sex == Sex.Female){
				cAnt.status = ChimeraAntClass.Queen;
				//print("PONDEUSE !!!");
				//Destroy(gameObject);
			}
		}
	}

	protected override void Feed(Species species){
		if(species.GetType() == typeof(Animal) || species.GetType() == typeof(Vegetal)){
			base.Feed(species);

			if(species.GetType() == typeof(Tree) && speciesGenomes.Contains(Genomes.Tree)){
				speciesGenomes.Add(Genomes.Tree);
			}
			if(species.GetType() == typeof(Wolf) && speciesGenomes.Contains(Genomes.Wolf)){
				speciesGenomes.Add(Genomes.Tree);
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

		if(dangerLvl >= 30){
            // ca rpz quoi ?
			System.Random random = new System.Random();
            int rand = random.Next(0,10);

			if(animal.dangerLvl > 3){
				rand -= random.Next(0,animal.dangerLvl);
			}

			if(animal.dangerLvl <= 3){
				rand += random.Next(0,animal.dangerLvl);
			}

			if(rand >= 50){
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