using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : ElementsManager
{
    [SerializeField] private GameObject playerManager;
    private PlayerShoot ShootPlayerScript;
    private FPSPlayer MouvementPlayerScript;
    [SerializeField] private PerkTpe perkType;
    [SerializeField] private string perkName = "";
    [SerializeField] private bool isBought = false;

    private enum PerkTpe
    {
        SpeedBoost,
        SpeedFire,
    }
    void Start()
    {
        if (playerManager != null)
        {
            ShootPlayerScript = playerManager.GetComponent<PlayerShoot>();
            MouvementPlayerScript = playerManager.GetComponent<FPSPlayer>();
        }
    }

    public override void MainAction()
    {
        if (!isBought)
        {

            switch (perkType)
            {
                case PerkTpe.SpeedBoost:
                    MouvementPlayerScript.SpeedBoostPerkActivate();
                    break;
                case PerkTpe.SpeedFire:
                    ShootPlayerScript.SpeedFirePerkActivate();
                    break;
            }
            isBought = true;
        }
    }

    public string GetPerkName()
    {
        return perkName;
    }
}
