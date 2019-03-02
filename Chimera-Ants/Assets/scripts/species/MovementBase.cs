using UnityEngine;
using System.Collections;


public abstract class MovementBase
{
    public abstract void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration);
}

public class Fly : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration);
    }
}
public class Dash : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }
}
public class Walk : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration);
    }
}
public class Swim : MovementBase
{
    public override void Apply(Rigidbody rigidbody, Vector3 direction, float speed, float acceleration)
    {
        rigidbody.transform.LookAt(rigidbody.transform.position + direction);
        rigidbody.MovePosition(rigidbody.transform.position + direction * speed * acceleration);
    }
}