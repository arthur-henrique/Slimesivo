
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ButtonStateChange : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler //IPointerExitHandler
{
    [SerializeField]
    private Image imageComponent;

    [SerializeField]
    private Sprite normalState, highlightedState, pressedState;


    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.sprite = normalState;
    }

    public void OnPointerUp(PointerEventData eventData) //Normal: This is the default state of the button.
    {
        //dar delay
        imageComponent.sprite = normalState;
    }

    public void OnPointerEnter(PointerEventData eventData) //Highlighted: This state occurs when the mouse is over the button.
    {
        imageComponent.sprite = highlightedState;
    }

    public void OnPointerDown(PointerEventData eventData) //Pressed: This state occurs when the button is pressed.
    {
        imageComponent.sprite = pressedState;
    }
}


/*
Setar a escala para cada funcao e ja aproveitar para falar que os filhos (textos) vao ficar mais escuros e ir para baixo se quiser
*/