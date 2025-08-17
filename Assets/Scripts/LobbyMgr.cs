using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMgr : MonoBehaviour
{
    [SerializeField] internal GameObject lobbyCanvas;
    [SerializeField] internal Image bgImg;
    [SerializeField] internal Button backBtn;
    [SerializeField] internal Text totalScoreTxt;
    
    [SerializeField] internal Button YSMSGameBtn;
    [SerializeField] internal Text YSMSBestScoreTxt;

    [SerializeField] internal Button SDJRGameBtn;
    [SerializeField] internal Text SDJRBestScoreTxt;

    [SerializeField] GameObject msgBoxObj;
    MessageBox msgBox;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!lobbyCanvas) lobbyCanvas = GameObject.Find("Canvas").gameObject;
        if (!bgImg) bgImg = lobbyCanvas.transform.Find("BGImg").GetComponent<Image>();
        if (!backBtn) backBtn = bgImg.transform.Find("BackBtn").GetComponent<Button>();
        if (!msgBoxObj) msgBoxObj = lobbyCanvas.transform.Find("MessageBox").gameObject;
        
        msgBox = msgBoxObj.GetComponent<MessageBox>();
    }


    // Start is called before the first frame update
    void Start()
    {
        CheckGM();
        totalScoreTxt.text = GlobalValue.g_TotalScore.ToString("N0");
        YSMSBestScoreTxt.text = GlobalValue.g_YSMSBestScore.ToString("N0");
        SDJRBestScoreTxt.text = GlobalValue.g_SDJRBestScore.ToString("N0");

        if (backBtn != null) 
            backBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("로그아웃 알림", "정말로 로그아웃 하시겠어요?", MessageState.YesNo);
            GlobalValue.g_MessYesNoKind = MessageYesNoKind.LobbyLogout;
        });

        if (YSMSGameBtn != null)
            YSMSGameBtn.onClick.AddListener(() => { SetGameKind(GameKind.YSMS); });

        if (SDJRGameBtn != null)
            SDJRGameBtn.onClick.AddListener(() => { SetGameKind(GameKind.SDJR); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CheckGM()
    {
        if (GlobalValue.g_Nickname == "운영자")
        {
            if (GlobalValue.g_GMGEM > 0) GlobalValue.g_UserGem = GlobalValue.g_GMGEM;
            if (GlobalValue.g_GMGOLD > 0) GlobalValue.g_UserGold = GlobalValue.g_GMGOLD;
            if (GlobalValue.g_GMRICE > 0) GlobalValue.g_RiceCount = GlobalValue.g_GMRICE;
        }

        if (GlobalValue.g_isFirstLogin)
        {
            GlobalValue.g_UserGem = 10;
            GlobalValue.g_UserGold = 20000;
            GlobalValue.g_isFirstLogin = false;
        }
    }

    void SetGameKind(GameKind gKind)
    {
        SoundManager.instance.PlayerSound("Button");
        GlobalValue.g_GameKind = gKind;
        SceneManager.LoadScene("ReadyScene");
    }
}
