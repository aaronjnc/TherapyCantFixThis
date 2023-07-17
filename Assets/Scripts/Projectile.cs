using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float liveTime;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        liveTime -= Time.deltaTime;
        if (liveTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (baseEnemy.GetFearEnemy())
        {
            return;
        }
        PlayerCharacter.Instance.AddAmmo();
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
