﻿//This class represents white bear behaviour and features
using UnityEngine;
public class WhiteBear : Bear{
    // Species method
	public override void developpement(){}
    public override Species reproduction(Species species){
    	WhiteBear bear = new WhiteBear();
    	return bear;
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
   		//return State.Leader;
   	}
	public override int dangerEvaluation(Species species){
		return 0;
	}
   	public override void kill(Species species){}
   	public override bool runAway(){
   		return false;
   	}
   	public override void other(){}
}