using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : PersistantSingleton<GameSettings,GameSettingsData>
{

}

[System.Serializable]
public class GameSettingsData
{
    public bool SpawnPlayer = false;
    public bool DisableSound = false;
}