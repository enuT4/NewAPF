using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ReadySceneMgr : MonoBehaviour
{
    [SerializeField] internal Button backBtn;
    [SerializeField] internal Button seeRankingBtn;
    [SerializeField] internal Text myRankTxt;
    int myRank = 0;

    [Header("-------- Upgrade Button --------")]
    [SerializeField] internal Button bonusUpBtn;
    [SerializeField] internal Text bonusLvTxt;
    [SerializeField] internal Text bonusAmTxt;
    [SerializeField] internal Button feverUpBtn;
    [SerializeField] internal Text feverLvTxt;
    [SerializeField] internal Text feverAmTxt;
    [SerializeField] internal Button superUpBtn;
    [SerializeField] internal Text superLvTxt;
    [SerializeField] internal Text superAmTxt;

    [Header("-------- Item Button --------")]
    [SerializeField] internal Button item1Btn;
    [SerializeField] internal Button item2Btn;
    [SerializeField] internal Button item3Btn;
    [SerializeField] internal Button item4Btn;
    string[] itemInfo = new string[4];
    string[] ysmsItemInfo = new string[4];
    string[] sdjrItemInfo = new string[4];
    [SerializeField] internal Button charShopBtn;
    [SerializeField] internal Button gameStartBtn;
    [SerializeField] internal Sprite[] buttonSprite;
    [SerializeField] internal Image[] checkImg;
    public static bool[] isItemChecked = new bool[4];
    int[] itemCostArr = new int[4];
    [SerializeField] internal Text[] itemCostTxtArr;

    [Header("-------- Info Text --------")]
    [SerializeField] internal Text gameNameTxt;
    [SerializeField] internal Text userGoldTxt;
    [SerializeField] internal Text userGemTxt;
    [SerializeField] internal Text itemInfoTxt;
    [SerializeField] internal Text userNickTxt;
    [SerializeField] internal Text userLvTxt;

    int goldVal = 0;
    int gemVal = 0;

    [SerializeField] internal GameObject upgradePanelObj;
    [SerializeField] internal GameObject upgradeCanvasObj;
    [SerializeField] internal GameObject rankingPanelObj;
    [SerializeField] internal GameObject messageBoxObj;

    bool isTutorialSkipOnOff = false;
    string gameName = "";

    [Header("-------- Rice Count --------")]
    [SerializeField] internal Text gameStartRiceTxt;
    DateTime checkTime;
    int[] fillArr = new int[5];
    [SerializeField] int currTimeSec = 0;
    int currYMD = 0;
    int tempYMD = 0;
    int timerSec = 0;
    int timerMin = 0;
    int riceFillTimeSec = 300;
    bool isRiceTimerStart = false;
    bool isDayChange = false;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {

        #region UI 연결
        if (!upgradeCanvasObj) upgradeCanvasObj = GameObject.Find("Canvas").gameObject;
        if (!backBtn) backBtn = upgradeCanvasObj.transform.Find("BackBtn").GetComponent<Button>();
        if (!seeRankingBtn) seeRankingBtn = upgradeCanvasObj.transform.Find("SeeRankingBtn").GetComponent<Button>();
        if (!myRankTxt) myRankTxt = seeRankingBtn.transform.GetChild(0).GetComponent<Text>();

        if (!bonusUpBtn) bonusUpBtn = upgradeCanvasObj.transform.Find("BonusUpBtn").GetComponent<Button>();
        if (!bonusAmTxt) bonusAmTxt = bonusUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!bonusLvTxt) bonusLvTxt = bonusUpBtn.transform.GetChild(2).GetComponent<Text>();
        if (!feverUpBtn) feverUpBtn = upgradeCanvasObj.transform.Find("FeverUpBtn").GetComponent<Button>();
        if (!feverAmTxt) feverAmTxt = feverUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!feverLvTxt) feverLvTxt = feverUpBtn.transform.GetChild(2).GetComponent<Text>();
        if (!superUpBtn) superUpBtn = upgradeCanvasObj.transform.Find("SuperUpBtn").GetComponent<Button>();
        if (!superAmTxt) superAmTxt = superUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!superLvTxt) superLvTxt = superUpBtn.transform.GetChild(2).GetComponent<Text>();

        if (!item1Btn) item1Btn = upgradeCanvasObj.transform.Find("Item1Btn").GetComponent<Button>();
        if (!item2Btn) item2Btn = upgradeCanvasObj.transform.Find("Item2Btn").GetComponent<Button>();
        if (!item3Btn) item3Btn = upgradeCanvasObj.transform.Find("Item3Btn").GetComponent<Button>();
        if (!item4Btn) item4Btn = upgradeCanvasObj.transform.Find("Item4Btn").GetComponent<Button>();
        if (!charShopBtn) charShopBtn = upgradeCanvasObj.transform.Find("CharShopBtn").GetComponent<Button>();
        if (!gameStartBtn) gameStartBtn = upgradeCanvasObj.transform.Find("GameStartBtn").GetComponent<Button>();

        if (!gameNameTxt) gameNameTxt = upgradeCanvasObj.transform.Find("Label").transform.GetChild(0).GetComponent<Text>();
        if (!userGoldTxt) userGoldTxt = upgradeCanvasObj.transform.Find("UserGoldText").GetComponent<Text>();
        if (!userGemTxt) userGemTxt = upgradeCanvasObj.transform.Find("UserGemText").GetComponent<Text>();
        if (!itemInfoTxt) itemInfoTxt = upgradeCanvasObj.transform.Find("ItemInfoText").GetComponent<Text>();
        if (!userNickTxt) userNickTxt = upgradeCanvasObj.transform.Find("UserNameText").GetComponent<Text>();
        if (!userLvTxt) userLvTxt = upgradeCanvasObj.transform.Find("UserNameText").GetComponent<Text>();

        if (!messageBoxObj) messageBoxObj = upgradeCanvasObj.transform.Find("MessageBox").gameObject;
        //if (!rankingPanelObj) rankingPanelObj = upgradeCanvasObj.transform.Find("RankingPanel").gameObject;
        //if (!upgradePanelObj) upgradePanelObj = upgradeCanvasObj.transform.Find("UpgradePanel").gameObject;
        
        for (int ii = 0; ii < itemCostTxtArr.Length; ii++)
        {
            if (!itemCostTxtArr[ii])
                itemCostTxtArr[ii] = upgradeCanvasObj.transform.Find("Item" + (ii + 1) + 
                    "Btn").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            if (!checkImg[ii])
                checkImg[ii] = upgradeCanvasObj.transform.Find("Item" + (ii + 1) +
                    "Btn").transform.GetChild(1).GetComponent<Image>();
        }

        //if (!itemCostTxtArr[0]) itemCostTxtArr[0] = upgradeCanvasObj.transform.Find("Item" + 1 + "Btn").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();

        #endregion


        CheckGameName();

    }

    void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        for (int ii = 0; ii < isItemChecked.Length; ii++)
        {
            if (isItemChecked[ii]) isItemChecked[ii] = false;
            checkImg[ii].gameObject.SetActive(isItemChecked[ii]);
        }

        for (int ii = 0; ii < itemCostTxtArr.Length; ii++)
            itemCostTxtArr[ii].text = itemCostArr[ii].ToString();

        if (backBtn != null) backBtn.onClick.AddListener(() => SceneManager.LoadScene("LobbyScene"));

        if (seeRankingBtn != null) seeRankingBtn.onClick.AddListener(() => rankingPanelObj.SetActive(true));

        if (bonusUpBtn != null)
            bonusUpBtn.onClick.AddListener(() =>
            {

            });

        if (feverUpBtn != null)
            feverUpBtn.onClick.AddListener(() =>
            {

            });

        if (superUpBtn != null)
            superUpBtn.onClick.AddListener(() =>
            {

            });




    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    void CheckGameName()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            if (GlobalValue.g_YSMSTutSkipYN == 1) isTutorialSkipOnOff = true;
            gameName = "YSMS";
            gameNameTxt.text = "삼촌의 니편내편";
            itemCostArr = new int[4] { 700, 900, 1100, 1 };
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            if (GlobalValue.g_SDJRTutSkipYN == 1) isTutorialSkipOnOff = true;
            gameName = "SDJR";
            gameNameTxt.text = "엄마의 삼단정리";
            itemCostArr = new int[4] { 1100, 900, 800, 1 };
        }
        SetItemInfo(GlobalValue.g_GameKind);
    }
    void SetItemInfo(GameKind gKind)
    {
        if (gKind == GameKind.YSMS)
            itemInfo = new string[4] { "[추가시간]타이머 5초 추가!", "[변신]일정 콤보마다 한 종류로 통일!",
            "[스피드]틀렸을 때 더 빨리 회복돼요~", "[슈퍼피버시작]100콤보부터 시작하는 슈퍼피버~" };
        else if (gKind == GameKind.SDJR)
            itemInfo = new string[4] { "[지우개]모든 블록을 전부 다 지워줘요~!", "[뿅망치]누르는 모든 블록을 뿅뿅 없애줘요~",
            "[한줄뿅]블록 한줄을 몽땅 없애줘요~", "[슈퍼피버시작]100콤보부터 시작하는 슈퍼피버~" };
    }



}
