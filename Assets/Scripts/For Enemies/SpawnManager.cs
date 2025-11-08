using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnDelay = 10f;

    private bool isSpawning = false;

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnEnemiesAtAllPoints), 0f, spawnDelay);
        }
    }

    private void SpawnEnemiesAtAllPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            int enemiesHere = Random.Range(0, 3);
            for (int i = 0; i < enemiesHere; i++)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
