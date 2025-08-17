using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MessageState
{
    OK,
    YesNo,
    MessageKind
}

public enum MessageYesNoKind
{
    GotoReady,
    LobbyLogout,
    GotoLobby,
    YesNoKindCount
}

public class MessageBox : MonoBehaviour
{
    [SerializeField] internal Button messageOKBtn;
    [SerializeField] internal Button messageYesBtn;
    [SerializeField] internal Button messageNoBtn;
    [SerializeField] internal Text messageLabel;
    [SerializeField] internal Text messageText;
    MessageState messState = MessageState.MessageKind;

    public static MessageBox inst;

    private void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!messageOKBtn) messageOKBtn = transform.Find("MessageOKBtn").GetComponent<Button>();
        if (!messageYesBtn) messageYesBtn = transform.Find("MessageYesBtn").GetComponent<Button>();
        if (!messageNoBtn) messageNoBtn = transform.Find("MessageNoBtn").GetComponent<Button>();
        if (!messageLabel) messageLabel = transform.Find("MessageLabel").GetComponent<Text>();
        if (!messageText) messageText = transform.Find("MessageText").GetComponent<Text>();

        inst = this;
    }


    private void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        if (messageOKBtn != null) messageOKBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayerSound("Button");
            gameObject.SetActive(false);
        });
        if (messageYesBtn != null) messageYesBtn.onClick.AddListener(YesBtnFunc);
        if (messageNoBtn != null) messageNoBtn.onClick.AddListener(NoBtnFunc);
    }

    //private void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }


    public void SetMessageText(string label, string text, MessageState messState = MessageState.OK)
    {
        if (messState == MessageState.OK)
        {
            messageOKBtn.gameObject.SetActive(true);
            messageYesBtn.gameObject.SetActive(false);
            messageNoBtn.gameObject.SetActive(false);
        }
        else if(messState == MessageState.YesNo)
        {
            messageOKBtn.gameObject.SetActive(false);
            messageYesBtn.gameObject.SetActive(true);
            messageNoBtn.gameObject.SetActive(true);
        }
        messageLabel.text = "¡Ú " + label + " ¡Ú";
        messageText.text = text;
    }

    void YesBtnFunc()
    {
        SoundManager.instance.PlayerSound("Button");
        Time.timeScale = 1.0f;
        if (GlobalValue.g_MessYesNoKind == MessageYesNoKind.LobbyLogout)
        {
            GlobalValue.g_UniqueID = "";
            GlobalValue.g_Nickname = "";
            GlobalValue.g_UserGold = 0;
            GlobalValue.g_UserGem = 0;
            GlobalValue.g_ExpPercent = 0;
            GlobalValue.g_RiceCount = 0;
            GlobalValue.g_IsRiceTimerStart = 0;
            GlobalValue.g_RiceCheckTime = 0;
            GlobalValue.g_RiceCheckDate = 0;
            GlobalValue.g_YSMSBestScore = 0;
            GlobalValue.g_SDJRBestScore = 0;
            GlobalValue.g_TotalScore = 0;
            for (int ii = 0; ii < 3; ii++)
            {
                GlobalValue.g_YSMSUpgradeLv[ii] = 0;
                GlobalValue.g_SDJRUpgradeLv[ii] = 0;
            }
            GlobalValue.g_YSMSTutSkipYN = 0;
            GlobalValue.g_SDJRTutSkipYN = 0;
            GlobalValue.g_GMGOLD = 0;
            GlobalValue.g_GMGEM = 0;
            GlobalValue.g_GMRICE = 0;
            GlobalValue.g_MessYesNoKind = MessageYesNoKind.YesNoKindCount;
            SceneManager.LoadScene("TitleScene");
        }
        else if (GlobalValue.g_MessYesNoKind == MessageYesNoKind.GotoReady)
        {
            if (GlobalValue.g_GameKind == GameKind.YSMS)
            {
                YSMSIngameMgr.spawnList.Clear();
            }
            else if (GlobalValue.g_GameKind == GameKind.SDJR)
            {

            }

            SceneManager.LoadScene("ReadyScene");
        }
        gameObject.SetActive(false);
    }

    void NoBtnFunc()
    {
        SoundManager.instance.PlayerSound("Button");
        gameObject.SetActive(false);
    }
}
