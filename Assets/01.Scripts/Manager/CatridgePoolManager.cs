using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatridgePoolManager : MonoBehaviour
{
    public static CatridgePoolManager Instance {get; private set;}

    public Catridge catridgePrefab;
    public List<Catridge> catridgeList = new List<Catridge>();
    public int amount = 10;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }

        for(int i = 0; i < amount; i++)
        {
            Catridge tmp = Instantiate(catridgePrefab, transform);
            tmp.gameObject.SetActive(false);
            catridgeList.Add(tmp);
        }
    }

    public Catridge GetOrCreate(Vector3 pos)
    {
        Catridge reqCatridge = null;

        for(int i = 0; i < catridgeList.Count; i++)
        {
            if(!catridgeList[i].gameObject.activeSelf)
            {
                reqCatridge = catridgeList[i];
                break;
            }
        }

        if(reqCatridge == null)
        {
            reqCatridge = Instantiate(catridgePrefab, transform);
            catridgeList.Add(reqCatridge);
        }

        reqCatridge.SetPosition(pos);
        reqCatridge.gameObject.SetActive(true);

        return reqCatridge;
    }
}
