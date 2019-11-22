using UnityEngine;

[System.Serializable]
public struct InputPackage
{
    public Vector2 LeftStick;
    public Vector2 RightStick;
    public Vector2 DPad;

    public float LT;
    public float RT;

    public bool LB;
    public bool LBClicked;
    public bool RB;
    public bool RBClicked;

    public bool A;
    public bool AClicked;
    public bool B;
    public bool BClicked;
    public bool X;
    public bool XClicked;
    public bool Y;
    public bool YClicked;

    public bool LeftStickButton;
    public bool LeftStickButtonClicked;
    public bool RightStickButton;
    public bool RightStickButtonClicked;

    public bool Start;
    public bool StartClicked;
    public bool Back;
    public bool BackClicked;


    public static InputPackage Empty { get => new InputPackage(); }

    public bool LeftStickMoved()
    {
        return LeftStick.magnitude > 0.1f;
    }

    public bool RightStickMoved()
    {
        return RightStick.magnitude > 0.1f;
    }

    public bool DPadMoved()
    {
        return DPad.magnitude > 0.1f;
    }

    public bool LTPressed()
    {
        return LT > 0.3f;
    }

    public bool RTPressed()
    {
        return RT > 0.3f;
    }

}