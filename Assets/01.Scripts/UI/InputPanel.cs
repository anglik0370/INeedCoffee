using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public JoyStick joyStick;
    public CanvasGroup joyStickGroup;

    private bool isGameStart = false;
    private bool isPause = false;

    private void Awake() 
    {
        isGameStart = false;
        isPause = false;
    }

    private void Start() 
    {
        GameManager.Instance.SubGameStart(() => 
        {
            isGameStart = true;
        });

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;

            joyStickGroup.alpha = 0f;
            joyStickGroup.interactable = false;
            joyStickGroup.blocksRaycasts = false;
        });

        GameManager.Instance.SubBackToMain(() => 
        {
            isGameStart = false;

            joyStickGroup.alpha = 0f;
            joyStickGroup.interactable = false;
            joyStickGroup.blocksRaycasts = false;
        });

        GameManager.Instance.SubPause(isPause => 
        {
            this.isPause = isPause;
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isGameStart || isPause) return;

        joyStickGroup.alpha = 1f;
        joyStickGroup.interactable = true;
        joyStickGroup.blocksRaycasts = true;

        joyStick.transform.position = eventData.position;

        joyStick.OnPointerDown(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isGameStart || isPause) return;

        joyStick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isGameStart || isPause) return;

        joyStickGroup.alpha = 0f;
        joyStickGroup.interactable = false;
        joyStickGroup.blocksRaycasts = false;

        joyStick.OnPointerUp(eventData);
    }
}
