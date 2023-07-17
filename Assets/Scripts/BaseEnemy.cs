using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseEnemy : MonoBehaviour
{
    private PlayerCharacter character;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float damage;
    private EnemyManager.EnemyStruct enemyInfo;

    public void SetInfo(PlayerCharacter character, EnemyManager.EnemyStruct enemyInfo)
    {
        this.character = character;
        this.enemyInfo = enemyInfo;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (character)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.transform.position, Time.deltaTime*speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter hitPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (hitPlayer)
        {
            hitPlayer.HitPlayer(enemyInfo);
            Destroy(gameObject);
        }
    }
}
