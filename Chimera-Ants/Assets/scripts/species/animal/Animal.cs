//This class represent all kind of animals and their behaviour

/* using Spieces;
using GroupStyle;
using NutritionStyle;
using DietaryRegime;
using AnimalManager;*/

public abstract class Animal : Species, AnimalManager{
   protected NutritionStyle[] nutritionStyle;
   protected GroupStyle[] groupStyle;
   protected DietaryRegime dietaryRegime;
   protected Sex sex;
   protected State state;
   protected int dangerLvl;

   //private static int animalBoidIdReference = 0;
   protected int animalBoidId;

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

   public void dangerEvaluation(Species species){

      if(species.GetType() == typeof(Animal)){
			Animal animal = (Animal) species;
			if(animal.getAnimalBoidId() != this.getAnimalBoidId()){
				increaseDangerLvl(1);
			}
			if(animal.getDietaryRegime() != this.getDietaryRegime()){
				increaseDangerLvl(2);
			}
			if(animal.getState() == State.Leader){
				increaseDangerLvl(2);
			}
      }
   }
   public int getDangerLvl(){
      return dangerLvl;
   }

   public void increaseDangerLvl(int nbLvl){
      this.dangerLvl = dangerLvl + nbLvl;
   }

   public void resetDangerLvl(){
      this.dangerLvl = 0;
   }

   public abstract void groupBehaviour();
   public abstract void familyBehaviour();
   public abstract void stateBehaviour();
   
   public abstract void kill(Species species);
   public abstract bool runAway(Animal animal);
   public abstract void other();


}