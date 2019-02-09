//Element is an abstract class whose all the open world Elements inherit
using UnityEngine;

public abstract class Element : MonoBehaviour{
   private static int idReference = 0;
   protected int id ; //Element identifiant
   protected GameObject model;
   /*protected Vector3 position;
   protected Vector3 scale;*/

   protected Element(){
      this.id = idReference;
      idReference ++;

   }


}