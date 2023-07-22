using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TherapistScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text pointsText;

    [Header("Health")]
    [SerializeField]
    private Button removeHealth;
    [SerializeField]
    private Button addHealth;
    [SerializeField]
    private TMP_Text healthText;

    [Header("Fire Rate")]
    [SerializeField]
    private Button removeFireRate;
    [SerializeField]
    private Button addFireRate;
    [SerializeField]
    private TMP_Text fireRateText;

    [Header("Speed")]
    [SerializeField]
    private Button removeSpeed;
    [SerializeField]
    private Button addSpeed;
    [SerializeField]
    private TMP_Text speedText;

    [Header("Ammo")]
    [SerializeField]
    private Button removeAmmo;
    [SerializeField]
    private Button addAmmo;
    [SerializeField]
    private TMP_Text ammoText;

    GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
        healthText.text = gameManager.GetHealth() + "";
        fireRateText.text = gameManager.GetFireRate() + "";
        speedText.text = gameManager.GetSpeed() + "";
        ammoText.text = gameManager.GetAmmo() + "";
        pointsText.text = gameManager.GetPoints() + "";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    private void AdjustPoints(int val)
    {
        int points = gameManager.AdjustPoints(val);
        pointsText.text = points + "";
        if (points < gameManager.GetUpgradeCost() && val < 0)
        {
            addAmmo.interactable = false;
            addSpeed.interactable = false;
            addHealth.interactable = false;
            addFireRate.interactable = false;
        }
        else if (points >= gameManager.GetUpgradeCost() && val > 0)
        {
            addAmmo.interactable = true;
            addSpeed.interactable = true;
            addHealth.interactable = true;
            addFireRate.interactable = true;
        }
    }

    public void AdjustHealth(int val)
    {
        bool canChange = gameManager.SetHealth(gameManager.GetHealth() + val);
        healthText.text = gameManager.GetHealth() + "";
        if (!canChange)
        {
            if (val < 0)
            {
                removeHealth.interactable = false;
            }
            else
            {
                addHealth.interactable = true;
            }
        }
        else
        {
            removeHealth.interactable = true;
            addHealth.interactable = true;
        }
        AdjustPoints(-1 * val);
    }

    public void AdjustFireRate(int val)
    {
        bool canChange = gameManager.SetFireRate(gameManager.GetFireRate() + val);
        fireRateText.text = gameManager.GetFireRate() + "";
        if (!canChange)
        {
            if (val < 0)
            {
                removeFireRate.interactable = false;
            }
            else
            {
                addFireRate.interactable = true;
            }
        }
        else
        {
            removeFireRate.interactable = true;
            addFireRate.interactable = true;
        }
        AdjustPoints(-1 * val);
    }

    public void AdjustSpeed(int val)
    {
        bool canChange = gameManager.SetSpeed(gameManager.GetSpeed() + val);
        speedText.text = gameManager.GetSpeed() + "";
        if (!canChange)
        {
            if (val < 0)
            {
                removeSpeed.interactable = false;
            }
            else
            {
                addSpeed.interactable = true;
            }
        }
        else
        {
            removeSpeed.interactable = true;
            addSpeed.interactable = true;
        }
        AdjustPoints(-1 * val);
    }

    public void AdjustAmmo(int val)
    {
        bool canChange = gameManager.SetAmmo(gameManager.GetAmmo() + val);
        speedText.text = gameManager.GetAmmo() + "";
        if (!canChange)
        {
            if (val < 0)
            {
                removeAmmo.interactable = false;
            }
            else
            {
                addAmmo.interactable = true;
            }
        }
        else
        {
            removeAmmo.interactable = true;
            addAmmo.interactable = true;
        }
        AdjustPoints(-1 * val);
    }

    public void ResetHealth()
    {
        float defaultHealth = gameManager.GetDefaultHealth();
        if (gameManager.GetHealth() > defaultHealth)
        {
            while (gameManager.GetHealth() != defaultHealth && removeHealth.IsInteractable())
            {
                AdjustHealth(-1);
            }
        }
        else
        {
            while (gameManager.GetHealth() != defaultHealth && addHealth.IsInteractable())
            {
                AdjustHealth(1);
            }
        }
    }

    public void ResetFireRate()
    {
        float defaultFireRate = gameManager.GetDefaultFireRate();
        if (gameManager.GetFireRate() > defaultFireRate)
        {
            while (gameManager.GetFireRate() != defaultFireRate && removeFireRate.IsInteractable())
            {
                AdjustFireRate(-1);
            }
        }
        else
        {
            while (gameManager.GetFireRate() != defaultFireRate && addFireRate.IsInteractable())
            {
                AdjustFireRate(1);
            }
        }
    }

    public void ResetSpeed()
    {
        float defaultSpeed = gameManager.GetDefaultSpeed();
        if (gameManager.GetFireRate() > defaultSpeed)
        {
            while (gameManager.GetSpeed() != defaultSpeed && removeSpeed.IsInteractable())
            {
                AdjustSpeed(-1);
            }
        }
        else
        {
            while (gameManager.GetSpeed() != defaultSpeed && addSpeed.IsInteractable())
            {
                AdjustSpeed(1);
            }
        }
    }

    public void ResetAmmo()
    {
        float defaultAmmo = gameManager.GetDefaultAmmo();
        if (gameManager.GetFireRate() > defaultAmmo)
        {
            while (gameManager.GetAmmo() != defaultAmmo && removeAmmo.IsInteractable())
            {
                AdjustAmmo(-1);
            }
        }
        else
        {
            while (gameManager.GetAmmo() != defaultAmmo && addAmmo.IsInteractable())
            {
                AdjustAmmo(1);
            }
        }
    }
}
