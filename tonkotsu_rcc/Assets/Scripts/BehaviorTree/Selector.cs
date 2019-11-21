using System.Collections.Generic;

public class Selector : BaseNodes
{
    private List<BaseNodes> nodes = new List<BaseNodes>();

    public Selector(List<BaseNodes> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (BaseNodes node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    {
                        continue;
                    }
                case NodeState.SUCCESS:
                    {
                        currentNodeState = NodeState.SUCCESS;
                        return currentNodeState;
                    }
                default:
                    {
                        continue;
                    }
            }
        }
        currentNodeState = NodeState.FAILURE;
        return currentNodeState;
    }
}