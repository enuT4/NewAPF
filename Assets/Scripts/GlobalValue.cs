using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static int[] g_YSMSUGLv = new int[3];
    public static int g_YSMSTutSkipYN;
    public static int g_YSMSBestScore;

    public static int[] g_SDJRUGLv = new int[3];
    public static int g_SDJRTutSkipYN;
    public static int g_SDJRBestScore;

    public static int g_TotalScore;

    public static int g_GMGOLD = 0;
    public static int g_GMGEM = 0;
    public static int g_GMRICE = 0;

}
