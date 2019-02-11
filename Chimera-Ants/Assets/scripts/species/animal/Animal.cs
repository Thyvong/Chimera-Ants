//This class represent all kind of animals and their behaviour

/* using Spieces;
using GroupStyle;
using NutritionStyle;
using DietaryRegime;
using AnimalManager;*/

public abstract class Animal : Species, AnimalManager{
   private NutritionStyle[] nutritionStyle;
   private GroupStyle[] groupStyle;
   private DietaryRegime dietaryRegime;
   private Sex sex;
   private State state;

   //private static int animalBoidIdReference = 0;
   private int animalBoidId;

   public int getAnimalBoidId(){
      return animalBoidId;
   }

   protected void setAnimalBoidId(int animalBoidId){
      this.animalBoidId = animalBoidId;
   }
    NutritionStyle[] getNutritionStyle(){
      return nutritionStyle;
   }

   protected void setNutritionStyle(NutritionStyle[] nutritionStyle){
      this.nutritionStyle = nutritionStyle;
   }

   public GroupStyle[] getGroupStyles(){
      return groupStyle;
   }

   protected void setGroupStyle(GroupStyle[] groupStyle){
      this.groupStyle = groupStyle;
   }

   public DietaryRegime getDietaryRegime(){
      return dietaryRegime;
   }

   protected void setDietaryRegime(DietaryRegime dietaryRegime){
      this.dietaryRegime = dietaryRegime;
   }

   public Sex getSex(){
      return sex;
   }

   protected void setSex(Sex sex){
      this.sex = sex;
   }

   public State getState(){
      return state;
   }

   protected void setState(State state){
      this.state = state;
   }

   public abstract void groupBehaviour();
   public abstract void familyBehaviour();
   public abstract void stateBehaviour();
   public abstract int dangerEvaluation(Species species);
   public abstract void kill(Species species);
   public abstract bool runAway();
   public abstract void other();


}