//This class represent all kind of animals and their behaviour
/*
using Spieces;
using GroupStyle;
using NutritionStyle;
using DietaryRegime;
using AnimalManager;
*/
public abstract class Animal : Species, AnimalManager{
   protected NutritionStyle[] nutritionStyle;
   protected GroupStyle[] groupStyle;
   protected DietaryRegime dietaryRegime;
   protected Sex sex;
   protected State state;

   //private static int animalBoidIdReference = 0;
   //protected int animalBoidId;

   public abstract void groupBehaviour();
   public abstract void familyBehaviour();
   public abstract void stateBehaviour();
   public abstract int dangerEvaluation();
   public abstract void kill(Species species);
   public abstract bool runAway();
   public abstract void other();


}