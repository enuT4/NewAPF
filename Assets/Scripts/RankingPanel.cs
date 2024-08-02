using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] internal Text gameNameTxt;
    [SerializeField] internal Button closePanelBtn;
    [SerializeField] internal Sprite[] buttonActiveImg;
    [SerializeField] internal GameObject rankingNodeObj;

    string gameName = "";

    [Header("-------- GameRankingPanel --------")]
    [SerializeField] internal ScrollRect gameRankScrlView;
    [SerializeField] internal GameObject gameRankContent;
    [SerializeField] internal Button gameRankBtn;

    [Header("-------- TotalRankingPanel --------")]
    [SerializeField] internal ScrollRect totalRankScrlView;
    [SerializeField] internal GameObject totalRankContent;
    [SerializeField] internal Button totalRankBtn;

    WaitForSeconds wfs = new WaitForSeconds(30);

    //[SerializeField] internal Image tempImg;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!gameNameTxt) gameNameTxt = transform.Find("GameText").GetComponent<Text>();
        if (!closePanelBtn) closePanelBtn = transform.Find("PanelCloseBtn").GetComponent<Button>();
        if (!gameRankBtn) gameRankBtn = transform.Find("GameRankingBtn").GetComponent<Button>();
        if (!totalRankBtn) totalRankBtn = transform.Find("TotalRankingBtn").GetComponent<Button>();

        TransformScrollView(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (closePanelBtn != null) closePanelBtn.onClick.AddListener(() => gameObject.SetActive(false));
        if (gameRankBtn != null) gameRankBtn.onClick.AddListener(() =>
        {
            TransformScrollView(true);
        });
        if (totalRankBtn != null) totalRankBtn.onClick.AddListener(() =>
        {
            TransformScrollView(false);
        });

        RefreshRankingList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        TransformScrollView(true);
    }

    void TransformScrollView(bool isGameScrlActive)
    {
        gameRankScrlView.gameObject.SetActive(isGameScrlActive);
        gameRankBtn.image.sprite = buttonActiveImg[Convert.ToInt32(!isGameScrlActive)];
        totalRankScrlView.gameObject.SetActive(!isGameScrlActive);
        totalRankBtn.image.sprite = buttonActiveImg[Convert.ToInt32(isGameScrlActive)];
    }
    
    void CheckGameName()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            gameName = "YSMS";
            gameNameTxt.text = "삼촌의 니편내편";
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            gameName = "SDJR";
            gameNameTxt.text = "엄마의 삼단정리";
        }
        GetRankingList(GlobalValue.g_GameKind);
    }

    void ClearRank()
    {
        RankingImgNode[] rankingNodes = gameRankContent.GetComponentsInChildren<RankingImgNode>();
        foreach (RankingImgNode node in rankingNodes)
            Destroy(node.gameObject);
    }

    void RefreshRankingList()
    {
        ClearRank();
        CheckGameName();
        GetTotalRankingList();
        StopCoroutine(DelayTime());
        StartCoroutine(DelayTime());
    }

    IEnumerator DelayTime()
    {
        yield return wfs;
        RefreshRankingList();
    }

    void GetMyRanking(GameKind gKind)
    {
        if (GlobalValue.g_UniqueID == "") return;
        if (gameName == "") return;

        var request = new GetLeaderboardAroundPlayerRequest()
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
                    }
                    else if (gKind == GameKind.SDJR)
                    {
                        GlobalValue.g_MySDJRRank = curBoard.Position + 1;
                        GlobalValue.g_SDJRBestScore = curBoard.StatValue;
                    }
                }
            },

            (error) =>
            {
                Debug.Log("등수 가져오기 실패");
            }

            );
    }

    void GetRankingList(GameKind gKind)
    {
        if (GlobalValue.g_UniqueID == "") return;
        if (gameName == "") return;

        var request = new GetLeaderboardRequest()
        {
            StartPosition = 0,
            StatisticName = gameName + "BestScore",
            MaxResultsCount = 20,
            ProfileConstraints = new PlayerProfileViewConstraints()
            { 
                ShowDisplayName = true
            }
        };

        PlayFabClientAPI.GetLeaderboard(
            request,
            (result) =>
            {
                if (rankingNodeObj == null) return;

                for (int ii = 0; ii < result.Leaderboard.Count; ii++)
                {
                    var curboard = result.Leaderboard[ii];
                    GameObject userObj = Instantiate(rankingNodeObj);
                    userObj.transform.SetParent(gameRankContent.transform, false);
                    RankingImgNode rkNode = userObj.GetComponent<RankingImgNode>();

                    rkNode.rankTxt.text = (ii + 1).ToString();
                    rkNode.bestScoreTxt.text = curboard.StatValue.ToString("N0");
                    rkNode.nicknameTxt.text = curboard.DisplayName;
                    if (curboard.PlayFabId == GlobalValue.g_UniqueID)
                    {
                        rkNode.buttonSprite.color = new Color32(120, 212, 242, 255);
                        //if (GlobalValue.facebookName != "")
                        //    rkNode.facebookImg.sprite = tempImg.sprite;
                    }
                }
                GetMyRanking(gKind);
            },
            (error) =>
            {
                Debug.Log("리더보드 불러오기 실패");
            }

            );
    }

    void GetMyTotalRanking()
    {
        if (GlobalValue.g_UniqueID == "") return;
        //if (gameName == "") return;

        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "TotalScore",
            MaxResultsCount = 1,
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            request,
            (result) =>
            {
                if (0 < result.Leaderboard.Count)
                {
                    var curBoard = result.Leaderboard[0];
                    GlobalValue.g_MyTotalRank = curBoard.Position + 1;
                    GlobalValue.g_TotalScore = curBoard.StatValue;
                }
            },
            (error) =>
            {
                Debug.Log("등수 가져오기 실패");
            }

            );
    }

    void GetTotalRankingList()
    {
        if (GlobalValue.g_UniqueID == "") return;
        //if (gameName == "") return;

        var request = new GetLeaderboardRequest()
        {
            StartPosition = 0,
            StatisticName = "TotalScore",
            MaxResultsCount = 20,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        };

        PlayFabClientAPI.GetLeaderboard(
            request,
            (result) =>
            {
                if (rankingNodeObj == null) return;

                for (int ii = 0; ii < result.Leaderboard.Count; ii++)
                {
                    var curboard = result.Leaderboard[ii];
                    GameObject userObj = Instantiate(rankingNodeObj);
                    userObj.transform.SetParent(totalRankContent.transform, false);
                    RankingImgNode rkNode = userObj.GetComponent<RankingImgNode>();
                    rkNode.rankTxt.text = (ii + 1).ToString();
                    rkNode.bestScoreTxt.text = curboard.StatValue.ToString("N0");
                    rkNode.nicknameTxt.text = curboard.DisplayName;
                    if (curboard.PlayFabId == GlobalValue.g_UniqueID)
                    {
                        rkNode.buttonSprite.color = new Color32(120, 212, 242, 255);
                        //if (GlobalValue.facebookName != "")
                        //    rkNode.facebookImg.sprite = tempImg.sprite;
                    }
                }
                GetMyTotalRanking();
            },
            (error) =>
            {
                Debug.Log("리더보드 불러오기 실패");
            }

            );
    }
}
