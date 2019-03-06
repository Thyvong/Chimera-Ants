//Element is an abstract class whose all the open world Elements inherit
using UnityEngine;

public abstract class Element : MonoBehaviour{

    private static int idReference = 0;
    protected int id ; //Element identifiant
    protected GameObject model;
    /*protected Vector3 position;
    protected Vector3 scale;*/

    protected Element(){
        id = idReference;
        idReference ++;


    }

    // pas besoin, toutes les classes sont instantité en même temps que le gameobject sur lequel ils sont accroché
    /*
    protected void createModel(string name){
        model = new GameObject(name);
        model.AddComponent<Rigidbody>();
    }
    */

}