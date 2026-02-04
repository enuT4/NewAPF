using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MemoryPoolObject
{
    float effectTime = 0.0f;
    float moveVelocity = 250.0f;
    float alphaVelocity = 3.5f;

    Text thisText = null;
    Color textColor;

    Vector3 currentPos = Vector3.zero;
    Outline[] textOutLine;
    Color outline1Color;
    Color outline2Color;
    Canvas SortLayerCanvas;


    // Start is called before the first frame update
    void Start()
    {
        thisText = GetComponent<Text>();
        textColor = thisText.color;
        textColor.a = 0.0f;

        textOutLine = thisText.GetComponentsInChildren<Outline>();
        outline1Color = textOutLine[0].effectColor;
        outline2Color = textOutLine[1].effectColor;
        outline1Color.a = 0.0f;
        outline2Color.a = 0.0f;

        if (GlobalValue.g_GameKind == GameKind.YSMS)
            moveVelocity = 250.0f;
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
            moveVelocity = 1.0f;

        SortLayerCanvas = GetComponent<Canvas>();
        SortLayerCanvas.overrideSorting = true;
        SortLayerCanvas.sortingOrder = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (effectTime <= 1.0f)
            effectTime += Time.deltaTime;

        if (effectTime < 0.3f)
        {
            currentPos = transform.position;
            currentPos.x -= Time.deltaTime * moveVelocity;
            transform.position = currentPos;

            textColor.a += Time.deltaTime * alphaVelocity;

            if (1.0f <= textColor.a)
                textColor.a = 1.0f;
            outline1Color.a = textColor.a * 0.5f;
            outline2Color.a = textColor.a * 0.5f;

            thisText.color = textColor;
            textOutLine[0].effectColor = outline1Color;
            textOutLine[1].effectColor = outline2Color;
        }

        if (0.5f < effectTime)
        {
            textColor.a += Time.deltaTime * alphaVelocity * 0.15f;
            if (1.0f <= textColor.a)
                textColor.a = 1.0f;
            outline1Color.a = textColor.a * 0.5f;
            outline2Color.a = textColor.a * 0.5f;

            thisText.color = textColor;
            textOutLine[0].effectColor = outline1Color;
            textOutLine[1].effectColor = outline2Color;

            currentPos = transform.position;
            currentPos.x -= Time.deltaTime * moveVelocity * 0.1f;
            transform.position = currentPos;

        }

        if (1.0f < effectTime)
        {

            ObjectReturn();
        }
    }

    public void InitTimeFunc(int showTime)
    {
        thisText = GetComponent<Text>();
        thisText.text = showTime.ToString();
        effectTime = 0.0f;

    }

    
}
