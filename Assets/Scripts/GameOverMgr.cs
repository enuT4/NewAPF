using Enut4LJR;
using JetBrains.Annotations;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using UnityEditor.PackageManager;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMgr : MonoBehaviour
{
    [Header("-------- UI --------")]
    [SerializeField] internal Image bgImg;
    [SerializeField] internal Text userLevelExpText;
    [SerializeField] internal Text userGoldText;
    [SerializeField] internal Text userGemText;
    [SerializeField] internal Text bestScoreText;
    [SerializeField] internal Text resultScoreText;
    [SerializeField] internal Image newRecordImg;
    [SerializeField] internal Text scBonusText;
    [SerializeField] internal Text rewardGoldText;
    [SerializeField] internal Text rewardExpText;
    [SerializeField] internal Text totalScoreText;
    [SerializeField] internal Image upImg;
    [SerializeField] internal Text inSchoolCompetitionText;
    [SerializeField] internal Text schoolInfoText;
    [SerializeField] internal Text regionCompetitionText;
    [SerializeField] internal Button friendBtn;
    [SerializeField] internal Button okBtn;
    [SerializeField] internal GameObject messageBoxObj;
    MessageBox msgBox;

    //
    bool isSendFriend = false;
    int resultScore = 0;
    int levelBonus = 0;
    int timeBonus = 0;
    [SerializeField] int rewardGold = 0;
    int rewardExp = 0;
    int myRank = 0;

    float delayTimer1 = 1.5f;
    float delayTimer2 = 0.0f;

    //���� ���� ������ ���� ���簪 ����
    int currentScore = 0;
    [SerializeField]int currentGold = 0;
    int scoreDifference = 0;
    int currentTotalScore = 0;
    int currentBestScore = 0;
    int regionScore = 0;

    //���� ���� ���� ���� ����
    [SerializeField]float goldUpSpeed = 0.0f;
    float currentScoreUpSpeed = 0.0f;
    float differenceUpSpeed = 0.0f;

    string gameName = "";
    bool[] isGetRegion = new bool[2];

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        SoundManager.instance.CallInstance();
        MusicManager.instance.CallInstance();

        //UI
        if (!bgImg) bgImg = GameObject.Find("Canvas").transform.Find("BgImg").GetComponent<Image>();
        if (!userLevelExpText) userLevelExpText = bgImg.transform.Find("UserLevelExpText").GetComponent<Text>();
        if (!userGoldText) userGoldText = bgImg.transform.Find("UserGoldText").GetComponent<Text>();
        if (!userGemText) userGemText = bgImg.transform.Find("UserGemText").GetComponent<Text>();
        if (!bestScoreText) bestScoreText = bgImg.transform.Find("BestScoreText").GetComponent<Text>();
        if (!resultScoreText) resultScoreText = bgImg.transform.Find("ResultScoreText").GetComponent<Text>();
        if (!newRecordImg) newRecordImg = resultScoreText.transform.Find("NewRecordImg").GetComponent<Image>();
        if (!scBonusText) scBonusText = bgImg.transform.Find("ScBonusText").GetComponent<Text>();
        if (!rewardGoldText) rewardGoldText = bgImg.transform.Find("RewardGoldText").GetComponent<Text>();
        if (!rewardExpText) rewardExpText = bgImg.transform.Find("RewardExpText").GetComponent<Text>();
        if (!totalScoreText) totalScoreText = bgImg.transform.Find("TotalScoreText").GetComponent<Text>();
        if (!upImg) upImg = totalScoreText.transform.Find("UpImg").GetComponent<Image>();
        if (!inSchoolCompetitionText) inSchoolCompetitionText = bgImg.transform.Find("InSchoolCompetitionText").GetComponent<Text>();
        if (!schoolInfoText) schoolInfoText = bgImg.transform.Find("SchoolInfoText").GetComponent<Text>();
        if (!regionCompetitionText) regionCompetitionText = bgImg.transform.Find("RegionCompetitionText").GetComponent<Text>();
        if (!friendBtn) friendBtn = bgImg.transform.Find("FriendButton").GetComponent<Button>();
        if (!okBtn) okBtn = bgImg.transform.Find("OKButton").GetComponent<Button>();
        if (!messageBoxObj) messageBoxObj = GameObject.Find("Canvas").transform.Find("MessageBox").gameObject;
        if (messageBoxObj != null)
        {
            msgBox = messageBoxObj.GetComponent<MessageBox>();
            if (messageBoxObj.activeSelf) messageBoxObj.SetActive(false);
        }


        for (int ii = 0; ii < isGetRegion.Length; ii++)
        {
            if (isGetRegion[ii])
                isGetRegion[ii] = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckGameNameFunc();
        if (friendBtn != null) friendBtn.onClick.AddListener(FriendBtnFunc);
        if (okBtn != null) okBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            isSendFriend = false;
            SceneManager.LoadScene("LobbyScene");
        });

        //������ ���� ��ư ��� ���α�
        friendBtn.gameObject.SetActive(false);
        okBtn.gameObject.SetActive(false);
        upImg.gameObject.SetActive(false);
        newRecordImg.gameObject.SetActive(false);

        userLevelExpText.text = GlobalValue.g_ExpPercent.ToString() + "%";
        userGoldText.text = currentGold.ToString("N0");
        userGemText.text = GlobalValue.g_UserGem.ToString();

        resultScoreText.text = currentScore.ToString("N0");

        scBonusText.text = "<color=#00ffff>���� ���ʽ� : " + levelBonus + "%</color>\n" +
            "<color=#ff00ff>�ð� ���ʽ� : " + timeBonus + "%</color>";
        rewardExpText.text = rewardExp.ToString();
        totalScoreText.text = currentTotalScore.ToString("N0");
        schoolInfoText.text = GlobalValue.g_SchoolName;

        

        isSendFriend = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        ShowScoreFunc();
        if (0.0f < delayTimer1)
        {
            delayTimer1 -= Time.deltaTime;
            if (delayTimer1 <= 0.0f)
                delayTimer2 = 1.5f;
        }
        else
        {
            ShowRegionScore();
            if (0.0f < delayTimer2)
            {
                delayTimer2 -= Time.deltaTime;
                if (delayTimer2 <= 0.0f)
                {
                    friendBtn.gameObject.SetActive(true);
                    okBtn.gameObject.SetActive(true);
                }
            }
        }

        if (isGetRegion[0] && isGetRegion[1])
        {
            if (gameName == "YSMS")
            {
                GlobalValue.g_YSMSRegionScore = regionScore + scoreDifference;
                NetworkMgr.inst.PushPacket(PacketType.YSMSRegionScore);
            }
            else if (gameName == "SDJR")
            {
                GlobalValue.g_SDJRRegionScore = regionScore + scoreDifference;
                NetworkMgr.inst.PushPacket(PacketType.SDJRRegionScore);
            }

            isGetRegion[1] = false;
        }

    }

    void CheckGameNameFunc()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            gameName = "YSMS";
            levelBonus = YSMSIngameMgr.inst.gameLevel * 5;
            timeBonus = YSMSIngameMgr.inst.bombCount * 3;
            inSchoolCompetitionText.text = "<color=#ffff00>-��</color>\n" + GlobalValue.g_YSMSBestScore.ToString();
            regionCompetitionText.text = "<color=#ffff00>-��</color>  " + GlobalValue.g_YSMSRegionScore.ToString();

            //���ʽ��� ������ ���� ���ھ� ���
            if (int.MaxValue - 10 <= (YSMSIngameMgr.inst.currentScore * (1 + (float)((levelBonus + timeBonus) / 100.0f))))
                resultScore = int.MaxValue - 10;
            else
                resultScore = (int)(YSMSIngameMgr.inst.currentScore * (1 + (float)((levelBonus + timeBonus) / 100.0f)));

            rewardGold = (int)(500 * (1 + (float)((levelBonus + timeBonus) / 100.0f)));
            rewardGoldText.text = rewardGold.ToString();

            if (resultScore <= GlobalValue.g_YSMSBestScore)
                bestScoreText.text = GlobalValue.g_YSMSBestScore.ToString("N0");
            else
                bestScoreText.text = resultScore.ToString("N0");

            currentBestScore = GlobalValue.g_YSMSBestScore;
            myRank = GlobalValue.g_MyYSMSRank;
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            //gameName = "SDJR";
            //levelBonus = SDJRIngameMgr.inst.gameLevel * 4;
            //timeBonus = SDJRIngameMgr.inst.bombCount * 3;
            //inSchoolCompetitionText.text = "<color=#ffff00>-��</color>\n" + GlobalValue.g_SDJRBestScore.ToString();
            //regionCompetitionText.text = "<color=#ffff00>-��</color>  " + GlobalValue.g_SDJRRegionScore.ToString();

            ////���ʽ��� ������ ���� ���ھ� ���
            //if (int.MaxValue - 10 <= (SDJRIngameMgr.inst.currentScore * (1 + (float)((levelBonus + timeBonus) / 100.0f))))
            //    resultScore = int.MaxValue - 10;
            //else
            //    resultScore = (int)(SDJRIngameMgr.inst.currentScore * (1 + (float)((levelBonus + timeBonus) / 100.0f)));

            //rewardGold = (int)(500 * (1 + (float)(3 * ((levelBonus - 1) + timeBonus) / 100.0f)));
            //rewardGoldText.text = rewardGold.ToString();

            //if (resultScore <= GlobalValue.g_SDJRBestScore)
            //    bestScoreText.text = GlobalValue.g_SDJRBestScore.ToString("N0");
            //else
            //    bestScoreText.text = resultScore.ToString("N0");

            //currentBestScore = GlobalValue.g_SDJRBestScore;
            //myRank = GlobalValue.g_MySDJRRank;
        }

        //��� �� ����
        currentGold = GlobalValue.g_UserGold;
        GlobalValue.g_UserGold += rewardGold;
        NetworkMgr.inst.PushPacket(PacketType.UserMoney);
        goldUpSpeed = (float)rewardGold / 1.5f;

        //���� ������ ǥ��
        currentScore = 0;
        currentScoreUpSpeed = (float)(resultScore) / 1.5f;

        //��� ��� ����
        scoreDifference = resultScore - currentBestScore;
        currentTotalScore = GlobalValue.g_TotalScore;

        //���� ���� ����
        GetRegionScoreFunc(gameName);
        SoundManager.instance.PlayBGMAfterSound("Clear", "MainBGM");

        if (scoreDifference > 0)
        {
            if (gameName == "YSMS")
            {
                GlobalValue.g_YSMSBestScore += scoreDifference;
                NetworkMgr.inst.PushPacket(PacketType.YSMSBestScore);
            }
            else if (gameName == "SDJR")
            {
                GlobalValue.g_SDJRBestScore += scoreDifference;
                NetworkMgr.inst.PushPacket(PacketType.SDJRBestScore);
            }
            myRank = 0;
            GlobalValue.g_TotalScore += scoreDifference;
            NetworkMgr.inst.PushPacket(PacketType.TotalBestScore);
            differenceUpSpeed = scoreDifference / 1.5f;
            isGetRegion[1] = true;
            SoundManager.instance.PlayerSound("Wow");
            
        }
    }

    void GetRegionScoreFunc(string gameName)
    {
        if (GlobalValue.g_UniqueID == "") return;
        if (gameName == "") return;

        var request = new GetLeaderboardRequest()
        {
            StartPosition = 0,
            StatisticName = gameName + "RegionScore",
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
                regionScore = result.Leaderboard[0].StatValue;
                isGetRegion[0] = true;
            },
            (error) =>
            {
                Debug.Log("�������� �ҷ����� ����");
            }
            );
    }


    void FriendBtnFunc()
    {
        SoundManager.instance.PlayerSound("Button");
        messageBoxObj.SetActive(true);
        msgBox.SetMessageText("�̱��� �˸�", "���� ������Ʈ�� ��ٷ��ּ��� ��0��", MessageState.OK);
    }

    void ShowScoreFunc()
    {
        //��� �ö󰡴� ����
        if (currentGold < GlobalValue.g_UserGold)
        {
            currentGold += (int)(Time.deltaTime * goldUpSpeed);
            if (GlobalValue.g_UserGold <= currentGold)
                currentGold = GlobalValue.g_UserGold;
            userGoldText.text = currentGold.ToString("N0");
        }

        //���� ���� �ö󰡴� ����
        currentScore += (int)(Time.deltaTime * currentScoreUpSpeed);
        if (resultScore <= currentScore)
            currentScore = resultScore;
        resultScoreText.text = currentScore.ToString("N0");

        if (scoreDifference > 0)    //��� ���
        {
            upImg.gameObject.SetActive(true);

            if (currentTotalScore < GlobalValue.g_TotalScore)
            {
                currentTotalScore += (int)(Time.deltaTime * differenceUpSpeed);
                if (GlobalValue.g_TotalScore <= currentTotalScore)
                {
                    currentTotalScore = GlobalValue.g_TotalScore;
                    newRecordImg.gameObject.SetActive(true);
                }
            }

            if (gameName == "YSMS")
            {
                if (currentBestScore < GlobalValue.g_YSMSBestScore)
                {
                    currentBestScore += (int)(Time.deltaTime * differenceUpSpeed);
                    if (GlobalValue.g_YSMSBestScore <= currentBestScore)
                        currentBestScore = GlobalValue.g_YSMSBestScore;
                }
            }
            else if (gameName == "SDJR")
            {
                if (currentBestScore < GlobalValue.g_SDJRBestScore)
                {
                    currentBestScore += (int)(Time.deltaTime * differenceUpSpeed);
                    if (GlobalValue.g_SDJRBestScore <= currentBestScore)
                        currentBestScore = GlobalValue.g_SDJRBestScore;
                }
            }
        }
        totalScoreText.text = currentTotalScore.ToString("N0");
        if (myRank == 0)
        {
            inSchoolCompetitionText.text = "<color=#ffff00>-��</color>\n" + currentBestScore.ToString("N0");
            RefreshMyRankFunc(gameName);
        }
        else
        {
            inSchoolCompetitionText.text = "<color=#ffff00>" + myRank + "��</color>\n" + currentBestScore.ToString("N0");
        }

    }

    void RefreshMyRankFunc(string gameName)
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
                    if (gameName == "YSMS")
                    {
                        GlobalValue.g_MyYSMSRank = curBoard.Position + 1;
                        myRank = GlobalValue.g_MyYSMSRank;
                    }
                    else if (gameName == "SDJR")
                    {
                        GlobalValue.g_MySDJRRank = curBoard.Position + 1;
                        myRank = GlobalValue.g_MySDJRRank;
                    }
                }
            },
            (error) =>
            {
                Debug.Log("��� �������� ����");
            }
            );
    }

    void ShowRegionScore()
    {
        if (!isGetRegion[0]) return;

        if (scoreDifference > 0)
        {
            if (gameName == "YSMS")
            {
                regionScore += (int)(Time.deltaTime * differenceUpSpeed);
                if (GlobalValue.g_YSMSRegionScore <= regionScore)
                {
                    regionScore = GlobalValue.g_YSMSRegionScore;
                    isGetRegion[0] = false;
                }
            }
            else if (gameName == "SDJR")
            {
                regionScore += (int)(Time.deltaTime * differenceUpSpeed);
                if (GlobalValue.g_SDJRRegionScore <= regionScore)
                {
                    regionScore = GlobalValue.g_SDJRRegionScore;
                    isGetRegion[0] = false;
                }
            }
        }
        else
            friendBtn.interactable = false;

        regionCompetitionText.text = "<color=#ffff00>" + GlobalValue.g_YSMSRegionRank + "��</color>  " + regionScore.ToString("N0");
    }

}
