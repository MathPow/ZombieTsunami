using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManagerGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private TextMeshProUGUI textRound;
    [SerializeField] private TextMeshProUGUI textAmmo;
    [SerializeField] private TextMeshProUGUI textMagazines;
    [SerializeField] private TextMeshProUGUI textReload;
    [SerializeField] private TextMeshProUGUI textPause;
    [SerializeField] private TextMeshProUGUI textGameOver;
    [SerializeField] private RawImage bloodScreen;
    [SerializeField] private GameObject gameManager;
    private GameManager gameManagerScript;
    public enum HudElements
    {
        Nothing,
        TextDoor,
        TextWeaponBox,
        TextAmmoBox,
        TextWallGun,
        TextWoodenWindow,
        TextUpgradingTable,
        TextATM,
        TextPerk,
        NotEnoughFunds
    }
    void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        textElement.enabled = false;
        textReload.enabled = false;
        textPause.enabled = false;
        textGameOver.enabled = false;
        ShowBloodScreen(100);
    }

    void Update()
    {
        if (gameManagerScript.IsGameOver())
            ShowGameOver();
        else
            RemoveGameOver();
        if (gameManagerScript.IsPauseGame())
            ShowPause();
        else
            RemovePause();
    }

    public void ActivateElement(HudElements el, int price = 0, string name = "")
    {
        switch (el)
        {
            case HudElements.TextDoor:
                textElement.text = "[E] - " + price.ToString() + "$ to clean the debris";
                break;
            case HudElements.TextWeaponBox:
                textElement.text = "[E] - " + price.ToString() + "$ to open mystery box";
                break;
            case HudElements.TextAmmoBox:
                textElement.text = "[E] - " + price.ToString() + "$ to get max ammo";
                break;
            case HudElements.TextWallGun:
                textElement.text = "[E] - " + price.ToString() + "$ to buy the wall gun";
                break;
            case HudElements.TextWoodenWindow:
                textElement.text = "[E] - to repare window";
                break;
            case HudElements.TextUpgradingTable:
                textElement.text = "[E] - " + price.ToString() + "$ to upgarde your gun";
                break;
            case HudElements.TextATM:
                textElement.text = "[E] - to CHEAT and get 2000$";
                break;
            case HudElements.TextPerk:
                textElement.text = "[E] - " + price.ToString() + "$ to buy " + name + " perk";
                break;
            case HudElements.NotEnoughFunds:
                textElement.text = "NOT ENOUGH FUNDS - Kill some zombies!";
                break;
        }
        textElement.enabled = true;
        RemoveReload();
    }

    public void DesactivateElement()
    {
        textElement.enabled = false;
    }
    public void ShowMoney(int money)
    {
        textMoney.text = "Money :" + money.ToString() + "$";
    }
    public void ShowRound(int round)
    {
        textRound.text = round.ToString();
    }
    public void ShowAmmo(int ammo)
    {
        textAmmo.text = "Ammo :" + ammo.ToString();
    }
    public void ShowMagazines(int magazines)
    {
        textMagazines.text = "Magazines :" + magazines.ToString();
    }
    public void ShowReload()
    {
        textReload.enabled = true;
    }
    public void RemoveReload()
    {
        textReload.enabled = false;
    }

    private void ShowPause()
    {
        textPause.enabled = true;
    }

    private void RemovePause()
    {
        textPause.enabled = false;
    }

    private void ShowGameOver()
    {
        textGameOver.enabled = true;
    }

    private void RemoveGameOver()
    {
        textGameOver.enabled = false;
    }

    public void ShowBloodScreen(int health)
    {
        Color imageColor = bloodScreen.color;
        imageColor.a = (100 - (float)health) / 100;
        bloodScreen.color = imageColor;
    }
}
