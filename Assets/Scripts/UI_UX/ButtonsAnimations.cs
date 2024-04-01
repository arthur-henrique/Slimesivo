using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsAnimations : MonoBehaviour
{
    Button button;
    private Vector3 buttonDimensions;
    [SerializeField] private int whichAnimationToSet;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ButtonAnimation);
        buttonDimensions = gameObject.transform.localScale;
    }

    private void ButtonAnimation()
    {
        switch(whichAnimationToSet)
        {
        case 1: //para os botoes de niveis do mapa
                print("BBB");
            LeanTween.scale(gameObject, buttonDimensions*1.2f, 0.5f);
            LeanTween.scale(gameObject, buttonDimensions, 0.1f).setDelay(0.5f);
        break;
            case 2:
                print("AAA");
                break;
        }
    }
}
