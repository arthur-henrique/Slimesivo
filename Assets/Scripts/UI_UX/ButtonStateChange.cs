using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class ButtonStateChange : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler//, IPointerExitHandler
{
    //[SerializeField]
    //private Sprite imageNormalState, imageHighlightedState, imagePressedState;
    //
    //[SerializeField]
    //private float scaleNormalState, scaleHighlightedState, scalePressedState;

    [SerializeField]
    private Image imageComponent;

    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private bool changeTextColor, moveTextDown;
    [SerializeField] private float changeTextScale;

    [SerializeField] private Sprite imageNormalState; [SerializeField] private float scaleNormalState;
    [SerializeField] private Sprite imageHighlightedState; [SerializeField] private float scaleHighlightedState;
    [SerializeField] private Sprite imagePressedState; [SerializeField] private float scalePressedState;

    private Vector3 initialObjectScale, initialTextScale;

    void Awake()
    {
        initialObjectScale = gameObject.transform.localScale;
        initialTextScale = buttonText.rectTransform.localScale;
    }

    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.sprite = imageNormalState;
        Resize(scaleNormalState);
    }

    public void Resize(float scaleFactor)
    {
        gameObject.transform.localScale = new Vector3(initialObjectScale.x * scaleFactor, initialObjectScale.y * scaleFactor, initialObjectScale.z * scaleFactor);
    }

    public void OnPointerUp(PointerEventData eventData) //Normal: This is the default state of the button.
    {
        StartCoroutine(DelayPointerUp());
    }
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    StartCoroutine(DelayPointerUp());
    //}

    IEnumerator DelayPointerUp()
    {
        yield return new WaitForSecondsRealtime(0.065f);
        imageComponent.sprite = imageNormalState;
        Resize(scaleNormalState);

        buttonText.rectTransform.localScale = initialTextScale;
        //buttonText.color = new Color32 (0xff, 0xf, 0xf, 0xff);
    }

    public void OnPointerEnter(PointerEventData eventData) //Highlighted: This state occurs when the mouse is over the button.
    {
        imageComponent.sprite = imageHighlightedState;
        Resize(scaleHighlightedState);

        buttonText.rectTransform.localScale = initialTextScale * changeTextScale;
    }

    public void OnPointerDown(PointerEventData eventData) //Pressed: This state occurs when the button is pressed.
    {
        imageComponent.sprite = imagePressedState;
        Resize(scalePressedState);

        buttonText.rectTransform.localScale = initialTextScale * changeTextScale;
    }

}


/*
Fazer os textos ficarem mais escuros e ir para baixo se quiser
*/