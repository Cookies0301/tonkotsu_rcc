using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEditorNodes : ScriptableObject
{
    public Rect windowRect;
    public string windowTitle;

    public virtual void DrawWindow()
    {

    }

    public virtual void DrawCurve()
    {

    }
}