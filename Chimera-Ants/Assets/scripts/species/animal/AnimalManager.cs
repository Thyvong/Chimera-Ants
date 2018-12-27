//This class manages and represent diferent behaviour of animals


interface AnimalManager{
    void groupBehaviour();
    void familyBehaviour();
    State stateBehaviour();
    int dangerEvaluation();
    void kill(Species species);
    bool runAway();
    void other();
}