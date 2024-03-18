/*using UnityEngine;
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
*/

using UnityEngine;
using UnityEngine.UI;

public class CenterButton : MonoBehaviour
{
    public Transform externalObject;
    public GameObject spriteGameObject;

    private RectTransform buttonRect;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        buttonRect = GetComponent<RectTransform>();

        // Set button's anchor to match its pivot point
        buttonRect.anchorMin = buttonRect.pivot;
        buttonRect.anchorMax = buttonRect.pivot;
        buttonRect.anchoredPosition = Vector2.zero;

        // Get the SpriteRenderer component from the spriteGameObject
        spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // Set button's size based on the dimensions of the sprite
            buttonRect.sizeDelta = spriteRenderer.sprite.bounds.size;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found on spriteGameObject!");
        }

        // Get the initial position of the external object
        Vector3 externalObjectPosition = externalObject.position;

        // Convert the position of the external object to canvas space
        Vector2 anchoredPosition = Camera.main.WorldToViewportPoint(externalObjectPosition);

        // Set the position of the button based on the position of the external object
        buttonRect.anchorMin = anchoredPosition;
        buttonRect.anchorMax = anchoredPosition;

        Debug.Log("External Object World Position: " + externalObject.position);
        Debug.Log("External Object Viewport Position: " + anchoredPosition);
    }
}
