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
    public bool isFirst = false;
    public static bool isClear = true;
    int dirKey = 0;
    float moveSpeed = 3.0f;
    Vector3 dirPos = Vector3.zero;
    public Vector3 destinationPos = Vector3.zero;
    Color charColor;
    float tempScale = 1.0f;
    float newScale = 1.0f;
    float scaleSpeed = 10.0f;
    float scaleRate;
   

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
        scaleRate = Screen.width / 1440.0f;
        moveSpeed *= scaleRate;
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
        if (YSMSIngameMgr.inst.isGameOver) return;


        if (isFirst)
        {//첫번째 친구의 편이 갈라지는 함수
            if (isLeft) dirKey = -1;
            else dirKey = 1;

            

            //YSMSIngameMgr.inst.UpdateCharArray();
            dirPos = new Vector3(2 * dirKey, -1, 0);
            transform.Translate(dirPos * moveSpeed);
            if (transform.position.y < -600.0f)     //수정 안해도 될 지도 하지만 600.0f가 애매한건 사실
            {
                isFirst = false;
                ObjectReturn();
                isMove = false;
                //YSMSIngameMgr.spawnList.RemoveAt(0);
            }
        }
        else
        {//편이 갈라진 첫번째 친구 자리를 채워 넣는 과정
            if (destinationPos == Vector3.zero) return;
            transform.position = Vector3.MoveTowards(transform.position, destinationPos, 3.0f);
            if (transform.localPosition.y <= destinationPos.y)
                isMove = false;

            tempScale = transform.localScale.x;
            if (tempScale < newScale)
            {
                tempScale += Time.deltaTime * scaleSpeed;
                if (tempScale >= newScale)
                    tempScale = newScale;
            }
            transform.localScale = new Vector3(newScale, newScale, 1.0f);
        }

        
        
    }

    public void SetDestinationScaleFunc(Vector3 destPos, float scaleRate)
    {
        destinationPos = destPos;
        newScale = scaleRate;
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
