using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombieManager : MonoBehaviour
{
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    [SerializeField] private GameObject hudManager;
    private HudManagerGame hudManagerScript;
    [SerializeField] private GameObject WoodenWindowsSection1;
    [SerializeField] private GameObject WoodenWindowsSection2;
    [SerializeField] private GameObject WoodenWindowsSection3;
    [SerializeField] private GameObject WoodenWindowsSection4;
    private bool isSection2Open = false;
    private bool isSection3Open = false;
    private bool isSection4Open = false;
    [SerializeField] private GameObject player;
    [SerializeField] private float spawnRate = 14f; //changer chaque round
    [SerializeField] private int nbOfZombieSpawned = 0; 
    [SerializeField] private int nbOfZombieThatCanBeActive = 5; //changer chaque round
    [SerializeField] private int maxZombiePerRound = 6; //changer chaque round
    [SerializeField] private float zombieSpeed = 2f; //changer chaque round
    [SerializeField] private int zombieHealth = 30; //changer chaque round
    [SerializeField] private GameObject zombiePrefab;
    private float spawnTimer = 0f;
    private const int ZOMBIE_POOL_SIZE = 30;

    private ObjectPool zombiePool;

    private bool roundHasEnded = false;
     
    void Start()
    {
        gameManagerScript = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        if (hudManager != null)
            hudManagerScript = hudManager.GetComponent<HudManagerGame>();
        zombiePool = new ObjectPool(zombiePrefab, ZOMBIE_POOL_SIZE);
    }

    void Update()
    {
        if (!gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver())
        {
            spawnTimer += Time.deltaTime;
            int nbOfZombieActive = zombiePool.FindActiveObjects().Count;
            if (spawnTimer >= spawnRate && nbOfZombieActive <= nbOfZombieThatCanBeActive && !roundHasEnded)
            {
                if (nbOfZombieSpawned != maxZombiePerRound)
                {
                    SpawnZombie();
                    spawnTimer = 0f;
                    nbOfZombieSpawned++;
                }
                else if (nbOfZombieActive == 0)
                {
                    roundHasEnded = true;
                    soundManagerScript.PlayRoundEnd();
                    Invoke("EndOfRound", 8f);
                }
            }
        }
    }

    private void SpawnZombie()
    {
        List<GameObject> WoodenWindows = ShuffleList(GetAllWoodenWindows());
        for (int i = 0; i < WoodenWindows.Count; i++)
        {
            if (Vector3.Distance(player.transform.position, WoodenWindows[i].transform.position) < 60f)
            {
                WoodenWindows[i].GetComponent<WoodenWindowManager>().SpawnZombie(zombiePool, zombieSpeed, zombieHealth);
                break;
            }
        }
    }

    private List<GameObject> GetAllWoodenWindows()
    {
        List<GameObject> WoodenWindows = new List<GameObject>();
        AddOneWoodenWindows(WoodenWindows, 1);
        if (isSection2Open)
            AddOneWoodenWindows(WoodenWindows, 2);
        if (isSection3Open)
            AddOneWoodenWindows(WoodenWindows, 3);
        if (isSection4Open)
            AddOneWoodenWindows(WoodenWindows, 4);
        return WoodenWindows;
    }

    private List<GameObject> AddOneWoodenWindows(List<GameObject> WoodenWindows, int sectionNumber)
    {
        GameObject WoodenWindowsSection = WoodenWindowsSection1;
        if (sectionNumber == 2)
            WoodenWindowsSection = WoodenWindowsSection2;
        else if (sectionNumber == 3)
            WoodenWindowsSection = WoodenWindowsSection3;
        else if (sectionNumber == 4)
            WoodenWindowsSection = WoodenWindowsSection4;

        for (int i = 0; i < WoodenWindowsSection.transform.childCount; i++)
        {
            Transform child = WoodenWindowsSection.transform.GetChild(i);
            WoodenWindows.Add(child.gameObject);
        }
        return WoodenWindows;
    }

    public void Section2IsOpen()
    {
        isSection2Open = true;
    }

    public void Section3IsOpen()
    {
         isSection3Open = true;
    }

    public void Section4IsOpen()
    {
        isSection4Open = true;
    }

    private List<GameObject> ShuffleList(List<GameObject> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private void EndOfRound()
    {
        roundHasEnded = false;
        nbOfZombieSpawned = 0;
        spawnRate *= gameManagerScript.SPAWN_RATE_MULTIPLICATOR;
        nbOfZombieThatCanBeActive += gameManagerScript.NB_OF_ZOMBIE_THAT_CAN_BE_ACTIVE_ADDITION;
        maxZombiePerRound += gameManagerScript.MAX_ZOMBIE_PER_ROUND_ADDITION;
        zombieSpeed *= gameManagerScript.ZOMBIE_SPEED_MULTIPLICATOR;
        zombieHealth += gameManagerScript.ZOMBIE_HEALTH_ADDITION;
        if (spawnRate < gameManagerScript.SPAWN_RATE_MAX)
            spawnRate = gameManagerScript.SPAWN_RATE_MAX;
        if (nbOfZombieThatCanBeActive > gameManagerScript.NB_OF_ZOMBIE_THAT_CAN_BE_ACTIVE_MAX)
            nbOfZombieThatCanBeActive = gameManagerScript.NB_OF_ZOMBIE_THAT_CAN_BE_ACTIVE_MAX;
        if (maxZombiePerRound > gameManagerScript.MAX_ZOMBIE_PER_ROUND_MAX)
            maxZombiePerRound = gameManagerScript.MAX_ZOMBIE_PER_ROUND_MAX;
        if (zombieSpeed > gameManagerScript.ZOMBIE_SPEED_MAX)
            zombieSpeed = gameManagerScript.ZOMBIE_SPEED_MAX;
        if (zombieHealth > gameManagerScript.ZOMBIE_HEALTH_MAX)
            zombieHealth = gameManagerScript.ZOMBIE_HEALTH_MAX;
        gameManagerScript.ChangeRound();
        //Afficher les rounds dans le hud ne fonctionne pas à partir du GameManager
        hudManagerScript.ShowRound(gameManagerScript.GetRound());
    }
}
