using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossesCreation
{
    public GameObject bossPrefab;
    public float timeToSpawn;
}
public class BossesSpawner : MonoBehaviour
{
    public BossesCreation[] bossesCreation;
    private GameObject boss;
    private GameManager gameManager;
    public bool bossEntry;
    private float timerModify = 0;
    private float deltaTime = 0.2f;
    private int currentIndex = 0;


    void Awake()
    {
        gameManager = GameManager.instance;
    }

   
    void Update()
    {

        if((gameManager.currentTime >= bossesCreation[currentIndex].timeToSpawn - deltaTime) && (gameManager.currentTime <= bossesCreation[currentIndex].timeToSpawn + deltaTime))
        {          
             bossEntry = true;
             SpawnBoss();
             currentIndex++;
        }

        CheckIndex(currentIndex);
    }

    void SpawnBoss()
    {
        if (bossEntry && !gameManager.isBossAlive)
        {
            boss = Instantiate(bossesCreation[currentIndex].bossPrefab) as GameObject;
            boss.transform.position = transform.position;
            gameManager.isBossAlive = true;
            bossEntry = false;                 
        }
    }
    
    void CheckIndex(int _bossesCreationIndex)
    {
        if (_bossesCreationIndex > bossesCreation.Length -1)
        {
            DisableObject();
        }
    }
    
    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
