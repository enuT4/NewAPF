using Enut4LJR;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ReadySceneMgr : MonoBehaviour
{
    public struct GameReadyInfo
    {
        public string gameName;
        public string gameNameText;
        public int[] itemCostArray;
        public string[] itemInfo;
        public int tutorialSkipYesNo;
        public int activeChildIdx;
    }

    [SerializeField] internal Button backBtn;
    [SerializeField] internal Button seeRankingBtn;
    [SerializeField] internal Text myRankTxt;
    internal GameObject bgImgObj;
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
    [SerializeField]internal Button[] itemBtns = new Button[4];
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
    static Dictionary<GameKind, GameReadyInfo> gameConfigs = null;

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
    [SerializeField] internal GameObject rankingPanelObj;
    [SerializeField] internal GameObject messageBoxObj;
    MessageBox msgBox;

    bool isTutorialSkipOnOff = false;
    string gameName = "";
    float bgmFadeOutTimer = 0.0f;

    [Header("-------- Rice Count --------")]
    [SerializeField] internal Text gameStartRiceTxt;
    int riceFillTimeSec = 300;
    int zeroRiceTimer = 0;
    float tempRiceTimer = 0.0f;
    int tempRiceCount = 0;
    long timeSinceRiceFillTime = 0;
    long presentServerTime = 0;
    long filledRiceCount = 0;
    float remainRiceTime = 0.0f;

    public static ReadySceneMgr inst;


    [SerializeField] internal Button riceBtn;


    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        #region UI 연결
        if (!bgImgObj) bgImgObj = GameObject.Find("Canvas").transform.Find("BGImg").gameObject;
        if (!backBtn) backBtn = bgImgObj.transform.Find("BackBtn").GetComponent<Button>();
        if (!seeRankingBtn) seeRankingBtn = bgImgObj.transform.Find("SeeRankingBtn").GetComponent<Button>();
        if (!myRankTxt) myRankTxt = seeRankingBtn.transform.GetChild(0).GetComponent<Text>();

        if (!bonusUpBtn) bonusUpBtn = bgImgObj.transform.Find("BonusUpBtn").GetComponent<Button>();
        if (!bonusAmTxt) bonusAmTxt = bonusUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!bonusLvTxt) bonusLvTxt = bonusUpBtn.transform.GetChild(2).GetComponent<Text>();
        if (!feverUpBtn) feverUpBtn = bgImgObj.transform.Find("FeverUpBtn").GetComponent<Button>();
        if (!feverAmTxt) feverAmTxt = feverUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!feverLvTxt) feverLvTxt = feverUpBtn.transform.GetChild(2).GetComponent<Text>();
        if (!superUpBtn) superUpBtn = bgImgObj.transform.Find("SuperUpBtn").GetComponent<Button>();
        if (!superAmTxt) superAmTxt = superUpBtn.transform.GetChild(1).GetComponent<Text>();
        if (!superLvTxt) superLvTxt = superUpBtn.transform.GetChild(2).GetComponent<Text>();

        if (!item1Btn) item1Btn = bgImgObj.transform.Find("Item1Btn").GetComponent<Button>();
        if (!item2Btn) item2Btn = bgImgObj.transform.Find("Item2Btn").GetComponent<Button>();
        if (!item3Btn) item3Btn = bgImgObj.transform.Find("Item3Btn").GetComponent<Button>();
        if (!item4Btn) item4Btn = bgImgObj.transform.Find("Item4Btn").GetComponent<Button>();
        for (int ii = 0; ii < itemBtns.Length; ii++)
            itemBtns[ii] = bgImgObj.transform.Find("Item" + (ii + 1) + "Btn").GetComponent<Button>();
        for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
            itemBtnSpriteGroupObj[ii] = itemBtns[ii].transform.GetChild(1).gameObject;
        if (!charShopBtn) charShopBtn = bgImgObj.transform.Find("CharShopBtn").GetComponent<Button>();
        if (!gameStartBtn) gameStartBtn = bgImgObj.transform.Find("GameStartBtn").GetComponent<Button>();
        if (!gameStartRiceTxt) gameStartRiceTxt = gameStartBtn.transform.GetChild(0).GetComponent<Text>();

        if (!gameNameTxt) gameNameTxt = bgImgObj.transform.Find("Label").transform.GetChild(0).GetComponent<Text>();
        if (!userGoldTxt) userGoldTxt = bgImgObj.transform.Find("UserGoldText").GetComponent<Text>();
        if (!userGemTxt) userGemTxt = bgImgObj.transform.Find("UserGemText").GetComponent<Text>();
        if (!itemInfoTxt) itemInfoTxt = bgImgObj.transform.Find("ItemInfoText").GetComponent<Text>();
        if (!userNickTxt) userNickTxt = bgImgObj.transform.Find("UserNameText").GetComponent<Text>();
        if (!userLvTxt) userLvTxt = bgImgObj.transform.Find("UserNameText").GetComponent<Text>();

        if (!messageBoxObj) messageBoxObj = GameObject.Find("Canvas").transform.Find("MessageBox").gameObject;
        if (messageBoxObj != null) msgBox = messageBoxObj.GetComponent<MessageBox>();
        if (!rankingPanelObj) rankingPanelObj = GameObject.Find("Canvas").transform.Find("RankingPanel").gameObject;
        if (!upgradePanelObj) upgradePanelObj = GameObject.Find("Canvas").transform.Find("UpgradePanel").gameObject;
        if (upgradePanelObj != null) ugPanel = upgradePanelObj.GetComponent<UpgradePanel>();

        for (int ii = 0; ii < itemCostTxtArr.Length; ii++)
        {
            if (!itemCostTxtArr[ii])
                itemCostTxtArr[ii] = bgImgObj.transform.Find("Item" + (ii + 1) + 
                    "Btn").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
            if (!checkImg[ii])
                checkImg[ii] = bgImgObj.transform.Find("Item" + (ii + 1) +
                    "Btn").transform.GetChild(2).GetComponent<Image>();
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
        if (!MusicManager.instance.IsMusicPlaying())
            MusicManager.instance.PlayMusic("MainBGM");

        SetDictionaryFunc();
        SetGameInfoDataFunc();
        UpgradeUpdate();
        UpdateRiceFunc();


        if (backBtn != null) backBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            SceneManager.LoadScene("LobbyScene");
        });
        if (seeRankingBtn != null) seeRankingBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            rankingPanelObj.SetActive(true);
        });

        if (bonusUpBtn != null)
            bonusUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Bonus); });
        if (feverUpBtn != null)
            feverUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Fever); });
        if (superUpBtn != null)
            superUpBtn.onClick.AddListener(() => { UpBtnFunc(UpgradeKind.Super); });


        if (itemBtns[0] != null)
            itemBtns[0].onClick.AddListener(() => { ItemBtnFunc(0); });
        if (itemBtns[1] != null)
            itemBtns[1].onClick.AddListener(() => { ItemBtnFunc(1); });
        if (itemBtns[2] != null)
            itemBtns[2].onClick.AddListener(() => { ItemBtnFunc(2); });
        if (itemBtns[3] != null)
            itemBtns[3].onClick.AddListener(() => { ItemBtnFunc(3); });


        if (charShopBtn != null)
            charShopBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                messageBoxObj.SetActive(true);
                msgBox.SetMessageText("미구현 알림", "다음 업데이트를 기다려주세요 ㅠ0ㅠ", MessageState.OK);
            });

        if (gameStartBtn != null)
            gameStartBtn.onClick.AddListener(CheckGameStart);

        userNickTxt.text = GlobalValue.g_Nickname;
        goldVal = GlobalValue.g_UserGold;
        gemVal = GlobalValue.g_UserGem;

        GetMyRanking(GlobalValue.g_GameKind);


        if (riceBtn != null)
            riceBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                GlobalValue.g_RiceCount++;
                gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();
            });
    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {

        if (bgmFadeOutTimer > 0.0f)
        {
            bgmFadeOutTimer -= Time.deltaTime * 0.5f;
            MusicManager.instance.SetVolume(bgmFadeOutTimer);
            if (bgmFadeOutTimer <= 0.0f)
            {
                bgmFadeOutTimer = 0.0f;
                GlobalValue.musicVolume = 1.0f;
                MusicManager.instance.StopMusic();
                SceneManager.LoadScene(gameName + "InGameScene");
            }
        }

        if (tempRiceTimer > 0.0f)
        {
            tempRiceTimer -= Time.deltaTime;
            if (tempRiceTimer < 0.0f)
            {
                tempRiceTimer = 0.0f;
                UpdateRiceFunc();
            }

            if (zeroRiceTimer != (int)tempRiceTimer)
            {
                zeroRiceTimer = (int)tempRiceTimer;
                UpdateRiceTextFunc();
            }
        }

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    UpdateRiceFunc();
        //}


        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    if (GlobalValue.g_Nickname == "운영자")
        //    {
        //        GlobalValue.g_YSMSBestScore = 0;
        //        GlobalValue.g_YSMSRegionScore = 0;
        //        GlobalValue.g_MyYSMSRank = -1;
        //        GlobalValue.g_SDJRBestScore = 0;
        //        GlobalValue.g_SDJRRegionScore = 0;
        //        GlobalValue.g_MySDJRRank = -1;
        //    }
        //}

    }

    void UpBtnFunc(UpgradeKind ugKind)
    {
        SoundManager.instance.PlayerSound("Button");
        ugPanel.ugKind = ugKind;
        upgradePanelObj.SetActive(true);
    }

    void ItemBtnFunc(int itemNum)
    {
        SoundManager.instance.PlayerSound("Button");
        isItemChecked[itemNum] = !isItemChecked[itemNum];
        checkImg[itemNum].gameObject.SetActive(isItemChecked[itemNum]);
        if (isItemChecked[itemNum])
            itemInfoTxt.text = itemInfo[itemNum];
        else
            itemInfoTxt.text = "아이템을 선택해서 플레이 할 수 있어요~";
        CalculateItemCost();
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

    void SetGameInfoDataFunc()
    {
        //체크 되어있는 상태라면 해제
        for (int ii = 0; ii < isItemChecked.Length; ii++)
        {
            if (isItemChecked[ii]) isItemChecked[ii] = false;
            checkImg[ii].gameObject.SetActive(isItemChecked[ii]);
        }

        for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
        {
            for (int jj = 0; jj < itemBtnSpriteGroupObj[ii].transform.childCount; jj++)
                itemBtnSpriteGroupObj[ii].transform.GetChild(jj).gameObject.SetActive(false);
        }

        if (!gameConfigs.TryGetValue(GlobalValue.g_GameKind, out var config))
        {
            Debug.LogError("오류");
            return;
        }

        isTutorialSkipOnOff = config.tutorialSkipYesNo == 1;
        gameName = config.gameName;
        gameNameTxt.text = config.gameNameText;
        itemInfo = config.itemInfo;
        itemCostArr = config.itemCostArray;
        for (int ii = 0; ii < itemBtnSpriteGroupObj.Length; ii++)
            itemBtnSpriteGroupObj[ii].transform.GetChild(config.activeChildIdx).gameObject.SetActive(true);

        

        for (int ii = 0; ii < itemCostTxtArr.Length; ii++)
            itemCostTxtArr[ii].text = itemCostArr[ii].ToString();
    }

    public void UpgradeUpdate()
    {
        switch (GlobalValue.g_GameKind)
        {
            case GameKind.YSMS:
                GlobalValue.YSMSUpgradeAmount();
                UpgradeAmountUpdate(GlobalValue.g_YSMSUpgradeLv[0], GlobalValue.g_YSMSUpgradeLv[1], GlobalValue.g_YSMSUpgradeLv[2]);
                UpgradeLevelUpdate(GlobalValue.g_YSMSUpgradeLv[0], GlobalValue.g_YSMSUpgradeLv[1], GlobalValue.g_YSMSUpgradeLv[2]);
                break;
            case GameKind.SDJR:
                GlobalValue.SDJRUpgradeAmount();
                UpgradeAmountUpdate(GlobalValue.g_SDJRUpgradeLv[0], GlobalValue.g_SDJRUpgradeLv[1], GlobalValue.g_SDJRUpgradeLv[2]);
                UpgradeLevelUpdate(GlobalValue.g_SDJRUpgradeLv[0], GlobalValue.g_SDJRUpgradeLv[1], GlobalValue.g_SDJRUpgradeLv[2]);
                break;
        }

        userGoldTxt.text = GlobalValue.g_UserGold.ToString();
        userGemTxt.text = GlobalValue.g_UserGem.ToString();
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
        if (bgmFadeOutTimer > 0.0f) return;

        SoundManager.instance.PlayerSound("Button");
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
                GlobalValue.g_RiceFillTime = GetServerTime() + riceFillTimeSec;

            NetworkMgr.inst.PushPacket(PacketType.UserRice);
            gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();

            GlobalValue.g_UserGem = gemVal;
            GlobalValue.g_UserGold = goldVal;

            NetworkMgr.inst.PushPacket(PacketType.UserMoney);

            SoundManager.instance.PlayerSound("GameStart");
            bgmFadeOutTimer = 1.0f;

            //튜토리얼은 일단 삭제
            //if (!isTutorialSkipOnOff)
            //    SceneManager.LoadScene("TutorialScene");
            //else
            //    SceneManager.LoadScene(gameName + "InGameScene");
        }


    }


    long GetServerTime()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + GlobalValue.g_ServerTimeOffSet;
    }

    void UpdateRiceFunc()
    {
        UpdateRiceCountFunc();
        UpdateRiceTextFunc();
    }

    void UpdateRiceCountFunc()
    {
        if (GlobalValue.g_RiceCount >= 5)
        {   //만약 밥이 이미 5개 이상이면 업데이트 필요 없음
            tempRiceTimer = 0;
            return;
        }

        presentServerTime = GetServerTime();

        if (presentServerTime < GlobalValue.g_RiceFillTime)
        {   //아직 밥이 차지 않았다면 남은 시간만 업데이트함
            remainRiceTime = GlobalValue.g_RiceFillTime - presentServerTime;
            tempRiceTimer = remainRiceTime;
        }
        else
        {
            timeSinceRiceFillTime = presentServerTime - GlobalValue.g_RiceFillTime;
            filledRiceCount = timeSinceRiceFillTime / riceFillTimeSec + 1;      // +1 : RiceFillTime이 지났다면 기본적으로 한개는 찼다는 뜻
            GlobalValue.g_RiceCount += filledRiceCount;
            if (GlobalValue.g_RiceCount >= 5)
            {
                GlobalValue.g_RiceCount = 5;
                tempRiceTimer = 0;      //변수 초기화
                remainRiceTime = 0;
            }
            else
            {   //밥이 다 안찼다면 찬 만큼 다음 밥이 차는 시간도 뒤로 정해짐
                GlobalValue.g_RiceFillTime += filledRiceCount * riceFillTimeSec;
                remainRiceTime = GlobalValue.g_RiceFillTime - presentServerTime;        //뒤로 미뤄진 시간을 기준으로 다시 계산
                tempRiceTimer = remainRiceTime;
            }
        }
    }

    void UpdateRiceTextFunc()
    {
        if (GlobalValue.g_RiceCount == 0)
            gameStartRiceTxt.text = ((int)remainRiceTime / 60).ToString() + ":" + ((int)remainRiceTime % 60).ToString("D2");
        else
            gameStartRiceTxt.text = GlobalValue.g_RiceCount.ToString();
        Debug.Log((zeroRiceTimer / 60).ToString() + ":" + (zeroRiceTimer % 60).ToString("D2"));
        //Debug.Log(filledRiceCount.ToString() + ":" + timeSinceRiceFillTime.ToString() + ":" + remainRiceTime.ToString() + ":" + ((int)remainRiceTime / 60).ToString() + ":" + ((int)remainRiceTime % 60).ToString());
    }

    static void SetDictionaryFunc()
    {
        if (gameConfigs != null)
        {
            Debug.Log("Configs 널 에러");
            return;
        }

        gameConfigs = new Dictionary<GameKind, GameReadyInfo>
        {
            {
                GameKind.YSMS, new GameReadyInfo{
                    gameName = "YSMS",
                    gameNameText = "삼촌의 니편내편",
                    itemCostArray = new int[] {700, 900, 1100, 1},
                    itemInfo = new string[4] { "[추가시간]타이머 5초 추가!", "[변신]일정 콤보마다 한 종류로 통일!",
            "[스피드]틀렸을 때 더 빨리 회복돼요~", "[슈퍼피버시작]100콤보부터 시작하는 슈퍼피버~" },
                    tutorialSkipYesNo = GlobalValue.g_YSMSTutSkipYN,
                    activeChildIdx = 0
                }
            },
            {
                GameKind.SDJR, new GameReadyInfo{
                    gameName = "SDJR",
                    gameNameText = "엄마의 삼단정리",
                    itemCostArray = new int[] {1100, 900, 800, 1},
                    itemInfo = new string[4] { "[지우개]모든 블록을 전부 다 지워줘요~!", "[뿅망치]누르는 모든 블록을 뿅뿅 없애줘요~",
            "[한줄뿅]블록 한줄을 몽땅 없애줘요~", "[슈퍼피버시작]100콤보부터 시작하는 슈퍼피버~" },
                    tutorialSkipYesNo = GlobalValue.g_SDJRTutSkipYN,
                    activeChildIdx = 1
                }
            }
        };

    }

}
