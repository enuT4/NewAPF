using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadTile1 : MonoBehaviour
{
    float rotationSpeed = 0.45f;


    //void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
            transform.Rotate(0.0f, 0.0f, rotationSpeed);
    }
}
