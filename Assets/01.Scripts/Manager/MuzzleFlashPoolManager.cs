using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashPoolManager : MonoBehaviour
{
    public static MuzzleFlashPoolManager Instance {get; private set;}

    public MuzzleFlash muzzleFlashPrefab;
    public List<MuzzleFlash> muzzleFlashList = new List<MuzzleFlash>();
    public int amount = 10;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for(int i = 0; i < amount; i++)
        {
            MuzzleFlash tmp = Instantiate(muzzleFlashPrefab, transform);
            tmp.gameObject.SetActive(false);
            muzzleFlashList.Add(tmp);
        }
    }

    public MuzzleFlash GetOrCreate(Vector3 pos)
    {
        MuzzleFlash reqMuzzleFlash = null;

        for(int i = 0; i < muzzleFlashList.Count; i++)
        {
            if(!muzzleFlashList[i].gameObject.activeSelf)
            {
                reqMuzzleFlash = muzzleFlashList[i];
                break;
            }
        }

        if(reqMuzzleFlash == null)
        {
            reqMuzzleFlash = Instantiate(muzzleFlashPrefab, transform);
            muzzleFlashList.Add(reqMuzzleFlash);
        }

        reqMuzzleFlash.SetPosition(pos);
        reqMuzzleFlash.gameObject.SetActive(true);

        return reqMuzzleFlash;
    }
}
