using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Health")]
    [SerializeField]
    private float defaultHealth = 0;
    private float currentHealth = 0;
    [SerializeField]
    private float maxHealth = 0;

    [Header("Fire Rate")]
    [SerializeField]
    private float defaultFireRate = 0;
    private float currentFireRate = 0;
    [SerializeField]
    private float maxFireRate = 0;

    [Header("Speed")]
    [SerializeField]
    private float defaultSpeed = 0;
    private float currentSpeed = 0;
    [SerializeField]
    private float maxSpeedValue = 0;
    [SerializeField]
    private float maxSpeed = 0;

    [Header("Ammo")]
    [SerializeField]
    private int defaultAmmo = 0;
    private int currentAmmo = 0;
    [SerializeField]
    private int maxAmmo = 0;

    [Header("Points")]
    [SerializeField]
    private int points = 0;
    [SerializeField]
    private int upgradeCost = 0;

    private const int MIN_VALUE = 1;

    private int totalCost = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        ResetValues();
    }

    public void ResetValues()
    {
        SetHealth(defaultHealth);
        SetFireRate(defaultFireRate);
        SetSpeed(defaultSpeed);
        SetAmmo(defaultAmmo);
        totalCost = 0;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetFireRate()
    {
        return currentFireRate;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public bool SetHealth(float health)
    {
        bool canChange = true;
        if (health < currentHealth)
        {
            if (health == MIN_VALUE)
            {
                canChange = false;
            }
        }
        else
        {
            if (health == maxHealth)
            {
                canChange = false;
            }
        }
        currentHealth = health;
        return canChange;
    }

    public bool SetFireRate(float fireRate)
    {
        bool canChange = true;
        if (fireRate < currentFireRate)
        {
            if (fireRate == MIN_VALUE)
            {
                canChange = false;
            }
        }
        else
        {
            if (fireRate == maxFireRate)
            {
                canChange = false;
            }
        }
        currentFireRate = fireRate;
        return canChange;
    }

    public bool SetSpeed(float speed)
    {
        bool canChange = true;
        if (speed < currentSpeed)
        {
            if (speed == MIN_VALUE)
            {
                canChange = false;
            }
        }
        else
        {
            if (speed == maxSpeedValue)
            {
                canChange = false;
            }
        }
        currentSpeed = speed;
        return canChange;
    }

    public bool SetAmmo(int ammo)
    {
        bool canChange = true;
        if (ammo < currentAmmo)
        {
            if (ammo == MIN_VALUE)
            {
                canChange = false;
            }
        }
        else
        {
            if (ammo == maxAmmo)
            {
                canChange = false;
            }
        }
        currentAmmo = ammo;
        return canChange;
    }

    public int AdjustPoints(int dir)
    {
        points += dir * upgradeCost;
        totalCost -= dir * upgradeCost;
        return points;
    }

    public int GetPoints()
    {
        return points;
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }

    public float GetDefaultHealth()
    {
        return defaultHealth;
    }

    public float GetDefaultFireRate()
    {
        return defaultFireRate;
    }

    public float GetDefaultSpeed()
    {
        return defaultSpeed;
    }

    public float GetDefaultAmmo()
    {
        return defaultAmmo;
    }

    public void KillEnemy()
    {
        points++;
    }

    public int GetTotalCost()
    {
        return totalCost;
    }

    public void SameStats()
    {
        points -= totalCost;
    }

    public float GetWalkSpeed()
    {
        return (currentSpeed / maxSpeedValue) * maxSpeed;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
}
