using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    RectTransform parentRect;
    RectTransform backgroundRect;
    RectTransform leverRect;

    public Player player;

    public float radius;

    public Vector3 moveDir;
    public Vector3 shotDir;

    public bool isTouch = false;

    private bool isGameStart = false;

    private void Awake() 
    {
        isGameStart = false;
    }

    private void Start() 
    {
        GameManager.Instance.SubGameStart(() => isGameStart = true);

        GameManager.Instance.SubGameOver(() =>
        {
            isGameStart = false;

            isTouch = false;
            player.Move(Vector3.zero);
        });

        parentRect = GetComponent<RectTransform>();
        backgroundRect = transform.Find("background").GetComponent<RectTransform>();
        leverRect = backgroundRect.transform.Find("lever").GetComponent<RectTransform>();

        //반지름을 가져온다
        radius = backgroundRect.rect.width * 0.5f;
    }

    private void Update() 
    {
        if(isTouch)
        {
            player.Move(moveDir);
        }
    }

    void OnTouch(Vector2 touch)
    {
        Vector2 leverPos = new Vector2(touch.x - backgroundRect.position.x, touch.y - backgroundRect.position.y);

        //레버의 위치가 반지름을 넘어가지 않게
        leverPos = Vector2.ClampMagnitude(leverPos, radius);
        leverRect.localPosition = leverPos;

        moveDir = new Vector3(leverPos.x, leverPos.y).normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isGameStart) return;

        OnTouch(eventData.position);
        isTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isGameStart) return;

        parentRect.transform.position = eventData.position;

        OnTouch(eventData.position);
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isGameStart) return;

        leverRect.localPosition = Vector2.zero;

        shotDir = moveDir * -1; //벡터의 반대방향 구하기

        moveDir = Vector2.zero;
        player.Move(moveDir);

        player.Shot(shotDir);

        isTouch = false;
    }
}
