using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATMManager : ElementsManager
{
    [SerializeField] private GameObject playerManager;
    private PlayerManager playerManagerScript;
    [SerializeField] private int moneyToAdd = 2000;
    void Start()
    {
        if (playerManager != null)
            playerManagerScript = playerManager.GetComponent<PlayerManager>();
    }

    void Update()
    {

    }

    public override void MainAction()
    {
        playerManagerScript.AddMoney(moneyToAdd);
    }
}
