using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Swipe_Touch : MonoBehaviour //colocar este script no Handle do Toggle e deixar o Handle apontado pro Touch por padrao. O Toggle Handle varia das posicoes -19.1 a 14.5
{
    [SerializeField] private GameObject swipeObject, touchObject;
    private Image swipeImage, touchImage;
    private bool willTheToggleSwitchToTouch;
    private RectTransform thisRectTransform;

    void Start()
    {
        swipeImage = swipeObject.GetComponent<Image>();
        touchImage = touchObject.GetComponent<Image>();
        thisRectTransform = GetComponent<RectTransform>();
        willTheToggleSwitchToTouch = false;
    }

    void OnMouseDown()
    {
        print("clicou");
    }

    public void ToggleSwipeTouch() //quando o player clica no Toggle - ele comeca ativado
    {

        if (willTheToggleSwitchToTouch == true)
        {
            swipeImage.color = Color.gray;
            touchImage.color = Color.white;

            //float newXPosition = initialHandlePosition.x - 118;
            thisRectTransform.anchoredPosition = new Vector2 (14.5f, 0f);

            willTheToggleSwitchToTouch = false;
        }
        else
        {
            touchImage.color = Color.gray;
            swipeImage.color = Color.white;

            thisRectTransform.anchoredPosition = new Vector2(-17.5f, 0f);

            willTheToggleSwitchToTouch = true;
        }
    }
}
