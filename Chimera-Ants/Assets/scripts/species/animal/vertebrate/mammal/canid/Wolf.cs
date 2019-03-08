//This class represents wolves behaviour and features
using UnityEngine;
public class Wolf : Canid{

    public new static int familyBoidIdReference = 0;


    // Species method
    public override Species Reproduction(Species species){
		
		if(species.GetType() != typeof(Wolf)) return null;
		
		Wolf wolfFather = (Wolf) species;
		Wolf wolfMother = this;
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
        wolfChild.familyBoidId = familyBoidId;
        /*if(sex == Sex.Male){
			transform.localScale = new Vector3(12f,12f,12f);
		}else{
			transform.localScale = new Vector3(10f,10f,10f);
		}*/
        wolfChild.transform.parent = transform.parent;
		isInReproductionTime = false;
		return wolfChild;
    }
    
	public override void Feed(Species species){
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
	public override void DangerEvaluation(Species species){
        base.DangerEvaluation(species);

    }
   	
	protected override void Awake()
    {
        base.Awake();

		SetAnimalBoidId(1);
        lifePoint = 100;
        baseLifePoint = 100;
        longevity = longevity * 8f;
        strength = 100;
        resistance = weight * 0.8f;
        lifeStyle = LifeStyle.Settled;
        hunger = 0;
        visionRange = 5f;

        if (sex == Sex.Male){
            print("getting big");
			weight = 20;
			transform.localScale += (transform.localScale * 0.05f);
		}
		else{
			weight = 15;
		}

		
        familyBoidId = familyBoidIdReference;
        familyBoidIdReference++;


        move = new RabbitMove(_rb);
	}
    /*
	protected override void OnTriggerEnter(Collider other){
        base.OnTriggerEnter(other);
        //If the gameObject is an animal
        Wolf wolf = other.GetComponent<Wolf>();
        if (wolf){
			//If they have the same sex
			if(wolf.sex == sex && state != wolf.state){
                //If it's a Challenger
                int score = 3;
                if (weight < wolf.weight)
                {
                    score--;
                }
                if (longevity < wolf.longevity)
                {
                    score--;
                }
                if (lifePoint < wolf.lifePoint)
                {
                    score--;
                }
                if (score >= 2)
                {
                    isWinner = true;
                }

            }			
        }
	}
    */
	public void OnTriggerStay(Collider other){
		if(other.gameObject.GetComponent<Wolf>() != null){
			//print("stay !!");
			Reproduction(other.gameObject.GetComponent<Wolf>());
		}
		
	}

    public override bool RunAway(Animal animal)
    {
        if (dangerLvl >= 30)
        {
            // ca rpz quoi ?
            System.Random random = new System.Random();
            int rand = random.Next(0, 10);

            if (animal.dangerLvl > 3)
            {
                rand -= random.Next(0, animal.dangerLvl);
            }

            if (animal.dangerLvl <= 3)
            {
                rand += random.Next(0, animal.dangerLvl);
            }

            if (rand >= 50)
            {
                return true;
            }

        }
        return false;
    }
    public override void other() { }

    protected override void Update(){
		base.Update();

	}

}