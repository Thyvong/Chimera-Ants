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

   	public abstract void groupBehaviour();
   	public abstract void familyBehaviour();
   	public abstract State stateBehaviour();
	public abstract int dangerEvaluation();
   	public abstract void kill(Species species);
   	public abstract bool runAway();
   	public abstract void other();

	/*protected Animal(){
		base.();
	}*/
}