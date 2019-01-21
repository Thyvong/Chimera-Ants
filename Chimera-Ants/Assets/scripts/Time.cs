//This class represent the time on the open world
using System;
using System.Diagnostics;
using UnityEngine;
public class Time : MonoBehaviour{
    private static DateTime date;
    private Time(){
        date = new DateTime();
        print(date);
    }

    //Conversion of 1 minute into "time_world duration in the open world
    /* if unit = 0 time_world will be year
       if unit = 1 time_world will be month
       if unit = 2 time_world will be week
       if unit = 3 time_world will be day
    */ 
    private void speedTime(float time_world, int unit){
        

    }

    public void Start(){
        Time t = new Time();
    }


}
