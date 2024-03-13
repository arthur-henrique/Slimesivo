using UnityEngine;
using UnityEngine.UI;

public class CenterButton : MonoBehaviour
{
    public Transform externalObject;

    private RectTransform buttonRect;

    private void Start()
    {
        buttonRect = GetComponent<RectTransform>();

        //para centralizar o Anchor com o Pivot do botao
        buttonRect.anchorMin = buttonRect.pivot;
        buttonRect.anchorMax = buttonRect.pivot;
        buttonRect.anchoredPosition = Vector2.zero;

        //pega a posicao inicial do External Object
        Vector3 externalObjectPosition = externalObject.position;

        //converte a posicao do External Object para o Espaco do Canvas
        Vector2 anchoredPosition = Camera.main.WorldToViewportPoint(externalObjectPosition);

        //transforma a posicao do botao baseado na posicao do External Object
        buttonRect.anchorMin = anchoredPosition;
        buttonRect.anchorMax = anchoredPosition;

        Debug.Log("External Object World Position: " + externalObject.position);
        Debug.Log("External Object Viewport Position: " + anchoredPosition);

    }

    //Esse script vai pegar o pivot do sprite como base (de onde posicionar o botao).
    //Eu preciso colocar este script no botao que eu quero que va para a posicao do sprite e deixar seu pivot centralizado.

}
