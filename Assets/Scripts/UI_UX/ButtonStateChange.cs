using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class ButtonStateChange : MonoBehaviour, /*IPointerEnterHandler,*/ IPointerDownHandler, IPointerUpHandler//, IPointerExitHandler
{
    //[SerializeField]
    //private Sprite imageNormalState, imageHighlightedState, imagePressedState;
    //
    //[SerializeField]
    //private float scaleNormalState, scaleHighlightedState, scalePressedState;

    [SerializeField]
    private Image imageComponent;

    [SerializeField] private GameObject buttonTextObject;
    private TMP_Text buttonText;
    [SerializeField] private bool changeTextColor;
    [SerializeField] private int finalTextColorRed, finalTextColorGreen, finalTextColorBlue;
    private Color initialTextColor;
    [SerializeField] private float changeTextScale, changeTextPosition_y;

    [SerializeField] private Sprite imageNormalState; [SerializeField] private float scaleNormalState;
    //[SerializeField] private Sprite imageHighlightedState; [SerializeField] private float scaleHighlightedState;
    [SerializeField] private Sprite imagePressedState; [SerializeField] private float scalePressedState;

    private Vector3 initialObjectScale, initialTextScale, initialTextPosition;

    void Awake()
    {
        buttonText = buttonTextObject.GetComponent<TMP_Text>();
        imageComponent = GetComponent<Image>();
        initialObjectScale = gameObject.transform.localScale;
        initialTextScale = buttonText.rectTransform.localScale;
    }

    void Start()
    {
        initialTextPosition = buttonTextObject.transform.position;
        initialTextColor = buttonText.color;
        imageComponent.sprite = imageNormalState;
        Resize(scaleNormalState);
    }

    void Update() //REMOVER ISSO DEPOIS
    {
        //Debug.Log(buttonTextObject.transform.position);
        //Debug.Log(initialTextPosition);
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
        buttonText.color = initialTextColor;
        buttonTextObject.transform.position = initialTextPosition;
    }

    //public void OnPointerEnter(PointerEventData eventData) //Highlighted: This state occurs when the mouse is over the button.
    //{
    //    imageComponent.sprite = imageHighlightedState;
    //    Resize(scaleHighlightedState);
    //
    //    buttonText.rectTransform.localScale = initialTextScale * changeTextScale;
    //    buttonTextObject.transform.position = initialTextPosition + new Vector3(0, changeTextPosition_y, 0);
    //    if (changeTextColor)
    //    {
    //        buttonText.color = new Color(finalTextColorRed / 255f, finalTextColorGreen / 255f, finalTextColorBlue / 255f);
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData) //Pressed: This state occurs when the button is pressed.
    {
        imageComponent.sprite = imagePressedState;
        Resize(scalePressedState);

        buttonText.rectTransform.localScale = initialTextScale * changeTextScale;
        buttonTextObject.transform.position = initialTextPosition + new Vector3(0, changeTextPosition_y, 0);
        if (changeTextColor)
        {
            buttonText.color = new Color(finalTextColorRed / 255f, finalTextColorGreen / 255f, finalTextColorBlue / 255f);
        }
    }

}