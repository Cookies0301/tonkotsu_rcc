using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentEditorNode : BaseEditorNodes
{
    string comment = "Here could stay your comment";

    public override void DrawWindow()
    {
        comment = GUILayout.TextArea(comment, 200);
    }
}