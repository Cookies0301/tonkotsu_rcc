public class Inverter : BaseNodes
{
    private BaseNodes node;
    public BaseNodes Node
    {
        get { return node; }
    }

    public Inverter(BaseNodes node)
    {
        this.node = node;
    }

    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.FAILURE:
                {
                    currentNodeState = NodeState.SUCCESS;
                    return currentNodeState;
                }
            case NodeState.SUCCESS:
                {
                    currentNodeState = NodeState.FAILURE;
                    return currentNodeState;
                }
            case NodeState.RUNNING:
                {
                    currentNodeState = NodeState.RUNNING;
                    return currentNodeState;
                }
        }
        currentNodeState = NodeState.SUCCESS;
        return currentNodeState;
    }
}