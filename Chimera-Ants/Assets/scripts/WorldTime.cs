//This class represent the time on the open world
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class WorldTime : MonoBehaviour{
    
    
    #region Singleton
    private static WorldTime _instance;
    private static DateTime _date;
    
    public static WorldTime Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Time");
                go.AddComponent<WorldTime>();
            }
            return _instance;
        }
    }
    public static DateTime Date
    {
        get
        {
            if (_date == null)
            {
                _date = new DateTime();
            }
            return _date;
        }
    }
    void Awake()
    {
        _instance = this;
        Time.timeScale = 1;
    }

    #endregion
    //Conversion of 1 minute into "time_world duration in the open world
    /* if unit = 0 time_world will be year
       if unit = 1 time_world will be month
       if unit = 2 time_world will be week
       if unit = 3 time_world will be day
       if unit = 4 time_world will be second
    */

    public Text UIText_WorldTime, UIText_RealTime,UIText_TimeScaler;
    public int timeScale = 4;

    
    public void Start(){
        WorldTime t = Instance;
        UIText_TimeScaler.text = "Time Seconds";


    }

    public void SwitchTimeScale()
    {
        timeScale = (timeScale - 1 >= 0) ? timeScale - 1 : 4;
        speedTime();
    }
    private void speedTime()
    {
        switch (timeScale)
        {
            case 0:
                {
                    Time.timeScale = 40;
                    UIText_TimeScaler.text = "Time Year";
                }
                break;
            case 1:
                {
                    Time.timeScale = 30;
                    UIText_TimeScaler.text = "Time Month";
                }
                break;
            case 2:
                {
                    Time.timeScale = 20;
                    UIText_TimeScaler.text = "Time Week";
                }
                break;
            case 3:
                {
                    Time.timeScale = 10;
                    UIText_TimeScaler.text = "Time Day";
                }
                break;
            case 4:
                {
                    Time.timeScale = 1;
                    UIText_TimeScaler.text = "Time Seconds";
                }
                break;
            default:
                {
                    Time.timeScale = 1;
                    UIText_TimeScaler.text = "Time Seconds";
                }
                break;

        }

    }

    private void Update()
    {
        UIText_RealTime.text = "Time since startup: " + Time.realtimeSinceStartup;
        UIText_WorldTime.text = "Time.time: " + Time.time;
    }

}
