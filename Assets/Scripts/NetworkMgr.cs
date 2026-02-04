using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
//using UnityEditor.PackageManager;

public enum PacketType
{
    UserMoney = 0,
    UserRice,
    TotalBestScore,
    YSMSBestScore,
    YSMSUpgradeLv,
    YSMSRegionScore,
    SDJRBestScore,
    SDJRUpgradeLv,
    SDJRRegionScore,
    NickUpdate,
    SkipOnOff
}

public class NetworkMgr : MonoBehaviour
{
    bool isNetworkLock = false;
    List<PacketType> packetBuff = new List<PacketType>();

    [HideInInspector] public string tempStrBuff = "";

    public static NetworkMgr inst;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        inst = this;
    }

    //void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNetworkLock)
        {
            if (0 < packetBuff.Count)
                ReqNetwork();
        }
    }

    void ReqNetwork()
    {
        if (packetBuff[0] == PacketType.UserMoney)
            UpdateUserMoneyCo();
        else if (packetBuff[0] == PacketType.UserRice)
            UpdateUserRiceCo();
        else if (packetBuff[0] == PacketType.TotalBestScore)
            UpdateTotalScoreCo();
        else if (packetBuff[0] == PacketType.YSMSBestScore)
            UpdateYSMSBestScoreCo();
        else if (packetBuff[0] == PacketType.YSMSUpgradeLv)
            UpdateYSMSUpgradeLvCo();
        else if (packetBuff[0] == PacketType.YSMSRegionScore)
            UpdateYSMSRegionScoreCo();
        else if (packetBuff[0] == PacketType.SDJRBestScore)
            UpdateSDJRBestScoreCo();
        else if (packetBuff[0] == PacketType.SDJRUpgradeLv)
            UpdateSDJRUpgradeLvCo();
        else if (packetBuff[0] == PacketType.SDJRRegionScore)
            UpdateSDJRRegionScoreCo();
        else if (packetBuff[0] == PacketType.NickUpdate)
            UpdateNickCo(tempStrBuff);
        else if (packetBuff[0] == PacketType.SkipOnOff)
            UpdateSkipCo();

        packetBuff.RemoveAt(0);
    }

    void UpdateUserMoneyCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"UserGold", GlobalValue.g_UserGold.ToString() },
                {"UserGem", GlobalValue.g_UserGem.ToString() }
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateUserRiceCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"UserRice", GlobalValue.g_RiceCount.ToString() },
                {"RiceFillTime", GlobalValue.g_RiceFillTime.ToString() },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateTotalScoreCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "TotalScore",
                    Value = GlobalValue.g_TotalScore,
                },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateYSMSBestScoreCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "YSMSBestScore",
                    Value = GlobalValue.g_YSMSBestScore,
                },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateYSMSUpgradeLvCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"YSMSBonusUGLv", GlobalValue.g_YSMSUpgradeLv[0].ToString() },
                {"YSMSFeverUGLv", GlobalValue.g_YSMSUpgradeLv[1].ToString() },
                {"YSMSSuperUGLv", GlobalValue.g_YSMSUpgradeLv[2].ToString() },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateYSMSRegionScoreCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "YSMSRegionScore",
                    Value = GlobalValue.g_YSMSRegionScore,
                },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateSDJRBestScoreCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "SDJRBestScore",
                    Value = GlobalValue.g_SDJRBestScore,
                },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateSDJRUpgradeLvCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"SDJRBonusUGLv", GlobalValue.g_SDJRUpgradeLv[0].ToString() },
                {"SDJRFeverUGLv", GlobalValue.g_SDJRUpgradeLv[1].ToString() },
                {"SDJRSuperUGLv", GlobalValue.g_SDJRUpgradeLv[2].ToString() },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateSDJRRegionScoreCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "SDJRRegionScore",
                    Value = GlobalValue.g_SDJRRegionScore,
                },
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    void UpdateNickCo(string nickName)
    {
        if (GlobalValue.g_UniqueID == "") return;
        if (nickName == "") return;

        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = nickName
            },
            (result) =>
            {
                GlobalValue.g_Nickname = result.DisplayName;
                isNetworkLock = false;
            },
            (error) =>
            {
                Debug.Log(error.GenerateErrorReport());
                isNetworkLock = false;
            }
            );
    }

    void UpdateSkipCo()
    {
        if (GlobalValue.g_UniqueID == "") return;

        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"YSMSTutSkipOnOff", GlobalValue.g_YSMSTutSkipYN.ToString() },
                {"SDJRTutSkipOnOff", GlobalValue.g_SDJRTutSkipYN.ToString() },                
            }
        };
        isNetworkLock = true;
        PlayFabClientAPI.UpdateUserData(
            request,
            (result) =>
            {
                isNetworkLock = false;
            },
            (error) =>
            {
                isNetworkLock = false;
            }
            );
    }

    public void PushPacket(PacketType pType)
    {
        bool isExist = false;
        for (int ii = 0; ii < packetBuff.Count; ii++)
        {
            if (packetBuff[ii] == pType)
                isExist = true;
        }

        if (!isExist)
            packetBuff.Add(pType);
    }
}
