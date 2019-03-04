//This class represent different tree their behaviour
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Vegetal{
    Fruit fruit;

    public override Species Reproduction(Species species){
        string source = "Prefabs/Trees/Tree1";

        if(longevity%537 == 0){
            System.Random random = new System.Random();
            int randX = random.Next(-25,25);
            int randY = random.Next(-25,25);

            Tree tree = ( (GameObject)Instantiate(Resources.Load(source), transform.position + new Vector3(randX,0,randY), new Quaternion())).GetComponent<Tree>();
            print("LO " + longevity);
            tree.gameObject.transform.localScale = new Vector3(10f,10f,10f);
            return tree;
        }
        return null;
    }

    public override void Developpement(){
        /*if(longevity%50 == 0){
            transform.localScale = transform.localScale*1.01f;
            print("GRANNNNNDEUUR");
        }*/
        longevity--;
        //print("LONG DEV " + longevity);
    }

    private void Update(){
        Reproduction(null);
        Developpement();
    }
    
    
}