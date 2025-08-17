using Enut4LJR;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    internal Image bgImg;

    [SerializeField] private Button gameStartBtn;
    GameObject loginPanelObj;

    bool isTest = false;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!SoundManager.instance) SoundManager.instance.CallInstance();
        if (!MusicManager.instance) MusicManager.instance.CallInstance();

        if (!bgImg) bgImg = GameObject.Find("Canvas").transform.Find("TitleImg").GetComponent<Image>();
        if (!gameStartBtn) gameStartBtn = bgImg.transform.Find("GameStartBtn").GetComponent<Button>();

        if (!loginPanelObj) loginPanelObj = GameObject.Find("Canvas").transform.Find("LoginPanelObj").gameObject;
        if (loginPanelObj.activeSelf) loginPanelObj.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (gameStartBtn != null) gameStartBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            loginPanelObj.SetActive(true);
        });

        MusicManager.instance.PlayMusic("MainBGM");
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GlobalValue.g_YSMSBestScore = 100;
            NetworkMgr.inst.PushPacket(PacketType.YSMSBestScore);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {

        }


    }
}
