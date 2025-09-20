using Enut4LJR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SDJRIngameMgr : MonoBehaviour
{
    //[Header("-------- Game UI --------")]
    internal GameObject bgImgObj;
    internal Image guageBackImg;
    internal Image guageBarImg;
    internal Text scoreText;
    internal Image timebarImg;
    Vector2 timebarPos;
    internal Text timerText;
    internal Button spawnLineBtn;
    internal Image spawnTutorialImg;
    float screenScale = 0.0f;



    //시간 변수
    float gameTime = 60.0f;
    float maxTime = 65.0f;
    bool[] isTimeShowArray = new bool[5];
    bool isTimeUp = false;
    bool isGameStart = false;
    bool isGameOver = false;

    //쿨다운 변수
    float bonusTileCooldown = 0.0f;
    float bonusSpawnTime = 0.0f;
    float eraserTileCooldown = 0.0f;
    float hammerTileCooldown = 0.0f;
    float hammerDurationTime = 0.0f;
    bool isHammerOn = false;
    float lineTileCooldown = 0.0f;
    float badTile1Cooldown = 0.0f;
    float badTile2Cooldown = 0.0f;
    [SerializeField] float lineSpawnDelayTime = 0.0f;
    float autoLineSpawnCooldown = 0.0f;
    [HideInInspector] public bool switchBool = false;

    //타이머 변수
    public float badTile1EffectTimer = 0.0f;
    public float badTile2EffectTimer = 0.0f;

    //폭탄 변수
    float maxGuage = 100.0f;
    float currentGuage = 0.0f;
    float guageAmount = 0.0f;
    int bombCount = 0;
    internal Image plus10SecImg;
    float plusShowTimer = 0.0f;
    int bombGuageCount = 0;

    //타일 관련 변수
    GameObject tileGroupObj;
    float tileGroupVelocity = 2000.0f;
    bool isTileGroupMove = false;
    Vector2 tileGroupCurrentPos = Vector2.zero;
    bool isFirstLineSpawn = true;
    float localPosY = 0.0f;
    public bool isOnlySelect = false;
    GameObject selectedTileObj;
    int[] movedTileNumber = new int[2];
    string keyString = "";
    int tileCount = 0;
    List<GameObject>[] tileListArray = new List<GameObject>[3];     //타일 리스트
    List<int>[] tileIndexListArray = new List<int>[3];                //타일의 종류 정보를 담은 리스트
    List<int>[] deleteTileListArray = new List<int>[3];               //타일 제거 리스트
    GameObject explosionEffectObj;                                  //타일 제거 효과(폭발)
    GameObject effectGroupobj;
    [HideInInspector] public bool isReverse = false;
    [HideInInspector] public bool isTileHide = false;
    internal Image badTileShowImg;
    GameObject tempTileObj;
    SDJRTileNode tempTileNode;
    TileType tempTileType;

    //타일 삭제 관련 
    TileType tempDeleteTileType;
    GameObject tempExplosionEffectObj;

    KeyDirection selectedKeyDirection;
    GameObject tempTileSelectObj;
    SDJRTileNode tempTileSelectNode;
    TileType tempTileSelectType;
    int oldDirectionInt;
    int newDirectionInt;
    
    int tempTileIdx;
    bool[] isSpawn = new bool[7];

    SDJRTileNode dedupeTileNode1;
    SDJRTileNode dedupeTileNode2;


    //점수 변수
    int currentScore = 0;
    int gameLevel = 1;

    //아이템 변수
    bool isEraserItemBought = false;
    bool isHammerItemBought = false;
    bool isLineItemBought = false;
    bool isSuperFeverItemBought = false;
    GameObject duringHammerObj;

    //업그레이드 변수
    int bonusScore = 0;
    float feverRate = 1.0f;
    float superRate = 0.0f;

    //슈퍼피버 변수
    int fireCombo = 13;
    float superFeverTimer = 0.0f;
    bool isSuperFeverOn = false;
    bool isSuperFeverLineSpawn = false;
    int tempLevel = 0;

    //라인 스폰 변수
    GameObject tempLineSpawnTextObj;
    LineSpawnText tempLineSpawnText;
    int tempRandomIndex = -1;
    Vector2 lineSpawnTextPosition = new Vector2(0.0f, 10.0f);
    float lineSpawnCoolDown = 0.25f;
    float tempLineSpawnTime = 0.0f;

    //콤보 관련 변수
    GameObject tempComboObj;
    ComboText tempComboText;
    int comboCount = 0;
    int judgeComboCount = 0;


    //키 관련 변수
    enum KeyDirection
    {
        Left,
        Right,
        Down,
        DirectionCount,
    }
    private Action leftKeyAction;
    private Action rightKeyAction;
    private Action downKeyAction;
    private Coroutine reverseCoroutine;


    //[Header("-------- Pause Panel --------")]
    internal Button pauseBtn;
    internal GameObject pausePanelObj;
    internal GameObject readyPanelObj;
    internal GameObject gameoverPanelObj;


    public static SDJRIngameMgr inst;



    private void Awake()
    {
        if (!SoundManager.instance) SoundManager.instance.CallInstance();
        if (!MusicManager.instance) MusicManager.instance.CallInstance();

        if (!bgImgObj) bgImgObj = GameObject.Find("Canvas").transform.Find("BgImg").gameObject;
        if (!guageBackImg) guageBackImg = bgImgObj.transform.Find("GuageBack").GetComponent<Image>();
        if (!guageBarImg) guageBarImg = guageBackImg.transform.GetChild(0).GetComponent<Image>();
        if (!scoreText) scoreText = bgImgObj.transform.Find("ScoreTextBack").transform.GetChild(0).GetComponent<Text>();
        if (!timebarImg) timebarImg = bgImgObj.transform.Find("TimeBarObj").transform.GetChild(0).GetComponent<Image>();
        if (!timerText) timerText = bgImgObj.transform.Find("TimeBarObj").transform.GetChild(1).GetComponent<Text>();
        if (!spawnLineBtn) spawnLineBtn = bgImgObj.transform.Find("SpawnLineBtn").GetComponent<Button>();
        if (!spawnTutorialImg) spawnTutorialImg = spawnLineBtn.transform.GetChild(0).GetComponent<Image>();
        if (!plus10SecImg) plus10SecImg = bgImgObj.transform.Find("Plus10SecImg").GetComponent<Image>();
        if (!tileGroupObj) tileGroupObj = bgImgObj.transform.Find("TileSpawnGroup").gameObject;

        if (!pausePanelObj) pausePanelObj = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        if (!readyPanelObj) readyPanelObj = GameObject.Find("Canvas").transform.Find("ReadyPanel").gameObject;
        if (!gameoverPanelObj) gameoverPanelObj = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;


        inst = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (readyPanelObj != null && !readyPanelObj.activeSelf)
            readyPanelObj.SetActive(true);

        if (pausePanelObj != null && pausePanelObj.activeSelf)
            pausePanelObj.SetActive(false);

        if (plus10SecImg != null && plus10SecImg.gameObject.activeSelf)
            plus10SecImg.gameObject.SetActive(false);

        CheckItemFunc();
        CheckUpgradeRateFunc();
        SetReverseFunc(false);

        InitTimerFunc();
        InitTileListFunc();
        InitLineSpawnFunc();

        //스폰 초기화
        for (int ii = 0; ii < isSpawn.Length; ii++)
            isSpawn[ii] = false;

        if (spawnLineBtn != null)
            spawnLineBtn.onClick.AddListener(() => { SpawnLineFunc(true); });

        guageAmount = 3.2f;
        currentGuage = 0.0f;
        guageBarImg.fillAmount = currentGuage / maxGuage;
        screenScale = 1440.0f / Screen.width;
        isGameStart = true;
    }

    // Update is called once per frame 
    void Update()
    {
        if (isGameStart)
        {
            gameTime -= Time.deltaTime;
            timerText.text = ((int)gameTime).ToString();
            if (Time.timeScale != 0.0f && (0.0f < gameTime && gameTime <= 60.0f))
                timebarImg.transform.Translate(Vector3.left * Time.deltaTime * (1440.0f / 60.0f) / screenScale);

            CheckTimeStateFunc(gameTime);

            if (0.0f < plusShowTimer)
            {
                if (!plus10SecImg.gameObject.activeSelf)
                    plus10SecImg.gameObject.SetActive(true);
                plusShowTimer -= Time.deltaTime;
                if (plusShowTimer < 0.0f)
                {
                    plusShowTimer = 0.0f;
                    plus10SecImg.gameObject.SetActive(false);
                }
            }

            if (!isGameOver && 0.0f < lineSpawnDelayTime && 0.0f < gameTime)
            {
                lineSpawnDelayTime -= Time.deltaTime;
                if (lineSpawnDelayTime < 0.0f)
                    lineSpawnDelayTime = 0.0f;
            }

        }

        TileCooldownFunc();
        LineSpawnTranslateFunc(tileGroupCurrentPos.y);
        DurationTimeFunc();





        if (Input.GetKeyDown(KeyCode.Space))
            SpawnLineFunc(true);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            leftKeyAction?.Invoke();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            rightKeyAction?.Invoke();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            downKeyAction?.Invoke();


    }

    void CheckItemFunc()
    {
        if (ReadySceneMgr.isItemChecked[0])         //지우개 아이템
            isEraserItemBought = true;
        if (ReadySceneMgr.isItemChecked[0])         //해머 아이템
            isHammerItemBought = true;
        if (ReadySceneMgr.isItemChecked[0])         //한줄 아이템
            isLineItemBought = true;
        if (ReadySceneMgr.isItemChecked[0])         //슈퍼피버 아이템
            isSuperFeverItemBought = true;
    }

    void CheckUpgradeRateFunc()
    {
        GlobalValue.SDJRUpgradeAmount();
        bonusScore = GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0]];
        feverRate = GlobalValue.feverAmount[GlobalValue.g_SDJRUpgradeLv[1]];
        superRate = GlobalValue.superAmount[GlobalValue.g_SDJRUpgradeLv[2]];

        //보너스 레벨에 따른 스폰 주기 계산
        bonusSpawnTime = 20 - ((5 / 15) * GlobalValue.g_SDJRUpgradeLv[0]);
    }

    void InitTimerFunc()
    {
        //스폰 타이머들 초기화
        bonusTileCooldown = bonusSpawnTime;
        eraserTileCooldown = 39.0f;
        hammerTileCooldown = 47.0f;
        lineTileCooldown = 27.0f;
        badTile1Cooldown = UnityEngine.Random.Range(25.0f, 30.0f);
        badTile2Cooldown = UnityEngine.Random.Range(20.0f, 25.0f);
        autoLineSpawnCooldown = 3.0f;
        gameLevel = 1;
        currentScore = 0;
        comboCount = 0;
        bombCount = 0;
        tileCount = 0;
        scoreText.text = currentScore.ToString("N0");
        timebarPos = timebarImg.transform.position;
        superFeverTimer = 0.0f;
    }

    void InitTileListFunc()
    {   //타일 리스트 및 타일 제거 리스트 초기화
        for (int ii = 0; ii < 3; ii++)
        {
            if (tileListArray[ii] != null)
                tileListArray[ii].Clear();
            tileListArray[ii] = new List<GameObject>();

            if (deleteTileListArray[ii] != null)
                deleteTileListArray[ii].Clear();
            deleteTileListArray[ii] = new List<int>();

            if (tileIndexListArray[ii] != null)
                tileIndexListArray[ii].Clear();
            tileIndexListArray[ii] = new List<int>();
        }
    }

    void InitLineSpawnFunc()
    {
        for (int ii = 0; ii < 5; ii++)
        {
            for (int jj = 0; jj < 3; jj++)
                MakeTileNodeFunc(tileListArray[jj], tileIndexListArray[jj]);
        }
        localPosY = -135.0f * 5;
        tileGroupCurrentPos = new Vector2(0.0f, 585.0f + localPosY);

        for (int ii = 0; ii < 3; ii++)
            UpdateTileListFunc(tileListArray[ii], ii);
    }

    void UpdateTileListFunc(List<GameObject> tileList, int lineIndex)
    {
        lineIndex--;
        for (int ii = 0; ii < tileList.Count; ii++)
        {
            //tileList[ii].transform.SetParent(tileGroupObj.transform, false);
            tileList[ii].transform.localPosition = new Vector2(lineIndex * 480.0f, 540.0f - 135.0f * ii);
        }
    }

    void MakeTileNodeFunc(List<GameObject> tileList, List<int> tileIndexList)
    {
        tempTileObj = MemoryPoolManager.instance.GetObject("TileSpawnGroup");
        tempTileNode = tempTileObj.GetComponent<SDJRTileNode>();
        tempTileNode.tileType = ChooseTileType();
        tempTileNode.SetTileType(tempTileNode.tileType, gameLevel);
        tempTileObj.SetActive(true);

        //터질 타일로 스폰하지 않게 설정
        if (tileList != null && tileList.Count >= 2)
        {
            dedupeTileNode1 = tileList[0].GetComponent<SDJRTileNode>();
            dedupeTileNode2 = tileList[1].GetComponent<SDJRTileNode>();
            if (dedupeTileNode1.tileIdx == dedupeTileNode2.tileIdx &&
                tempTileNode.tileIdx == dedupeTileNode1.tileIdx)
                tempTileNode.SetTileType(tempTileNode.tileType, gameLevel, true, tempTileNode.tileIdx);
        }

        //tempTileNode.SetTileIndexFunc(tempTileNode.tileIdx);      //수정수정 -> 굳이 이함수를 넣지 않아도 될 것 같은데?
        if (tempTileNode == null) Debug.Log("이거");
        if (tileIndexList == null) Debug.Log("이거2");
        tileIndexList.Insert(0, tempTileNode.tileIdx);
        tileList.Insert(0, tempTileObj);
    }

    public TileType ChooseTileType()
    {
        tempTileIdx = -1;       //초기화
        for (int ii = 0; ii < isSpawn.Length; ii++)
        {
            if (isSpawn[ii])    //스폰될 준비가 되어있다
            {
                tempTileIdx = ii;
                isSpawn[ii] = false;
                break;
            }
            else
                continue;
        }
        if (tempTileIdx == 0) return TileType.Bonus;            //보너스
        else if (tempTileIdx == 1) return TileType.Special1;    //지우개
        else if (tempTileIdx == 2) return TileType.Special2;    //해머
        else if (tempTileIdx == 3) return TileType.Special3;    //한줄
        else if (tempTileIdx == 4) return TileType.Bad1;        //방해타일1
        else if (tempTileIdx == 5) return TileType.Bad2;        //방해타일2
        else if (tempTileIdx == 6) return TileType.Bomb;        //폭탄
        else return TileType.Normal;                            //일반타일
    }

    public void GameStartAfterReadyFunc()
    {
        isGameStart = true;
        if (isSuperFeverItemBought)
            comboCount = 100;
    }



    void SpawnLineFunc(bool isManual)
    {   //라인 스폰 함수
        if (pausePanelObj.activeSelf) return;
        if (0.0f < lineSpawnDelayTime) return;
        if (isGameOver || isTimeUp) return;



        if (isManual)
        {
            if (isFirstLineSpawn)
            {
                spawnTutorialImg.gameObject.SetActive(false);
                isFirstLineSpawn = false;
            }
            currentScore += 200;

            //점수 연출
            tempLineSpawnTextObj = MemoryPoolManager.instance.GetObject("LineSpawnTextGroup");
            tempLineSpawnTextObj.transform.localPosition = lineSpawnTextPosition;
            //tempLineSpawnText = tempLineSpawnTextObj.GetComponent<LineSpawnText>();
            //tempLineSpawnText.InitLineSpawnTextFunc();
            tempLineSpawnTextObj.SetActive(true);

            judgeComboCount = gameLevel + 2;
            autoLineSpawnCooldown = 4.0f - (gameLevel - 1) * 0.3f;
        }

        tempRandomIndex = UnityEngine.Random.Range(0, 3);
        switch (tempRandomIndex)
        {   //스폰이 준비된 스페셜 타일의 등장 위치를 랜덤하게 나타내주기 위해
            case 0:
                MakeTileNodeFunc(tileListArray[0], tileIndexListArray[0]);
                MakeTileNodeFunc(tileListArray[1], tileIndexListArray[1]);
                MakeTileNodeFunc(tileListArray[2], tileIndexListArray[2]);
                break;
            case 1:
                MakeTileNodeFunc(tileListArray[1], tileIndexListArray[1]);
                MakeTileNodeFunc(tileListArray[2], tileIndexListArray[2]);
                MakeTileNodeFunc(tileListArray[0], tileIndexListArray[0]);
                break;
            case 2:
                MakeTileNodeFunc(tileListArray[2], tileIndexListArray[2]);
                MakeTileNodeFunc(tileListArray[0], tileIndexListArray[0]);
                MakeTileNodeFunc(tileListArray[1], tileIndexListArray[1]);
                break;
        }
        for (int ii = 0; ii < 3; ii++)
            UpdateTileListFunc(tileListArray[ii], ii);

        localPosY = -135.0f * 4;
        tileGroupCurrentPos = new Vector2(0.0f, 585.0f + localPosY);
        isTileGroupMove = true;
        lineSpawnDelayTime = 0.2f;
    }

    void LineSpawnTranslateFunc(float currentPosY)
    {
        if (!isTileGroupMove) return;

        localPosY -= Time.deltaTime * tileGroupVelocity;
        if (localPosY <= -135.0f * 5)
        {
            localPosY = -135.0f * 5;
            isTileGroupMove = false;

            // 게임오버 판정
            //for (int ii = 0; ii < 3; ii++)
            //{
            //    if (tileListArray[ii].Count >= 12)
            //        isGameOver = true;
            //}
        }
        tileGroupCurrentPos = new Vector2(0.0f, 675.0f + localPosY);
        tileGroupObj.transform.localPosition = tileGroupCurrentPos;
    }

    void CheckTimeStateFunc(float gameTime)
    {

    }

    #region 쿨타임함수 모음

    void TileCooldownFunc()
    {
        //SpawnLineCooldownFunc();              //수정수정수정
        BonusTileCooldownFunc();
        EraserTileCooldownFunc();
        HammerTileCooldownFunc();
        LineTileCooldownFunc();
        BadTile1CooldownFunc();
        BadTile2CooldownFunc();
    }

    void SpawnLineCooldownFunc()
    {//자동으로 타일이 내려오는 시간을 조절하는 타이머
        if (isGameOver || gameTime <= 0.0f) return;

        if (0.0f < autoLineSpawnCooldown)
        {
            autoLineSpawnCooldown -= Time.deltaTime;
            if (autoLineSpawnCooldown < 0.0f)
            {
                SpawnLineFunc(false);
                autoLineSpawnCooldown = 4.0f - (gameLevel - 1) * 0.3f;     //Lv 1 ~ 6 : 4초 ~ 2.5초
            }
        }
    }

    void BonusTileCooldownFunc()
    {//보너스 타일 스폰 타이머
        if (GlobalValue.g_SDJRUpgradeLv[0] == 0) return;        //보너스 레벨을 1 이상 찍지 않았다면
        if (isSpawn[0]) return;                                 //보너스타일이 준비되지 않았다면
        if (0.0f < bonusTileCooldown)
        {
            bonusTileCooldown -= Time.deltaTime;
            if (bonusTileCooldown < 0.0f)
            {
                isSpawn[0] = true;
                bonusTileCooldown = bonusSpawnTime;
            }
        }
    }

    void EraserTileCooldownFunc()
    {//지우개 아이템 스폰 타이머
        if (!isEraserItemBought) return;                            //지우개 아이템을 사지 않았다면
        if (isSpawn[1]) return;                                 //지우개 아이템이 준비되지 않았다면
        if (0.0f < eraserTileCooldown)
        {
            eraserTileCooldown -= Time.deltaTime;
            if (eraserTileCooldown < 0.0f)
            {
                isSpawn[1] = true;
                eraserTileCooldown = 37.0f;
            }
        }
    }

    void HammerTileCooldownFunc()
    {//해머 아이템 스폰 타이머
        if (!isHammerItemBought) return;                            //해머 아이템을 사지 않았다면
        if (isSpawn[2]) return;                                 //해머 아이템이 준비되지 않았다면
        if (isHammerOn) return;                                 //해머 아이템이 발동중이라면
        if (0.0f < hammerTileCooldown)
        {
            hammerTileCooldown -= Time.deltaTime;
            if (hammerTileCooldown < 0.0f)
            {
                isSpawn[2] = true;
                hammerTileCooldown = 29.0f;
            }
        }
    }

    void LineTileCooldownFunc()
    {//한줄뿅 아이템 스폰 타이머
        if (isLineItemBought) return;                               //한줄뿅 아이템을 하지 않았다면
        if (isSpawn[3]) return;                                 //한줄뿅 아이템이 준비되지 않았다면
        if (0.0f < lineTileCooldown)
        {
            lineTileCooldown -= Time.deltaTime;
            if (lineTileCooldown < 0.0f)
            {
                isSpawn[3] = true;
                lineTileCooldown = 27.0f;
            }
        }
    }

    void BadTile1CooldownFunc()
    {//방해타일 1 스폰 타이머
        if (gameLevel < 4) return;                              //레벨 4부터 등장
        if (isSpawn[4]) return;                                 //스폰 준비가 되지 않았다면
        if (0.0f < badTile1Cooldown)
        {
            badTile1Cooldown -= Time.deltaTime;
            if (badTile1Cooldown < 0.0f)
            {
                isSpawn[4] = true;
                badTile1Cooldown = UnityEngine.Random.Range(25.0f, 30.0f);
            }
        }

    }

    void BadTile2CooldownFunc()
    {//방해타일 2 스폰 타이머
        if (gameLevel < 6) return;                              //레벨 6부터 등장
        if (isSpawn[5]) return;                                 //스폰 준비가 되지 않았다면
        if (0.0f < badTile2Cooldown)
        {
            badTile2Cooldown -= Time.deltaTime;
            if (badTile2Cooldown < 0.0f)
            {
                isSpawn[5] = true;
                badTile2Cooldown = UnityEngine.Random.Range(20.0f, 25.0f);
            }
        }
    }

    void DurationTimeFunc()
    {
        HammerTimerFunc();
        BadTileEffectTimerFunc();
        SuperFeverTimerFunc();
    }

    void HammerTimerFunc()
    {//해머 아이템 지속시간 타이머
        if (!isHammerItemBought) return;                        //해머 아이템을 사지 않았다면
        if (!isHammerOn) return;                                //해머 아이템이 발동되지 않았다면
        if (0.0f < hammerDurationTime)
        {
            hammerDurationTime -= Time.deltaTime;
            if (hammerDurationTime < 0.0f)
            {
                isHammerOn = false;
                //해머 아이템 파괴 함수                          ///수정수정수정수정
                if (superFeverTimer <= 0.0f)
                {
                    if (comboCount < 13)
                        fireCombo = 13;
                    else
                        fireCombo = comboCount + 20;
                }
                isSpawn[2] = false;
                //hammerDurationTime = 2.5f;                    //해머타일이 터지면 그때 적용해도 될듯
            }
        }
    }

    void BadTileEffectTimerFunc()
    {
        //방해타일 1
        if (0.0f < badTile1EffectTimer)
        {
            badTile1EffectTimer -= Time.deltaTime;
            if (badTile1EffectTimer < 0.0f)
            {
                badTile1EffectTimer = 0.0f;
                isReverse = false;
            }
        }

        //방해타일 2
        if (0.0f < badTile2EffectTimer)
        {
            badTile2EffectTimer -= Time.deltaTime;
            if (badTile2EffectTimer < 0.0f)
            {
                badTile2EffectTimer = 0.0f;
                isTileHide = false;
                switchBool = true;
            }
        }
    }

    void SuperFeverTimerFunc()
    {
        if (0.0f < superFeverTimer)
        {
            superFeverTimer -= Time.deltaTime;
            if (superFeverTimer < 0.0f)
            {
                superFeverTimer = 0.0f;
                gameLevel = tempLevel;
                if (!isHammerOn)
                    fireCombo = comboCount + 41;
            }
        }
    }


    #endregion


    void SetReverseFunc(bool isReverse)
    {
        if (isReverse)
        {
            leftKeyAction = () => ActionFunc(KeyDirection.Right);
            rightKeyAction = () => ActionFunc(KeyDirection.Left);
        }
        else
        {
            leftKeyAction = () => ActionFunc(KeyDirection.Left);
            rightKeyAction = () => ActionFunc(KeyDirection.Right);
        }
        downKeyAction = () => ActionFunc(KeyDirection.Down);
    }

    void ActionFunc(KeyDirection direction)
    {
        if (isGameOver || isTimeUp) return;
        if (pausePanelObj.activeSelf) return;
        if (isHammerOn)
            HammerSelectFunc(direction);
        else
            TileSelectFunc(direction);
    }

    void TileSelectFunc(KeyDirection direction)
    {
        if (!isOnlySelect)
        {
            if (direction == KeyDirection.Left)
            {
                if (tileListArray[0].Count == 0) return;        //선택한 곳에 타일이 하나도 없다면 아무런 행동을 하지 않음
                tempTileSelectObj = tileListArray[0][tileListArray[0].Count - 1];
            }
            else if (direction == KeyDirection.Down)
            {
                if (tileListArray[1].Count == 0) return;
                tempTileSelectObj = tileListArray[1][tileListArray[1].Count - 1];
            }
            else if (direction == KeyDirection.Right)
            {
                if (tileListArray[2].Count == 0) return;
                tempTileSelectObj = tileListArray[2][tileListArray[2].Count - 1];
            }
            tempTileSelectNode = tempTileSelectObj.GetComponent<SDJRTileNode>();
            tempTileSelectType = tempTileSelectNode.tileType;
            if (tempTileSelectType == TileType.Bad1 || tempTileSelectType == TileType.Bad2 || tempTileSelectType == TileType.GameOver) return;
            //tempTileSelectNode.isSelected = true;
            tempTileSelectNode.SelectOrDeselectFunc(true);
            selectedTileObj = tempTileSelectObj;
            isOnlySelect = true;
        }
        else
        {
            tempTileSelectNode = selectedTileObj.GetComponent<SDJRTileNode>();
            if (selectedKeyDirection != direction)      //처음 선택한 키와 다른 쪽 키를 선택 -> 그쪽으로 이동
            {
                MoveTileFunc(selectedTileObj, direction);
                judgeComboCount--;
                if (judgeComboCount < 0)
                {
                    comboCount = 0;
                    judgeComboCount = gameLevel + 2;
                    fireCombo = 13;
                    isSuperFeverOn = false;
                }
            }
            else
                tempTileSelectNode.isSpecialTilemoved = false;
            isOnlySelect = false;
            tempTileSelectNode.SelectOrDeselectFunc(false);
            JudgeTileFunc();
        }
        selectedKeyDirection = direction;
    }

    void MoveTileFunc(GameObject selectedGameObject, KeyDirection direction)
    {
        movedTileNumber[0] = -1;
        movedTileNumber[1] = -1;
        newDirectionInt = SelectedDirectionToInt(direction);
        oldDirectionInt = SelectedDirectionToInt(selectedKeyDirection);
        tileListArray[oldDirectionInt].RemoveAt(tileListArray[oldDirectionInt].Count - 1);
        tileListArray[newDirectionInt].Add(selectedGameObject);
        movedTileNumber[0] = newDirectionInt;                           //SelectedTile의 열
        movedTileNumber[1] = tileListArray[newDirectionInt].Count - 1;  //SelectedTile의 행

        //tempTileNode = selectedGameObject.GetComponent<SDJRTileNode>();
        //if (tempTileNode.tileType != TileType.Normal)
        //    tempTileNode.isSpecialTilemoved = true;

        tileIndexListArray[oldDirectionInt].RemoveAt(tileIndexListArray[oldDirectionInt].Count - 1);
        tileIndexListArray[newDirectionInt].Add(tempTileNode.tileIdx);


        for (int ii = 0; ii < 3; ii++)
            UpdateTileListFunc(tileListArray[ii], ii);
    }

    int SelectedDirectionToInt(KeyDirection direction)
    {
        if (direction == KeyDirection.Left)
            return 0;
        else if (direction == KeyDirection.Down)
            return 1;
        else if (direction == KeyDirection.Right)
            return 2;
        else
            return -1;

    }

    void JudgeTileFunc()
    {
        if (isOnlySelect) return;
        if (isTileGroupMove) return;
        if (isSuperFeverLineSpawn) return;

        for (int ii = 0; ii < 3; ii++)              //삭제할 리스트 초기화
            deleteTileListArray[ii].Clear();

        //스페셜 타일들이 제대로 발동되지 않았을 때
        if (selectedTileObj != null)
        {
            JudgeSpecialTileFunc(selectedTileObj);
            JudgeNormalTileFunc(selectedTileObj, true);
        }
        else
            JudgeNormalTileFunc(selectedTileObj, false);

        //선정된 타일들 제거
        if (!(comboCount == fireCombo && !isSuperFeverOn) || !isHammerOn)
            DestroyTileFunc(selectedTileObj, tileListArray, deleteTileListArray);

        for (int ii = 0; ii < 3; ii++)
            UpdateTileListFunc(tileListArray[ii], ii);
    }

    void JudgeSpecialTileFunc(GameObject selectedTile)
    {
        tempTileType = selectedTile.GetComponent<SDJRTileNode>().tileType;
        if (tempTileType == TileType.Normal) return;                        //특수타일 경우만 고려

        if (tempTileType == TileType.Bomb)              //폭탄
        {
            bombGuageCount = 0;
            for (int ii = movedTileNumber[0] - 1; ii < movedTileNumber[0] + 2; ii++)
            {
                if (ii < 0 || ii > 2) continue;     //왼쪽이나 오른쪽일 때 out of range 방지
                for (int jj = movedTileNumber[1] - 2; jj < movedTileNumber[1] + 3; jj++)
                {
                    if (jj < 0 || jj > tileListArray[ii].Count) continue;
                    deleteTileListArray[ii].Add(jj);
                    bombGuageCount++;
                }
            }
        }
        else if (tempTileType == TileType.Bonus)        //보너스
        {
            if (movedTileNumber[1] != 0 && selectedTile != null)
            {
                tempTileIdx = tileIndexListArray[movedTileNumber[0]][movedTileNumber[1] - 1];
                for (int ii = 0; ii < 3; ii++)
                {
                    for (int jj = 0; jj < tileListArray[ii].Count; jj++)
                    {
                        if (tileListArray[ii][jj].GetComponent<SDJRTileNode>().tileIdx == tempTileIdx)
                            deleteTileListArray[ii].Add(jj);
                    }
                }
            }
            deleteTileListArray[movedTileNumber[0]].Add(movedTileNumber[1]);        //자신도 터지게
        }
        else if (tempTileType == TileType.Special1)     //지우개
        {
            bombGuageCount = 0;
            for (int ii = 0; ii < 3; ii++)
            {
                for (int jj = 0; jj < tileListArray[ii].Count; jj++)
                {
                    deleteTileListArray[ii].Add(jj);
                    bombGuageCount++;
                }
            }
            bombGuageCount /= 2;
        }
        else if (tempTileType == TileType.Special2)     //해머
        {
            deleteTileListArray[movedTileNumber[0]].Add(movedTileNumber[1]);        //자신도 터지게
            isHammerOn = true;

        }
        else if (tempTileType == TileType.Special3)     //한줄뿅
        {
            bombGuageCount = 0;
            for (int jj = 0; jj < tileListArray[movedTileNumber[0]].Count; jj++)
            {
                deleteTileListArray[movedTileNumber[0]].Add(jj);
                bombGuageCount++;
            }
            bombGuageCount /= 2;
        }
        comboCount++;
        judgeComboCount = gameLevel + 2;

        for(int ii = 0;ii<3;ii++)
            deleteTileListArray[ii] = deleteTileListArray[ii].Distinct().ToList();

        ComboTextFunc(comboCount);
    }

    void JudgeNormalTileFunc(GameObject selectedTile, bool isComboPlus)
    {
        //제거되어야 할 3라인의 타일들 선정
        for (int ii = 0; ii < 3; ii++)
        {
            if (tileListArray[ii].Count < 3) continue;      //타일이 3개보다 적다면 터질 타일이 없으므로 계산 필요x

            for (int jj = 0; jj < tileListArray[ii].Count - 2; jj++)        //리스트 한바퀴를 돌아서 제거해야할 타일 선정
            {
                if (tileIndexListArray[ii][jj] == tileIndexListArray[ii][jj + 1] &&
                    tileIndexListArray[ii][jj + 1] == tileIndexListArray[ii][jj + 2])
                {
                    if (jj >= 1)
                    {
                        if (tileIndexListArray[ii][jj - 1] == 12 ||
                            tileIndexListArray[ii][jj - 1] == 13 || 
                            tileIndexListArray[ii][jj - 1] == 14)
                            deleteTileListArray[ii].Add(jj - 1);            //터트린 타일 위에 방해타일이 있다면 같이 터지도록
                    }
                    deleteTileListArray[ii].Add(jj);                        //터질 타일을 리스트에 등록
                    deleteTileListArray[ii].Add(jj + 1);                    //이후 콜백함수 한꺼번에 터트리도록
                    deleteTileListArray[ii].Add(jj + 2);
                }
            }
            deleteTileListArray[ii] = deleteTileListArray[ii].Distinct().ToList();      //중복으로 등록된 타일 제거
        }

        if (isComboPlus)
        {   //아이템의 효과가 아니라 일반 타일을 옮겨 터트렸을 때 콤보를 카운트하도록
            if (isSelectedTileinDeleteList() && selectedTile.GetComponent<SDJRTileNode>().tileType == TileType.Normal)
            {
                comboCount++;
                judgeComboCount = gameLevel + 2;
                ComboTextFunc(comboCount);
            }
        }

    }

    bool isSelectedTileinDeleteList()
    {   // 본인이 선택하여 옮긴 타일이 터졌는지를 확인
        for (int jj = 0; jj < deleteTileListArray[movedTileNumber[0]].Count; jj++)
        {
            if (deleteTileListArray[movedTileNumber[0]][jj] == movedTileNumber[1])
                return true;
        }

        return false;
    }

    void DestroyTileFunc(GameObject selectedTile, List<GameObject>[] tileList, List<int>[] delTileList)
    {
        if (selectedTile != null)
        {
            tempTileSelectType = selectedTile.GetComponent<SDJRTileNode>().tileType;

            for (int ii = 0; ii < 3; ii++)
            {
                for (int jj = delTileList[ii].Count; jj >= 0; jj--)
                {
                    //본인이 터질 방해타일이면 나중에 한꺼번에 터뜨리기 위해 예외처리
                    tempDeleteTileType = tileList[ii][delTileList[ii][jj]].GetComponent<SDJRTileNode>().tileType;
                    if (tempTileSelectType != TileType.Normal || tempDeleteTileType == TileType.GameOver ||
                        tempDeleteTileType == TileType.Bad1 || tempDeleteTileType == TileType.Bad2)
                        continue;

                    //터뜨린 타일 좌우에 방해타일이 있다면 방해타일도 터트림
                    if (ii == 0 || ii == 2)
                    {   //양 끝은 가운데만 보면 된다
                        if (tileList[1].Count < delTileList[ii][jj])
                        {
                            tempDeleteTileType = tileList[1][delTileList[ii][jj]].GetComponent<SDJRTileNode>().tileType;
                            if (tempDeleteTileType == TileType.Bad1 || tempDeleteTileType == TileType.Bad2 || tempDeleteTileType == TileType.GameOver)
                            {
                                //DestroyTileFunc1();
                            }
                        }
                    }
                }


            }
        }
    }

    void DestroyTileFunc1(GameObject destroyTileObj, List<GameObject> destroyTargetList, int destroyTargetInt)
    {
        ExplosionEffectFunc(destroyTileObj);
        
        //해당 타일 파괴(objectreturn)
        //타일 리스트의 removeat
        tileCount++;
        AddScoreFunc();
    }

    void ExplosionEffectFunc(GameObject explodeTile)
    {

    }

    void AddScoreFunc()
    {

    }

    void ComboTextFunc(int combo)
    {

    }


    void HammerSelectFunc(KeyDirection direction)
    {
        if (direction == KeyDirection.Left)
        {
            Debug.Log("왼쪽");
        }
        else if (direction == KeyDirection.Right)
        {
            Debug.Log("오른쪽");
        }
        else if (direction == KeyDirection.Down)
        {

        }
    }

    public void ReverseForSeconds(float duration)
    {
        if (reverseCoroutine != null)
            StopCoroutine(reverseCoroutine);

        reverseCoroutine = StartCoroutine(ReverseCoroutine(duration));
    }

    private IEnumerator ReverseCoroutine(float duration)
    {
        SetReverseFunc(true);

        yield return new WaitForSeconds(duration);

        SetReverseFunc(false);
        reverseCoroutine = null;
    }
}


    
