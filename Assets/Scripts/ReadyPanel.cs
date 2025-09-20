using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPanel : MonoBehaviour
{
    internal Image readyImg;
    internal Image goImg;
    float readyTimer;
    float goTimer;
    Color readyImgColor;

    void Awake()
    {
        SoundManager.instance.CallInstance();
        MusicManager.instance.CallInstance();

        if (!readyImg) readyImg = transform.Find("ReadyImg").GetComponent<Image>();
        if (!goImg) goImg = transform.Find("GoImg").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (readyImg != null && goImg != null)
        {
            if (!readyImg.gameObject.activeSelf)
                readyImg.gameObject.SetActive(true);
            if (goImg.gameObject.activeSelf)
                goImg.gameObject.SetActive(false);

            readyTimer = 2.5f;
            SoundManager.instance.PlayerSound("Ready");
        }

    }

    // Update is called once per frame
    void Update()
    {
        ReadyTimerFunc();
        GoTimerFunc();
    }

    void ReadyTimerFunc()
    {
        if (readyTimer > 0.0f)
        {
            readyTimer -= Time.deltaTime;
            readyImgColor = readyImg.color;
            readyImgColor.a = Mathf.Lerp(1.0f - 0.5f * readyTimer, 2.0f, 0.2f);
            if (1.0f <= readyImgColor.a)
                readyImgColor.a = 1.0f;
            readyImg.color = readyImgColor;

            if (readyTimer < 0.0f)
            {
                SoundManager.instance.PlayerSound("Go");
                readyImg.gameObject.SetActive(false);
                goImg.gameObject.SetActive(true);
                goTimer = 1.0f;
                readyTimer = 0.0f;
            }
        }
    }

    void GoTimerFunc()
    {
        if (goTimer > 0.0f)
        {
            if (readyImg.gameObject.activeSelf || !goImg.gameObject.activeSelf) return;

            goTimer -= Time.deltaTime;
            if (goTimer <= 0.0f)
            {
                ClassifyGameKindFunc();
                this.gameObject.SetActive(false);
                
            }
        }
    }

    void ClassifyGameKindFunc()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            YSMSIngameMgr.inst.GameStartAfterReadyFunc();
            MusicManager.instance.PlayMusic("YSMSIngameBGM");
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            
            //MusicManager.instance.PlayMusic("SDJRIngameBGM");        //수정수정수정수정
        }
    }
}
