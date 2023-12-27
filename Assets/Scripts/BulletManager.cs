using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private float damage = 20;
    [SerializeField] private int nbOfZombieHit = 1;
    [SerializeField] private int moneyPerHit = 5;
    [SerializeField] private GameObject bulletSpawn;

    void OnEnable()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
            DesactivateBullet();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            player.GetComponent<PlayerManager>().AddMoney(moneyPerHit);
            other.GetComponent<Actor>().LoseHealth(damage);
        }
        nbOfZombieHit--;
        if (nbOfZombieHit == 0)
        {
            DesactivateBullet();
        }
    }

    public void ShootBullet(float bulletSpeed, Transform bulletTransform, float bulletSize, float bulletDamage, int bulletNbOfZombieHit, int bulletMoneyPerHit)
    {
        GetComponent<Rigidbody>().velocity = bulletSpeed * bulletTransform.forward;
        transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        damage = bulletDamage;
        nbOfZombieHit = bulletNbOfZombieHit;
        moneyPerHit = bulletMoneyPerHit;
        Invoke("DesactivateBullet", 2f);
    }

    private void DesactivateBullet()
    {
        gameObject.SetActive(false);
    }
}