using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Swipe_Touch : MonoBehaviour //colocar este script no Handle do Toggle e deixar o Handle apontado pro Touch por padrao. O Toggle Handle varia das posicoes -19.1 a 14.5
{
    [SerializeField] private GameObject swipeObject, touchObject;
    private Image swipeImage, touchImage;
    private bool willTheToggleSwitchToTouch;
    private Vector3 initialHandlePosition;

    void Start()
    {
        swipeImage = swipeObject.GetComponent<Image>();
        touchImage = touchObject.GetComponent<Image>();
        initialHandlePosition = transform.position;
        willTheToggleSwitchToTouch = false;
    }

    public void ToggleSwipeTouch() //quando o player clica no Toggle - ele comeca ativado
    {

        if (willTheToggleSwitchToTouch == true)
        {
            swipeImage.color = Color.gray;
            touchImage.color = Color.white;
            //move o toggle
            //LeanTween.moveX(gameObject, gameObject.transform.position.x - 33.6f, 1f).setEaseInOutBounce();

            //float newXPosition = initialHandlePosition.x - 118;

            transform.position = initialHandlePosition;
            willTheToggleSwitchToTouch = false;
        }
        else
        {
            touchImage.color = Color.gray;
            swipeImage.color = Color.white;
            //move o toggle
            //LeanTween.moveX(gameObject, gameObject.transform.position.x + 33.6f, 1f).setEaseInBack();

            float newXPosition = transform.position.x - 118;
            Vector3 newPosition = transform.position;
            newPosition.x = newXPosition;
            transform.position = newPosition;
            willTheToggleSwitchToTouch = true;
        }
    }
}
