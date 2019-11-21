using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviorEditor : EditorWindow
{
    #region Variables
    static List<BaseEditorNodes> windows = new List<BaseEditorNodes>();
    Vector2 mousePosition;
    bool makeTransition;
    bool clickedOnWindow;
    BaseEditorNodes selectedNode;

    public enum UserActions
    {
        AddNode,
        AddTransition,
        DeleteNode,
        AddCommentNode
    }
    #endregion

    #region Init
    [MenuItem("Behavior Editor/Editor")]
    static void ShowEditor()
    {
        BehaviorEditor editor = EditorWindow.GetWindow<BehaviorEditor>();
        editor.minSize = new Vector2(800, 600);
    }
    #endregion

    #region GUI Methods
    private void OnGUI()
    {
        Event events = Event.current;
        mousePosition = events.mousePosition;
        UserInput(events);
        DrawWindows();
        if(GUI.Button(new Rect(10, 10, 100, 20), "Clear Me"))
        {
            ClearMe();
        }

    }

    void DrawWindows()
    {
        BeginWindows();
        foreach (BaseEditorNodes node in windows)
        {
            node.DrawCurve();
        }
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }
        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    void UserInput(Event events)
    {
        if (events.button == 1 && !makeTransition)
        {
            if (events.type == EventType.MouseDown)
            {
                RightClick(events);
            }
        }

        if (events.button == 0 && !makeTransition)
        {
            if (events.type == EventType.MouseDown)
            {

            }
        }
    }

    void RightClick(Event events)
    {
        selectedNode = null;
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].windowRect.Contains(events.mousePosition))
            {
                clickedOnWindow = true;
                selectedNode = windows[i];
                break;
            }
        }
        if (!clickedOnWindow)
        {
            AddNewNode(events);
        }
        else
        {
            ModifyNode(events);
        }
        clickedOnWindow = false;
    }

    void AddNewNode(Event events)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Add State"), false, CallBack, UserActions.AddNode);
        menu.AddItem(new GUIContent("Add Comment"), false, CallBack, UserActions.AddCommentNode);
        menu.ShowAsContext();
        events.Use();
    }

    void ModifyNode(Event events)
    {
        GenericMenu menu = new GenericMenu();
        if (selectedNode is StateEditorNode)
        {
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Transition"), false, CallBack, UserActions.AddTransition);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Delete Node"), false, CallBack, UserActions.DeleteNode);
        }

        if(selectedNode is CommentEditorNode)
        {
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Delete Node"), false, CallBack, UserActions.DeleteNode);
        }
        menu.ShowAsContext();
        events.Use();
    }

    void CallBack(object objects)
    {
        UserActions actions = (UserActions)objects;
        switch (actions)
        {
            case UserActions.AddNode:
                StateEditorNode stateNode = ScriptableObject.CreateInstance<StateEditorNode>();
                
                stateNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                stateNode.windowTitle = "State";
               
                windows.Add(stateNode);
                break;
            case UserActions.AddTransition:
                break;
            case UserActions.DeleteNode:
                if(selectedNode != null)
                {
                    windows.Remove(selectedNode);
                }
                break;
            case UserActions.AddCommentNode:
                CommentEditorNode commentNode = ScriptableObject.CreateInstance<CommentEditorNode>();

                commentNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100);
                commentNode.windowTitle = "Comment";

                windows.Add(commentNode);
                break;
        }

        #endregion
    }

    public void ClearMe()
    {
        windows.Clear();
    }
}