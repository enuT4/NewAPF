using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    float baseMoveOutSpeed = 3600.0f;
    float moveOutSpeed;
    float baseMoveNextSpeed = 1500.0f;
    float moveNextSpeed;
    Vector3 dirPos = Vector3.zero;
    public Vector3 destinationPos = Vector3.zero;
    Color charColor;
    float tempScale = 1.0f;
    float newScale = 1.0f;
    float scaleSpeed = 10.0f;
    float scaleRate;
    Camera mainCam;

    private void Awake()
    {
        scaleRate = Screen.width / 1440.0f;
        moveOutSpeed = baseMoveOutSpeed * scaleRate;
        moveNextSpeed = baseMoveNextSpeed * scaleRate;
        mainCam = Camera.main;
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
        if (YSMSIngameMgr.inst.isGameOver) return;


        if (isFirst)
        {//첫번째 친구의 편이 갈라지는 함수
            if (isLeft) dirKey = -1;
            else dirKey = 1;

            //YSMSIngameMgr.inst.UpdateCharArray();
            dirPos = new Vector3(2 * dirKey, -1, 0);
            transform.Translate(dirPos * moveOutSpeed * Time.deltaTime);
            Vector3 viewPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
            Debug.Log(viewPos);
            if (IsOutOfScreen(viewPos))
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
            transform.position = Vector3.MoveTowards(transform.position, destinationPos, moveNextSpeed * Time.deltaTime);
            if (transform.localPosition.y <= destinationPos.y)
                isMove = false;

            tempScale = transform.localScale.x;
            if (tempScale < newScale)
            {
                tempScale += Time.deltaTime * scaleSpeed;
                if (tempScale >= newScale)
                    tempScale = newScale;
            }
            transform.localScale = new Vector3(tempScale, tempScale, 1.0f);
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

    bool IsOutOfScreen(Vector3 pos)
    {
        return pos.x < 0 || pos.x > Screen.width || pos.y < 0 || pos.y > Screen.height;
    }
}
