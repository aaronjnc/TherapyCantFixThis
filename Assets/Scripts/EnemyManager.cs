using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    PlayerCharacter character;
    [SerializeField]
    private float MIN_DISTANCE = 5;
    [SerializeField]
    private float MAX_DISTANCE = 15;
    List<BaseEnemy> enemies = new List<BaseEnemy>();

    protected override void Awake()
    {
        base.Awake();
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        float distance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        float angle = Random.Range(0, 360);
        Vector3 pos = new Vector3(distance * Mathf.Sin(angle), distance * Mathf.Cos(angle), 0);
        GameObject newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        BaseEnemy enemyScript = newEnemy.GetComponent<BaseEnemy>();
        enemies.Add(enemyScript);
        enemyScript.SetPlayer(character);
    }
}
