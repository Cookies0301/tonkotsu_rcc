using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;

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
            else
            {
                drawCheatButtons = false;
            }
        }

        if (cheatMenuOpen && virtualController.GetPackage().RB)
        {
            BeatHandler.AddControllerMarker();
        }
    }

    private void FindCheats()
    {
        //Find all objects in scene
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        
        GameObject[] gameObjectsTemp = AddChildren(rootObjects);
        GameObject[] gameObjects = AddDontDestroyOnLoadObjects(gameObjectsTemp);
        

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

            for (int i = 0; i < components.Count; i++)
            {
                SaveAllCheatMethods(components[i]);
            }
        }
    }

    private GameObject[] AddChildren(GameObject[] rootObjects)
    {
        var foundObjects = new List<GameObject>();

        for(int i = 0; i < rootObjects.Length; i++)
        {
            foundObjects.Add(rootObjects[i]);

            AddDescendants(rootObjects[i], foundObjects);
        }

        return foundObjects.ToArray();
    }

    private void AddDescendants(GameObject gameObject,  List<GameObject> addTo)
    {
        foreach(Transform child in gameObject.transform)
        {
            addTo.Add(child.gameObject);
            AddDescendants(child.gameObject, addTo);
        }
    }

    private GameObject[] AddDontDestroyOnLoadObjects(GameObject[] gameObjects)
    {
        var foundObjects = new List<GameObject>();

        for(int i = 0; i < gameObjects.Length; i++)
        {
            foundObjects.Add(gameObjects[i]);
        }

        GameObject[] dontDestroyOnLoadRootObjects;
        GameObject temp = null;
        
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad( temp );
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate( temp );
            temp = null;
    
            dontDestroyOnLoadRootObjects = dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if( temp != null )
                Object.DestroyImmediate( temp );
        }

        var dontDestroyOnLoadObjects = AddChildren(dontDestroyOnLoadRootObjects);
        
        for(int i = 0; i < dontDestroyOnLoadObjects.Length; i++)
        {
            foundObjects.Add(dontDestroyOnLoadObjects[i]);
        }
        
        return foundObjects.ToArray();
    }


    private void SaveAllCheatMethods(Component component)
    {
        MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.GetCustomAttribute<CheatMethodAttribute>() != null).ToArray();

        foreach (var method in methods)
        {
            methodsList.Add(method);
            componentListToMethod.Add(component);
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