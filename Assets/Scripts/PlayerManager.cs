using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HudManagerGame;
using static PlayerShoot;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject hudManager;
    private HudManagerGame hudManagerScript;
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] private GameObject playerGun;
    [SerializeField] private Mesh[] playerGuns;
    private MeshFilter meshFilterPlayerGun;
    [SerializeField] public int money = 500;
    [SerializeField] private int health;
    [SerializeField] private int MAX_HEALTH = 100;
    [SerializeField] private int MONEY_LOST_TOUCH_WATER = 20;
    [SerializeField] private int DAMAGE_TOUCH_WATER = 10;
    [SerializeField] private float TIME_TOUCH_WATER = 0.2f;
    private float timerPlayerOnWater = 0;
    private bool isGunUpgraded = false;

    private GameObject elementToBuy;
    private HudElements actualElement = HudElements.Nothing;
    private float coolDownAction = 2f;
    private float coolDownZombieHit = 1f;

    [SerializeField] private Material gunMaterial;

    public enum Guns
    {
        Pistol1,
        Pistol2,
        AssultRifle1,
        AssultRifle2,
        ShotGun1,
        Smg1,
        Sniper1
    }

    void Start()
    {
        meshFilterPlayerGun = playerGun.GetComponent<MeshFilter>();
        ChangeGun(playerGuns[0]);
        health = MAX_HEALTH;
        if (hudManager != null)
            hudManagerScript = hudManager.GetComponent<HudManagerGame>();
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        timerPlayerOnWater += Time.deltaTime;
        if (!gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver()) 
        {
            coolDownZombieHit -= Time.deltaTime;
            coolDownAction -= Time.deltaTime;
            if (coolDownZombieHit < -7f && health < MAX_HEALTH)
                Heal();
            if (Input.GetKeyDown(KeyCode.E) && actualElement != HudElements.Nothing && coolDownAction <= 0)
            {
                coolDownAction = 2f;
                ElementsManager elementsManagerScript = elementToBuy.GetComponent<ElementsManager>();
                if (elementsManagerScript.price <= money)
                {
                    money -= elementsManagerScript.price;
                    hudManagerScript.ShowMoney(money);
                    soundManagerScript.PlayMoney();
                    elementsManagerScript.MainAction();
                }
                else
                {
                    hudManagerScript.ActivateElement(HudElements.NotEnoughFunds);
                    soundManagerScript.PlayNotEnoughMoney();
                }
            }
        }
    }

    private void Heal() 
    {
        health = MAX_HEALTH;
        hudManagerScript.ShowBloodScreen(health);
    }

    private void Damage(int damage)
    {
        health -= damage;
        coolDownZombieHit = 1f;
        if (health < 0 && !gameManagerScript.IsGameOver())
        {
            soundManagerScript.PlayGameOver();
            gameManagerScript.GameOver();
            health = 0;
        }
        hudManagerScript.ShowBloodScreen(health);
    }

    public void ChangeGun(Mesh mesh)
    {
        meshFilterPlayerGun.mesh = mesh;
        GetComponent<PlayerShoot>().SetCurrentGun(FindTheCurrentGun());
        isGunUpgraded = false;
        ChangeMatGun(gunMaterial);
    }

    private Guns FindTheCurrentGun()
    {
        Guns currentGun = Guns.Pistol1;
        String gunName = meshFilterPlayerGun.mesh.name.Replace("Instance", "").Replace(" ", "");
        if (gunName == "Pistol2")
            currentGun = Guns.Pistol2;
        else if (gunName == "AssultRifle2")
            currentGun = Guns.AssultRifle1;
        else if (gunName == "AssultRifle3")
            currentGun = Guns.AssultRifle2;
        else if (gunName == "Body.002")
            currentGun = Guns.ShotGun1;
        else if (gunName == "Smg2")
            currentGun = Guns.Smg1;
        else if (gunName == "Sniper")
            currentGun = Guns.Sniper1;

        return currentGun;
    }

    public Mesh GetRandomGunMesh()
    {
        System.Random random = new System.Random();
        isGunUpgraded = false;
        ChangeMatGun(gunMaterial);
        return playerGuns[random.Next(0, 7)];
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int moneyToAdd)
    {
        if(money + moneyToAdd > 0)
            money += moneyToAdd;
        else
            money = 0;
        StartCoroutine(SlowlyShowMoney(moneyToAdd));
    }

    private IEnumerator SlowlyShowMoney(int moneyToAdd)
    {
        while (moneyToAdd >= 0)
        {
            moneyToAdd -= 10;
            hudManagerScript.ShowMoney(money - moneyToAdd);
            yield return new WaitForSeconds(0.01f);
        }
        hudManagerScript.ShowMoney(money);
    }

    public void UpgradeGun(Material mat)
    {
        isGunUpgraded = true;
        ChangeMatGun(mat);
    }
    public bool IsUpgradeGun()
    {
        return isGunUpgraded;
    }

    private void ChangeMatGun(Material mat)
    {
        MeshRenderer meshRenderer = playerGun.GetComponent<MeshRenderer>();
        meshRenderer.material = mat;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            ElementsManager elementsManagerScript = elementToBuy.GetComponent<DoorManager>();
            hudManagerScript.ActivateElement(HudElements.TextDoor, elementsManagerScript.price);
        } 
        else if (other.CompareTag("WeaponBox"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            ElementsManager elementsManagerScript = elementToBuy.GetComponent<WeaponBoxManager>();
            hudManagerScript.ActivateElement(HudElements.TextWeaponBox, elementsManagerScript.price);
        }
        else if (other.CompareTag("AmmoBox"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            ElementsManager elementsManagerScript = elementToBuy.GetComponent<AmmoBoxManager>();
            hudManagerScript.ActivateElement(HudElements.TextAmmoBox, elementsManagerScript.price);
        }
        else if (other.CompareTag("WallGun"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            ElementsManager elementsManagerScript = elementToBuy.GetComponent<WallGunManager>();
            hudManagerScript.ActivateElement(HudElements.TextWallGun, elementsManagerScript.price);
        }
        else if (other.CompareTag("WoodenWindow"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            hudManagerScript.ActivateElement(HudElements.TextWoodenWindow);
        }
        else if (other.CompareTag("UpgradingTable"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            ElementsManager elementsManagerScript = elementToBuy.GetComponent<UpgradingTableManager>();
            hudManagerScript.ActivateElement(HudElements.TextUpgradingTable, elementsManagerScript.price);
        }
        else if (other.CompareTag("ATM"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            hudManagerScript.ActivateElement(HudElements.TextATM);
        }
        else if (other.CompareTag("Perk"))
        {
            elementToBuy = other.gameObject;
            actualElement = HudElements.TextDoor;
            PerkManager elementsManagerScript = elementToBuy.GetComponent<PerkManager>();
            hudManagerScript.ActivateElement(HudElements.TextPerk, elementsManagerScript.price, elementsManagerScript.GetPerkName());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door") || other.CompareTag("WeaponBox") || other.CompareTag("AmmoBox") || other.CompareTag("WallGun") || other.CompareTag("WoodenWindow") || other.CompareTag("UpgradingTable") || other.CompareTag("ATM") || other.CompareTag("Perk"))
        {
            hudManagerScript.DesactivateElement();
            actualElement = HudElements.Nothing;
            elementToBuy = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie") && coolDownZombieHit <= 0)
        {
            Damage(40);
        }
        if (other.CompareTag("Water") && timerPlayerOnWater >= TIME_TOUCH_WATER)
        {
            timerPlayerOnWater = 0;
            AddMoney(-1 * MONEY_LOST_TOUCH_WATER);
            Damage(DAMAGE_TOUCH_WATER);
        }
    }
}
