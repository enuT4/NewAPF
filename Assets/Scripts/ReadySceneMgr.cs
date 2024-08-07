using JetBrains.Annotations;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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
    internal Button[] itemBtns = new Button[4];
    [SerializeField] internal Button item1Btn;
    [SerializeField] internal Button item2Btn;
    [SerializeField] internal Button item3Btn;
    [SerializeField] internal Button item4Btn;
    internal GameObject[] itemBtnSpriteGroupObj = new GameObject[4];
    string[] itemInfo = new string[4];
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

    int tempTotalGold = 0;
    int temtTotalGem = 0;
    int goldVal = 0;
    int gemVal = 0;

    [SerializeField] internal GameObject upgradePanelObj;
    UpgradePanel ugPanel;
    [SerializeField] internal GameObject upgradeCanvasObj;
    [SerializeField] internal GameObject rankingPanelObj;
    [SerializeField] internal GameObject messageBoxObj;
    MessageBox msgBox;

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

    ReadySceneMgr inst;

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
        for (int ii = 0; ii < itemBtns.Length; ii++) itemBtns[ii] = upgradeCanvasObj.transform.Find("Item" + (ii + 1) + "Btn").GetComponent<Button>();
        for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++) itemBtnSpriteGroupObj[ii] = itemBtns[ii].transform.GetChild(2).gameObject;
        if (!charShopBtn) charShopBtn = upgradeCanvasObj.transform.Find("CharShopBtn").GetComponent<Button>();
        if (!gameStartBtn) gameStartBtn = upgradeCanvasObj.transform.Find("GameStartBtn").GetComponent<Button>();

        if (!gameNameTxt) gameNameTxt = upgradeCanvasObj.transform.Find("Label").transform.GetChild(0).GetComponent<Text>();
        if (!userGoldTxt) userGoldTxt = upgradeCanvasObj.transform.Find("UserGoldText").GetComponent<Text>();
        if (!userGemTxt) userGemTxt = upgradeCanvasObj.transform.Find("UserGemText").GetComponent<Text>();
        if (!itemInfoTxt) itemInfoTxt = upgradeCanvasObj.transform.Find("ItemInfoText").GetComponent<Text>();
        if (!userNickTxt) userNickTxt = upgradeCanvasObj.transform.Find("UserNameText").GetComponent<Text>();
        if (!userLvTxt) userLvTxt = upgradeCanvasObj.transform.Find("UserNameText").GetComponent<Text>();

        if (!messageBoxObj) messageBoxObj = upgradeCanvasObj.transform.Find("MessageBox").gameObject;
        if (messageBoxObj != null) msgBox = messageBoxObj.GetComponent<MessageBox>();
        if (!rankingPanelObj) rankingPanelObj = upgradeCanvasObj.transform.Find("RankingPanel").gameObject;
        if (!upgradePanelObj) upgradePanelObj = upgradeCanvasObj.transform.Find("UpgradePanel").gameObject;
        if (upgradePanelObj != null) ugPanel = upgradePanelObj.GetComponent<UpgradePanel>();
        
        for (int ii = 0; ii < itemCostTxtArr.Length; ii++)
        {
            if (!itemCostTxtArr[ii])
                itemCostTxtArr[ii] = upgradeCanvasObj.transform.Find("Item" + (ii + 1) + 
                    "Btn").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            if (!checkImg[ii])
                checkImg[ii] = upgradeCanvasObj.transform.Find("Item" + (ii + 1) +
                    "Btn").transform.GetChild(1).GetComponent<Image>();
        }

        for (int ii = 0; ii < itemBtns.Length; ii++)
        {
            if (!itemCostTxtArr[ii]) 
                itemCostTxtArr[ii] = itemBtns[ii].transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        }

        #endregion

        inst = this;

        
    }

    void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        CheckGameName();
        UpgradeUpdate();

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
            bonusUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Bonus); });
        if (feverUpBtn != null)
            feverUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Fever); });
        if (superUpBtn != null)
            superUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Super); });
        

        for (int ii = 0; ii < itemBtns.Length; ii++)
        {
            itemBtns[ii].onClick.AddListener(() =>
            {
                ItemBtnFunc(ii);
                CalculateItemCost();
            });
        }

        if (charShopBtn != null)
            charShopBtn.onClick.AddListener(() =>
            {
                messageBoxObj.SetActive(true);
                msgBox.SetMessageText("미구현 알림", "다음 업데이트를 기다려주세요 ㅠ0ㅠ", MessageState.OK);
            });

        if (gameStartBtn != null)
            gameStartBtn.onClick.AddListener(CheckGameStart);

        userNickTxt.text = GlobalValue.g_Nickname;

        if (GlobalValue.g_IsRiceTimerStart == 1) isRiceTimerStart = true;

        goldVal = GlobalValue.g_UserGold;
        gemVal = GlobalValue.g_UserGem;
        tempYMD = GlobalValue.g_RiceCheckDate;

        GetMyRanking(GlobalValue.g_GameKind);
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetMyRanking(GlobalValue.g_GameKind);
        }
    }

    void UpBtnFunc(UpgradeKind ugKind)
    {
        ugPanel.ugKind = ugKind;
        upgradePanelObj.SetActive(true);
    }

    void CheckRiceCount()
    {
        for (int ii = 0; ii < 5; ii++)
            fillArr[ii] = 0;

        if (GlobalValue.g_RiceCount < 5)
        {
            for (int ii = 0; ii < 5; ii++)
                fillArr[ii] = GlobalValue.g_RiceCheckTime + riceFillTimeSec * (ii + 1);
        }
    }

    void CheckGameName()
    {
        for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
        {
            for (int jj = 0; jj < itemBtnSpriteGroupObj[ii].transform.childCount; jj++)
                itemBtnSpriteGroupObj[ii].transform.GetChild(jj).gameObject.SetActive(false);
        }

        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            if (GlobalValue.g_YSMSTutSkipYN == 1) isTutorialSkipOnOff = true;
            gameName = "YSMS";
            gameNameTxt.text = "삼촌의 니편내편";
            itemCostArr = new int[4] { 700, 900, 1100, 1 };
            
            for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
                itemBtnSpriteGroupObj[ii].transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            if (GlobalValue.g_SDJRTutSkipYN == 1) isTutorialSkipOnOff = true;
            gameName = "SDJR";
            gameNameTxt.text = "엄마의 삼단정리";
            itemCostArr = new int[4] { 1100, 900, 800, 1 };

            for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
                itemBtnSpriteGroupObj[ii].transform.GetChild(1).gameObject.SetActive(true);
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

    void ItemBtnFunc(int itemNum)
    {
        isItemChecked[itemNum] = !isItemChecked[itemNum];

        if (isItemChecked[itemNum])
            itemInfoTxt.text = itemInfo[itemNum];
        else
            itemInfoTxt.text = "아이템을 선택해서 플레이 할 수 있어요~";
    }

    void CalculateItemCost()
    {
        tempTotalGold = 0;
        temtTotalGem = 0;
        for (int ii = 0; ii < 3; ii++)
        {
            if (isItemChecked[ii]) tempTotalGold += itemCostArr[ii];
        }
        if (isItemChecked[3]) temtTotalGem = itemCostArr[3];

        goldVal = GlobalValue.g_UserGold - tempTotalGold;
        gemVal = GlobalValue.g_UserGem - temtTotalGem;
        userGoldTxt.text = goldVal.ToString();
        userGemTxt.text = gemVal.ToString();

        for (int ii = 0; ii < 4; ii++)
            checkImg[ii].gameObject.SetActive(isItemChecked[ii]);
    }

    public void UpgradeUpdate()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            GlobalValue.YSMSUpgradeAmount();
            UpgradeAmountUpdate(GlobalValue.g_YSMSUpgradeLv[0], GlobalValue.g_YSMSUpgradeLv[1], GlobalValue.g_YSMSUpgradeLv[2]);
            UpgradeLevelUpdate(GlobalValue.g_YSMSUpgradeLv[0], GlobalValue.g_YSMSUpgradeLv[1], GlobalValue.g_YSMSUpgradeLv[2]);
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            GlobalValue.SDJRUpgradeAmount();
            UpgradeAmountUpdate(GlobalValue.g_SDJRUpgradeLv[0], GlobalValue.g_SDJRUpgradeLv[1], GlobalValue.g_SDJRUpgradeLv[2]);
            UpgradeLevelUpdate(GlobalValue.g_SDJRUpgradeLv[0], GlobalValue.g_SDJRUpgradeLv[1], GlobalValue.g_SDJRUpgradeLv[2]);
        }
    }

    void UpgradeLevelUpdate(int bonusLv, int feverLv, int superLv)
    {
        if (bonusLv == 15) bonusLvTxt.text = "Lv Max";
        else bonusLvTxt.text = "Lv " + bonusLv.ToString();

        if (feverLv == 15) feverLvTxt.text = "Lv Max";
        else feverLvTxt.text = "Lv " + feverLv.ToString();

        if (superLv == 15) superLvTxt.text = "Lv Max";
        else superLvTxt.text = "Lv " + superLv.ToString();
    }

    void UpgradeAmountUpdate(int bonusLv, int feverLv, int superLv)
    {
        if (bonusLv == 0)
        {
            bonusAmTxt.gameObject.SetActive(false);
            bonusUpBtn.image.sprite = buttonSprite[0];
        }
        else
        {
            bonusUpBtn.image.sprite = buttonSprite[1];
            bonusAmTxt.text = "+" + GlobalValue.bonusAmount[bonusLv].ToString();
            bonusAmTxt.gameObject.SetActive(true);
        }

        if (feverLv == 0)
            feverUpBtn.image.sprite = buttonSprite[0];
        else
            feverUpBtn.image.sprite = buttonSprite[1];
        feverAmTxt.text = GlobalValue.feverAmount[feverLv].ToString("F1");
        feverAmTxt.gameObject.SetActive(true);

        if (superLv == 0)
        {
            superAmTxt.gameObject.SetActive(false);
            superUpBtn.image.sprite = buttonSprite[0];
        }
        else
        {
            superUpBtn.image.sprite = buttonSprite[1];
            superAmTxt.text = GlobalValue.superAmount[superLv].ToString("F1") + "초";
            superAmTxt.gameObject.SetActive(true);
        }
    }

    void GetMyRanking(GameKind gKind)
    {
        if (GlobalValue.g_UniqueID == "") return;
        if (gameName == "") return;

        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = gameName + "BestScore",
            MaxResultsCount = 1,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            request,
            (result) =>
            {
                if (0 < result.Leaderboard.Count)
                {
                    var curBoard = result.Leaderboard[0];

                    if (gKind == GameKind.YSMS)
                    {
                        GlobalValue.g_MyYSMSRank = curBoard.Position + 1;
                        GlobalValue.g_YSMSBestScore = curBoard.StatValue;
                        myRank = GlobalValue.g_MyYSMSRank;
                    }
                    else if (gKind == GameKind.SDJR)
                    {
                        GlobalValue.g_MySDJRRank = curBoard.Position + 1;
                        GlobalValue.g_SDJRBestScore = curBoard.StatValue;
                        myRank = GlobalValue.g_MySDJRRank;
                    }
                }

                if (myRank != 0 && myRankTxt != null)
                    myRankTxt.text = myRank + "위\n" + "<color=#bb3e01>순위보기</color>";
            },

            (error) =>
            {
                Debug.Log("등수 가져오기 실패" + error.GenerateErrorReport());
            }
            );
    }

    void CheckGameStart()
    {
        if (goldVal < 0 || gemVal < 0)
        {
            messageBoxObj.SetActive(true);
            msgBox.SetMessageText("재화 부족 알림", "재화가 부족해요 ㅠ0ㅠ", MessageState.OK);
        }
        else if (GlobalValue.g_RiceCount <= 0)
        {
            messageBoxObj.SetActive(true);
            msgBox.SetMessageText("밥 부족 알림", "밥이 부족해요 ㅠ0ㅠ", MessageState.OK);
        }
        else
        {
            GlobalValue.g_RiceCount--;

            if (GlobalValue.g_RiceCount == 4)
            {
                checkTime = DateTime.Now;
                GlobalValue.g_RiceCheckTime = CalculateDatetoSec(checkTime);
                GlobalValue.g_RiceCheckDate = CalculateYMDtoNum(checkTime);
                GlobalValue.g_IsRiceTimerStart = 1;
                isRiceTimerStart = true;
                CheckRiceCount();
                Debug.Log(GlobalValue.g_RiceCheckDate + " : " + GlobalValue.g_RiceCheckTime);
            }

            NetworkMgr.inst.PushPacket(PacketType.UserRice);

            gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();
            GlobalValue.g_UserGem = gemVal;
            GlobalValue.g_UserGold = goldVal;

            NetworkMgr.inst.PushPacket(PacketType.UserMoney);

            if (!isTutorialSkipOnOff)
                SceneManager.LoadScene("TutorialScene");
            else
                SceneManager.LoadScene(gameName + "InGameScene");
        }


    }

    int CalculateYMDtoNum(DateTime date, int tempYMD = 0)
    {
        int tempNum = (date.Year % 100) * 10000 + date.Month * 100 + date.Day;
        if (tempYMD != tempNum && tempYMD != 0)
            isDayChange = true;
        return tempNum;
    }

    int CalculateDatetoSec(DateTime date)
    {
        int tempSec = date.Hour * 3600 + date.Minute * 60 + date.Second;
        if (isDayChange)
        {
            tempSec += (CalculateYMDtoNum(date) - GlobalValue.g_RiceCheckDate) * 86400;
            isDayChange = false;
        }

        return tempSec;
    }

    void CalculateTimeFunc()
    {
        if (!isRiceTimerStart)
        {
            gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();
            return;
        }

        currYMD = CalculateYMDtoNum(DateTime.Now, tempYMD);
        currTimeSec = CalculateDatetoSec(DateTime.Now);

        if (fillArr[0] <= currTimeSec)
        {
            for (int ii = 0; ii < 4; ii++)
            {
                if (fillArr[ii] <= currTimeSec && currTimeSec < fillArr[ii + 1])
                {
                    GlobalValue.g_RiceCount += ii + 1;
                    GlobalValue.g_RiceCheckTime = fillArr[ii];
                }
            }
            if (fillArr[4] <= currTimeSec)
                GlobalValue.g_RiceCount += 5;

            if (GlobalValue.g_RiceCount >= 5)
            {
                GlobalValue.g_RiceCount = 5;
                GlobalValue.g_IsRiceTimerStart = 0;
                GlobalValue.g_RiceCheckTime = -1;
                GlobalValue.g_RiceCheckDate = 0;
                isRiceTimerStart = false;
            }
            CheckRiceCount();
            NetworkMgr.inst.PushPacket(PacketType.UserRice);
        }

        if (GlobalValue.g_RiceCount == 0)
        {
            timerMin = (fillArr[0] - currTimeSec) / 60;
            timerSec = (fillArr[0] - currTimeSec) % 60;
            gameStartRiceTxt.text = timerMin.ToString() + ":" + timerSec.ToString("D2");
        }
        else
            gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();

        tempYMD = currYMD;
    }

}
