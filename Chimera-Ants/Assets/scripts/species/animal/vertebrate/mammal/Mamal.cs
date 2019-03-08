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
		
        
	}
    
}