using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : ElementsManager
{
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private bool multipleDoor = false;
    [SerializeField] private int sectionUnlock = 2;
    [SerializeField] private GameObject spawnZombieManager;
    private float startY;
    private float endY = 6f;
    private float elapsedTime = 0f;
    private bool isDoorVanish = false;
    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (isDoorVanish)
        {
            Transform transformDoor = transform;
            if (multipleDoor)
            {
                transformDoor = transform.parent;
            }
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / transitionSpeed);
            float newY = Mathf.Lerp(startY, endY, t);
            transformDoor.position = new Vector3(transformDoor.position.x, newY, transformDoor.position.z);

            if (transformDoor.position.y >= endY)
            {
                Destroy(transformDoor.gameObject);
            }
        }
    }

    public override void MainAction()
    {
        isDoorVanish = true;
        if (sectionUnlock == 2)
            spawnZombieManager.GetComponent<SpawnZombieManager>().Section2IsOpen();
        else if (sectionUnlock == 3)
            spawnZombieManager.GetComponent<SpawnZombieManager>().Section3IsOpen();
        else if (sectionUnlock == 4)
            spawnZombieManager.GetComponent<SpawnZombieManager>().Section4IsOpen();
    }
}
