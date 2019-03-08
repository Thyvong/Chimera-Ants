using UnityEngine;
using System.Collections.Generic;
using System.Linq;


/* Base class for Movement Behaviour */
public abstract class Movement
{
    public Vector3 direction { get;  set; }
    public float speed { get; protected set; }
    public float maxSpeed { get; protected set; }
    public float acceleration { get; protected set; }
    public float stepAcceleration { get; protected set; }

    public Dictionary<string,MovementBase> knownMode { get; protected set; }
    protected MovementBase currentMode;

    private Rigidbody _rb;

    public Movement(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel)
    {
        direction = dir;
        speed = 0;
        acceleration = 0;
        maxSpeed = maxvitesse;
        stepAcceleration = maxaccel;
        _rb = rigidbody;

        knownMode = new Dictionary<string, MovementBase>();
        currentMode = null;
    }
    public Movement(Rigidbody rigidbody) : this(rigidbody, rigidbody.transform.forward, 1, 1) { }

    // le choix de mode à utiliser n'est pas la responsabilité de cette classe
    // " la species est le pilote, cette classe est la voiture "
    public MovementBase ChangeMode(string mode)
    {
        if ( knownMode.ContainsKey(mode) ) currentMode = knownMode[mode]; // s'il ne contient pas mode, la ligne n'est pas exécutée
        return currentMode;
    }

    public void Apply(Vector3 dir)
    {
        direction = dir;
        currentMode.Apply(_rb, dir, speed, acceleration);
               
        if (speed < maxSpeed)
            speed += maxSpeed * acceleration;
        else
            speed = maxSpeed;
        
        if(acceleration < 1)
            acceleration += stepAcceleration;
        else
            acceleration = 1;
    }
    public void Stop()
    {
        speed = acceleration = 0;
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
        knownMode.Add("Dash", new Dash());
        
        currentMode = knownMode["Walk"];
    }
    public ChimeraAntMove(Rigidbody rigidbody) : this(rigidbody, rigidbody.transform.forward, 2, 0.02f) { }


    // Permet de gagner des mouvements et d'améliorer sa vitesse
    public void AddMovement(Movement move)
    {
        System.Random rng = new System.Random();
        List<string> keys = Enumerable.ToList(move.knownMode.Keys);
        int choice = rng.Next(keys.Count);

        string name = keys[choice];

        if (!knownMode.ContainsKey(name)) knownMode.Add(name, move.knownMode[name]);

        maxSpeed += 0.1f * move.maxSpeed;
        stepAcceleration += 0.1f * move.stepAcceleration;
    }
    // only used by queen
    public void InheritMovement(ChimeraAntMove target)
    {
        target.stepAcceleration = stepAcceleration;
        target.maxSpeed = maxSpeed;
        target.knownMode = knownMode;
        target.currentMode = currentMode;
    }
}


public class DefaultMove : Movement
{
    public DefaultMove(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel) : base(rigidbody, dir, maxvitesse, maxaccel)
    {
        knownMode.Add("Walk", new Walk());
        currentMode = knownMode["Walk"];
    }
    public DefaultMove(Rigidbody rigidbody) : this(rigidbody, rigidbody.transform.forward, 1, 0.3f) { }

}

public class RabbitMove : Movement
{
    public RabbitMove(Rigidbody rigidbody, Vector3 dir, float maxvitesse, float maxaccel) : base( rigidbody,  dir, maxvitesse,  maxaccel)
    {
        knownMode.Add("Dash",new Dash());
        knownMode.Add("Jump", new Jump());
        knownMode.Add("Walk",new Walk());
        currentMode = knownMode["Walk"];
    }
    public RabbitMove(Rigidbody rigidbody) : this(rigidbody, rigidbody.transform.forward, 1, 0.3f) { }

}

