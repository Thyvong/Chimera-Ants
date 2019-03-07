//This class represent all kind of animals and their behaviour
using UnityEngine;
using System.Collections.Generic;



public abstract class Animal : Species, AnimalManager{
    public NutritionStyle[] nutritionStyle { get; protected set; }
    public GroupStyle[] groupStyle { get; protected set; }
    public DietaryRegime dietaryRegime { get; protected set; }
    public Sex sex; // { get; set; }

    protected bool isInReproductionTime = false;
    public State state { get; protected set; }
    public int dangerLvl { get; protected set; }
    public int animalBoidId { get; protected set; } // personal id
    public int familyBoidId { get; protected set; } // group id
    public static int familyBoidIdReference = 0;
    public bool isInBoid = false;
    public List<Animal> animalInBoids;

    public Vector3 direction;

    //Méthode abstraite
    public abstract void groupBehaviour();
    public abstract void stateBehaviour();
    public abstract void other();

    

    //Fait
    public void SetAnimalBoidId(int id){
        animalBoidId = id;
    }
    
    //Fait
    public void SetSex(Sex genre){
        sex = genre;
    }
   
   //Fait
    protected void SetState(State rang){
        state = rang;
    }

    //Fait
    public virtual void DangerEvaluation(Species species){
        if(species.GetType() == typeof(Animal)){
			Animal animal = (Animal) species;

            //If it's not the same animal species
			if(animal.animalBoidId != animalBoidId){
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
    //Fait
    protected void increaseDangerLvl(int nbLvl){
        dangerLvl = dangerLvl + nbLvl;
    }

    //Fait
    public void resetDangerLvl(){
        dangerLvl = 0;
    }

    //Fait
    public bool RunAway(Animal animal){

        //If dangerLvl is high enough
        if(dangerLvl >= 3){
            //The rand value represent the bluff mecanism
            System.Random random = new System.Random();
            int rand = random.Next(0,10);

            //If the animal is affraid
            if(animal.dangerLvl > 3){
                //dangerLvl decrease
                rand -= random.Next(1,animal.dangerLvl);
            }
            //If the animal is not affraid
            if(animal.dangerLvl <= 3){
                //dangerLvl increase
                rand += random.Next(1,animal.dangerLvl);
            }

            if(rand >= 5){
                return true;
            }     
        }
        return false;
   	}

    //Fait
    public virtual void familyBehaviour(){
        boidBehaviour();
        stateBehaviour();
    }

    //Fait
    public void boidBehaviour(){

        if(isInBoid == true){
            
            Animal nearestNeighbour = null;
            float minDistance = 999999999f;

            foreach(Animal animal in animalInBoids){
                
                // Selection of the nearest neighbour
                if( Vector3.Distance(transform.position, animal.transform.position) < minDistance && animal.familyBoidId == familyBoidId && minDistance > 3.5f ){
                    nearestNeighbour = animal;
                    minDistance = Vector3.Distance(transform.position, animal.transform.position);

                    //We come closer
                    transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position,0.1f ) ;
                    transform.LookAt(nearestNeighbour.transform.position);
                    print("On s'approche");
                }
            }
            
            if( minDistance < 3.5f){
                //We move away
                transform.position = Vector3.MoveTowards(transform.position, nearestNeighbour.transform.position*(-1),0.1f ) ;
                transform.LookAt(nearestNeighbour.transform.position*(-1));
                print("On s'éloigne");
            }

            //We center
            Vector3 direction = new Vector3(0,0,0);
            foreach(Animal animal in animalInBoids){
                direction += animal.transform.forward; 
            }
            transform.LookAt(direction);
            Deplacement(direction);
        }  

    }
    
    //Fait
    public void Attack(Species species){
        if(species.lifePoint > 0){
			species.TakeDamage( strength * weight );
		}
    }
    

    //UNITY Methode
    //Fait
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

        direction = new Vector3(0f,0f,0f);
    }

    //Fait
    public virtual void Start(){
        System.Random random = new System.Random();
        if (random.Next() % 2 == 0){
            sex = Sex.Male;
            
        }
        else{
            sex = Sex.Female;
        }

        if(state == State.Leader){
            System.Random rand = new System.Random();
            direction = new Vector3( rand.Next(-200,200), 0, rand.Next(-200,200));
        }
        print("type" + GetType() + " sex1 " + sex);
        
    }

    //Fait
    public void OnTriggerEnter(Collider other){
        print("On trigger enter");
        //If the gameObject is an animal
        if(other.gameObject.GetComponent<Animal>() != null){

            //If the animals belongs to the same family
            if(other.gameObject.GetComponent<Animal>().familyBoidId == familyBoidId){

                if( animalInBoids.Contains(other.gameObject.GetComponent<Animal>()) == false){
                        //Add to the list
                        print("add");
                        animalInBoids.Add(other.gameObject.GetComponent<Animal>());
                }                
            }
        }
	}
	
	public void OnTriggerExit(Collider other){
        
        //If the animal leaves the trigger
        if(other.gameObject.GetComponent<Animal>() != null){
            //Remove the animal to the list
            print("remove");
            animalInBoids.Remove(other.gameObject.GetComponent<Animal>());

            
        }
	}

    public virtual void Update(){
        base.Update();
        //Developpement();
        //If isInBoid = true boidBehaviour is active 
        if(animalInBoids != null){
            isInBoid = true;
        }
        else{
            isInBoid = false;
        }
        familyBehaviour();

        if(longevity%500 == 0){
            //print( GetType() + " family Boid Id =" + familyBoidId + "isInBOID " + isInBoid );  
        }

        //print("animal count " + animalInBoids.Count);
        if(animalInBoids.Count > 3){
                System.Random random = new System.Random();
                if(state == State.Leader){
                    direction = new Vector3( random.Next(-200,200), 0, random.Next(-200,200));
                    Deplacement(direction);
                    print("nouvelle direction = " + direction);
                }
            }

        
        
    }
    
    
}