using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManager;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject hudManager;
    private HudManagerGame hudManagerScript;
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] private float bulletSpeed = 60f;//changer selon le gun
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 20;
    [SerializeField] private float bulletWaitingTime = 0.4f;//changer selon le gun
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private int ammo = 0;
    [SerializeField] private int maxAmmo = 12; //changer selon le gun
    [SerializeField] private int magasines = 0;
    [SerializeField] private int maxMagasines = 6; //changer selon le gun
    [SerializeField] private float bulletSize = 0.07f; //changer selon le gun
    [SerializeField] private int nbOfZombieHitWithOneBullet = 1; //changer selon le gun
    [SerializeField] private float bulletDamage = 20; //changer selon le gun
    [SerializeField] private int moneyPerHit = 5; //changer selon le gun
    [SerializeField] private float reloadWaitingTime = 1.5f;//changer selon le gun
    private float lastBulletShotTime = 0;
    private bool isSpeedFirePerkActive = false;
    [SerializeField] private float SPEED_BULLET_PERK_MULTIPLICATOR = 0.65f;
    private Guns actualGun;

    private ObjectPool bulletPool;

    void Start()
    {
        ammo = maxAmmo;
        magasines = maxMagasines;
        if (hudManager != null)
            hudManagerScript = hudManager.GetComponent<HudManagerGame>();
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        hudManagerScript.ShowAmmo(ammo);
        hudManagerScript.ShowMagazines(magasines);
        bulletPool = new ObjectPool(bulletPrefab, bulletPoolSize);
        lastBulletShotTime = bulletWaitingTime;
    }

    void Update()
    {
        HandlePlayerShoot();
    }

    public void SpeedFirePerkActivate()
    {
        bulletWaitingTime = bulletWaitingTime * SPEED_BULLET_PERK_MULTIPLICATOR;
        isSpeedFirePerkActive = true;
    }

    private void HandlePlayerShoot()
    {
        if (!gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver())
        {
            lastBulletShotTime -= Time.deltaTime;
            if (Input.GetMouseButton(0) && lastBulletShotTime < 0)
            {
                lastBulletShotTime = bulletWaitingTime;
                if (ammo > 0)
                {
                    ShootBullet();
                    ammo--;
                    hudManagerScript.ShowAmmo(ammo);
                    soundManagerScript.PlayGunShot();
                }
                else
                {
                    hudManagerScript.ShowReload();
                    soundManagerScript.PlayNotEnoughMoney();
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && ammo != maxAmmo)
            {
                lastBulletShotTime = reloadWaitingTime;
                if (magasines > 0)
                {
                    ammo = maxAmmo;
                    magasines--;
                    hudManagerScript.ShowAmmo(ammo);
                    hudManagerScript.ShowMagazines(magasines);
                    soundManagerScript.PlayLoadGun();
                }
                hudManagerScript.RemoveReload();
            }
        }
    }

    private void ShootBullet()
    {
        if (gameObject.GetComponent<PlayerManager>().IsUpgradeGun())
            GetAvaibleBulletManager().ShootBullet(bulletSpeed, bulletSpawn.transform, bulletSize, bulletDamage * 1.30f, nbOfZombieHitWithOneBullet + 1, moneyPerHit);
        else
            GetAvaibleBulletManager().ShootBullet(bulletSpeed, bulletSpawn.transform, bulletSize, bulletDamage, nbOfZombieHitWithOneBullet, moneyPerHit);
    }

    private BulletManager GetAvaibleBulletManager()
    {
        GameObject bullet = bulletPool.GetObjectFromPool(transform.position, transform.rotation);
        return bullet.GetComponent<BulletManager>();
    }

    public void MaxAmmo()
    {
        ammo = maxAmmo;
        magasines = maxMagasines;
        if (hudManagerScript == null)
            hudManagerScript = hudManager.GetComponent<HudManagerGame>();
        hudManagerScript.ShowAmmo(ammo);
        hudManagerScript.ShowMagazines(magasines);
    }

    public void SetCurrentGun(Guns gun)
    {
        SetStats(gun);
        if (isSpeedFirePerkActive)
        {
            bulletWaitingTime = bulletWaitingTime * SPEED_BULLET_PERK_MULTIPLICATOR;
        }
        MaxAmmo();
    }

    //Marche pas seulment en exécutable
    private void SetStats(Guns gun)
    {
        switch (gun)
        {
            case Guns.Pistol1:
                bulletSpeed = 60f;
                bulletWaitingTime = 0.4f;
                maxAmmo = 12;
                maxMagasines = 6;
                bulletSize = 0.08f;
                bulletDamage = 20;
                nbOfZombieHitWithOneBullet = 1;
                moneyPerHit = 10;
                reloadWaitingTime = 1.5f;
                break;
            case Guns.Pistol2:
                bulletSpeed = 45f;
                bulletWaitingTime = 0.6f;
                maxAmmo = 10;
                maxMagasines = 6;
                bulletSize = 0.12f;
                bulletDamage = 30;
                nbOfZombieHitWithOneBullet = 2;
                moneyPerHit = 14;
                reloadWaitingTime = 2f;
                break;
            case Guns.AssultRifle1:
                bulletSpeed = 80f;
                bulletWaitingTime = 0.1f;
                maxAmmo = 32;
                maxMagasines = 12;
                bulletSize = 0.04f;
                bulletDamage = 12;
                nbOfZombieHitWithOneBullet = 1;
                moneyPerHit = 4;
                reloadWaitingTime = 1.8f;
                break;
            case Guns.AssultRifle2:
                bulletSpeed = 75f;
                bulletWaitingTime = 0.18f;
                maxAmmo = 28;
                maxMagasines = 12;
                bulletSize = 0.05f;
                bulletDamage = 15;
                nbOfZombieHitWithOneBullet = 1;
                moneyPerHit = 6;
                reloadWaitingTime = 1.6f;
                break;
            case Guns.ShotGun1:
                bulletSpeed = 35f;
                bulletWaitingTime = 0.7f;
                maxAmmo = 2;
                maxMagasines = 14;
                bulletSize = 0.4f;
                bulletDamage = 120;
                nbOfZombieHitWithOneBullet = 4;
                moneyPerHit = 38;
                reloadWaitingTime = 4f;
                break;
            case Guns.Smg1:
                bulletSpeed = 80f;
                bulletWaitingTime = 0.1f;
                maxAmmo = 24;
                maxMagasines = 15;
                bulletSize = 0.04f;
                bulletDamage = 12;
                nbOfZombieHitWithOneBullet = 1;
                moneyPerHit = 4;
                reloadWaitingTime = 1f;
                break;
            case Guns.Sniper1:
                bulletSpeed = 85f;
                bulletWaitingTime = 1f;
                maxAmmo = 6;
                maxMagasines = 12;
                bulletSize = 0.05f;
                bulletDamage = 180;
                nbOfZombieHitWithOneBullet = 6;
                moneyPerHit = 32;
                reloadWaitingTime = 3f;
                break;
        }
    }
}