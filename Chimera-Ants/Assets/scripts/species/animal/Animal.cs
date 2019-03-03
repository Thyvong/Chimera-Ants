//This class represent all kind of animals and their behaviour
using UnityEngine;

public abstract class Animal : Species, AnimalManager{
    public NutritionStyle[] nutritionStyle { get; protected set; }
    public GroupStyle[] groupStyle { get; protected set; }
    public DietaryRegime dietaryRegime { get; protected set; }
    public Sex sex { get; protected set; }
    public State state { get; protected set; }
    public int dangerLvl { get; protected set; }
    
    public int animalBoidId { get; protected set; } // personal id
    public int familyBoidId { get; protected set; } // group id
    public static int familyBoidIdReference = 0;

    

    public void SetAnimalBoidId(int id){
        animalBoidId = id;
    }
    
    public void SetSex(Sex genre){
        sex = genre;
    }
   
    protected void SetState(State rang){
        state = rang;
    }

    public virtual void DangerEvaluation(Species species){

        if(species.GetType() == typeof(Animal)){
			Animal animal = (Animal) species;
			if(animal.animalBoidId != animalBoidId){ // ?? si animal is not this ??
				increaseDangerLvl(1);
			}
			if(animal.dietaryRegime != dietaryRegime){
				increaseDangerLvl(2);
			}
			if(animal.state == State.Leader){
				increaseDangerLvl(2);
			}
        }
    }
    // Increase danger level by nblvl
    protected void increaseDangerLvl(int nbLvl){
        dangerLvl = dangerLvl + nbLvl;
    }

    public void resetDangerLvl(){
        dangerLvl = 0;
    }

   public abstract void groupBehaviour();
   public abstract void familyBehaviour();
   public abstract void stateBehaviour();
   
   public abstract bool RunAway(Animal animal);
   public abstract void other();

    public void Attack(Species species){
        if(species.lifePoint > 0){
			species.TakeDamage( strength * weight );
		}
    }

    private void OnTriggerEnter(Collider other){

        if( RunAway( (Animal) other.GetComponent(typeof(Animal)) ) == true ){
            print("RunAway true"); 
            transform.position = Vector3.MoveTowards(transform.position, other.GetComponent(typeof(Animal)).transform.position*(-1f),0.1f ) ;
        }
        transform.position = Vector3.MoveTowards(transform.position, other.GetComponent(typeof(Animal)).transform.position  -  other.GetComponent(typeof(Animal)).transform.forward,0.01f ) ;
        print("Collision detecter enter");

	}

	private void OnTriggerStay(Collider other){

        if( RunAway( (Animal) other.GetComponent(typeof(Animal)) ) == false ){
            print("RunAway false on s'approche"); 
            transform.position = Vector3.MoveTowards(transform.position, other.GetComponent(typeof(Animal)).transform.position  -  other.GetComponent(typeof(Animal)).transform.forward,0.01f ) ;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, other.GetComponent(typeof(Animal)).transform.position  -  other.GetComponent(typeof(Animal)).transform.forward,0.01f ) ;
        print("Collision detecter stay");


        
        //Feed( (Species) other.GetComponent<Species>()); marche pas
	}
	
	private void OnTriggerExit(Collider other){
		if( RunAway( (Animal) other.GetComponent(typeof(Animal)) ) == false ){
            print("RunAway false"); 
            transform.position = Vector3.MoveTowards(transform.position, other.GetComponent(typeof(Animal)).transform.position  -  other.GetComponent(typeof(Animal)).transform.forward,0.01f ) ;
        }
	}



    private void OnCollisionEnter(Collision other){

        
        
    }

    protected void OnCollisionStay(Collision other){
        if( familyBoidId != other.gameObject.GetComponent<Animal>().familyBoidId ){
            Attack( other.gameObject.GetComponent<Animal>() );
            if( other.gameObject.GetComponent<Animal>().lifePoint <= 0 ){
                Feed( other.gameObject.GetComponent<Animal>() );
                print("Collision familiale");
                return;
            }
            Feed( other.gameObject.GetComponent<Species>() );
            print("Collision manger");
        }
    }

    private void OnCollisionExit(Collision other){
        
    }

}