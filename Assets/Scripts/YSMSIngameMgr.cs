using Enut4LJR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class YSMSIngameMgr : MonoBehaviour
{
    internal Image bgImg;
    float moveSpeed = 4.0f;

    [Header("-------- Game UI --------")]
    [SerializeField] internal Button leftBtn;
    [SerializeField] internal Button rightBtn;
    [SerializeField] internal Text scoreText;
    [SerializeField] internal Image guagebarbackImg;
    [SerializeField] internal Image guagebarImg;
    Vector3 timebarMaxPos;
    float timebarX;
    internal GameObject timerBarObj;
    [SerializeField] internal Image timebarImg;
    [SerializeField] internal Text timerText;

    [SerializeField] internal Button pauseBtn;
    [SerializeField] internal GameObject pausePanelObj;
    ComboText tempComboText;

    float gameTime = 10.0f;
    float maxTime = 60.0f;
    [HideInInspector] public int currentScore = 0;
    float scoreRate = 0.0f;

    float maxGuage = 100.0f;
    float currentGuage = 0.0f;
    [HideInInspector] public int bombCount = 0;
    float plusShowTimer = 0.0f;

    float falseTimer = 0.0f;
    float setFalseTime = 1.0f;
    float scale = 1.073366f;    //?
    int monsterCount = 0;

    bool initSpawnOnOff = false;
    bool[] isLevelLeft = new bool[2];

    int levelLeftIndex = 0;
    int levelRightIndex = 0;
    int leftorrightIndex = 0;
    int randomCharIndex = 0;

    [SerializeField] internal Image timeupImg;
    [SerializeField] internal Image plus10SecImg;

    [Header("-------- Combo Text --------")]
    GameObject comboTextObj;
    [SerializeField] internal GameObject comboClone;
    public int comboCount = 0;
    public float checkComboTimer = 0.0f;
    bool isBonusHit;
    bool isGameStart = false;

    [Header("-------- Char Spawn Setting --------")]
    [SerializeField] internal Transform charSpawnTr;
    [SerializeField] internal Transform crossroadTr;
    [SerializeField] internal GameObject showLeftCharGroupObj;
    [SerializeField] internal GameObject showRightCharGroupObj;
    [SerializeField] GameObject tempNodeObj;
    float[] posArray = new float[10];
    float[] scaleArray = new float[10];
    float[] speedArray = new float[10];
    int[] selectArray = new int[8];
    int[] leftArray = new int[3];
    int[] rightArray = new int[3];
    public int gameLevel = 1;
    Vector3 destinationPos = Vector3.zero;
    float newScale;
    float screenScale;
    YSMSCharNode firstCharNode;
    Canvas tempSortLayerCanvas;

    GameObject readyPanelObj;

    //아이템
    bool isSuperFeverOn = false;
    bool isTransformTimerOn = false;
    int randomTransformCount = 0;
    int transformCount = 0;
    float transformItemTime = 0.0f;
    float transformTimer = 0.0f;
    [SerializeField] internal Image transformItemImg;

    //업그레이드
    int bonusScore = 0;
    float feverRate = 1.0f;
    float superRate = 1.0f;
    bool isBonutHit = false;
    //Vector3 bonusTextPos;
    GameObject bonusTextObj;
    GameObject bonusClone;
    BonusText bonusTxt;

    //[Header("-------- GameOver --------")]
    //게임 오버
    GameObject gameoverPanelObj;
    int showTime = 0;
    [SerializeField] bool[] isTimeShowArray = new bool[5];
    GameObject tempTimerObj = null;
    public bool isGameOver = false;


    public static List<GameObject> spawnList = new List<GameObject>();


    public static YSMSIngameMgr inst;

    void Awake() => AwakeFunc();
    

    void AwakeFunc()
    {
        if (!SoundManager.instance) SoundManager.instance.CallInstance();
        if (!MusicManager.instance) MusicManager.instance.CallInstance();

        inst = this;

        if (!bgImg) bgImg = GameObject.Find("Canvas").transform.Find("BGImg").GetComponent<Image>();
        if (!charSpawnTr) charSpawnTr = bgImg.transform.Find("SpawnGroup").gameObject.transform;
        if (!crossroadTr) crossroadTr = bgImg.transform.Find("CrossroadPos").gameObject.transform;
        if (!showLeftCharGroupObj) showLeftCharGroupObj = bgImg.transform.Find("ShowLCharGroup").gameObject;
        if (!showRightCharGroupObj) showRightCharGroupObj = bgImg.transform.Find("ShowRCharGroup").gameObject;
        if (!guagebarbackImg) guagebarbackImg = bgImg.transform.Find("BombGuageBack").GetComponent<Image>();
        if (!guagebarImg) guagebarImg = guagebarbackImg.transform.GetChild(0).GetComponent<Image>();
        if (!comboTextObj) comboTextObj = bgImg.transform.Find("ComboSpawnGroup").gameObject;
        if (!bonusTextObj) bonusTextObj = bgImg.transform.Find("BonusTextSpawnGroup").gameObject;
        if (!plus10SecImg) plus10SecImg = bgImg.transform.Find("Plus10SecImg").GetComponent<Image>();
        if (!transformItemImg) transformItemImg = bgImg.transform.Find("TransformItemImg").GetComponent<Image>();

        if (!leftBtn) leftBtn = bgImg.transform.Find("LeftBtn").GetComponent<Button>();
        if (!rightBtn) rightBtn = bgImg.transform.Find("RightBtn").GetComponent<Button>();

        if (!pauseBtn) pauseBtn = bgImg.transform.Find("PauseBtn").GetComponent<Button>();
        if (!pausePanelObj) pausePanelObj = GameObject.Find("Canvas").transform.Find("PausePanel").gameObject;
        if (!readyPanelObj) readyPanelObj = GameObject.Find("Canvas").transform.Find("ReadyPanel").gameObject;
        if (!gameoverPanelObj) gameoverPanelObj = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;

        if (!timerBarObj) timerBarObj = bgImg.transform.Find("TimerBarObj").gameObject;
        if (!timebarImg) timebarImg = timerBarObj.transform.GetChild(0).GetComponent<Image>();
        if (!timerText) timerText = timerBarObj.transform.GetChild(1).GetComponent<Text>();
        if (!scoreText) scoreText = bgImg.transform.Find("ScoreText").GetComponent<Text>();
        if (!timeupImg) timeupImg = bgImg.transform.Find("TimeUpImg").GetComponent<Image>();
        //if (!tempposObj) tempposObj = charSpawnTr.GetChild(0).gameObject;

        //수정수정수정수정수정
        if (GlobalValue.g_GameKind != GameKind.YSMS)
        {
            Debug.Log("YSMS로 gamekind 변경");
            GlobalValue.g_GameKind = GameKind.YSMS;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckItemFunc();
        CheckUpgradeRateFunc();


        if (showLeftCharGroupObj != null)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                for (int jj = 0; jj < 8; jj++) 
                {
                    if(showLeftCharGroupObj.transform.GetChild(ii).transform.GetChild(jj).gameObject.activeSelf)
                        showLeftCharGroupObj.transform.GetChild(ii).transform.GetChild(jj).gameObject.SetActive(false);
                }
            }
        }

        if (showRightCharGroupObj != null)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                for (int jj = 0; jj < 8; jj++) 
                {
                    if(showRightCharGroupObj.transform.GetChild(ii).transform.GetChild(jj).gameObject.activeSelf)
                        showRightCharGroupObj.transform.GetChild(ii).transform.GetChild(jj).gameObject.SetActive(false);
                }
            }
        }

        if (pausePanelObj != null && pausePanelObj.activeSelf)
            pausePanelObj.SetActive(false);

        if (gameoverPanelObj != null && gameoverPanelObj.activeSelf)
            gameoverPanelObj.SetActive(false);

        if (readyPanelObj != null && !readyPanelObj.activeSelf)
            readyPanelObj.SetActive(true);

        if (timeupImg != null && timeupImg.gameObject.activeSelf)
            timeupImg.gameObject.SetActive(false);

        if (plus10SecImg != null && plus10SecImg.gameObject.activeSelf)
            plus10SecImg.gameObject.SetActive(false);

        if (transformItemImg != null && transformItemImg.gameObject.activeSelf)
            transformItemImg.gameObject.SetActive(false);
            

        if (isGameOver == true) isGameOver = false;

        InitSpawnFunc();
        timebarMaxPos = timebarImg.transform.position;
        timerText.text = ((int)gameTime).ToString();

        if (leftBtn != null)
            leftBtn.onClick.AddListener(LeftBtnFunc);
        if (rightBtn != null)
            rightBtn.onClick.AddListener(RightBtnFunc);

        for (int ii = 0; ii < speedArray.Length; ii++)
            speedArray[ii] = moveSpeed / (Mathf.Pow(scale, ii));

        gameLevel = 1;
        currentScore = 0;
        comboCount = 0;
        monsterCount = 0;
        scoreText.text = currentScore.ToString("N0");
        screenScale = 1440.0f / Screen.width;
        randomTransformCount = Random.Range(130, 200);

        for (int ii = 0; ii < isTimeShowArray.Length; ii++)
            isTimeShowArray[ii] = true;


        if (pauseBtn != null)
            pauseBtn.onClick.AddListener(() => PauseBtnFunc(true));
    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (!isGameOver)
        {
            if (!isGameStart) return;
            gameTime -= Time.deltaTime;
            timerText.text = ((int)gameTime).ToString();
            if (Time.timeScale != 0.0f && (0.0f < gameTime && gameTime <= 60.0f))
                timebarImg.transform.Translate(Vector3.left * Time.deltaTime * (1440.0f / 60.0f) / screenScale);

            CheckTimeStateFunc(gameTime);

            FalseTimerFunc();
            PlusShowTimerFunc();
            CheckComboTimerFunc();
            TransformTimerFunc();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                LeftBtnFunc();

            if (Input.GetKeyDown(KeyCode.RightArrow))
                RightBtnFunc();

        }
        else
        {
            gameoverPanelObj.SetActive(true);
            gameoverPanelObj.GetComponent<GameOverPanel>().TimeUpOrGameOver(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseBtnFunc(true);
        }



        if (Input.GetKeyDown(KeyCode.G))
        {
            screenScale = 1440.0f / Screen.width;
            Debug.Log(Screen.width + " : " + screenScale);
            Debug.Log(timebarImg.transform.localScale + "   :   " + timebarImg.rectTransform.sizeDelta);
        }
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    //for(int ii = 0;ii<spawnList.Count;ii++)
        //    //    Debug.Log(spawnList[ii].transform.position.y);
        //    //Debug.Log(spawnList[8].transform.position.y);
        //    Debug.Log(spawnList.Count + " : " + posArray.Length);
        //}

    }

    void InitChar()
    {   //무슨 캐릭터로 문제가 나올 것인지 정해주는 함수
        for (int ii = 0; ii < 2; ii++)
        {   //3, 5번째 캐릭터가 어느쪽으로 나올건지 정해줌
            int levelLeftorRight = Random.Range(0, 2);
            if (levelLeftorRight == 0)
                isLevelLeft[ii] = true;
            else
                isLevelLeft[ii] = false;
        }

        int bonusLevel = GlobalValue.g_YSMSUpgradeLv[0];
        if (bonusLevel == 0)
            selectArray[7] = 8;
        else
        {
            GlobalValue.g_GameKind = GameKind.YSMS;
            BonusText.inst.InitScore(GlobalValue.g_GameKind);
            if (1 <= bonusLevel && bonusLevel < 6)
            {   //보너스 캐릭터가 4번째에 등장
                if (isLevelLeft[0])
                {   //오른쪽
                    selectArray[4] = 8;
                    bonusTextObj.transform.position = showRightCharGroupObj.transform.GetChild(1).transform.position;
                }
                else
                {   //왼쪽
                    selectArray[1] = 8;
                    bonusTextObj.transform.position = showLeftCharGroupObj.transform.GetChild(1).transform.position;
                }
            }
            else if (7 <= bonusLevel && bonusLevel <= 10)
            {   //보너스 캐릭터가 3번째에 등장
                if (isLevelLeft[0])
                {
                    selectArray[1] = 8;
                    bonusTextObj.transform.position = showLeftCharGroupObj.transform.GetChild(1).transform.position;
                }
                else
                {
                    selectArray[4] = 8;
                    bonusTextObj.transform.position = showRightCharGroupObj.transform.GetChild(1).transform.position;
                }
            }
            else if (11 <= bonusLevel && bonusLevel <= 13)
            {   //오른쪽
                selectArray[3] = 8;
                bonusTextObj.transform.position = showRightCharGroupObj.transform.GetChild(0).transform.position;
            }
            else
            {   //왼쪽
                selectArray[0] = 8;
                bonusTextObj.transform.position = showLeftCharGroupObj.transform.GetChild(0).transform.position;
            }
        }

        int charCount = 7;
        bool isSameNum;
        for (int ii = 0; ii < charCount + 1; ii++) 
        {
            if (selectArray[ii] == 8) continue;
            while (true)
            {
                selectArray[ii] = Random.Range(1, charCount + 1);
                isSameNum = false;
                for (int jj = 0; jj < ii; jj++)
                {
                    if (selectArray[jj] == selectArray[ii])
                    {
                        isSameNum = true;
                        break;
                    }
                }
                if (!isSameNum) break;
            }
        }

        for (int ii = 0; ii < 3; ii++)
        {
            leftArray[ii] = selectArray[ii];
            rightArray[ii] = selectArray[3 + ii];
        }
    }

    void InitSpawnFunc()
    {
        InitChar();
        initSpawnOnOff = true;
        scaleArray[9] = 0.7f / scale;
        posArray[9] = posArray[8] + 1.0f;
        float charlineDistance = (charSpawnTr.position.y - crossroadTr.position.y) / 9.0f * -1;
        for (int ii = 0; ii < 9; ii++)
        {
            scaleArray[ii] = 0.7f + 0.5f / 9.0f * (8 - ii);
            posArray[ii] = charlineDistance * (8 - ii);
        }

        for (int ii = 0; ii < 9; ii++)
        {
            int leftorrightInt = Random.Range(0, 2);
            if (leftorrightInt == 0)
                SpawnChar(leftArray[0], posArray[8 - ii], scaleArray[8 - ii]);
            else
                SpawnChar(rightArray[0], posArray[8 - ii], scaleArray[8 - ii]);
        }
        ShowChar(leftArray[0], true, 1);
        ShowChar(rightArray[0], false, 1);

        levelLeftIndex = 1;
        initSpawnOnOff = false;


    }


    void SpawnChar(int index, float posRate, float scaleRate)
    {
        tempNodeObj = MemoryPoolManager.instance.GetObject("SpawnGroup");
        tempNodeObj.GetComponent<YSMSCharNode>().SetCharResource(index);
        tempNodeObj.transform.position = new Vector3(charSpawnTr.position.x, charSpawnTr.position.y + posRate, charSpawnTr.position.z);
        tempNodeObj.transform.localScale = new Vector2(scaleRate, scaleRate);

        if (initSpawnOnOff)
            spawnList.Insert(0, tempNodeObj);
        else
            spawnList.Add(tempNodeObj);
        
    }

    void ShowChar(int index, bool isLeft, int level)
    {
        level--;
        index--;
        if (isLeft)
            showLeftCharGroupObj.transform.GetChild(level).transform.GetChild(index).gameObject.SetActive(true);
        else
            showRightCharGroupObj.transform.GetChild(level).transform.GetChild(index).gameObject.SetActive(true);


    }

    void CheckItemFunc()
    {
        if (ReadySceneMgr.isItemChecked[0]) gameTime = 65.0f;
        if (ReadySceneMgr.isItemChecked[1]) transformItemTime = 3.0f;
        if (ReadySceneMgr.isItemChecked[2]) setFalseTime = 0.5f;
        if (ReadySceneMgr.isItemChecked[3]) isSuperFeverOn = true;
    }

    void CheckUpgradeRateFunc()
    {
        GlobalValue.YSMSUpgradeAmount();
        bonusScore = GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]];
        feverRate = GlobalValue.feverAmount[GlobalValue.g_YSMSUpgradeLv[1]];
        superRate = GlobalValue.superAmount[GlobalValue.g_YSMSUpgradeLv[2]];
    }

    public void GameStartAfterReadyFunc()
    {
        isGameStart = true;
        if (isSuperFeverOn)
            comboCount = 100;
    }

    void LeftBtnFunc()
    {
        if (pausePanelObj.activeSelf) return;
        if (isGameOver) return;
        if (0.0f < falseTimer) return;

        firstCharNode = spawnList[0].GetComponent<YSMSCharNode>();
        if (firstCharNode.isMove) return;

        if (firstCharNode.charImgIndex == leftArray[0] || firstCharNode.charImgIndex == leftArray[1] ||
            firstCharNode.charImgIndex == leftArray[2] || firstCharNode.charImgIndex == (int)YSMSCharType.YSMSBomb)
            ClassifyCorrectFunc(true);
        else
            ClassifyIncorrectFunc();
    }

    void RightBtnFunc()
    {
        if (pausePanelObj.activeSelf) return;
        if (isGameOver) return;
        if (0.0f < falseTimer) return;

        firstCharNode = spawnList[0].GetComponent<YSMSCharNode>();
        if (firstCharNode.isMove) return;

        if (firstCharNode.charImgIndex == rightArray[0] || firstCharNode.charImgIndex == rightArray[1] ||
            firstCharNode.charImgIndex == rightArray[2] || firstCharNode.charImgIndex == (int)YSMSCharType.YSMSBomb) 
            ClassifyCorrectFunc(false);
        else
            ClassifyIncorrectFunc();
    }

    void ClassifyCorrectFunc(bool isLeft)
	{
        if (firstCharNode.charImgIndex == (int)YSMSCharType.YSMSBomb)
            GameTimePlusFunc();
        monsterCount++;
        comboCount++;
        ComboTextFunc(comboCount);
        TransformFunc(randomTransformCount);
        checkComboTimer = 1.0f;
        if (firstCharNode.charImgIndex == 8)
            isBonusHit = true;
        AddScoreFunc();
        currentGuage += 1.5f;
        tempSortLayerCanvas = spawnList[0].GetComponent<Canvas>();
        tempSortLayerCanvas.overrideSorting = true;
        tempSortLayerCanvas.sortingOrder = 11;
        firstCharNode.isFirst = true;
        firstCharNode.isMove = true;
        firstCharNode.isLeft = isLeft;
        spawnList.RemoveAt(0);
        for (int ii = 0; ii < spawnList.Count; ii++)
        {
            spawnList[ii].GetComponent<YSMSCharNode>().SetDestinationScaleFunc(new Vector3(spawnList[ii].transform.position.x,
                screenScale * posArray[ii], spawnList[ii].transform.position.z), scaleArray[ii]);
            spawnList[ii].GetComponent<YSMSCharNode>().isMove = true;
        }
        if (maxGuage <= currentGuage)
        {
            currentGuage = 0.0f;
            SpawnChar((int)YSMSCharType.YSMSBomb, posArray[9], scaleArray[9]);
            SoundManager.instance.PlayerSound("Bell");
        }
        else
            MakeNewCharFunc();
        guagebarImg.fillAmount = currentGuage / maxGuage;
        SetLayerFunc();
        UpdateLevelFunc();
        //SoundManager.instance.PlayerSound("Whip", 0.4f);
        SoundManager.instance.PlayGUISound("Whip", 0.4f);
    }
    void ClassifyIncorrectFunc()
	{
        currentGuage -= 15.0f;
        if (currentGuage <= 0.0f)
            currentGuage = 0.0f;
        guagebarImg.fillAmount = currentGuage / maxGuage;
        firstCharNode.isMove = false;
        falseTimer = setFalseTime;
        firstCharNode.ErrorColorChangeFunc(true);
        SoundManager.instance.PlayerSound("Fail", 0.7f);
    }

    void FalseTimerFunc()
    {
        if (0.0f < falseTimer)
        {
            falseTimer -= Time.deltaTime;
            if (falseTimer < 0.0f)
            {
                falseTimer = 0.0f;
                YSMSCharNode firstCharNode = spawnList[0].GetComponent<YSMSCharNode>();
                firstCharNode.ErrorColorChangeFunc(false);
            }
        }
    }

    void PlusShowTimerFunc()
    {
        if (0.0f < plusShowTimer)
        {
            plus10SecImg.gameObject.SetActive(true);
            plusShowTimer -= Time.deltaTime;
            if (plusShowTimer < 0.0f)
            {
                plusShowTimer = 0.0f;
                plus10SecImg.gameObject.SetActive(false);
            }
        }
    }

    void CheckComboTimerFunc()
    {
        if (0.0f < checkComboTimer)
        {
            checkComboTimer -= Time.deltaTime;
            if (checkComboTimer < 0.0f)
            {
                checkComboTimer = 0.0f;
                comboCount = 0;
            }
        }
    }

    void TransformTimerFunc()
    {
        if (0.0f < transformTimer)
        {
            transformTimer -= Time.deltaTime;
            if (transformTimer <= 0.0f)
            {
                transformTimer = 0.0f;
                randomTransformCount = Random.Range(130, 200);
                isTransformTimerOn = false;
                transformItemImg.gameObject.SetActive(isTransformTimerOn);
            }
        }
    }

    void GameTimePlusFunc()
    {
        gameTime += 10.0f;
        bombCount++;
        if (maxTime <= gameTime)
            gameTime = maxTime;

        for (int ii = 0; ii < isTimeShowArray.Length; ii++)
        {
            if (isTimeShowArray[ii] == false)
                isTimeShowArray[ii] = true;
        }

        timebarX = timebarImg.transform.position.x + (1440.0f / 60.0f) * 10.0f / screenScale;
        if (timebarX >= timebarMaxPos.x)
            timebarX = timebarMaxPos.x;
        plusShowTimer = 1.0f;
        timebarImg.transform.position = new Vector3(timebarX, timebarImg.transform.position.y, timebarImg.transform.position.z);
        //SoundManager.instance.PlayerSound("Dingdong", 20.0f);
    }

    void MakeNewCharFunc()
    {
        if (maxGuage <= currentGuage) return;

        if (0.0f < transformTimer)
            SpawnChar(leftArray[0], posArray[9], scaleArray[9]);
        else
        {
            leftorrightIndex = Random.Range(0, 2);
            //leftorrightIndex = 1;
            if (leftorrightIndex == 1)
            {
                randomCharIndex = Random.Range(0, levelLeftIndex);
                //randomCharIndex = 0;
                SpawnChar(leftArray[randomCharIndex], posArray[9], scaleArray[9]);
            }
            else
            {
                randomCharIndex = Random.Range(0, levelRightIndex);
                SpawnChar(rightArray[randomCharIndex], posArray[9], scaleArray[9]);
            }
        }        
    }

    void SetLayerFunc()
    {
        for (int ii = 0; ii < spawnList.Count; ii++)
        {
            if (spawnList[ii] == null) continue;

            tempSortLayerCanvas = spawnList[ii].GetComponent<Canvas>();
            tempSortLayerCanvas.overrideSorting = true;
            tempSortLayerCanvas.sortingOrder = 10 - ii;
        }
    }

    void UpdateLevelFunc()
    {
        if (gameLevel == 4 && 600 < monsterCount)
        {
            gameLevel = 5;
            if (isLevelLeft[1])
                ShowChar(rightArray[2], !isLevelLeft[1], 3);
            else
                ShowChar(leftArray[2], !isLevelLeft[1], 3);
            levelLeftIndex = 3;                             //(gameLevel + 1) / 2;
        }
        else if (gameLevel == 3 && 500 < monsterCount)
        {
            gameLevel = 4;
            if (isLevelLeft[1])
            {
                ShowChar(leftArray[2], isLevelLeft[1], 3);
                levelLeftIndex = 3;
            }
            else
            {
                ShowChar(rightArray[2], isLevelLeft[1], 3);
                levelLeftIndex = 2;
            }
        }
        else if (gameLevel == 2 && 250 < monsterCount)
        {
            gameLevel = 3;
            if (isLevelLeft[0])
                ShowChar(rightArray[1], !isLevelLeft[0], 2);
            else
                ShowChar(leftArray[1], !isLevelLeft[0], 2);
            levelLeftIndex = 2;                         //(gameLevel + 1) / 2;
        }
        else if (gameLevel == 1 && 100 < monsterCount)
        {
            gameLevel = 2;
            if (isLevelLeft[0])
            {
                ShowChar(leftArray[1], isLevelLeft[0], 2);
                levelLeftIndex = 2;
            }
            else
            {
                ShowChar(rightArray[1], isLevelLeft[0], 2);
                levelLeftIndex = 1;
            }
        }
        levelRightIndex = gameLevel + 1 - levelLeftIndex;
    }

    public void UpdateCharArray()
    {
        for (int ii = 1; ii < 10; ii++)
        {
            //destinationPos = new Vector3(spawnList[ii].transform.position.x, posArray[ii - 1] + 2000.0f, spawnList[ii].transform.position.z);
            destinationPos = new Vector3(spawnList[ii].transform.position.x, posArray[ii - 1], spawnList[ii].transform.position.z);
            //spawnList[ii].transform.position = Vector3.MoveTowards(spawnList[ii].transform.position, destinationPos, speedArray[ii - 1]);
            spawnList[ii].transform.position = Vector3.MoveTowards(spawnList[ii].transform.position, destinationPos, 30.0f);
            if (Vector3.Distance(spawnList[ii].transform.position, destinationPos) < 0.01f)
                spawnList[ii].transform.position = destinationPos;
            newScale = scaleArray[ii - 1];
            spawnList[ii].transform.localScale = new Vector3(newScale, newScale, 1.0f);
        }
    }

    void TransformFunc(int tempCount)
    {
        if (isTransformTimerOn) return;
        if (GlobalValue.g_YSMSUpgradeLv[2] == 0 && !ReadySceneMgr.isItemChecked[1]) return;
        
        transformCount++;
        if (tempCount < transformCount)
        {
            SoundManager.instance.PlayerSound("Transform");
            transformTimer = superRate + transformItemTime;
            transformCount = 0;
            isTransformTimerOn = true;
            transformItemImg.gameObject.SetActive(isTransformTimerOn);
        }
    }

    public void ComboTextFunc(int tempCombo)
    {
        for (int ii = 0; ii < comboTextObj.transform.childCount; ii++)
        {
            if (!comboTextObj.transform.GetChild(ii).gameObject.activeSelf)
                continue;
            else
                comboTextObj.transform.GetChild(ii).GetComponent<ComboText>().ComboObjReturnFunc();
        }

        comboClone = MemoryPoolManager.instance.GetObject("ComboSpawnGroup");
        comboClone.transform.position = comboTextObj.transform.position;
        comboClone.GetComponent<ComboText>().SetComboTextFunc(tempCombo);
        comboClone.SetActive(true);

    }

    void AddScoreFunc()
    {
        scoreRate = 372.2f * (100.0f + gameLevel) * 0.01f * (100.0f + comboCount) * 0.01f * feverRate;
        if (isBonusHit)
        {
            scoreRate += GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]];
            BonusTextFunc();
        }
        currentScore += (int)scoreRate;
        scoreText.text = currentScore.ToString("N0");
        GlobalValue.g_YSMSTempScore = currentScore;
    }

    void BonusTextFunc()
    {
        if (!isBonusHit) return;

        bonusClone = MemoryPoolManager.instance.GetObject("BonusTextSpawnGroup");
        bonusClone.transform.position = bonusTextObj.transform.position;
        bonusClone.GetComponent<BonusText>().SetScore(GameKind.YSMS);
        //bonusClone.GetComponent<BonusText>().SetScore(GlobalValue.g_GameKind);
        bonusClone.SetActive(true);

        isBonusHit = false;

    }


    public void PauseBtnFunc(bool isPause)
    {
        SoundManager.instance.PlayerSound("Button");
        if (pausePanelObj != null)
            pausePanelObj.SetActive(isPause);

        //일시정지 악용 방지
        //캐릭터 비활성화
        foreach (GameObject charNode in spawnList)
            charNode.SetActive(!isPause);

        
        if (isPause)
		{
            //콤보텍스트 비활성화
            for (int ii = 0; ii < comboTextObj.transform.childCount; ii++)
			{
                if (comboTextObj.transform.GetChild(ii).gameObject.activeSelf)
                {
                    comboTextObj.transform.GetChild(ii).gameObject.SetActive(false);
                    comboTextObj.transform.GetChild(ii).GetComponent<ComboText>().isPause = true;
                    break;
                }
                else
                    continue;
			}
            Time.timeScale = 0.0f;
            SoundManager.instance.PauseAllSound();
        }
        else
		{
            //콤보텍스트 활성화
            for (int ii = 0; ii < comboTextObj.transform.childCount; ii++)
            {
                tempComboText = comboTextObj.transform.GetChild(ii).GetComponent<ComboText>();
                if (tempComboText.comboCount == comboCount && tempComboText.isPause)
				{
                    tempComboText.isPause = false;
					comboTextObj.transform.GetChild(ii).gameObject.SetActive(true);
                    tempComboText = null;
                    break;
				}
				else
					continue;
            }
            Time.timeScale = 1.0f;
            SoundManager.instance.ResumeAllSound();
        }
        MusicManager.instance.PauseResumeMusic(isPause);

    }


    void CheckTimeStateFunc(float gameTime)
	{
        if (gameTime > 6.0f) return;
        showTime = (int)gameTime;
        if (1 <= showTime && showTime <= 5)
        {
            CountTimeFunc(showTime, isTimeShowArray[showTime - 1]);
            isTimeShowArray[showTime - 1] = false;
        }
        else if (showTime == 0)
        {
            if (!isGameOver)
            {
                gameTime = 0.0f;
                spawnList.Clear();
                for (int ii = 0; ii < isTimeShowArray.Length; ii++)
                    isTimeShowArray[ii] = false;
                if (timeupImg != null)
                    timeupImg.gameObject.SetActive(true);
                MusicManager.instance.StopMusic();
                SoundManager.instance.PlayerSound("TimeUp", 1.3f);
                isGameOver = true;
            }
        }
        else
        {
            for (int ii = 0; ii < isTimeShowArray.Length; ii++)
            {
                if (isTimeShowArray[ii] == false)
                    isTimeShowArray[ii] = true;
            }
        }
        
	}

    public void CountTimeFunc(int showTime, bool isTimeShow)
    {
        if (!isTimeShow) return;
        //if (!timerObj) return;

        tempTimerObj = MemoryPoolManager.instance.GetObject("TimerTextSpawnGroup");
        if (tempTimerObj != null)
            tempTimerObj.GetComponent<TimerText>().InitTimeFunc(showTime);
        tempTimerObj.transform.localPosition = Vector3.zero;

    }

}
