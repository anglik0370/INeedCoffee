using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer healthBar;

    [SerializeField]
    private Vector3 originPos;

    public void ResetTransform()
    {
        transform.localPosition = originPos;
    }

    public void UpdateHealthBar(float max, float cur)
    {
        healthBar.transform.localScale = new Vector3(cur / max, 1, 1);
    }
}
