using Enut4LJR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class YSMSIngameMgr : MonoBehaviour
{
    //[SerializeField] GameObject tempposObj;
    [SerializeField] internal GameObject YSMSCanvasObj;

    
    //[SerializeField] internal GameObject charPrefab;

    
    float moveSpeed = 4.0f;

    [Header("-------- Game UI --------")]
    [SerializeField] internal Button leftBtn;
    [SerializeField] internal Button rightBtn;
    [SerializeField] internal Text scoreText;
    [SerializeField] internal Image guagebarbackImg;
    [SerializeField] internal Image guagebarImg;
    [SerializeField] internal Image timebarImg;
    Vector3 timebarMaxPos;
    float timebarX;
    [SerializeField] internal Text timerText;

    [SerializeField] internal Button pauseBtn;
    [SerializeField] internal GameObject pausePanelObj;
    ComboText tempComboText;

    float gameTime = 60.0f;
    float maxTime = 60.0f;
    int currentScore = 0;
    float scoreRate = 0.0f;

    float maxGuage = 100.0f;
    float currentGuage = 0.0f;
    int bombCount = 0;
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
    float gameoverImgShowTimer = 3.0f;

    [Header("-------- Combo Text --------")]
    GameObject comboTextObj;
    [SerializeField] internal GameObject comboClone;
    public int comboCount = 0;
    public float checkComboTimer = 0.0f;
    bool isBonusHit;



    [Header("-------- Game Ready --------")]
    [SerializeField] internal GameObject readyPanelObj;
    [SerializeField] internal Image readyImg;
    [SerializeField] internal Image goImg;
    float readyTimer = 1.0f;
    float goTimer = 0.0f;
    bool isGameStart = false;
    Color readyImgColor;

    [Header("-------- Char Spawn Setting --------")]
    [SerializeField] internal Transform charSpawnTr;
    [SerializeField] internal GameObject showLeftCharGroupObj;
    [SerializeField] internal GameObject showRightCharGroupObj;
    [SerializeField] GameObject tempNodeObj;
    float[] posArray = new float[10];
    float[] scaleArray = new float[10];
    float[] speedArray = new float[10];
    int[] selectArray = new int[8];
    int[] leftArray = new int[3];
    int[] rightArray = new int[3];
    int gameLevel = 1;
    Vector3 destinationPos = Vector3.zero;
    float newScale;
    YSMSCharNode firstCharNode;


    //아이템
    bool isSuperFeverOn = false;
    bool isTransformTimerOn = false;
    int randomTransformCount = 0;
    int transformCount = 0;
    float transformItemTime = 0.0f;
    float transformTimer = 0.0f;
    [SerializeField] internal Image transformIconImage;

    //업그레이드
    int bonusScore = 0;
    float feverRate = 1.0f;
    float superRate = 1.0f;
    bool isBonutHit = false;
    //Vector3 bonusTextPos;
    GameObject bonusTextObj;
    GameObject bonusClone;
    BonusText bonusTxt;


    public static List<GameObject> spawnList = new List<GameObject>();


    public static YSMSIngameMgr inst;

    void Awake() => AwakeFunc();
       

    void AwakeFunc()
    {
        if (!SoundManager.instance) SoundManager.instance.CallInstance();
        if (!MusicManager.instance) MusicManager.instance.CallInstance();

        inst = this;

        if (!YSMSCanvasObj) YSMSCanvasObj = GameObject.Find("Canvas").gameObject;
        if (!readyPanelObj) readyPanelObj = YSMSCanvasObj.transform.Find("ReadyPanel").gameObject;
        if (!readyImg) readyImg = readyPanelObj.transform.GetChild(0).GetComponent<Image>();
        if (!goImg) goImg = readyPanelObj.transform.GetChild(1).GetComponent<Image>();
        if (!charSpawnTr) charSpawnTr = YSMSCanvasObj.transform.Find("SpawnGroup").gameObject.transform;
        if (!showLeftCharGroupObj) showLeftCharGroupObj = YSMSCanvasObj.transform.Find("ShowLCharGroup").gameObject;
        if (!showRightCharGroupObj) showRightCharGroupObj = YSMSCanvasObj.transform.Find("ShowRCharGroup").gameObject;
        if (!guagebarbackImg) guagebarbackImg = YSMSCanvasObj.transform.Find("BombGuageBack").GetComponent<Image>();
        if (!guagebarImg) guagebarImg = guagebarbackImg.transform.GetChild(0).GetComponent<Image>();
        if (!comboTextObj) comboTextObj = YSMSCanvasObj.transform.Find("ComboSpawnGroup").gameObject;
        if (!bonusTextObj) bonusTextObj = YSMSCanvasObj.transform.Find("BonusTextSpawnGroup").gameObject;
        if (!plus10SecImg) plus10SecImg = YSMSCanvasObj.transform.Find("Plus10SecImg").GetComponent<Image>();

        if (!leftBtn) leftBtn = YSMSCanvasObj.transform.Find("LeftBtn").GetComponent<Button>();
        if (!rightBtn) rightBtn = YSMSCanvasObj.transform.Find("RightBtn").GetComponent<Button>();

        if (!pauseBtn) pauseBtn = YSMSCanvasObj.transform.Find("PauseBtn").GetComponent<Button>();
        if (!pausePanelObj) pausePanelObj = YSMSCanvasObj.transform.Find("PausePanel").gameObject;

        if (!timebarImg) timebarImg = YSMSCanvasObj.transform.Find("BGImg").transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        if (!timerText) timerText = YSMSCanvasObj.transform.Find("BGImg").transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        if (!scoreText) scoreText = YSMSCanvasObj.transform.Find("ScoreText").GetComponent<Text>();
        //if (!tempposObj) tempposObj = charSpawnTr.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckItemFunc();
        CheckUpgradeRateFunc();

        if (readyImg != null)
        {
            if (!readyImg.gameObject.activeSelf) readyImg.gameObject.SetActive(true);
            if (goImg.gameObject.activeSelf) goImg.gameObject.SetActive(false);
        }

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

        if (timeupImg != null && timeupImg.gameObject.activeSelf)
            timeupImg.gameObject.SetActive(false);

        if (plus10SecImg != null && plus10SecImg.gameObject.activeSelf)
            plus10SecImg.gameObject.SetActive(false);

        InitSpawnFunc();
        timebarMaxPos = timebarImg.transform.position; ;
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

        


        if (pauseBtn != null)
            pauseBtn.onClick.AddListener(() => PauseBtnFunc(true));
    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (!isGameStart)
        {
            if (readyTimer > 0.0f) 
            {
                readyTimer -= Time.deltaTime;
                readyImgColor = readyImg.color;
                readyImgColor.a = Mathf.Lerp(1.0f - readyTimer, 2.0f, 0.2f);
                if (1.0f <= readyImgColor.a)
                    readyImgColor.a = 1.0f;
                readyImg.color = readyImgColor;

                if (readyTimer < 0.0f)
                {
                    readyImg.gameObject.SetActive(false);
                    goImg.gameObject.SetActive(true);
                    goTimer = 1.0f;
                    readyTimer = 0.0f;
                }
            }

            if (goTimer > 0.0f)
            {
                if (readyImg.gameObject.activeSelf || !goImg.gameObject.activeSelf) return;

                goTimer -= Time.deltaTime;
                if (goTimer <= 0.0f)
                {
                    readyPanelObj.SetActive(false);
                    isGameStart = true;
                    MusicManager.instance.PlayMusic("IngameBGM");
                    if (isSuperFeverOn)
                        comboCount = 100;
                }
            }
        }
        else
        {
            gameTime -= Time.deltaTime;
            timerText.text = ((int)gameTime).ToString();
            if (Time.timeScale != 0.0f && (0.0f < gameTime && gameTime <= 60.0f))
                timebarImg.transform.Translate(-0.055f, 0, 0);

            CheckTimeStateFunc(gameTime);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                LeftBtnFunc();

            if (Input.GetKeyDown(KeyCode.RightArrow))
                RightBtnFunc();

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

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    Debug.Log(showLeftCharGroupObj.transform.position + ":" + bonusTextObj.transform.position);
        //    Debug.Log(showLeftCharGroupObj.transform.localPosition + ":" + bonusTextObj.transform.localPosition);

        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    //for(int ii = 0;ii<spawnList.Count;ii++)
        //    //    Debug.Log(spawnList[ii].transform.position.y);
        //    //Debug.Log(spawnList[8].transform.position.y);
        //    Debug.Log(spawnList.Count + " : " + posArray.Length);
        //}

    }

    void InitChar()
    {
        for (int ii = 0; ii < 2; ii++)
        {   //3, 5번째 캐릭터가 어느쪽으로 나올건지 정해줌
            int levelLeftorRight = Random.Range(0, 2);
            if (levelLeftorRight == 0)
                isLevelLeft[ii] = true;
            else
                isLevelLeft[ii] = false;
        }

        int bonusLevel = GlobalValue.g_YSMSUpgradeLv[0];
        bonusLevel = 8;
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

        for (int ii = 0; ii < 9; ii++)
        {
            scaleArray[ii] = 0.7f + 0.5f / 9.0f * (8 - ii);
            posArray[ii] = -125.0f * (8 - ii);
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

    void LeftBtnFunc()
    {
        if (pausePanelObj.activeSelf) return;
        if (gameTime <= 0.0f) return;
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
        if (gameTime <= 0.0f) return;
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
        SetLayerFunc();
        UpdateLevelFunc();
        firstCharNode.isMove = true;
        firstCharNode.isLeft = isLeft;
        SoundManager.instance.PlayerSound("Whip", 0.4f);
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
        SoundManager.instance.PlayerSound("Fail");
    }

    void GameTimePlusFunc()
    {
        gameTime += 10.0f;
        bombCount++;
        if (maxTime <= gameTime)
            gameTime = maxTime;

        timebarX = timebarImg.transform.position.x + (400.0f / 6.0f);
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
            if (leftorrightIndex == 1)
            {
                randomCharIndex = Random.Range(0, levelLeftIndex);
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
        for (int ii = 0; ii < 10; ii++)
        {
            if (spawnList[ii] == null) continue;

            Canvas SortLayerCanvas = spawnList[ii].GetComponent<Canvas>();
            SortLayerCanvas.overrideSorting = true;
            SortLayerCanvas.sortingOrder = 10 - ii;
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
            destinationPos = new Vector3(spawnList[ii].transform.position.x, posArray[ii - 1] + 2000.0f, spawnList[ii].transform.position.z);
            spawnList[ii].transform.position = Vector3.MoveTowards(spawnList[ii].transform.position, destinationPos, speedArray[ii - 1]);
            if (spawnList[ii].transform.position.y < destinationPos.y)
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
            transformTimer = superRate + transformItemTime;
            transformCount = 0;
            isTransformTimerOn = true;
            transformIconImage.gameObject.SetActive(isTransformTimerOn);
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
        currentGuage += 1.5f;
        if (maxGuage <= currentGuage)
        {
            currentGuage = 0.0f;
            SpawnChar((int)YSMSCharType.YSMSBomb, posArray[9], scaleArray[9]);
            SoundManager.instance.PlayerSound("Bell");
        }
        else
        {
            MakeNewCharFunc();
        }
        guagebarImg.fillAmount = currentGuage / maxGuage;
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
        if (pausePanelObj != null)
            pausePanelObj.SetActive(isPause);

        //일시정지 악용 방지
        //캐릭터 비활성화
        foreach (GameObject charNode in spawnList)
            charNode.SetActive(!isPause);

        //콤보텍스트 비활성화
        if (isPause)
		{
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
		}
        else
		{
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
        }


		if (isPause)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;

        MusicManager.instance.PauseResumeMusic(isPause);
    }


    void CheckTimeStateFunc(float gameTime)
	{

	}
}
