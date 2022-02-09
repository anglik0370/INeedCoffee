using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountUI : MonoBehaviour
{
    Text text;

    private void Awake() 
    {
        text = GetComponentInChildren<Text>();
    }

    public void UpdateCountText(int count)
    {
        text.text = $"x{count}";
    }
}
