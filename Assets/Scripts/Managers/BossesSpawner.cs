using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossesCreation
{
    public GameObject bossPrefab;
    public bool bossEntry;
}
public class BossesSpawner : MonoBehaviour
{
    public BossesCreation bossesCreation;
    private GameObject boss;


    void Awake()
    {
        bossesCreation.bossEntry = false;
        GameManager.instance.isBossEntry = bossesCreation.bossEntry;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(bossesCreation.bossEntry)
        {
            boss = Instantiate(bossesCreation.bossPrefab) as GameObject;
            boss.transform.position = transform.position;
            bossesCreation.bossEntry = false;
            GameManager.instance.isBossAlive = true;
        }
	}
}
