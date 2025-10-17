using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDuring : MonoBehaviour
{
    Quaternion hammerRotation;
    float rotationSpeed = 3.0f;
    bool isUp = false;

    //void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HammerRotationFunc();
        
    }

    void HammerRotationFunc()
    {
        if (!gameObject.activeSelf) return;
        hammerRotation = transform.rotation;
        if (!isUp)
        {
            hammerRotation.z += Time.deltaTime * rotationSpeed;
            if (0.5f <= hammerRotation.z)
                isUp = true;
        }
        else
        {
            hammerRotation.z -= Time.deltaTime * rotationSpeed;
            if (hammerRotation.z <= -0.13f)
                isUp = false;
        }
        transform.rotation = hammerRotation;
    }
}
