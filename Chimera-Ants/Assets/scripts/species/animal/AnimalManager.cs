//This class manages and represent diferent behaviour of animals


interface AnimalManager{
    void groupBehaviour();
    void familyBehaviour();
    void stateBehaviour();

    void DangerEvaluation(Species species);
    void Attack(Species species);
    bool RunAway(Animal animal);
    void other();
}