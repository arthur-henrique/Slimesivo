using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private int maxPage;
    private int currentPage;
    private Vector3 pageWidth;
    private Vector3 targetPos;
    [SerializeField] private RectTransform mapPages;
    [SerializeField] private float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    [SerializeField] private GameObject map1;
    private float dragThreshold;
    [SerializeField] private int thresholdValue; //8 parece ser ideal

    private void Awake()
    {
        currentPage = 1;
        targetPos = mapPages.localPosition;
        dragThreshold = Screen.width / thresholdValue;
    }

    void Start()
    {
        var rectTransform = map1.GetComponent<RectTransform>();
        float width = rectTransform.rect.width;
        pageWidth = new Vector3(-width, 0, 0);
    } 

    public void NextPage()
    {
        if(currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageWidth;
            MovePage();
        }
    }

    public void PreviousPage()
    {
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageWidth;
            MovePage();
        }
    }

    private void MovePage()
    {
        mapPages.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(Mathf.Abs(eventData.position.x-eventData.pressPosition.x) > dragThreshold)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                PreviousPage();
            }
            else
            {
                NextPage();
            }
        }
        else
        {
            MovePage();
        }
    }
}