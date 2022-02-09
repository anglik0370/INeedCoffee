using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer healthBar;

    public void UpdateHealthBar(float max, float cur)
    {
        healthBar.transform.localScale = new Vector3(cur / max, 1, 1);
    }
}
