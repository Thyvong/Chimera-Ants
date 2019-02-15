//This class manages and represent diferent behaviour of animals


interface AnimalManager{
    void groupBehaviour();
    void familyBehaviour();
    void stateBehaviour();
    void dangerEvaluation(Species species);
    void kill(Species species);
    bool runAway(Animal animal);
    void other();
}