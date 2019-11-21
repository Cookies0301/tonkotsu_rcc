using System.Collections.Generic;

public class Sequence : BaseNodes
{
    private List<BaseNodes> nodes = new List<BaseNodes>();

    public Sequence(List<BaseNodes> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool childRunning = false;

        foreach (BaseNodes node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    {
                        currentNodeState = NodeState.FAILURE;
                        return currentNodeState;
                    }
                case NodeState.SUCCESS:
                    {
                        continue;
                    }
                case NodeState.RUNNING:
                    {
                        childRunning = true;
                        continue;
                    }
                default:
                    {
                        currentNodeState = NodeState.SUCCESS;
                        return currentNodeState;
                    }
            }
        }
        currentNodeState = childRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return currentNodeState;
    }
}