// Coordinate's and its operation representation 

using UnityEngine;
using System;
public class Coordinate : MonoBehaviour{

    private float abscissa;
    private float ordinate;
    private float depth;
    public Coordinate(float abscissa, float ordinate, float depth){
        this.abscissa = abscissa;
        this.ordinate = ordinate;
        this.depth = depth;
    }

    //print the coordinate
    public void printCoordinate(){
        print("abscisse = " + this.abscissa);
        print("ordinate = " + this.ordinate);
        print("depth = " + this.depth);
    }

    public void abscissaTranslation(float arrival_abscissa){
        this.abscissa = arrival_abscissa;
    }

    public void ordinateTranslation(float arrival_ordinate){
        this.ordinate = arrival_ordinate;
    }

    public void depthTranslation(float arrival_depth){
        this.depth = arrival_depth;
    }

    public void abscissaRotation(double angle){
        this.abscissa = (float) (this.abscissa * Math.Sin(angle) + this.ordinate * Math.Cos(angle));
    }

    public void ordinateRotation(double angle){
        this.ordinate = (float) (- this.abscissa * Math.Sin(angle) + this.ordinate * Math.Cos(angle));
    }

    /*public void Start(){
        Coordinate position = new Coordinate(1,2,3);
        print("first iteration ! ");
        position.printCoordinate();

        print("second iteration ! ");
        position.abscissaTranslation(10.3f);
        position.ordinateTranslation(20.4f);
        position.depthTranslation(30.5f);

        position.printCoordinate();
    } */
}