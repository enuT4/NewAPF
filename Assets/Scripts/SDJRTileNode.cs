using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TileType
{
    Bomb = 0,
    Normal,         //1 ~ 7
    Bonus,          //8
    Special1,       //지우개, 9
    Special2,       //해머, 10
    Special3,       //한줄, 11
    Bad1,           //좌우반전, 12
    Bad2,           //블록 가리기, 13
    GameOver,       //게임오버, 14
    TileTypeCount
}

public class SDJRTileNode : MemoryPoolObject
{
    [SerializeField] internal GameObject[] tileGroupArray = new GameObject[15];
    [SerializeField] internal GameObject tileSelectImg;
    [SerializeField] internal GameObject badCountImg;
    [SerializeField] internal Text badCountText;
    [SerializeField] internal GameObject blockedTileImg;
    float badCountTimer = 0.0f;

    public int tileIdx = -1;
    
    [HideInInspector] public bool isSelected = false;
    [HideInInspector] public TileType tileType = TileType.TileTypeCount;
    public bool isSpecialTilemoved = false;

    //void Awake() => AwakeFunc();
    private void AwakeFunc()
    {
        for (int ii = 0; ii < tileGroupArray.Length; ii++)
        {
            if (!tileGroupArray[ii])
                tileGroupArray[ii] = transform.Find("TileImgGroup").transform.GetChild(ii).gameObject;
        }
        if (!tileSelectImg) tileSelectImg = transform.Find("SelectImg").gameObject;
        if (!badCountImg) badCountImg = transform.Find("BadTileTimerImg").gameObject;
        if (!badCountText) badCountText = badCountImg.transform.GetChild(0).GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tileSelectImg != null && tileSelectImg.activeSelf)
            tileSelectImg.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (0.0f < badCountTimer)
        {
            badCountTimer -= Time.deltaTime;
            badCountText.text = ((int)badCountTimer).ToString();
            if (badCountTimer <= 0.0f)
            {
                badCountTimer = 0.0f;
                badCountImg.SetActive(false);
                SetTileIndexFunc(14);
                BadTilePenaltyFunc();
            }
        }
    }

    public void SetTileType(TileType tileType, int gameLevel, bool isDup = false, int dupIndex = -1)
    {
        if ((int)tileType < 0 || (int)TileType.TileTypeCount < (int)tileType)
            return;

        if (tileType == TileType.Normal)
        {
            tileIdx = Random.Range(0, gameLevel + 1);
            if (isDup && tileIdx == dupIndex)
                tileIdx = (tileIdx == gameLevel) ? 0 : tileIdx + 1;     //중복 방지
        }
        else
            CheckTileTypeFunc(tileType);

        SetTileIndexFunc(tileIdx);
    }

    public void SetTileIndexFunc(int tileIdx)
    {
        if (tileIdx == -1) return;

        for (int ii = 0; ii < tileGroupArray.Length; ii++)
        {
            if (tileGroupArray[ii].activeSelf)
                tileGroupArray[ii].SetActive(false);
        }
        tileGroupArray[tileIdx].SetActive(true);
    }

    void CheckTileTypeFunc(TileType tileType)
    {
        if (tileType == TileType.Bonus) tileIdx = 7;
        else if (tileType == TileType.Bomb) tileIdx = 8;
        else if (tileType == TileType.Special1) tileIdx = 9;
        else if (tileType == TileType.Special2) tileIdx = 10;
        else if (tileType == TileType.Special3) tileIdx = 11;
        else if (tileType == TileType.Bad1)
        {
            tileIdx = 12;
            badCountImg.SetActive(true);
            badCountTimer = 10.0f;
        }
        else if (tileType == TileType.Bad2)
        {
            tileIdx = 13;
            badCountImg.SetActive(true);
            badCountTimer = 10.0f;
        }
        else if (tileType == TileType.GameOver) tileIdx = 14;

    }

    void BadTilePenaltyFunc()
    {
        if (tileType == TileType.Bad1)
        {   //좌우반전
            SDJRIngameMgr.inst.isReverse = true;
            SDJRIngameMgr.inst.ReverseForSeconds(5.0f);

        }
        else if (tileType == TileType.Bad2)
        {   //가리개
            SDJRIngameMgr.inst.isTileHide = true;
            //SDJRIngameMgr.inst.switchBool = true;
            SDJRIngameMgr.inst.CoverTileForSeconds(5.0f);
        }
        else return;
        //페널티 발동 -> 본인은 도움이 되지 않는 타일로 변신
        SoundManager.instance.PlayerSound("BadTile");


    }

    public void SelectOrDeselectFunc(bool isSelected)
    {
        tileSelectImg.SetActive(isSelected);
    }

    public void DestroyFunc()
	{
        if (badCountTimer > 0.0f) badCountTimer = 0.0f;
        if (badCountImg.activeSelf) badCountImg.SetActive(false);

        ObjectReturn();
	}

    
}
