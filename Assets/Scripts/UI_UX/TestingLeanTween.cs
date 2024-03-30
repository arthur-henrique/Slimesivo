using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLeanTween : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Awake()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, -360, 1f).setLoopClamp();
        transform.localScale = Vector2.zero;

    }
    void Start()
    {
        transform.LeanScale(Vector2.one, 1f);
        LeanTween.scale(gameObject, new Vector3(2.8f, 2.8f, 2.8f), 3f); 
    }

    public void Update()
    {
        //transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }

    public void Close()
    {
    }

}
