using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BaseEnemy : MonoBehaviour
{
    private PlayerCharacter character;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float speed;
    private float damage;
    private EnemyManager.EnemyStruct enemyInfo;
    private bool fearEnemy;
    [SerializeField]
    private float maxDistance;
    public void SetInfo(PlayerCharacter character, EnemyManager.EnemyStruct enemyInfo)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.character = character;
        this.enemyInfo = enemyInfo;
        speed = enemyInfo.speed;
        damage = enemyInfo.damage;
        spriteRenderer.sprite = enemyInfo.enemySprite;
        animator.runtimeAnimatorController = enemyInfo.animatorController;
    }

    public void SetFearEnemy()
    {
        fearEnemy = true;
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r, c.g, c.b, 120);
    }

    public bool GetFearEnemy()
    {
        return fearEnemy;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (character)
        {
            if (Vector3.Distance(transform.position, character.transform.position) > maxDistance)
            {
                Destroy(gameObject);
            }
            transform.position = Vector3.MoveTowards(transform.position, character.transform.position, Time.deltaTime*speed);
            if (character.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter hitPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (hitPlayer)
        {
            if (!fearEnemy)
                hitPlayer.HitPlayer(enemyInfo);
            Destroy(gameObject);
        }
    }
}
