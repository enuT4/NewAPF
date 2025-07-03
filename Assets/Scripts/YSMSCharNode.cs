using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum YSMSCharType
{
    YSMSBomb = 0,
    YSMSChar1,
    YSMSChar2,
    YSMSChar3,
    YSMSChar4,
    YSMSChar5,
    YSMSChar6,
    YSMSChar7,
    YSMSChar8,
    CharCount
}

public class YSMSCharNode : MemoryPoolObject
{
    public int charImgIndex = 0;
    public bool isMove = false;
    public bool isLeft = false;
    public static bool isClear = true;
    int dirKey = 0;
    float moveSpeed = 30.0f;
    Vector3 dirPos = Vector3.zero;
    Color charColor;
   

    public void SetCharResource(int index)
    {
        if (index < (int)YSMSCharType.YSMSBomb || (int)YSMSCharType.YSMSChar8 < index)
            return;

        charImgIndex = index;

        for (int ii = 0; ii < 9; ii++)
        {
            if (transform.GetChild(ii).gameObject.activeSelf) 
                transform.GetChild(ii).gameObject.SetActive(false);
        }        
        transform.GetChild(index).gameObject.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        charColor = transform.GetChild(charImgIndex).GetComponent<Image>().color;
    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        MoveFunc();
    }

    void MoveFunc()
    {
        if (!isMove) return;

        if (isLeft) dirKey = -1;
        else dirKey = 1;

        YSMSIngameMgr.inst.UpdateCharArray();
        dirPos = new Vector3(2 * dirKey, -1, 0);
        transform.Translate(dirPos * moveSpeed);
        if (transform.position.y < -600.0f)
        {
            ObjectReturn();
            isMove = false;
            YSMSIngameMgr.spawnList.RemoveAt(0);
        }
    }

    public void ErrorColorChangeFunc(bool isError)
    {
        if (isMove) return;

        if (isError)
            charColor = new Color(0.3f, 0.3f, 0.3f);
        else
            charColor = new Color(1.0f, 1.0f, 1.0f);

        transform.GetChild(charImgIndex).GetComponent<Image>().color = charColor;

    }
}
