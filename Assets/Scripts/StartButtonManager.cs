using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonManager : MonoBehaviour
{
    private Button startButton;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(Click);
    }

    private void Click()
    {
        gameManagerScript.StartNextlevel(0f);
    }
}
