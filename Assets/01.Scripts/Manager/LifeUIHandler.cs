using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LifeUIHandler : MonoBehaviour
{
    private List<LifeUI> lifeUIList;

    private void Awake() 
    {
        lifeUIList = GetComponentsInChildren<LifeUI>().ToList();
    }

    public void RemoveLife()
    {
        for(int i = lifeUIList.Count - 1; i >= 0; i--)
        {
            if(!lifeUIList[i].isEmpty)
            {
                lifeUIList[i].Empty();
                return;
            }
        }
    }
}
