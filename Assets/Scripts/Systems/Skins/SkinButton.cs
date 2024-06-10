using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private AnimatorController playableAnimator;
    [SerializeField] private AnimatorController menuAnimator;

    // Start is called before the first frame update
    public void OnClickSkin()
    {
        GameManager.instance.playerAnimator = playableAnimator;
        GameManager.instance.menuPlayerAnimator = menuAnimator;

        MainMenuCanvasScript.Instance.ChangePlayerSkin(menuAnimator);
    }
}
