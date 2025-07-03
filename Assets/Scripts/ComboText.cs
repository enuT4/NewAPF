using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MemoryPoolObject
{
    float effectTime = 0.0f;

    [SerializeField] internal Text comboText;
    int comboCount = 0;

    float textGrowSpeed = 600.0f;
    [SerializeField] internal Canvas sortLayerCanvas;

    private void Awake()
    {
        if (!comboText) comboText = transform.GetChild(0).GetComponent<Text>();
        if (!sortLayerCanvas) sortLayerCanvas = comboText.GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        comboText.fontSize = 100;
        sortLayerCanvas.overrideSorting = true;
    }

    // Update is called once per frame
    void Update()
    {
        effectTime += Time.deltaTime;
        if (effectTime < 0.2f)
        {
            comboText.fontSize += (int)(Time.deltaTime * textGrowSpeed);
            if (160 <= comboText.fontSize)
                comboText.fontSize = 160;
            sortLayerCanvas.sortingOrder = 7;
        }
        else if (effectTime < 0.5f)
        {
            comboText.fontSize -= (int)(Time.deltaTime * textGrowSpeed * 0.2f);
            if (comboText.fontSize <= 140)
                comboText.fontSize = 140;
            sortLayerCanvas.sortingOrder = 6;
        }
        else if (effectTime < 0.8f)
        {
            comboText.fontSize = 140;
            sortLayerCanvas.sortingOrder = 5;
        }
        else
        {
            ComboObjReturnFunc();
        }
    }


    public void SetComboTextFunc(int combo)
    {
        comboText.text = combo + " Combo";
    }

    public void ComboObjReturnFunc()
    {
        comboText.fontSize = 100;
        effectTime = 0.0f;
        ObjectReturn();
    }
}
