//This class represent all kind of animals and their behaviour
using UnityEngine;
using System.Collections.Generic;



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

    public bool isInBoid = false;
    public List<Animal> animalInBoids;
    

    

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
   public abstract void stateBehaviour();
   
   //public abstract bool RunAway(Animal animal);

    public virtual void Start(){
        System.Random random = new System.Random();
        if (random.Next() % 2 == 0)
        {
            sex = Sex.Male;
        }
        else
        {
            sex = Sex.Female;
        }
        print("SEX = " + sex);
        
   }



   public bool RunAway(Animal animal){

        if(dangerLvl >= 3){
            // ca rpz quoi ?
            System.Random random = new System.Random();
            int rand = random.Next(0,10);

            if(animal.dangerLvl > 3){
                rand -= random.Next(1,animal.dangerLvl);
            }

            if(animal.dangerLvl <= 3){
                rand += random.Next(1,animal.dangerLvl);
            }

            if(rand >= 5){
                print("DANGER");
                return true;
            }
                
        }
        print("NO DANGER");
        return false;
   	}

    public abstract void other();

    public virtual void familyBehaviour(){
        
    }

    public void boidBehaviour(/*List<Animal> animals*/){

        if(isInBoid == true){
            
            Animal nearestNeighbour = null;
            float minDistance = 999999999f;
            print("Méthode Boids");
            foreach(Animal animal in animalInBoids){
                
                // Selection of the nearest neighbour
                if( Vector3.Distance(transform.position, animal.transform.position) < minDistance ){
                    nearestNeighbour = animal;
                    minDistance = Vector3.Distance(transform.position, animal.transform.position);
                    print("Selection du plus proche");

                    transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position,0.1f ) ;
                    transform.LookAt(nearestNeighbour.transform.position);//On s'approche
                }
            }
            
            if( minDistance < 3.5f ){
                //on s'éloigne
                transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position*(-1),0.1f ) ;
                transform.LookAt(nearestNeighbour.transform.position*(-1));//on s'éloigne 
                print("éloignement");
            }
            Vector3 direction = new Vector3(0,0,0);
            foreach(Animal animal in animalInBoids){
                direction += animal.transform.forward; 
            }
            transform.LookAt(direction);
            Deplacement(direction);
            print("Déplacement BOIDS");
        }  
    }
    public void Awake(){
        GetComponent<SphereCollider>().isTrigger = true;

        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rb.mass = weight;
        _rb.drag = 5;
        _rb.angularDrag = weight / 10.0f;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Attack(Species species){
        if(species.lifePoint > 0){
			species.TakeDamage( strength * weight );
		}
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.GetComponent<Animal>() != null){
            animalInBoids.Add(other.gameObject.GetComponent<Animal>());
            print("ajout - Animal OnTriggrEnter");
        }

        

        /* mettre à true isInBoid
           puis mettre comportement boid dans familyBehaviour
           puis dans ontriggerEnter appeler familyBehaviour
           mettre dans update si tab animals non vide alors isInBoids = true
        */


	}
	
	private void OnTriggerExit(Collider other){

        if(other.gameObject.GetComponent<Animal>() != null){
            animalInBoids.Remove(other.gameObject.GetComponent<Animal>());
            print("Suppression - animal ontriggerexit");
        }
	}

    public virtual void Update(){
        
        if(animalInBoids != null){
            isInBoid = true;
            print("Update - isInBoid == true");
        }
        else{
            isInBoid = false;
            print("Update- isInBoid == false");
        }
        boidBehaviour();
        print("UPDATE ANIMAL");

        
        
    }


}