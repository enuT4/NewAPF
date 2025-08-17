using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MemoryPoolObject
{
    float effectTime = 0.0f;

    [SerializeField] internal Text comboText;
    [HideInInspector] public int comboCount = 0;
    [HideInInspector] public bool isPause = false;
    float screenScaleRate = Screen.width / 1440.0f;
    float textGrowSpeed;
    [SerializeField] internal Canvas sortLayerCanvas;
    float[] comboFontSizeArray;
    float tempFontSize;

    private void Awake()
    {
        if (!comboText) comboText = transform.GetChild(0).GetComponent<Text>();
        if (!sortLayerCanvas) sortLayerCanvas = comboText.GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        comboText.fontSize = 80;
        sortLayerCanvas.overrideSorting = true;
        textGrowSpeed = 500.0f * screenScaleRate;
        comboFontSizeArray = new float[3] { 160 * screenScaleRate, 140 * screenScaleRate, 140 * screenScaleRate };

    }

    // Update is called once per frame
    void Update()
    {
        effectTime += Time.deltaTime;
        if (effectTime < 0.2f)
        {
            tempFontSize += Time.deltaTime * textGrowSpeed * screenScaleRate;
            if (comboFontSizeArray[0] <= tempFontSize)
                tempFontSize = comboFontSizeArray[0];
            if (sortLayerCanvas.sortingOrder != 7)
                sortLayerCanvas.sortingOrder = 7;
        }
        else if (effectTime < 0.4f)
        {
            tempFontSize -= Time.deltaTime * textGrowSpeed * screenScaleRate;
            if (tempFontSize <= comboFontSizeArray[1])
                tempFontSize = comboFontSizeArray[1];
            if (sortLayerCanvas.sortingOrder != 6)
                sortLayerCanvas.sortingOrder = 6;
        }
        else if (effectTime < 0.7f)
        {
            tempFontSize = comboFontSizeArray[2];
            if (sortLayerCanvas.sortingOrder != 5)
                sortLayerCanvas.sortingOrder = 5;
        }
        else
        {
            ComboObjReturnFunc();
        }
        comboText.fontSize = (int)tempFontSize;
    }


    public void SetComboTextFunc(int combo)
    {
        comboCount = combo;
        comboText.text = combo + " Combo";
    }

    public void ComboObjReturnFunc()
    {
        comboText.fontSize = 80;
        effectTime = 0.0f;
        comboCount = 0;
        ObjectReturn();
    }
}
