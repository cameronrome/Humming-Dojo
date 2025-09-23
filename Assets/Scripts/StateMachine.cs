public class StateMachine<T> where T : State
{
    public T currState { get; private set; }
    public T prevState { get; private set; }

    public virtual void Init(T initState)
    {
        currState = initState;
        currState.Enter();
    }

    public virtual void Transition(T newState)
    {
        prevState = currState;
        currState.Exit();
        currState = newState;
        currState.Enter();
    }
}
