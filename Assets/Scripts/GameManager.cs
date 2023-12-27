using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    private bool isGamepad = false;
    private bool isGameOver = false;
    private int round = 1;
    private int kill = 0;

    private const int StartScene = 0;
    private const int GameScene = 1;
    private const int EndScene = 2;
    private int actualLevel = 0;
    bool scenesAreInTransition = false;

    public float SPAWN_RATE_MULTIPLICATOR = 0.95f;
    public float SPAWN_RATE_MAX = 2f;
    public int NB_OF_ZOMBIE_THAT_CAN_BE_ACTIVE_ADDITION = 1;
    public int NB_OF_ZOMBIE_THAT_CAN_BE_ACTIVE_MAX = 30;
    public int MAX_ZOMBIE_PER_ROUND_ADDITION = 2;
    public int MAX_ZOMBIE_PER_ROUND_MAX = 55;
    public float ZOMBIE_SPEED_MULTIPLICATOR = 1.2f;
    public float ZOMBIE_SPEED_MAX = 8f;
    public int ZOMBIE_HEALTH_ADDITION = 8;
    public int ZOMBIE_HEALTH_MAX = 180;

    private static bool gameIsPaused;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    private void PauseGame()
    {
        if (gameIsPaused) Time.timeScale = 0f;
        else Time.timeScale = 1;
    }

    public bool IsPauseGame()
    {
        return gameIsPaused;
    }

    public void GameOver()
    {
        isGameOver = true;
        StartNextlevel(18f);
    }

    public void addKill(int money)
    {
        kill++;
    }

    public void ChangeRound()
    {
        round++;
    }

    public int GetRound()
    {
        return round;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool getIsGamepad()
    {
        return isGamepad;
    }

    private int GetNextLevel()
    {
        if (++actualLevel == EndScene + 1)
            actualLevel = GameScene;
        isGamepad = false;
        isGameOver = false;
        round = 1;
        kill = 0;

        return actualLevel;
    }

    public void StartNextlevel(float delay)
    {
        if (scenesAreInTransition) return; 

        scenesAreInTransition = true;

        StartCoroutine(RestartLevelDelay(delay, GetNextLevel()));
    }

    private IEnumerator RestartLevelDelay(float delay, int level)
    {
        yield return new WaitForSeconds(delay);

        if (level == GameScene)
            SceneManager.LoadScene("GameScene");
        else
            SceneManager.LoadScene("EndScene");

        scenesAreInTransition = false;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
