using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    private GameObject gameManager;
    private GameManager gameManagerScript;
    [SerializeField] GameObject goal;
    [SerializeField] private float health = 0;
    [SerializeField] private int moneyPerKill = 60;
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;
    [SerializeField] private float speed = 3f;
    private bool canMove = true;

    private const float TIMER_BEFORE_DEAD_IN_WATER = 15f;
    private float waterTimer = 0;

    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        if (goal == null)
            goal = GameObject.FindWithTag("Player");
        if (navMeshAgent != null)
            navMeshAgent.destination = goal.transform.position;

        InvokeRepeating("LoopAudio", 10f, 10f);
    }

    void Update()
    {
        if (canMove && !gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver())
        {
            if (navMeshAgent == null)
            {
                transform.position = Vector3.MoveTowards(transform.position, goal.transform.position, speed * Time.deltaTime);
            } else
            {
                navMeshAgent.destination = goal.transform.position;
            }
        }
    }

    private void LoopAudio()
    {
            if (!audioSource.isPlaying && canMove && !gameManagerScript.IsPauseGame() && !gameManagerScript.IsGameOver() && gameObject.activeSelf)
            {
                audioSource.Play();
            }
    }

    public void LoseHealth(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            goal.GetComponent<PlayerManager>().AddMoney(moneyPerKill);
            gameObject.SetActive(false);
        }
    }

    public void StopMoving()
    {
        canMove = false;
    }

    public void StartMoving()
    {
        canMove = true;
    }

    public void SetSpeed(float actorSpeed)
    {
        if (actorSpeed >= 0f)
            speed = actorSpeed;
    }

    public void SetHealth(int actorHealth)
    {
        if (actorHealth >= 0f)
            health = actorHealth;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= TIMER_BEFORE_DEAD_IN_WATER)
            {
                LoseHealth(gameManagerScript.ZOMBIE_HEALTH_MAX);
                waterTimer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterTimer = 0f;
        }
    }
}
