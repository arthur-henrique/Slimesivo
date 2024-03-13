using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonCollider : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    //Para este script funcionar, tem que botar o script no botao (que eu quero que tenha o collider igual a imagem que eu anexei nele),
    //depois eu tenho que ir ate o sprite que eu estou utilizando para a imagem do botao e ticar a opcao Advanced --> Read/Write Enable
}
