//This class represents wolves behaviour and features
using UnityEngine;
public class Wolf : Canid{

	
    // Species method
    public override Species Reproduction(Species species){
		
		if(species.GetType() != typeof(Wolf)) return null;
		
		Wolf wolfFather = (Wolf) species;
		Wolf wolfMother = (Wolf) this;
        
		if (state != State.Leader || wolfFather.state != State.Leader){
			return null;
		} 
		if (wolfMother.sex != Sex.Female || wolfFather.sex != Sex.Male){
			return null;
		} 
		if (isInReproductionTime == false){
			return null;
		} 

		int familyId = wolfFather.familyBoidId;
        string source = "Prefabs/WolfHybride";

    	Wolf wolfChild =( (GameObject)Instantiate(Resources.Load(source), transform.position - transform.forward*(2f), new Quaternion())).GetComponent<Wolf>();

		/*if(sex == Sex.Male){
			transform.localScale = new Vector3(12f,12f,12f);
		}else{
			transform.localScale = new Vector3(10f,10f,10f);
		}*/
		isInReproductionTime = false;
		return wolfChild;
    }
    
	public void Feed(Species species){
		if(species.GetType() == typeof(Animal)){
			base.Feed(species);
		}
	}
    public override void Drink(){}
	// Animal method
    public override void groupBehaviour(){}
   	public override void familyBehaviour(){
		   base.familyBehaviour();
		   stateBehaviour();

		   if(longevity <2500 && longevity %600 == 0 && longevity > 1000){
			   isInReproductionTime = true;
		   }
		   else{
			   isInReproductionTime = false;
		   }
	   }
   	public override void stateBehaviour(){
		
		if(isWinner == true){
			state = State.Leader;
		}
		else{
			state = State.Follower;
		}
	}
	public void dangerEvaluation(Species species){}
   	public override void other(){}


	public override void Start(){
		//base.Start();

		SetAnimalBoidId(1);

		longevity = longevity * 8f;
        strength = 100;
		
		if(sex == Sex.Male){
			weight = 20;
			transform.localScale = new Vector3(12f,12f,12f);
		}
		else{
			weight = 15;
			transform.localScale = new Vector3(10f,10f,10f);
		}

		familyBoidIdReference++;
        familyBoidId = Animal.familyBoidIdReference;

        lifePoint = 100;
        baseLifePoint = 100;

        resistance = weight* 0.8f;
        lifeStyle = LifeStyle.Settled;
        speed = 6;
        hunger = 0;
        visionRange = 5f;
	}

	public void OnTriggerEnter(Collider other){
        
        //If the gameObject is an animal
        if(other.gameObject.GetComponent<Wolf>() != null){
			//If they have the same sex
			if(other.gameObject.GetComponent<Wolf>().sex == sex){
				//If it's a Challenger
				if(state != State.Leader && other.gameObject.GetComponent<Wolf>().state == State.Leader){
					int score = 3;
					if(weight >  other.gameObject.GetComponent<Wolf>().weight){
						score--;
					}
					if(longevity > other.gameObject.GetComponent<Wolf>().longevity){
						score--;
					}
					if(lifePoint > other.gameObject.GetComponent<Wolf>().lifePoint){
						score--;
					}
					if(score >= 2){
						isWinner = true;
					}
				}
			}			
        }
	}

	public void OnTriggerStay(Collider other){
		if(other.gameObject.GetComponent<Wolf>() != null){
			//print("stay !!");
			Reproduction(other.gameObject.GetComponent<Wolf>());
		}
		
	}

	public void Update(){
		base.Update();
		familyBehaviour();

	}
}