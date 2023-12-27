using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonManager : MonoBehaviour
{
    private Button resetButton;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(Click);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Click()
    {
        gameManagerScript.ResetGame();
    }
}
