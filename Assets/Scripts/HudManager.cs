using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Slider playerSpeedSlider;
    [SerializeField] private TextMeshProUGUI playerSpeedResultText;

    private GameObject gameManager;
    private GameManager gameManagerScript;

    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject optionsCanvas;
    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        Button optionsButtonScript = optionsButton.GetComponent<Button>();
        optionsButtonScript.onClick.AddListener(ToggleCanvas);
        Button backButtonScript = backButton.GetComponent<Button>();
        backButtonScript.onClick.AddListener(ToggleCanvas);
        optionsCanvas.SetActive(false);
    }

    void Update()
    {
        playerSpeedResultText.text = playerSpeedSlider.value.ToString();
    }
    private void ToggleCanvas()
    {
        startCanvas.SetActive(!startCanvas.activeSelf);
        optionsCanvas.SetActive(!startCanvas.activeSelf);
    }
}
