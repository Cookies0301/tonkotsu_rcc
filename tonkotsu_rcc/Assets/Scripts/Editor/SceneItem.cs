using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneItem : Editor
{
    [MenuItem("Open Scene/Prototype_Alex")]
    public static void PrototypeAlex()
    {
        OpenScene("/Game/Prototype_Alex");
    }

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

    [MenuItem("Open Scene/Prototype_Niklas")]
    public static void PrototypeNiklas()
    {
        OpenScene("/Other/Prototype_Niklas");
    }

    [MenuItem("Open Scene/Prototype_Nikolas")]
    public static void PrototypeNikolas()
    {
        OpenScene("/Other/Prototype_Nikolas");
    }


    static void OpenScene(string name)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}
