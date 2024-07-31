using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public enum GameKind
{
    YSMS = 0,
    SDJR = 1,
    GameCount,
}

public class GlobalValue : MonoBehaviour
{
    public static bool g_isFirstLogin = false;
    public static string g_UniqueID;
    public static string g_Nickname;

    public static int g_UserGold;
    public static int g_UserGem;

    public static int g_RiceCount;
    public static int g_IsRiceTimerStart;
    public static int g_RiceCheckTime;
    public static int g_RiceCheckDate;

    public static int g_GMGOLD = 0;
    public static int g_GMGEM = 0;
    public static int g_GMRICE = 0;

    public static int[] bonusAmount = new int[16];
    public static int[] bonusUGCost = new int[15] { 1300, 1800, 2400, 3100, 5000, 7000, 9000, 11000, 13000, 15000, 20000, 25000, 30000, 35000, 50000 };
    public static float[] feverAmount = new float[16];
    public static int[] feverUGCost = new int[15] { 1500, 1900, 2500, 3300, 5500, 7700, 9900, 12500, 16000, 20000, 25000, 30000, 35000, 40000, 55000 };
    public static float[] superAmount = new float[16];
    public static int[] superUGCost = new int[15] { 1000, 1500, 2200, 2900, 3800, 4900, 6200, 7500, 9000, 10000, 15000, 20000, 25000, 30000, 45000 };

    public static GameKind g_GameKind = GameKind.GameCount;

    public static int[] g_YSMSUGLevel = new int[3] { 0, 0, 0 };
    public static int g_YSMSBestScore = 0;
    public static int g_MyYSMSRank = 0;
    public static int g_YSMSRegScore = 0;
    public static int g_YSMSRegRank = 1;
    public static int g_YSMSTutSkipYN = 0;

    public static int[] g_SDJRUGLevel = new int[3] { 0, 0, 0 };
    public static int g_SDJRBestScore = 0;
    public static int g_MySDJRRank = 0;
    public static int g_SDJRRegScore = 0;
    public static int g_SDJRRegRank = 1;
    public static int g_SDJRTutSkipYN = 0;

    public static int g_TotalScore = 0;
    public static int g_MyTotalRank = 0;
    public static string g_SchoolName = "";

    public static MessageYesNoKind g_MessYesNoKind = MessageYesNoKind.YesNoKindCount;

    public static void YSMSUGAm()
    {
        bonusAmount = new int[16] { 0, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600, 2000 };
        feverAmount = new float[16] { 1.0f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 3.0f };
        superAmount = new float[16] { 0.0f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 2.0f, 2.2f, 2.4f, 2.6f, 3.0f, 3.3f, 3.6f, 4.0f };
    }

    public static void SDJRUGAm()
    {
        bonusAmount = new int[16] { 0, 500, 700, 900, 1200, 1500, 2000, 3000, 4000, 5500, 7000, 9000, 11000, 13000, 15000, 17000 };
        feverAmount = new float[16] { 1.0f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 3.0f };
        superAmount = new float[16] { 0.0f, 1.0f, 1.2f, 1.4f, 1.7f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 4.5f, 5.0f, 5.5f, 6.0f, 7.0f, 8.0f };
    }


}
