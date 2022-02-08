using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance {get; private set;}

    public Bullet bulletPrefab;
    public List<Bullet> bulletList = new List<Bullet>();
    public int amount = 10;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for(int i = 0; i < amount; i++)
        {
            Bullet tmp = Instantiate(bulletPrefab, transform);
            tmp.gameObject.SetActive(false);
            bulletList.Add(tmp);
        }
    }

    public Bullet GetOrCreate(Vector3 pos)
    {
        Bullet reqBullet = null;

        for(int i = 0; i < bulletList.Count; i++)
        {
            if(!bulletList[i].gameObject.activeSelf)
            {
                reqBullet = bulletList[i];
                break;
            }
        }

        if(reqBullet == null)
        {
            reqBullet = Instantiate(bulletPrefab, transform);
            bulletList.Add(reqBullet);
        }

        reqBullet.SetPosition(pos);
        reqBullet.gameObject.SetActive(true);

        return reqBullet;
    }
}
