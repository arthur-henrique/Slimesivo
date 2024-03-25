using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        GameManager.instance.SceneLoad();
    }
}
