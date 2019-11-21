using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

//////////////////////
/// Input wrapper used to get input in the project,  simulates a XBoxController input
/// Is on top of the Unity Input system and hides it's implementation
//////////////////////

public class VirtualController : MonoBehaviour
{
    [SerializeField] VirtualControllerInputType startingInputType;

    IInputProvider currentProvider;
    InputPackage currentPackage;

    [ReadOnly]
    [SerializeField] VirtualControllerInputType currentType;

    private void Start()
    {
        if(currentType == VirtualControllerInputType.None)
        {
            ChangeInputType(startingInputType);
        }
    }

    private void Update()
    {
        currentPackage = currentProvider.GetPackage();
    }

    public void ChangeInputType( VirtualControllerInputType newType)
    {
        currentType = newType;
        currentProvider = newType.GetProvider();
    }


    /////////////////////
    ///Main method to get input, has a running time of 1, returns an on update updated InputPackage
    ////////////////////
    public InputPackage GetPackage()
    {
        return currentPackage;
    }

    ////////////////////
    /// Should be called in OnGUI, renders the virtual controller to the screen
    ///////////////////
    public void DrawGUI(Texture2D[] controllerTextures)
    {
        float screenHeight = Screen.height;
        //-500
        GUI.DrawTexture(new Rect(35, screenHeight - 300, 455, 275), controllerTextures[0]);
        GUI.DrawTexture(new Rect(245,screenHeight - 280, 30, 30), controllerTextures[10]);

        if (currentPackage.LeftStickButton)
        {
            ShowAction(115, screenHeight - 250, 70, 70, controllerTextures[1]);
        }
        else
        {
            GUI.DrawTexture(new Rect(115, screenHeight - 250, 70, 70), controllerTextures[1]);
        }

        if (currentPackage.LeftStickMoved())
        {
            ShowAction(130 + (currentPackage.LeftStick.x * 15), screenHeight - 235 - (currentPackage.LeftStick.y * 15), 40, 40, controllerTextures[2]);
        }
        else
        {
            GUI.DrawTexture(new Rect(130, screenHeight - 235, 40, 40), controllerTextures[2]);
        }

        if (currentPackage.RightStickButton)
        {
            ShowAction(280, screenHeight - 190, 70, 70, controllerTextures[1]);
        }
        else
        {
            GUI.DrawTexture(new Rect(280, screenHeight - 190, 70, 70), controllerTextures[1]);
        }

        if (currentPackage.RightStickMoved())
        {
            ShowAction(295 + (currentPackage.RightStick.x * 15), screenHeight - 175 + (currentPackage.RightStick.y * 15), 40, 40, controllerTextures[2]);
        }
        else
        {
            GUI.DrawTexture(new Rect(295, screenHeight - 175, 40, 40), controllerTextures[2]);
        }

        if (currentPackage.DPadMoved())
        {
            if (currentPackage.DPad.x >= 0.1f)
            {
                ShowAction(170, screenHeight - 190, 70, 70, controllerTextures[6]);
            }
            if (currentPackage.DPad.y <= -0.1f)
            {
                ShowAction(170, screenHeight - 190, 70, 70, controllerTextures[5]);
            }
            if (currentPackage.DPad.y >= 0.1f)
            {
                ShowAction(170, screenHeight - 190, 70, 70, controllerTextures[7]);
            }
            if (currentPackage.DPad.y <= -0.1f)
            {
                ShowAction(170, screenHeight - 190, 70, 70, controllerTextures[4]);
            }
        }
        else
        {
            GUI.DrawTexture(new Rect(170, screenHeight - 190, 70, 70), controllerTextures[3]);
        }

        if (currentPackage.X)
        {
            ShowAction(335, screenHeight - 230, 30, 30, controllerTextures[8]);
        }
        else
        {
            GUI.DrawTexture(new Rect(335, screenHeight - 230, 30, 30), controllerTextures[8]);
        }

        if (currentPackage.A)
        {
            ShowAction(365, screenHeight - 260, 30, 30, controllerTextures[8]);
        }
        else
        {
            GUI.DrawTexture(new Rect(365, screenHeight - 260, 30, 30), controllerTextures[8]);
        }

        if (currentPackage.B)
        {
            ShowAction(395, screenHeight - 230, 30, 30, controllerTextures[8]);
        }
        else
        {
            GUI.DrawTexture(new Rect(395, screenHeight - 230, 30, 30), controllerTextures[8]);
        }

        if (currentPackage.Y)
        {
            ShowAction(365, screenHeight - 200, 30, 30, controllerTextures[8]);
        }
        else
        {
            GUI.DrawTexture(new Rect(365, screenHeight - 200, 30, 30), controllerTextures[8]);
        }

        if (currentPackage.Back)
        {
            ShowAction(220, screenHeight - 225, 20, 20, controllerTextures[9]);
        }
        else
        {
            GUI.DrawTexture(new Rect(220, screenHeight - 225, 20, 20), controllerTextures[9]);
        }

        if (currentPackage.Start)
        {
            ShowAction(280, screenHeight - 225, 20, 20, controllerTextures[9]);
        }
        else
        {
            GUI.DrawTexture(new Rect(280, screenHeight - 225, 20, 20), controllerTextures[9]);
        }

        if (currentPackage.LB)
        {
            ShowAction(110, screenHeight - 330, 75, 40, controllerTextures[11]);
        }
        else
        {
            GUI.DrawTexture(new Rect(110, screenHeight - 330, 75, 40), controllerTextures[11]);
        }

        if (currentPackage.RB)
        {
            ShowAction(335, screenHeight - 330, 75, 40, controllerTextures[12]);
        }
        else
        {
            GUI.DrawTexture(new Rect(335, screenHeight - 330, 75, 40), controllerTextures[12]);
        }

        if (currentPackage.LTPressed())
        {
            ShowAction(130, screenHeight - 390, 50, 65, controllerTextures[13]);
        }
        else
        {
            GUI.DrawTexture(new Rect(130, screenHeight - 390, 50, 65), controllerTextures[13]);
        }

        if (currentPackage.RTPressed())
        {
            ShowAction(340, screenHeight - 390, 50, 65, controllerTextures[14]);
        }
        else
        {
            GUI.DrawTexture(new Rect(340, screenHeight - 390, 50, 65), controllerTextures[14]);
        }
    }

    private void ShowAction(float leftBound, float upBound, float width, float hight, Texture2D image)
    {
        GUI.DrawTexture(new Rect(leftBound, upBound, width, hight), image, ScaleMode.ScaleToFit, true, 0, Color.red, 0, 0);
    }

}

public enum VirtualControllerInputType
{
    None,
    Player
}

public static class VirtualControllerInputTypeMethods
{
    public static IInputProvider GetProvider(this VirtualControllerInputType type)
    {
        switch (type)
        {
            case VirtualControllerInputType.Player:
                return new PlayerInput();

            default:
                return new EmptyInput();
        }
    }
}