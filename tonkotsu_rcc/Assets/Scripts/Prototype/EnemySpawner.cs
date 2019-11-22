using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnTickFrequency;
    [SerializeField] AnimationCurve spawnChanceOverTimePerTick;
    [SerializeField] bool on = true;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float range = 20;
    [SerializeField] int enemiesPerSpawn = 1;
    [SerializeField] bool enemiesHaveRandomOffset = false;
    [ShowIf("enemiesHaveRandomOffset")]
    [SerializeField] float offsetRange = 1f;
    
    [SerializeField] static int enemyMaxCount = 50;



    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {

            yield return new WaitForSeconds(spawnTickFrequency);

            if (!on)
            {
                continue;
            }

            if(Vector3.Distance(PlayerHandler.Player.position, transform.position) > range || EnemyHandler.Instance.CurrentEnemyCount > enemyMaxCount)
            {
                continue;
            }

            bool spawn = Random.value < spawnChanceOverTimePerTick.Evaluate(Time.time);

            if (spawn)
            {
                for (int i = 0; i < enemiesPerSpawn; i++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                    if(enemiesHaveRandomOffset)
                    {
                        enemy.transform.position += new Vector3(Random.Range(-offsetRange,offsetRange), 0f, Random.Range(-offsetRange,offsetRange));
                    }
                }              
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
