using UnityEngine;


public abstract class BaseNodes : ScriptableObject
{
    protected NodeState currentNodeState;


    public NodeState nodeState
    {
        get { return currentNodeState; }
    }

    public BaseNodes() { }

    public abstract NodeState Evaluate();
}