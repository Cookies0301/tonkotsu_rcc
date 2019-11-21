using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneItem : Editor
{
    [MenuItem("Open Scene/MainMenu")]

    public static void OpenMainMenu()
    {
        OpenScene("/Game/MainMenu");
    }

    [MenuItem("Open Scene/LucaCoding")]

    public static void LucaCoding()
    {
        OpenScene("/Other/LucaCoding");
    }

    static void OpenScene(string name)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}
