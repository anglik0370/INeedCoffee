using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    private CanvasGroup cvs;

    [SerializeField]
    private List<CanvasGroup> imgList;
    [SerializeField]
    private List<string> explanationList;

    [SerializeField]
    private Button prevBtn;
    [SerializeField]
    private Button nextBtn;

    [SerializeField]
    private Text explanationText;

    [SerializeField]
    private int idx;

    private void Awake() 
    {
        prevBtn.onClick.AddListener(() => 
        {
            idx--;
            Tutorial();
        });
        nextBtn.onClick.AddListener(() => 
        {
            idx++;
            Tutorial();
        });

        cvs = GetComponent<CanvasGroup>();

        Close();
    }

    public void Open()
    {
        cvs.alpha = 1f;
        cvs.interactable = true;
        cvs.blocksRaycasts= true;

        Tutorial();
    }

    public void Close()
    {
        idx = 0;

        cvs.alpha = 0f;
        cvs.interactable = false;
        cvs.blocksRaycasts= false;

        for(int i = 0; i < imgList.Count; i++)
        {
            imgList[i].alpha = 0;
        }

        explanationText.text = string.Empty;
    }

    public void Tutorial()
    {
        idx = Mathf.Clamp(idx, 0, imgList.Count - 1);

        if(idx == 0)
        {
            prevBtn.gameObject.SetActive(false);
            nextBtn.gameObject.SetActive(true);
        }
        else if(idx == imgList.Count - 1)
        {
            prevBtn.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(false);
        }
        else
        {
            prevBtn.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
        }

        for(int i = 0; i < imgList.Count; i++)
        {
            if(i == idx)
            {
                imgList[i].alpha = 1;
                explanationText.text = explanationList[i];
            }
            else
            {
                imgList[i].alpha = 0;
            }
        }
    }
}
