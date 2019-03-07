//This class represents different kind of mammal, features and behaviour
using UnityEngine;
public abstract class Mammal : Vertebrate{
    
    protected bool isWinner = true;

    public override void stateBehaviour(){
		
		if(isWinner == true){
			state = State.Leader;
		}
		else{
			state = State.Follower;
		}
	}

    public override void Developpement(){
		base.Developpement();
		longevity--;
		if(longevity > 1200){
			transform.localScale = new Vector3(transform.localScale.x+0.005f,transform.localScale.y+0.005f,transform.localScale.z+0.005f);
			weight += 0.05f;
		}
		if(longevity < 1200 ){
			transform.localScale = new Vector3(transform.localScale.x+0.002f,transform.localScale.y+0.002f,transform.localScale.z+0.002f);
			weight -= 0.01f;
		}
        
	}
    
}