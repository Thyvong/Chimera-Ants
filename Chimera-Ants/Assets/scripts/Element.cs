//Element is an abstract class whose all the open world Elements inherit
using UnityEngine;

public abstract class Element : MonoBehaviour{
   private static int idReference = 0;
   protected int id ; //Element identifiant
<<<<<<< HEAD
   protected GameObject model;
   /*protected Vector3 position;
   protected Vector3 scale;*/
=======
>>>>>>> fb551fe5bb3aa1c851881da8de5dde07ef8b54cd

   protected Element(){
      this.id = idReference;
      idReference ++;
<<<<<<< HEAD

   }

   protected void createModel(string name){
      model = new GameObject(name);
      model.AddComponent<Rigidbody>();
=======
>>>>>>> fb551fe5bb3aa1c851881da8de5dde07ef8b54cd
   }


}