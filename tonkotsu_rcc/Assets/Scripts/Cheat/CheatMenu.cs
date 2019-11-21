using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Reflection;
using System.Linq;

//////////////////////
/// Singleton, visualizes cheats, and methods marked with [CheatAtribute] 
//////////////////////

public class CheatMenu : Singleton<CheatMenu>
{
    [SerializeField] VirtualController virtualController;
    [SerializeField] Texture2D[] virtualControllerTextures;

    List<MethodInfo> methodsList;
    List<Component> componentListToMethod;

    bool cheatMenuOpen;
    bool drawCheatButtons;
    bool drawController;

    protected override void Awake()
    {
        base.Awake();

        methodsList = new List<MethodInfo>();
        componentListToMethod = new List<Component>();

        if(virtualController == null)
        {
            virtualController = gameObject.AddComponent<VirtualController>();
            virtualController.ChangeInputType(VirtualControllerInputType.Player);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            cheatMenuOpen = !cheatMenuOpen;

            if(cheatMenuOpen)
            {
                FindCheats();
            }
        }
    }

    private void FindCheats()
    {
        //Find all objects in scene
        var objects = GameObject.FindObjectsOfType((typeof(GameObject)));

        Debug.Log("Objects: " + objects.Length);

        GameObject[] gameObjects = new GameObject[objects.Length];
        Debug.Log("GameObjects: " + gameObjects.Length);

        //convert Objects to GameObjects
        int index = 0;
        foreach (Object specificObject in objects)
        {
            gameObjects[index] = specificObject as GameObject;
            index++;
        }
        index = 0;

        //Get all their Components
        List<Component> components = new List<Component>();
        if (gameObjects != null)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                var foundComponents = gameObjects[i].GetComponents<Component>();

                foreach (Component component in foundComponents)
                {
                    components.Add(component);
                }
            }

            Debug.Log("Components: " + components.Count);

            for (int i = 0; i < components.Count; i++)
            {
                SaveAllCheatMethods(components[i]);
            }
        }
    }

    private void SaveAllCheatMethods(Component component)
    {
        MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.GetCustomAttribute<CheatMethodAttribute>() != null).ToArray();

        foreach (var method in methods)
        {
            if (!methodsList.Contains(method))
            {
                methodsList.Add(method);
                componentListToMethod.Add(component);
            }
        }
    }

    private void OnGUI()
    {
        if (cheatMenuOpen)
        {
            OpenCheats();
        }

        if(drawCheatButtons)
        {
            DrawCheatButtons();
        }

        if (drawController)
        {
            virtualController.DrawGUI(virtualControllerTextures);
        }
    }

    private void OpenCheats()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Virtual Controller"))
        {
            drawController = !drawController;
        }

        if (GUI.Button(new Rect(10, 70, 150, 50), "Visualize Beat"))
        {
            BeatHandler.BeatVisualize();
        }

        if (GUI.Button(new Rect(10, 130, 150, 50), "Show Cheats"))
        {
            drawCheatButtons = !drawCheatButtons;
        }

    }

    private void DrawCheatButtons()
    {
        int offsetX = 0;
        int offsetY = 0;
        int buttonsInRow = Screen.height/50;
        int allowedButtons = buttonsInRow;

        for (int index = 0; index < methodsList.Count; index++)
        {
            string buttonName = componentListToMethod[index].gameObject.name + " " + methodsList[index].Name.ToString();
            if(GUI.Button(new Rect(200 + offsetX, 130 + offsetY, 200, 50), buttonName))
            {
                methodsList[index].Invoke(componentListToMethod[index], null);
            }
            offsetY += 60;

            if (index == allowedButtons)
            {
                offsetX += 200;
                offsetY = 0;
                allowedButtons += buttonsInRow;
            }
        }
    }
}