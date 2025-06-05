using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet(Vector2 spawnPosition, Vector2 direction)
    {
        GameObject bullet = null;

        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
        }

        bullet.transform.position = spawnPosition;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }

        return bullet;
    }
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
