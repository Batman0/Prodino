using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public Transform[] enemySpawnPoints;
	public bool canSpawnHorde = true;
	public float enemySpawnDelay;
	public GameObject []enemyPrefab;
    public int numberOfEnemies;
    private int enemyType;
    private int circleEnemyIndex = 1;
    private int circleEnemySpawnPointIndex = 0;
    public int[] usedSpawnPoints;

    void Start()
    {
        usedSpawnPoints = new int[enemySpawnPoints.Length];
        GameObject enemy = Instantiate(enemyPrefab[circleEnemyIndex], enemySpawnPoints[circleEnemySpawnPointIndex].transform.position, enemyPrefab[enemyType].transform.rotation) as GameObject;
    }

    // Update is called once per frame
    void Update () {
		if (canSpawnHorde) {
			StartCoroutine (EnemySpawn ());
		}
	}

	IEnumerator EnemySpawn(){
		canSpawnHorde = false;
		yield return new WaitForSeconds (enemySpawnDelay);

		//Spawn dei nemici in ordine di spawn possibile cambiare per farlo essere random o deciso da script senza dover
		//cambiare gli spawnpoint in sceneview
		for (int i = 0; i < numberOfEnemies; i++) {
            do
            {
                enemyType = Random.Range(0, enemyPrefab.Length);
            }
            while (enemyType == circleEnemyIndex);
            int spawnIndex = Random.Range(0, enemySpawnPoints.Length);
            Vector3 spawnPoint = new Vector3(enemySpawnPoints[spawnIndex].transform.position.x + usedSpawnPoints[spawnIndex] * 3, enemySpawnPoints[spawnIndex].transform.position.y, enemySpawnPoints[spawnIndex].transform.position.z);
            GameObject enemy = Instantiate(enemyPrefab[enemyType], spawnPoint, enemyPrefab[enemyType].transform.rotation) as GameObject;
            usedSpawnPoints[spawnIndex]++;
        }
		canSpawnHorde = true;
		yield return null;
	}
}
