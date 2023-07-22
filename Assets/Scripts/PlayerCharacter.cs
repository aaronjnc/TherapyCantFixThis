using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : Singleton<PlayerCharacter>
{
    InputController controller;
    Rigidbody2D rb;
    private float speed;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float dampTime = .15f;
    [SerializeField]
    Camera cam;
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    GameObject bullet;
    private float bulletSpeed;
    private float speedMod = 1;
    private float accuracy = 0;
    private int maxAmmo;
    private int ammo;
    private float fireRate;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = true;
        maxHealth = GameManager.Instance.GetHealth();
        maxAmmo = GameManager.Instance.GetAmmo();
        speed = GameManager.Instance.GetSpeed();
        fireRate = GameManager.Instance.GetFireRate();
        bulletSpeed = speed;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        controller = new InputController();
        ammo = maxAmmo;
        controller.PlayerCore.Movement.performed += Move;
        controller.PlayerCore.Movement.canceled += StopMove;
        controller.PlayerCore.Movement.Enable();
        controller.PlayerCore.Attack.performed += Attack;
        controller.PlayerCore.Attack.Enable();
    }

    void Move(CallbackContext ctx)
    {
        Vector3 dir = ctx.ReadValue<Vector2>().normalized;
        rb.velocity = speed * speedMod * dir;
    }

    void StopMove(CallbackContext ctx)
    {
        rb.velocity = Vector3.zero;
    }

    void Attack(CallbackContext ctx)
    {
        if (ammo == 0)
        {
            return;
        }
        Vector3 mouseScreenPos = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = transform.position.z;
        Vector3 dir = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float finalAngle = UnityEngine.Random.Range(angle - accuracy, angle + accuracy);
        Projectile newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0,0, finalAngle)).GetComponent<Projectile>();
        if (newBullet)
        {
            Vector3 bulletVel = newBullet.transform.right * bulletSpeed + new Vector3(rb.velocity.x, rb.velocity.y, 0);
            newBullet.SetVelocity(bulletVel);
        }
        ammo--;
    }

    private void FixedUpdate()
    {
        Vector3 point = cam.WorldToViewportPoint(transform.position);
		Vector3 delta = transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        destination.z = cam.transform.position.z;
		cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, dampTime);
    }

    public void HitPlayer(EnemyManager.EnemyStruct enemyAttack)
    {
        health -= enemyAttack.damage;
        if (health <= 0)
        {
            controller.Disable();
            Destroy(gameObject);
        }
        switch (enemyAttack.enemyType)
        {
            case EnemyManager.EnemyType.Sadness:
                speedMod *= .5f;
                break;
            case EnemyManager.EnemyType.Anger:
                accuracy = 30;
                break;
            case EnemyManager.EnemyType.Happiness:
                cam.orthographicSize = 5;
                break;
            case EnemyManager.EnemyType.Fear:
                EnemyManager.Instance.SetFearful(true);
                break;
        }
    }

    public void AddAmmo()
    {
        ammo++;
    }
}
