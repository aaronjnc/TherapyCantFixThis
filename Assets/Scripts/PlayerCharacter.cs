using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyEffects))]
public class PlayerCharacter : Singleton<PlayerCharacter>
{
    InputController controller;
    Rigidbody2D rb;
    EnemyEffects enemyEffects;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField]
    SpriteRenderer armRenderer;
    private float speed;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float dampTime = .15f;
    [SerializeField]
    Camera cam;
    private float health;
    private float maxHealth;
    [SerializeField]
    GameObject bullet;
    private float bulletSpeed;
    private float speedMod = 1;
    private float accuracy = 0;
    private int maxAmmo;
    private int ammo;
    private float fireRate;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TMP_Text ammoText;
    [SerializeField]
    private TMP_Text pointsText;
    [SerializeField]
    private GameObject deathScreen;
    [SerializeField]
    private GameObject playerHUD;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Button sameStats;
    [SerializeField]
    private Sprite defaultSprite;
    private bool bCanShoot = true;
    private float reloadTime = 0;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = true;
        maxHealth = GameManager.Instance.GetHealth();
        maxAmmo = GameManager.Instance.GetAmmo();
        speed = GameManager.Instance.GetWalkSpeed();
        fireRate = GameManager.Instance.GetFireRate();
        bulletSpeed = GameManager.Instance.GetMaxSpeed();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyEffects = GetComponent<EnemyEffects>();
        animator = GetComponent<Animator>();
        controller = new InputController();
        ammo = maxAmmo;
        animator.enabled = false;
        reloadTime = GameManager.Instance.GetReloadTime();
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        healthSlider.value = health;
        UpdateAmmo();
        UpdatePoints();
        controller.PlayerCore.Movement.performed += Move;
        controller.PlayerCore.Movement.canceled += StopMove;
        controller.PlayerCore.Movement.Enable();
        controller.PlayerCore.Attack.performed += Attack;
        controller.PlayerCore.Attack.Enable();
        controller.PlayerCore.Pause.performed += Pause;
        controller.PlayerCore.Pause.Enable();
    }

    void Pause(CallbackContext ctx)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    void Move(CallbackContext ctx)
    {
        Vector3 dir = ctx.ReadValue<Vector2>().normalized;
        animator.enabled = true;
        if (dir.x < 0)
        {
            spriteRenderer.flipX = true;
            armRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            armRenderer.flipX = false;
        }
        rb.velocity = speed * speedMod * dir;
    }

    void StopMove(CallbackContext ctx)
    {
        animator.enabled = false;
        spriteRenderer.sprite = defaultSprite;
        rb.velocity = Vector3.zero;
    }

    void Attack(CallbackContext ctx)
    {
        if (ammo == 0)
        {
            StartCoroutine("ReloadRate");
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
        UpdateAmmo();
    }

    IEnumerator ReloadRate()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        UpdateAmmo();
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
        healthSlider.value = health;
        if (health <= 0)
        {
            controller.Disable();
            Time.timeScale = 0;
            if (GameManager.Instance.GetPoints() < GameManager.Instance.GetTotalCost())
            {
                sameStats.interactable = false;
            }
            else
            {
                sameStats.interactable = true;
            }
            playerHUD.SetActive(false);
            deathScreen.SetActive(true);
            Destroy(spriteRenderer);
            Destroy(armRenderer);
        }
        enemyEffects.AddEffect(enemyAttack.enemyType);
    }

    public void AddAmmo()
    {
        ammo++;
        UpdateAmmo();
    }

    public void UpdateAmmo()
    {
        ammoText.text = "Ammo: " + ammo + "/" + maxAmmo;
    }

    public void UpdatePoints()
    {
        pointsText.text = "Points: " + GameManager.Instance.GetPoints();
    }

    public void Therapy()
    {
        GameManager.Instance.ResetValues();
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void RunItBack()
    {
        GameManager.Instance.SameStats();
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SetSpeedMod(float newSpeedMod)
    {
        speedMod = newSpeedMod;
    }

    public void SetOrthographicSize(float newOrthographicSize)
    {
        cam.orthographicSize = newOrthographicSize;
    }

    public void SetAccuracy(float newAccuracy)
    {
        accuracy = newAccuracy;
    }
}
