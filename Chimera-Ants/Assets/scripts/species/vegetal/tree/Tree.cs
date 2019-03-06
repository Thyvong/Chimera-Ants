//This class represent different tree their behaviour
using System;
using System.Collections.Generic;
using UnityEngine;
public class Tree : Vegetal{
    Fruit fruit;

    public override Species Reproduction(Species species){
        string source = "Prefabs/Trees/Tree1";

        if(longevity%1000 == 0){
            Tree tree = ( (GameObject)Instantiate(Resources.Load(source), transform.position + transform.forward, new Quaternion())).GetComponent<Tree>();
            return tree;
        }
        return null;
		
    }

    public override void Developpement(){
        if(longevity%50 == 0){
            transform.localScale = transform.localScale*1.01f;
            print("GRANNNNNDEUUR");
        }
        longevity--;
    }

    private void Update(){
        Reproduction(null);
        Developpement();
    }
    
    
}