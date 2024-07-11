using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    //화면 관련 변수 선언
    int setWidth = 1440;
    int setHeight = 3200;

    int deviceWidth = Screen.width;
    int deviceHeight = Screen.height;

    float newWidth = 0.0f;
    float newHeight = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        SetScreenSize();
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    void SetScreenSize()
    {
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)setWidth / deviceHeight)
        {
            newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1.0f - newWidth) / 2.0f, 0.0f, newWidth, 1.0f);
        }
        else
        {
            newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            Camera.main.rect = new Rect(0.0f, (1.0f - newHeight) / 2.0f, 1.0f, newHeight);
        }
    }
}
