using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawndata;


    float timer;

    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.gametime / 10f), spawndata.Length - 1);

        timer += Time.deltaTime;

        if (timer > spawndata[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }
    void Spawn()
    {
        GameObject Enemy = GameManager.Instance.pool.Get(0);
        Enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        Enemy.GetComponent<Enermy>().Init(spawndata[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}