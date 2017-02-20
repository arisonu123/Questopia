using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour {
#pragma warning disable 649
    [SerializeField]
    private enemy[] enemies;
#pragma warning restore 649
    [SerializeField]
    private float delay=2;
    [SerializeField]
    private int maxSpawnedEnemies=5;

    private int currentSpawnedEnemies = 0;

    private void Start()
    {
        InvokeRepeating("spawnEnemy", 0, delay);
    }

    private void spawnEnemy()
    {
        if (currentSpawnedEnemies >= maxSpawnedEnemies||GameObject.Find("Main Camera")==null)
        {
            return;
        }
        else
        {
            var spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position,transform.rotation)as enemy;
            spawnedEnemy.transform.parent = gameObject.transform;
            currentSpawnedEnemies += 1;
            spawnedEnemy.healthScript.eventsList.onDie.AddListener(handleEnemyDied);
        }
    }

    private void handleEnemyDied()
    {
        currentSpawnedEnemies--;
    }
}
