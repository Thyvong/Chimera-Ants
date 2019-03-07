//This class represent different tree their behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Vegetal{
    Fruit fruit;

    public Tree(){
        //print("hello Tree");
        
    }
    public void Start(){
        print("ROOOH");
    }

    public override Species Reproduction(Species species){
        
        string source = "Prefabs/Trees/Tree1";

        if(longevity%931 == 0){
            System.Random random = new System.Random();
            int randX = random.Next(-100,100);
            int randY = random.Next(-100,100);
            //print("Instatiating " + source);

            Tree tree = ( (GameObject)Instantiate(Resources.Load(source), transform.position + new Vector3(randX,0,randY), new Quaternion())).GetComponent<Tree>();
            //print("LO " + longevity);
            tree.gameObject.transform.localScale = new Vector3(10f,10f,10f);
            return tree;
        }
        return null;
    }

    public override void Developpement(){
        longevity--;
        transform.localScale *= 1.0001f; 
    }

    private void Update(){
        Reproduction(null);
        Developpement();
    }
    
    
}