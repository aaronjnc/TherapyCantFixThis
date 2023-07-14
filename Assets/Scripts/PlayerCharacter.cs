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
    [SerializeField]
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
    [SerializeField]
    private float bulletSpeed;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = true;
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        controller = new InputController();
        controller.PlayerCore.Movement.performed += Move;
        controller.PlayerCore.Movement.canceled += StopMove;
        controller.PlayerCore.Movement.Enable();
        controller.PlayerCore.Attack.performed += Attack;
        controller.PlayerCore.Attack.Enable();
    }

    void Move(CallbackContext ctx)
    {
        rb.velocity = ctx.ReadValue<Vector2>() * speed;
    }

    void StopMove(CallbackContext ctx)
    {
        rb.velocity = Vector3.zero;
    }

    void Attack(CallbackContext ctx)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Projectile newBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector3 dir = mouseWorldPos - transform.position;
        dir.z = 0;
        dir.Normalize();
        if (newBullet)
        {
            newBullet.SetVelocity(dir * bulletSpeed);
        }
    }

    private void FixedUpdate()
    {
        Vector3 point = cam.WorldToViewportPoint(transform.position);
		Vector3 delta = transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        destination.z = cam.transform.position.z;
		cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, dampTime);
    }

    public void HitPlayer(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
