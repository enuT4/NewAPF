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

    GameObject canvasObj;
    [SerializeField] private Button gameStartBtn;
    GameObject loginPanelObj;
    [SerializeField] private Button settingBtn;
    [HideInInspector] public GameObject settingPanelObj;
    GameObject settingChildObj;

    bool isTest = false;
    bool isPrePlay = false;
    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!canvasObj) canvasObj = GameObject.Find("Canvas").gameObject;
        if (!SoundManager.instance) SoundManager.instance.CallInstance();
        if (!MusicManager.instance) MusicManager.instance.CallInstance();

        if (!bgImg) bgImg = canvasObj.transform.Find("TitleImg").GetComponent<Image>();
        if (!gameStartBtn) gameStartBtn = bgImg.transform.Find("GameStartBtn").GetComponent<Button>();

        if (!loginPanelObj) loginPanelObj = canvasObj.transform.Find("LoginPanelObj").gameObject;
        if (loginPanelObj.activeSelf) loginPanelObj.SetActive(false);

        if (!settingBtn) settingBtn = bgImg.transform.Find("SettingBtn").GetComponent<Button>();
        if (!settingPanelObj) settingPanelObj = canvasObj.transform.Find("SettingPanel").gameObject;
        if (!settingChildObj) settingChildObj = settingPanelObj.transform.GetChild(0).gameObject;

    }


    // Start is called before the first frame update
    void Start()
    {
        if (gameStartBtn != null) gameStartBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            loginPanelObj.SetActive(true);
        });

        if (settingBtn != null) settingBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            settingPanelObj.SetActive(true);
            if (settingChildObj != null && !settingChildObj.activeSelf) settingChildObj.SetActive(true);
        });

        MusicManager.instance.PlayMusic("MainBGM");
        PrePlaySoundFunc();
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

    void PrePlaySoundFunc()
    {
        if (isPrePlay) return;

        isPrePlay = true;
        StartCoroutine(SoundManager.instance.PlaySoundInAdvance());
    }

    public void TurnOffSettingPanelFunc()
    {
        if (settingPanelObj.activeSelf) settingPanelObj.SetActive(false);
    }
}
