using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] private GameObject soundManager;
    private SoundManager soundManagerScript;
    [SerializeField] private float MAX_HEIGHT_WATER = 0.14f;
    [SerializeField] private float MIN_HEIGHT_WATER = -4.17f;
    [SerializeField] private float TIME_BEFORE_LOW = 3f;
    [SerializeField] private float NB_OF_MINUTES_BETWEEN_RISE = 5;

    private bool manageRiseCallOneTime = false;

    void Start()
    {
        if (soundManager != null)
            soundManagerScript = soundManager.GetComponent<SoundManager>();
        StartCoroutine(ManageRise());
    }

    void Update()
    {

    }

    private IEnumerator ManageRise()
    {
        while (true)
        {
            if (manageRiseCallOneTime)
            {
                soundManagerScript.StartRisingWaterSound();
                StartCoroutine(RiseWater());
            } else
            {
                manageRiseCallOneTime = true;
            }
            yield return new WaitForSeconds(NB_OF_MINUTES_BETWEEN_RISE * 60);
        }
    }

    private IEnumerator RiseWater()
    {
        while (MAX_HEIGHT_WATER > transform.position.y)
        {
            Vector3 newWaterPosition = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            transform.position = newWaterPosition;
            yield return new WaitForSeconds(0.1f);
        }
        Invoke("LowWater", TIME_BEFORE_LOW);
    }

    private void LowWater()
    {
        soundManagerScript.StopRisingWaterSound();
        Vector3 newWaterPosition = new Vector3(transform.position.x, MIN_HEIGHT_WATER, transform.position.z);
        transform.position = newWaterPosition;
    }
}
