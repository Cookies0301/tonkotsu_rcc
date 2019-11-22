using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : Singleton<EnemyHandler>
{
    private int currentEnemyCount = 0;

    public int CurrentEnemyCount { get => currentEnemyCount; set => currentEnemyCount = value; }

    protected override void Awake() 
    {
        base.Awake();

        Enemy.onEnemySpawn += AddEnemy;
        Enemy.onEnemyDeath += RemoveEnemy;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Enemy.onEnemySpawn -= AddEnemy;
        Enemy.onEnemyDeath -= RemoveEnemy;
    }

    private void AddEnemy()
    {
        currentEnemyCount++;
    }

    private void RemoveEnemy()
    {
        currentEnemyCount--;
    }

    private void OnGUI() 
    {
        #if UNITY_EDITOR

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 40;
        myStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 100, 20), "Enemy Count: " + currentEnemyCount,myStyle);

        #endif
    }
}
