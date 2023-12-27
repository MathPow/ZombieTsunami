using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoxManager : ElementsManager
{
    [SerializeField] private GameObject playerManager;
    private PlayerManager playerManagerScript;
    [SerializeField] private GameObject randomGun;
    private MeshFilter randomGunMesh;
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    private int nbOfGunCycled = 0;
    private Mesh gunMesh;
    void Start()
    {
        if (playerManager != null)
            playerManagerScript = playerManager.GetComponent<PlayerManager>();
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        randomGunMesh = randomGun.GetComponent<MeshFilter>();
        randomGun.SetActive(false);
    }

    public override void MainAction()
    {
        if (!randomGun.activeSelf)
        {
            randomGun.SetActive(true);
            StartCoroutine(RandomGun());
        }
    }

    private IEnumerator RandomGun()
    {
        while (nbOfGunCycled != 10)
        {
            gunMesh = playerManagerScript.GetRandomGunMesh();
            randomGunMesh.mesh = gunMesh;
            nbOfGunCycled++;
            yield return new WaitForSeconds(0.25f);
        }
        Invoke("DesactivateAnimation", 0.8f);
    }

    private void DesactivateAnimation()
    {
        nbOfGunCycled = 0;
        soundManagerScript.PlayBuyWeaponBox();
        playerManagerScript.ChangeGun(gunMesh);
        randomGun.SetActive(false);
    }
}
