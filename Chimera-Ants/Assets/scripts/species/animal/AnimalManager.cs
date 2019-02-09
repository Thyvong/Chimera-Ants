//This class manages and represent diferent behaviour of animals


interface AnimalManager{
    void groupBehaviour();
    void familyBehaviour();
    void stateBehaviour();
    int dangerEvaluation(Species species);
    void kill(Species species);
    bool runAway();
    void other();
}