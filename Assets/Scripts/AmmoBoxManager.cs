using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxManager : ElementsManager
{
    [SerializeField] private GameObject player;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public override void MainAction()
    {
        player.GetComponent<PlayerShoot>().MaxAmmo();
    }
}
