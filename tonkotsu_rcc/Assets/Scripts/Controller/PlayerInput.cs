using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IInputProvider
{
    private InputPackage oldInput = new InputPackage();

    public InputPackage GetPackage()
    {
        InputPackage input = new InputPackage();

        input.LeftStick = GetVectorFromAxis("Horizontal", "Vertical");
        input.RightStick = GetVectorFromAxis("CameraHorizontal", "CameraVertical");
        input.DPad = GetVectorFromAxis("CrossHorizontal", "CrossVertical");

        input.LT = GetAxis("TriggerLeft");
        input.RT = GetAxis("TriggerRight");

        var newLB = GetButton("LeftBumper");
        input.LBClicked = newLB && !oldInput.LB;
        input.LB = newLB;

        var newRB = GetButton("RightBumper");
        input.RBClicked = newRB && !oldInput.RB;
        input.RB = newRB;

        var newA = GetButton("A");
        input.AClicked = newA && !oldInput.AClicked;
        input.A = newA;

        var newB = GetButton("B");
        input.BClicked = newB && !oldInput.BClicked;
        input.B = newB;

        var newX = GetButton("X");
        input.XClicked = newX && !oldInput.X;
        input.X = newX;

        var newY = GetButton("Y");
        input.YClicked = newY && !oldInput.Y;
        input.Y = newY;

        var newLSB = GetButton("MoveButton");
        input.LeftStickButtonClicked = newLSB && !oldInput.LeftStickButton;
        input.LeftStickButton = newLSB;

        var newRSB = GetButton("CameraButton");
        input.RightStickButtonClicked = newRSB && !oldInput.RightStickButton;
        input.RightStickButton = newRSB;

        var newStart = GetButton("Start");
        input.StartClicked = newStart && !oldInput.Start;
        input.Start = newStart;

        var newBack = GetButton("Select");
        input.BackClicked = newBack && !oldInput.Back;
        input.Back = newBack;

        oldInput = input;
        return input;
    }

    private Vector2 GetVectorFromAxis(string a1, string a2)
    {
        return new Vector2(GetAxis(a1), GetAxis(a2));
    }

    private float GetAxis(string inputStr)
    {
        return Input.GetAxisRaw(inputStr);
    }

    private bool GetButton(string inputStr)
    {
        return Input.GetButton(inputStr);
    }
}