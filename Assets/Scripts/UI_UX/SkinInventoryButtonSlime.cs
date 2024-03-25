using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Para acessar o inventario de skin. ESTE SCRIPT TEM QUE ESTAR ANEXADO NO PLAYER DO MAIN MENU
/// </summary>
public class SkinInventoryButtonSlime : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HighlightSlimeButton() //colocar no Event Trigger --> Pointer Down
    {
        spriteRenderer.color = Color.gray;
    }

    public void AccessInventoryPanel() //colocar no Event Trigger --> Pointer Up
    {
        spriteRenderer.color = Color.white;
        print("abre o painel do inventario");
    }
}


//OBS: para poder fazer com que o player seja um botao, tem que adicionar collider 2d e um Event Trigger nele. Ja na camera, tem que adicionar um Physics 2d raycaster 