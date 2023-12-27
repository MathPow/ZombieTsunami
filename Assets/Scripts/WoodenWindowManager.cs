using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenWindowManager : ElementsManager
{
    [SerializeField] private GameObject player;
    private const int NB_OF_PLANKS = 4;
    [SerializeField] private int MONEY_BONUS = 10;
    [SerializeField] private float BREAKING_TIME_SEC = 2.5f;

    private float timerForRepare = 2f;

    void Update()
    {
        timerForRepare -= Time.deltaTime;
    }

    public override void MainAction()
    {
        if (timerForRepare < 0)
        {
            timerForRepare = 2f;
            List<GameObject> desactiveChildren = FindDesactiveChildrenOfParent(transform);
            List<GameObject> activeChildren = FindActiveChildrenOfParent(transform);
            if (NB_OF_PLANKS > activeChildren.Count)
            {
                desactiveChildren[0].SetActive(true);
                player.GetComponent<PlayerManager>().AddMoney(MONEY_BONUS);
            }
        }
    }

    public List<GameObject> FindActiveChildrenOfParent(Transform parent)
    {
        List<GameObject> activeChildren = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.gameObject.activeSelf && !child.gameObject.CompareTag("WoodenFrame") && !child.gameObject.CompareTag("MapDot"))
            {
                activeChildren.Add(child.gameObject);
            }
        }
        return activeChildren;
    }

    public List<GameObject> FindDesactiveChildrenOfParent(Transform parent)
    {
        List<GameObject> desactiveChildren = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (!child.gameObject.activeSelf && !child.gameObject.CompareTag("WoodenFrame"))
            {
                desactiveChildren.Add(child.gameObject);
            }
        }
        return desactiveChildren;
    }

    public void SpawnZombie(ObjectPool zombiePool, float zombieSpeed, int zombieHealth)
    {
        List<GameObject> activeChildren = FindActiveChildrenOfParent(transform);
        if (0 < activeChildren.Count)
        {
            StartCoroutine(DestroyPlanks(zombiePool, zombieSpeed, zombieHealth));
        } 
        else
        {
            GameObject zombie = zombiePool.GetObjectFromPool(transform.position, new Quaternion(0, 0, 0, 0));
            Actor actorScript = zombie.GetComponent<Actor>();
            actorScript.SetSpeed(zombieSpeed);
            actorScript.SetHealth(zombieHealth);
        }
    }

    private IEnumerator DestroyPlanks(ObjectPool zombiePool, float zombieSpeed, int zombieHealth)
    {
        List<GameObject> activeChildren = FindActiveChildrenOfParent(transform);
        while (0 < activeChildren.Count)
        {
            activeChildren[0].SetActive(false);
            activeChildren = FindActiveChildrenOfParent(transform);
            yield return new WaitForSeconds(BREAKING_TIME_SEC);
        }
        GameObject zombie = zombiePool.GetObjectFromPool(transform.position, new Quaternion(0, 0, 0, 0));
        Actor actorScript = zombie.GetComponent<Actor>();
        actorScript.SetSpeed(zombieSpeed);
        actorScript.SetHealth(zombieHealth);
    }
}
