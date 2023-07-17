using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    GameObject enemyPrefab;
    PlayerCharacter character;
    [SerializeField]
    private float MIN_DISTANCE = 5;
    [SerializeField]
    private float MAX_DISTANCE = 15;
    List<BaseEnemy> enemies = new List<BaseEnemy>();
    [SerializeField]
    List<EnemyStruct> enemyTypes = new List<EnemyStruct>();
    Dictionary<EnemyType, EnemyStruct> enemyMap = new Dictionary<EnemyType, EnemyStruct>();
    [SerializeField]
    private float MIN_SPAWN_TIME = 1;
    [SerializeField]
    private float MAX_SPAWN_TIME = 3;
    int spawns = 0;
    int enemiesSpawned = 1;
    private bool fearful;

    [Serializable]
    public struct EnemyStruct
    {
        public EnemyType enemyType;
        public float speed;
        public float damage;
        public Material material;
    } 

    public enum EnemyType
    {
        Sadness,
        Happiness,
        Fear,
        Anger
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (EnemyStruct enemy in enemyTypes)
        {
            enemyMap.Add(enemy.enemyType, enemy);
        }
    }

    private void Start()
    {
        character = PlayerCharacter.Instance;
        SpawnEnemy();
        StartCoroutine("SpawnWait");
    }

    void SpawnEnemy()
    {
        float distance = UnityEngine.Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        float angle = UnityEngine.Random.Range(0, 360);
        Vector3 pos = new Vector3(distance * Mathf.Sin(angle), distance * Mathf.Cos(angle), 0);
        GameObject newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        BaseEnemy enemyScript = newEnemy.GetComponent<BaseEnemy>();
        enemies.Add(enemyScript);
        enemyScript.SetInfo(character, enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]);
    }

    void SpawnFearEnemy()
    {
        float distance = UnityEngine.Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        float angle = UnityEngine.Random.Range(0, 360);
        Vector3 pos = new Vector3(distance * Mathf.Sin(angle), distance * Mathf.Cos(angle), 0);
        GameObject newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        BaseEnemy enemyScript = newEnemy.GetComponent<BaseEnemy>();
        enemies.Add(enemyScript);
        enemyScript.SetInfo(character, enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]);
        enemyScript.SetFearEnemy();
    }

    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME));
        for (int i = 0; i < enemiesSpawned; i++)
        {
            SpawnEnemy();
        }
        spawns += 1;
        if (spawns % 10 == 0)
        {
            MIN_SPAWN_TIME -= .1f;
        }
        if (spawns % 15 == 0)
        {
            enemiesSpawned++;
        }
        if (fearful)
        {
            SpawnFearEnemy();
        }
        StartCoroutine("SpawnWait");
    }

    public void SetFearful(bool isFearful)
    {
        fearful = isFearful;
    }
}
