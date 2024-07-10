using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
//이메일 형식 스크립트
using System.Globalization;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;

public class LoginPanel : MonoBehaviour
{
    [Header("-------- Login --------")]
    [SerializeField] internal GameObject loginPanelObj;
    [SerializeField] internal InputField loginIDInputField;
    [SerializeField] internal InputField loginPWInputField;
    [SerializeField] internal Button loginBtn;
    [SerializeField] internal Button loginSignUpBtn;
    [SerializeField] internal Button loginCloseBtn;
    string lIDStr;
    string lPWStr;

    [Header("-------- SignUp --------")]
    [SerializeField] internal GameObject signupPanelObj;
    [SerializeField] internal InputField signupIDInputField;
    [SerializeField] internal InputField signupPWInputField;
    [SerializeField] internal InputField signupPWCfmInputField;
    [SerializeField] internal Text signupIDCfmText;
    [SerializeField] internal Button signupIDCfmBtn;
    [SerializeField] internal Button signupCreateIDBtn;
    [SerializeField] internal Button signupCloseBtn;
    string suIDStr;
    string suPWStr;
    string suPWCfmStr;
    bool isIDCfm = false;
    bool invalidEmailType = false;       // 이메일 포맷이 올바른지 체크
    bool isValidFormat = false;          // 올바른 형식인지 아닌지 체크

    string msgText;
    float msgTimer = 0.0f;
    WaitForSeconds msgWFS = new WaitForSeconds(5.0f);

    private void Awake() => AwakeFunc();
    void AwakeFunc()
    {
        if (!loginPanelObj) loginPanelObj = transform.Find("LoginPanel").gameObject;
        if (!loginIDInputField) loginIDInputField = loginPanelObj.transform.Find("LoginBoxIDInputField").GetComponent<InputField>();
        if (!loginPWInputField) loginPWInputField = loginPanelObj.transform.Find("LoginBoxPWInputField").GetComponent<InputField>();
        if (!loginBtn) loginBtn = loginPanelObj.transform.Find("LoginOKBtn").GetComponent<Button>();
        if (!loginSignUpBtn) loginSignUpBtn = loginPanelObj.transform.Find("LoginSignupBtn").GetComponent<Button>();
        if (!loginCloseBtn) loginCloseBtn = loginPanelObj.transform.Find("LoginPanelCloseBtn").GetComponent<Button>();

        if (!signupPanelObj) signupPanelObj = transform.Find("SignupPanel").gameObject;
        if (!signupIDInputField) signupIDInputField = signupPanelObj.transform.Find("SignUpBoxIDInputField").GetComponent<InputField>();
        if (!signupPWInputField) signupPWInputField = signupPanelObj.transform.Find("SignUpBoxPWInputField").GetComponent<InputField>();
        if (!signupPWCfmInputField) signupPWInputField = signupPanelObj.transform.Find("SignUpBoxPWCfmInputField").GetComponent<InputField>();
        if (!signupIDCfmBtn) signupIDCfmBtn = signupPanelObj.transform.Find("IDCfmBtn").GetComponent<Button>();
        if (!signupCreateIDBtn) signupCreateIDBtn = signupPanelObj.transform.Find("CreateIDBtn").GetComponent<Button>();
        if (!signupIDCfmText) signupIDCfmText = signupPanelObj.transform.Find("IDCfmText").GetComponent<Text>();
        if (!signupCloseBtn) signupCloseBtn = signupPanelObj.transform.Find("SignUpPanelCloseBtn").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (loginBtn != null) loginBtn.onClick.AddListener(LoginBtnFunc);
        if (loginSignUpBtn != null) loginSignUpBtn.onClick.AddListener(() =>
        {
            loginPanelObj.SetActive(false);
            signupPanelObj.SetActive(true);
        });
        if (loginCloseBtn != null) loginCloseBtn.onClick.AddListener(() => gameObject.SetActive(false));

        if (signupIDCfmBtn != null) signupIDCfmBtn.onClick.AddListener(IDConfirmFunc);
        if (signupCreateIDBtn != null) signupCreateIDBtn.onClick.AddListener(CreateIDFunc);
        if (signupCloseBtn != null) signupCloseBtn.onClick.AddListener(() =>
        {
            loginPanelObj.SetActive(true);
            signupPanelObj.SetActive(false);
        });

        if (!string.IsNullOrEmpty(signupIDCfmText.text)) signupIDCfmText.text = "";
        if (signupIDCfmText.gameObject.activeSelf) signupIDCfmText.gameObject.SetActive(false);
        //초기화
        suIDStr = "";
        suPWStr = "";
        suPWCfmStr = "";
        isIDCfm = false;
    }

    // Update is called once per frame
    void Update()
    {
        //(PC)탭(쉬프트탭)키를 누를 때 인풋필드간 이동
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginPWInputField.isFocused) loginIDInputField.Select();

            if (signupPWInputField.isFocused) signupIDInputField.Select();
            if (signupPWCfmInputField.isFocused) signupPWInputField.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginIDInputField.isFocused) loginPWInputField.Select();

            if (signupIDInputField.isFocused) signupPWInputField.Select();
            if (signupPWInputField.isFocused) signupPWCfmInputField.Select();
        }
    }

    private void OnEnable()
    {
        if (!loginPanelObj.activeSelf) loginPanelObj.SetActive(true);
        if (signupPanelObj.activeSelf) signupPanelObj.SetActive(false);
    }
    void LoginBtnFunc()         //로그인 함수
    {
        lIDStr = loginIDInputField.text;
        lPWStr = loginPWInputField.text;
        lIDStr = lIDStr.Trim();
        lPWStr = lPWStr.Trim();

        if (string.IsNullOrEmpty(lIDStr) || string.IsNullOrEmpty(lPWStr))
        {
            MessageBox.inst.SetMessageText("로그인 오류 알림", "빈칸 없이 입력해주세요~!");
            return;
        }

        if (!(3 <= lIDStr.Length && lIDStr.Length < 20))
        {
            MessageBox.inst.SetMessageText("로그인 오류 알림", "아이디는 3글자 이상 20글자 미만으로 작성해주세요~!");
            return;
        }

        if (!(6 <= lPWStr.Length && lPWStr.Length < 20))
        {
            MessageBox.inst.SetMessageText("로그인 오류 알림", "비밀번호는 6글자 이상 20글자 미만으로 작성해주세요~!");
            return;
        }

        if (!CheckEmailAddress(lIDStr))
        {
            MessageBox.inst.SetMessageText("로그인 오류 알림", "이메일 형식이 맞지 않아요 ㅠ0ㅠ");
            return;
        }

        var option = new GetPlayerCombinedInfoRequestParams()
        {
            GetPlayerProfile = true,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowAvatarUrl = true,
            },

            GetPlayerStatistics = true,
            GetUserData = true

        };

        var request = new LoginWithEmailAddressRequest()
        {
            Email = lIDStr,
            Password = lPWStr,
            InfoRequestParameters = option
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        Debug.Log("로그인 함수");
    }

    void OnLoginSuccess(PlayFab.ClientModels.LoginResult result)
    {
        GlobalValue.g_UniqueID = result.PlayFabId;

        if (result.InfoResultPayload != null)
        {
            GlobalValue.g_Nickname = result.InfoResultPayload.PlayerProfile.DisplayName;

            foreach (var eachStat in result.InfoResultPayload.PlayerStatistics)
            {
                if (eachStat.StatisticName == "YSMSBestScore")
                    GlobalValue.g_YSMSBestScore = eachStat.Value;
                else if (eachStat.StatisticName == "SDJRBestScore")
                    GlobalValue.g_SDJRBestScore = eachStat.Value;
                else if (eachStat.StatisticName == "TotalScore")
                    GlobalValue.g_TotalScore = eachStat.Value;
            }

            int a_GetValue = 0;
            foreach (var eachData in result.InfoResultPayload.UserData)
            {
                if (eachData.Key == "UserGold")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_UserGold = a_GetValue;
                }
                else if (eachData.Key == "UserGem")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_UserGem = a_GetValue;
                }
                else if (eachData.Key == "UserRice")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_RiceCount = a_GetValue;
                }
                else if (eachData.Key == "IsRiceTimer")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_IsRiceTimerStart = a_GetValue;
                }
                else if (eachData.Key == "RiceCheckTime")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_RiceCheckTime = a_GetValue;
                }
                else if (eachData.Key == "RiceCheckDate")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_RiceCheckDate = a_GetValue;
                }
                else if (eachData.Key == "YSMSBonusUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_YSMSUGLv[0] = a_GetValue;
                }
                else if (eachData.Key == "YSMSFeverUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_YSMSUGLv[1] = a_GetValue;
                }
                else if (eachData.Key == "YSMSSuperUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_YSMSUGLv[2] = a_GetValue;
                }
                else if (eachData.Key == "YSMSTutSkipOnOff")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_YSMSTutSkipYN = a_GetValue;
                }
                else if (eachData.Key == "SDJRBonusUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_SDJRUGLv[0] = a_GetValue;
                }
                else if (eachData.Key == "SDJRFeverUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_SDJRUGLv[1] = a_GetValue;
                }
                else if (eachData.Key == "SDJRSuperUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_SDJRUGLv[2] = a_GetValue;
                }
                else if (eachData.Key == "SDJRTutSkipOnOff")
                {
                    if (int.TryParse(eachData.Value.Value, out a_GetValue))
                        GlobalValue.g_SDJRTutSkipYN = a_GetValue;
                }
            }
        }

        if (GlobalValue.g_isFirstLogin)
        {
            GlobalValue.g_GMGOLD = 10000000;
            GlobalValue.g_GMGEM = 10000;
            GlobalValue.g_GMRICE = 100;
            SceneManager.LoadScene("CreateCharScene");
        }
        else
        {
            SceneManager.LoadScene("LobbyScene");
        }



    }

    void OnLoginFailure(PlayFabError error)
    {
        string errorReason = error.GenerateErrorReport();
        if (errorReason.Contains("User not found") || errorReason.Contains("Invalid email address"))
            errorReason = "아이디나 비밀번호를 확인해주세요~!";
        else if (errorReason.Contains("Email address is not valid"))
            errorReason = "이메일 형식이 맞지 않아요 ㅠ0ㅠ";

        MessageBox.inst.SetMessageText("로그인 오류", errorReason);
    }

    void IDConfirmFunc()        //아이디 확인 함수
    {
        suIDStr = signupIDInputField.text;
        suIDStr = suIDStr.Trim();

        if (string.IsNullOrEmpty(suIDStr))
        {
            MsgOn("빈칸 없이 입력해주세요~!");
            return;
        }

        if (!(3 <= suIDStr.Length && suIDStr.Length < 20))
        {
            MsgOn("아이디는 3글자 이상 20글자 미만으로 작성해주세요~!");
            return;
        }

        if (!CheckEmailAddress(suIDStr))
        {
            MsgOn("이메일 형식이 맞지 않아요 ㅠ0ㅠ");
            return;
        }

        var request = new RegisterPlayFabUserRequest()
        {
            Email = suIDStr,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnIDCfmSuccess, OnIDCfmFailure);


        Debug.Log("아이디 확인 함수");
    }

    void OnIDCfmSuccess(RegisterPlayFabUserResult result)
    {
        MsgOn("사용할 수 있는 이메일이에요~!");
        isIDCfm = true;
    }

    void OnIDCfmFailure(PlayFabError error)
    {
        if (error.GenerateErrorReport().Contains("already exists"))
            MsgOn("중복된 이메일이 있네요 ㅠ0ㅠ");
        if (isIDCfm) isIDCfm = false;
    }

    void CreateIDFunc()         //아이디 생성 함수
    {
        if (isIDCfm)
        {
            MessageBox.inst.SetMessageText("가입 오류 알림", "아이디 중복확인을 먼저 해주세요 ㅠ0ㅠ");
            return;
        }
        suPWStr = signupPWInputField.text;
        suPWCfmStr = signupPWCfmInputField.text;

        suPWStr = suPWStr.Trim();
        suPWCfmStr = suPWCfmStr.Trim();

        if (string.IsNullOrEmpty(suIDStr) || string.IsNullOrEmpty(suPWStr) || string.IsNullOrEmpty(suPWCfmStr))
        {
            MessageBox.inst.SetMessageText("가입 오류 알림", "빈칸 없이 입력해주세요~!");
            return;
        }

        if (!(6 <= suPWStr.Length && suPWStr.Length < 20))
        {
            MessageBox.inst.SetMessageText("가입 오류 알림", "비밀번호는 6글자 이상 20글자 미만으로 작성해주세요~!");
            return;
        }

        if (suPWCfmStr != suPWStr)
        {
            MessageBox.inst.SetMessageText("가입 오류 알림", "비밀번호가 일치하지 않아요 ㅠ-ㅠ");
            return;
        }

        var request = new RegisterPlayFabUserRequest()
        {
            Email = suIDStr,
            Password = suPWStr,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnIDRegisterSuccess, OnIDRegisterFailure);
        Debug.Log("아이디 생성 함수");
    }

    void OnIDRegisterSuccess(RegisterPlayFabUserResult result)
    {
        MessageBox.inst.SetMessageText("가입 성공", "새로운 가족이 된 걸 축하해요 >w<");
        if (loginPanelObj != null) loginPanelObj.SetActive(true);
        if (signupPanelObj != null) signupPanelObj.SetActive(false);
        GlobalValue.g_isFirstLogin = true;
    }

    void OnIDRegisterFailure(PlayFabError error)
    {
        string errorReason = error.GenerateErrorReport();
        if (errorReason.Contains("already exists"))
            errorReason = "중복된 이메일이 있네요 ㅠ0ㅠ";
        else if (errorReason.Contains("address is not valid"))
            errorReason = "이메일 형식이 맞지 않아요~!";

        MessageBox.inst.SetMessageText("가입 오류 알림", errorReason);
    }

    void MsgOn(string mess = "")
    {
        signupIDCfmText.text = mess;
        if (!signupIDCfmText.gameObject.activeSelf) signupIDCfmText.gameObject.SetActive(true);
        StartCoroutine(DelayTime());
    }

    IEnumerator DelayTime()
    {
        yield return msgWFS;
        if (signupIDCfmText.gameObject.activeSelf) signupIDCfmText.gameObject.SetActive(false);
    }

    private bool CheckEmailAddress(string emailStr)
    {
        if (string.IsNullOrEmpty(emailStr)) isValidFormat = false;

        emailStr = Regex.Replace(emailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true 로 반환할 시, 올바른 이메일 포맷임.
        isValidFormat = Regex.IsMatch(emailStr,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
        return isValidFormat;
    }

    private string DomainMapper(Match match)
    {
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalidEmailType = true;
        }
        return match.Groups[1].Value + domainName;
    }
}
