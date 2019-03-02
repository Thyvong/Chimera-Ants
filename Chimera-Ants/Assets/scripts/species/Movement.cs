using UnityEngine;
using System.Collections.Generic;
using System.Linq;


/* Base class for Movement Behaviour */
public abstract class Movement
{
    public Vector3 direction { get; protected set; }
    public float speed { get; protected set; }
    public float maxSpeed { get; protected set; }
    public float acceleration { get; protected set; }
    public float maxAcceleration { get; protected set; }

    public Dictionary<string,MovementBase> knownMode { get; protected set; }
    protected MovementBase currentMode;

    private Rigidbody _rb;

    public Movement(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel)
    {
        direction = dir;
        speed = 0;
        acceleration = 0;
        maxSpeed = maxvitesse;
        maxAcceleration = maxaccel;
        _rb = rigidbody;

        knownMode = new Dictionary<string, MovementBase>();
        currentMode = null;
    }
    public Movement(Rigidbody rigidbody) : this(rigidbody, new Vector3(0, 0, 0), 1, 1) { }

    // le choix de mode à utiliser n'est pas la responsabilité de cette classe
    // " la species est le pilote, cette classe est la voiture "
    public MovementBase ChangeMode(string mode)
    {
        if ( knownMode.ContainsKey(mode) ) currentMode = knownMode[mode]; // s'il ne contient pas mode, la ligne n'est pas exécutée
        return currentMode;
    }

    public void Apply()
    {
        currentMode.Apply(_rb,direction, speed, acceleration);
    }
}

/*----------------------------------
    Movement spécifique
  ----------------------------------   
*/
public class ChimeraAntMove : Movement
{
    public ChimeraAntMove(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel) : base(rigidbody, dir, maxvitesse, maxaccel)
    {
        knownMode.Add("Walk", new Walk());
        currentMode = knownMode["Walk"];
    }
    public ChimeraAntMove(Rigidbody rigidbody) : this(rigidbody, new Vector3(0, 0, 0), 1, 0.5f) { }


    // Permet de gagner des mouvements et d'améliorer sa vitesse
    public void AddMovement(Movement move)
    {
        System.Random rng = new System.Random();
        List<string> keys = Enumerable.ToList(move.knownMode.Keys);
        int choice = rng.Next(keys.Count);

        string name = keys[choice];

        if (!knownMode.ContainsKey(name)) knownMode.Add(name, move.knownMode[name]);

        maxSpeed += 0.1f * move.maxSpeed;
        maxAcceleration += 0.1f * move.maxAcceleration;
    }

}




public class RabbitMove : Movement
{
    public RabbitMove(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel) : base( rigidbody,  dir, maxvitesse,  maxaccel)
    {
        knownMode.Add("Dash",new Dash());
        knownMode.Add("Walk",new Walk());
        currentMode = knownMode["Walk"];
    }
    public RabbitMove(Rigidbody rigidbody) : this(rigidbody, new Vector3(0, 0, 0), 5, 3) { }

}

