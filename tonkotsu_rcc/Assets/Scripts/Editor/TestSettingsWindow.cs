using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestSettingsWindow : EditorWindow
{
    [MenuItem("Window/Test Settings")]
    static private void OpenWindow()
    {
        TestSettingsWindow window = (TestSettingsWindow)GetWindow(typeof(TestSettingsWindow), false, "Test Settings");
        window.minSize = new Vector2(175, 250);        
        window.Show();
    }

    private void OnGUI() 
    {
        //General Settings
        GUILayout.Label("General Settings:", EditorStyles.boldLabel);
        TestSettings.EnableSound = EditorGUILayout.Toggle("Enable Sound", TestSettings.EnableSound);

        //Spawn Settings
        GUILayout.Label("Spawn Settings:", EditorStyles.boldLabel);
        TestSettings.SpawnPlayer = EditorGUILayout.Toggle("Spawn Player", TestSettings.SpawnPlayer);       
    }
}

public class TestSettings
{
    public static bool SpawnPlayer = true;
    public static bool EnableSound = true;
}