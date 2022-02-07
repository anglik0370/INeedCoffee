using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public JoyStick joyStick;
    public CanvasGroup joyStickGroup;

    public void OnPointerDown(PointerEventData eventData)
    {
        joyStickGroup.alpha = 1f;
        joyStickGroup.interactable = true;
        joyStickGroup.blocksRaycasts = true;

        joyStick.transform.position = eventData.position;

        joyStick.OnPointerDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        joyStick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joyStickGroup.alpha = 0f;
        joyStickGroup.interactable = false;
        joyStickGroup.blocksRaycasts = false;

        joyStick.OnPointerUp(eventData);
    }
}
