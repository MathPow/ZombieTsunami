using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGunManager : ElementsManager
{
    [SerializeField] private GameObject playerManager;
    private PlayerManager playerManagerScript;
    [SerializeField] private Mesh gunMesh;
    void Start()
    {
        if (playerManager != null)
            playerManagerScript = playerManager.GetComponent<PlayerManager>();
    }

    public override void MainAction()
    {
        playerManagerScript.ChangeGun(gunMesh);
    }
}
