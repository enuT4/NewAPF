using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] internal Button continueBtn;
    [SerializeField] internal Button restartBtn;
    [SerializeField] internal Button gamehelpBtn;
    [SerializeField] internal Button gotolobbyBtn;
    [SerializeField] internal GameObject gamehelpPanelObj;
    [SerializeField] internal Button gamehelpPanelCloseBtn;
    GameObject[] gamehelpImgArr;
    int gameCount;

    [SerializeField] GameObject msgBoxObj;
    MessageBox msgBox;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!continueBtn) continueBtn = transform.Find("ContinueBtn").GetComponent<Button>();
        if (!restartBtn) restartBtn = transform.Find("RestartBtn").GetComponent<Button>();
        if (!gamehelpBtn) gamehelpBtn = transform.Find("GameHelpBtn").GetComponent<Button>();
        if (!gotolobbyBtn) gotolobbyBtn = transform.Find("GotoLobbyBtn").GetComponent<Button>();
        if (!gamehelpPanelObj) gamehelpPanelObj = transform.Find("GameHelpPanelImg").gameObject;
        if (!gamehelpPanelCloseBtn) gamehelpPanelCloseBtn = gamehelpPanelObj.transform.GetChild(1).GetComponent<Button>();
        if (msgBoxObj != null) msgBox = msgBoxObj.GetComponent<MessageBox>();
        if (gamehelpPanelObj != null) gameCount = gamehelpPanelObj.transform.Find("GameHelp").transform.childCount;
        gamehelpImgArr = new GameObject[gameCount];
        for (int ii = 0; ii < gameCount; ii++)
            gamehelpImgArr[ii] = gamehelpPanelObj.transform.Find("GameHelp").transform.GetChild(ii).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gamehelpPanelObj != null && gamehelpPanelObj.activeSelf)
            gamehelpPanelObj.SetActive(false);

        if (continueBtn != null)
            continueBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                //게임 일시정지 풀렸을 때 콤보 및 시간 재개 함수
                if (GlobalValue.g_GameKind == GameKind.YSMS)
                    YSMSIngameMgr.inst.PauseBtnFunc(false);

            });

        if (restartBtn != null)
            restartBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                msgBoxObj.SetActive(true);
                msgBox.SetMessageText("경고 알림", "게임을 나가면 기록이 저장되지 않아요 ㅠ0ㅠ\n" +
                    "그래도 다시 하시겠어요?", MessageState.YesNo);
                GlobalValue.g_MessYesNoKind = MessageYesNoKind.GotoReady;
            });

        if (gamehelpBtn != null)
            gamehelpBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                if (gamehelpPanelObj != null)
                    gamehelpPanelObj.SetActive(true);
            });

        if (gotolobbyBtn != null)
            gotolobbyBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                msgBoxObj.SetActive(true);
                msgBox.SetMessageText("경고 알림", "게임을 나가면 기록이 저장되지 않아요 ㅠ0ㅠ\n" +
                    "그래도 다시 하시겠어요?", MessageState.YesNo);
                GlobalValue.g_MessYesNoKind = MessageYesNoKind.GotoLobby;
            });

        if (gamehelpPanelCloseBtn != null)
            gamehelpPanelCloseBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayerSound("Button");
                if (gamehelpPanelObj != null)
                    gamehelpPanelObj.SetActive(false);
            });

        if(msgBoxObj.activeSelf) msgBoxObj.SetActive(false);

        CheckGameHelpFunc();
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    void CheckGameHelpFunc()
    {
        for (int ii = 0; ii < gameCount; ii++)
        {
            if (gamehelpImgArr[ii] != null && gamehelpImgArr[ii].activeSelf)
                gamehelpImgArr[ii].SetActive(false);
        }

        if (GlobalValue.g_GameKind == GameKind.YSMS)
            gamehelpImgArr[0].SetActive(true);
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
            gamehelpImgArr[1].SetActive(true);

    }
}
