using UnityEngine;

public class State
{
    protected float stateTimer;

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void FixedUpdate()
    {
    }
}