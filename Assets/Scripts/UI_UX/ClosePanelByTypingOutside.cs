using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePanelByTypingOutside : MonoBehaviour
{
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //Mouse was clicked outside
        gameObject.SetActive(false);
    }
}
