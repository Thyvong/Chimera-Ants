//This class represent Chimera ants features and behaviour
using System;
using UnityEngine;

public class ChimeraAnt : Bug, ChimeraAntManager{

    public GameObject ant;

    public ChimeraAntClass status; // Ant class ranking
    private Species[] speciesGenomes; // Assimililated species genomes
	private static int antBoidIdReference = 0; // nb of boid groups created
    private int antBoidId; // personal id for boid groups

    public int cpt=0;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        print(antBoidIdReference); // set by reproduction or at initialize
        switch (status) {

            case ChimeraAntClass.Queen:
                {
                    transform.localScale = new Vector3(50, 50, 50);
                }break;

            case ChimeraAntClass.Soldier:
                {
                    transform.localScale = new Vector3(30, 30, 30);
                }
                break;
                
            default:
                break;
        }
    }

    //Constructor
    private ChimeraAnt(){
		this.setAnimalBoidId(0);

		print("ANIMAL BOID = " + getAnimalBoidId());

		this.status = ChimeraAntClass.Queen;
		//print("id = " + this.id);
		this.antBoidId = antBoidIdReference;
		//print("ant boid id = " + antBoidId);
		antBoidIdReference++;
		//print("status = " + this.status);
	}
    
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
		int rand = -1;
		if(species.GetType() == typeof(ChimeraAnt)){
			ChimeraAnt cAnt = new ChimeraAnt();
			cAnt.antBoidId = this.antBoidId;
			print("cAnt boid number = "+ cAnt.antBoidId);

            System.Random random = new System.Random();
			rand = random.Next();
			if(rand%2 == 0){
				cAnt.status = ChimeraAntClass.Worker;
			}
			else{
				cAnt.status = ChimeraAntClass.Soldier;
			}
			print("status enfant = " + cAnt.status);
            Instantiate(ant, transform.position+new Vector3(rand/2,0,rand).normalized * 10, new Quaternion(0, rand, 0,0));
			return cAnt;	
			//print("OK");
		}
		//print("NO");
		return null;

    }
    public override void feed(Species species){}
    public override void drink(){}
    public override void death(){}

    // Animal method
    public override void Move()
    {
        rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
        return;
    }
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){}
   	public override void stateBehaviour(){
		if(this.status == ChimeraAntClass.Queen || this.status == ChimeraAntClass.King){
			this.setState(State.Leader);
			print("Leader");
		}
		else{
			this.setState(State.Follower);
			print("Follower");
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

    private void FixedUpdate()
    {
        cpt++;
        Move();
        if (status == ChimeraAntClass.Queen && cpt%200==0 )
            reproduction(new ChimeraAnt());
    }


}