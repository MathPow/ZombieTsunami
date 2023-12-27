using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradingTableManager : ElementsManager
{
    [SerializeField] private GameObject playerManager;
    private PlayerManager playerManagerScript;
    [SerializeField] private Material newMat;
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
        playerManagerScript.UpgradeGun(newMat);
    }
}
