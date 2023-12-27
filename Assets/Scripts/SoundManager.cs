using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] private AudioClip moneySound;
    private AudioSource moneySource;
    [SerializeField] private AudioClip notEnoughMoneySound;
    private AudioSource notEnoughMoneySource;
    [SerializeField] private AudioClip backgroundMusicSound;
    private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip buyWeaponBoxSound;
    private AudioSource buyWeaponBoxSource;
    [SerializeField] private AudioClip footstepSound;
    private AudioSource footstepSource;
    [SerializeField] private AudioClip pingSound;
    private AudioSource pingSource;
    [SerializeField] private AudioClip gunShotSound;
    private AudioSource gunShotSource;
    [SerializeField] private AudioClip loadGunSound;
    private AudioSource loadGunSource;
    [SerializeField] private AudioClip risingWaterSound;
    private AudioSource risingWaterSource;
    [SerializeField] private AudioClip roundEndSound;
    private AudioSource roundEndSource;
    [SerializeField] private AudioClip gameOverSound;
    private AudioSource gameOverSource;
    private bool hasStop = false;

    void Start()
    {
        moneySource = gameObject.AddComponent<AudioSource>();
        moneySource.clip = moneySound;
        notEnoughMoneySource = gameObject.AddComponent<AudioSource>();
        notEnoughMoneySource.clip = notEnoughMoneySound;
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusicSound;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = 0.05f;
        buyWeaponBoxSource = gameObject.AddComponent<AudioSource>();
        buyWeaponBoxSource.clip = buyWeaponBoxSound;
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepSound;
        footstepSource.loop = true;
        footstepSource.volume = 0.5f;
        pingSource = gameObject.AddComponent<AudioSource>();
        pingSource.clip = pingSound;
        gunShotSource = gameObject.AddComponent<AudioSource>();
        gunShotSource.clip = gunShotSound;
        gunShotSource.volume = 0.2f;
        loadGunSource = gameObject.AddComponent<AudioSource>();
        loadGunSource.clip = loadGunSound;
        loadGunSource.volume = 0.3f;
        risingWaterSource = gameObject.AddComponent<AudioSource>();
        risingWaterSource.clip = risingWaterSound;
        risingWaterSource.volume = 0.6f;
        roundEndSource = gameObject.AddComponent<AudioSource>();
        roundEndSource.clip = roundEndSound;
        roundEndSource.volume = 0.3f;
        gameOverSource = gameObject.AddComponent<AudioSource>();
        gameOverSource.clip = gameOverSound;
        gameOverSource.volume = 0.2f;

        StartBackgroundMusic();

        if (gameManager == null)
            gameManager = GameObject.FindWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManagerScript.IsPauseGame() || gameManagerScript.IsGameOver())
        {
            hasStop = true;
            PauseBackgroundMusic();
            StopFootstep();
        }
        else if (!gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver() && hasStop)
        {
            hasStop = false;
            StartBackgroundMusic();
        }
    }

    public void PlayMoney()
    {
        moneySource.Play();
    }

    public void PlayNotEnoughMoney()
    {
        notEnoughMoneySource.Play();
    }

    private void StartBackgroundMusic()
    {
        backgroundMusicSource.Play();
    }

    private void PauseBackgroundMusic()
    {
        backgroundMusicSource.Pause();
    }

    public void PlayBuyWeaponBox()
    {
        buyWeaponBoxSource.Play();
    }

    public void StartFootstep()
    {
        footstepSource.Play();
    }

    public void StopFootstep()
    {
        footstepSource.Pause();
    }

    public void PlayPing()
    {
        pingSource.Play();
    }

    public void PlayGunShot()
    {
        gunShotSource.Play();
    }

    public void PlayLoadGun()
    {
        loadGunSource.Play();
    }

    public void StartRisingWaterSound()
    {
        risingWaterSource.Play();
    }

    public void StopRisingWaterSound()
    {
        risingWaterSource.Stop();
    }

    public void PauseRisingWaterSound()
    {
        risingWaterSource.Pause();
    }

    public void PlayRoundEnd()
    {
        roundEndSource.Play();
    }

    public void PlayGameOver()
    {
        gameOverSource.Play();
    }
}
