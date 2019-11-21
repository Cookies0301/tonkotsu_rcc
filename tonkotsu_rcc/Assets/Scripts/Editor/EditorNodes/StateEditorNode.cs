using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StateEditorNode : BaseEditorNodes
{
    bool collapse;
    public BaseNodes currentState;

    public override void DrawWindow()
    {
        if (currentState == null)
        {
            EditorGUILayout.LabelField("Add state to modify: ");
        }
        else
        {
            if (!collapse)
            {
                windowRect.height = 300;
            }
            else
            {
                windowRect.height = 100;
            }
            collapse = EditorGUILayout.Toggle(" ", collapse);
        }

        currentState = (BaseNodes)EditorGUILayout.ObjectField(currentState, typeof(BaseNodes), false);
    }

    public override void DrawCurve()
    {

    }
}