using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public float baseSpawnInterval = 3f;

    private int waveNumber = 1;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            int enemyCount = Mathf.Clamp(3 + (waveNumber * 2), 3, 30); // Increase enemy count
            float spawnInterval = Mathf.Clamp(baseSpawnInterval - (waveNumber * 0.1f), 0.5f, baseSpawnInterval); // Faster spawns

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, waypoints[0].position, Quaternion.identity);
                enemy.GetComponent<Enemy>().waypoints = waypoints;

                // Introduce Elite Enemies (25% chance after Wave 5)
                if (waveNumber > 5 && Random.value < 0.25f)
                {
                    enemy.GetComponent<Enemy>().isElite = true;
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            waveNumber++;
            WaveManager.instance.NextWave();

            yield return new WaitForSeconds(5f); // Delay before next wave starts
        }
    }
}
