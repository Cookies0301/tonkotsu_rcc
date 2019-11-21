using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheatMethod : MonoBehaviour
{
    [CheatMethod] 
    public void Cheat()
    {
        Debug.LogError("Cheater1!");
    }

    [CheatMethod] 
    public void Cheat2()
    {
        Debug.LogError("Cheater2!");
    }
}