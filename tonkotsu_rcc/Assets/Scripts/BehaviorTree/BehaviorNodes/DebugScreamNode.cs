using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugScreamNode", menuName = "Behavior Tree/Debug Scream Node")]
public class DebugScreamNode : BaseNodes
{
    public override NodeState Evaluate()
    {
        Debug.Log("!SCREAM!");
        return NodeState.RUNNING;
    }
}