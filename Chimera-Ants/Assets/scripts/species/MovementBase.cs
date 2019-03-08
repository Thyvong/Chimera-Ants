using UnityEngine;
using System.Collections;


public abstract class MovementBase
{
    protected static float SCALE = 0.005f;
    public abstract void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration);
}

public class Fly : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration * SCALE);
    }
}
public class Jump : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        //rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        if (rigidbody.velocity != Vector3.zero) return;
        rigidbody.AddForce((direction +  Vector3.up) * speed * 10, ForceMode.Impulse);
    }
    
}
public class Dash : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        //rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        if (rigidbody.velocity != Vector3.zero) return;
        rigidbody.AddForce(direction * speed * 10, ForceMode.Impulse);
    }

}
public class Walk : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration * SCALE);
    }
}
public class Swim : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration * SCALE);
    }
}