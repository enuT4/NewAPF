using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
//�̸��� ���� ��ũ��Ʈ
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
    [SerializeField] internal InputField signupNickInputField;
    [SerializeField] internal Button signupCreateIDBtn;
    [SerializeField] internal Button signupCloseBtn;
    string suIDStr;
    string suPWStr;
    string suPWCfmStr;
    string suNickStr;
    bool invalidEmailType = false;       // �̸��� ������ �ùٸ��� üũ
    bool isValidFormat = false;          // �ùٸ� �������� �ƴ��� üũ

    string errorReason;
    string errorStr;
    [SerializeField] GameObject msgBoxObj;
    MessageBox msgBox;

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
        if (!signupPWCfmInputField) signupPWCfmInputField = signupPanelObj.transform.Find("SignUpBoxPWCfmInputField").GetComponent<InputField>();
        if (!signupNickInputField) signupNickInputField = signupPanelObj.transform.Find("SignUpBoxNickInputField").GetComponent<InputField>();
        if (!signupCreateIDBtn) signupCreateIDBtn = signupPanelObj.transform.Find("CreateIDBtn").GetComponent<Button>();
        if (!signupCloseBtn) signupCloseBtn = signupPanelObj.transform.Find("SignUpPanelCloseBtn").GetComponent<Button>();

        if (!msgBoxObj) msgBoxObj = transform.Find("MessageBox").gameObject;
        if (msgBoxObj != null) msgBox = msgBoxObj.GetComponent<MessageBox>();
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

        //if (signupIDCfmBtn != null) signupIDCfmBtn.onClick.AddListener(IDConfirmFunc);
        if (signupCreateIDBtn != null) signupCreateIDBtn.onClick.AddListener(CreateIDFunc);
        if (signupCloseBtn != null) signupCloseBtn.onClick.AddListener(() =>
        {
            loginPanelObj.SetActive(true);
            signupPanelObj.SetActive(false);
        });

        //if (!string.IsNullOrEmpty(signupIDCfmText.text)) signupIDCfmText.text = "";
        //if (signupIDCfmText.gameObject.activeSelf) signupIDCfmText.gameObject.SetActive(false);
        //�ʱ�ȭ
        suIDStr = "";
        suPWStr = "";
        suPWCfmStr = "";
        //isIDCfm = false;
    }

    // Update is called once per frame
    void Update()
    {
        //(PC)��(����Ʈ��)Ű�� ���� �� ��ǲ�ʵ尣 �̵�
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginPWInputField.isFocused) loginIDInputField.Select();

            if (signupPWInputField.isFocused) signupIDInputField.Select();
            if (signupPWCfmInputField.isFocused) signupPWInputField.Select();
            if (signupNickInputField.isFocused) signupPWCfmInputField.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginIDInputField.isFocused) loginPWInputField.Select();

            if (signupIDInputField.isFocused) signupPWInputField.Select();
            if (signupPWInputField.isFocused) signupPWCfmInputField.Select();
            if (signupPWCfmInputField.isFocused) signupNickInputField.Select();
        }
    }

    private void OnEnable()
    {
        if (!loginPanelObj.activeSelf) loginPanelObj.SetActive(true);
        if (signupPanelObj.activeSelf) signupPanelObj.SetActive(false);
    }
    void LoginBtnFunc()         //�α��� �Լ�
    {
        lIDStr = loginIDInputField.text;
        lPWStr = loginPWInputField.text;
        lIDStr = lIDStr.Trim();
        lPWStr = lPWStr.Trim();

        if (string.IsNullOrEmpty(lIDStr) || string.IsNullOrEmpty(lPWStr))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("�α��� ���� �˸�", "��ĭ ���� �Է����ּ���~!");
            return;
        }

        if (!(3 <= lIDStr.Length && lIDStr.Length < 20))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("�α��� ���� �˸�", "���̵�� 3���� �̻� 20���� �̸����� �ۼ����ּ���~!");
            return;
        }

        if (!(6 <= lPWStr.Length && lPWStr.Length < 20))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("�α��� ���� �˸�", "��й�ȣ�� 6���� �̻� 20���� �̸����� �ۼ����ּ���~!");
            return;
        }

        if (!CheckEmailAddress(lIDStr))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("�α��� ���� �˸�", "�̸��� ������ ���� �ʾƿ� ��0��");
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
        Debug.Log("�α��� �Լ�");
    }

    void OnLoginSuccess(LoginResult result)
    {
        GlobalValue.g_UniqueID = result.PlayFabId;

        if (result.InfoResultPayload != null)
        {
            //GlobalValue.g_Nickname = result.InfoResultPayload.PlayerProfile.DisplayName;

            foreach (var eachStat in result.InfoResultPayload.PlayerStatistics)
            {
                if (eachStat.StatisticName == "YSMSBestScore")
                    GlobalValue.g_YSMSBestScore = eachStat.Value;
                else if (eachStat.StatisticName == "SDJRBestScore")
                    GlobalValue.g_SDJRBestScore = eachStat.Value;
                else if (eachStat.StatisticName == "TotalScore")
                    GlobalValue.g_TotalScore = eachStat.Value;
            }

            int tempValue = 0;
            foreach (var eachData in result.InfoResultPayload.UserData)
            {
                if (eachData.Key == "UserGold")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_UserGold = tempValue;
                }
                else if (eachData.Key == "UserGem")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_UserGem = tempValue;
                }
                else if (eachData.Key == "UserRice")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_RiceCount = tempValue;
                }
                else if (eachData.Key == "IsRiceTimer")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_IsRiceTimerStart = tempValue;
                }
                else if (eachData.Key == "RiceCheckTime")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_RiceCheckTime = tempValue;
                }
                else if (eachData.Key == "RiceCheckDate")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_RiceCheckDate = tempValue;
                }
                else if (eachData.Key == "YSMSBonusUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_YSMSUGLv[0] = tempValue;
                }
                else if (eachData.Key == "YSMSFeverUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_YSMSUGLv[1] = tempValue;
                }
                else if (eachData.Key == "YSMSSuperUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_YSMSUGLv[2] = tempValue;
                }
                else if (eachData.Key == "YSMSTutSkipOnOff")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_YSMSTutSkipYN = tempValue;
                }
                else if (eachData.Key == "SDJRBonusUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_SDJRUGLv[0] = tempValue;
                }
                else if (eachData.Key == "SDJRFeverUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_SDJRUGLv[1] = tempValue;
                }
                else if (eachData.Key == "SDJRSuperUGLv")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_SDJRUGLv[2] = tempValue;
                }
                else if (eachData.Key == "SDJRTutSkipOnOff")
                {
                    if (int.TryParse(eachData.Value.Value, out tempValue))
                        GlobalValue.g_SDJRTutSkipYN = tempValue;
                }
            }
        }

        GlobalValue.g_GMGOLD = 10000000;
        GlobalValue.g_GMGEM = 10000;
        GlobalValue.g_GMRICE = 100;
        SceneManager.LoadScene("LobbyScene");

    }

    void OnLoginFailure(PlayFabError error)
    {
        errorStr = "";
        errorReason = "";

        errorReason = error.GenerateErrorReport();
        if (errorReason.Contains("User not found") || errorReason.Contains("Invalid email address"))
            errorStr = "���̵� ��й�ȣ�� Ȯ�����ּ���~!";
        else if (errorReason.Contains("Email address is not valid"))
            errorStr = "�̸��� ������ ���� �ʾƿ� ��0��";
        else
        {
            errorStr = "�� �� ���� ������ �߻��߾�� ��_��";
            Debug.Log(error.GenerateErrorReport());
        }

        msgBoxObj.SetActive(true);
        msgBox.SetMessageText("�α��� ����", errorStr);
    }

    void CreateIDFunc()         //���̵� ���� �Լ�
    {
        suIDStr = signupIDInputField.text;
        suPWStr = signupPWInputField.text;
        suPWCfmStr = signupPWCfmInputField.text;
        suNickStr = signupNickInputField.text;

        suIDStr = suIDStr.Trim();
        suPWStr = suPWStr.Trim();
        suPWCfmStr = suPWCfmStr.Trim();
        suNickStr = suNickStr.Trim();

        if (string.IsNullOrEmpty(suIDStr) || string.IsNullOrEmpty(suPWStr) ||
            string.IsNullOrEmpty(suPWCfmStr) || string.IsNullOrEmpty(suNickStr))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "��ĭ ���� �Է����ּ���~!");
            return;
        }

        if (!(3 <= suIDStr.Length && suIDStr.Length < 20))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "���̵�� 3���� �̻� 20���� �̸����� �ۼ����ּ���~!");
            return;
        }

        if (!(6 <= suPWStr.Length && suPWStr.Length < 20))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "��й�ȣ�� 6���� �̻� 20���� �̸����� �ۼ����ּ���~!");
            return;
        }

        if (!(2 <= suNickStr.Length && suNickStr.Length < 14))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "�г��� 3���� �̻� 14���� �̸����� �ۼ����ּ���~!");
            return;
        }

        if (suPWCfmStr != suPWStr)
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "��й�ȣ�� ��ġ���� �ʾƿ� ��-��");
            return;
        }

        if (!CheckEmailAddress(suIDStr))
        {
            msgBoxObj.SetActive(true);
            msgBox.SetMessageText("���� ���� �˸�", "�̸��� ������ ���� �ʾƿ� ��0��");
            return;
        }

        var request = new RegisterPlayFabUserRequest()
        {
            Email = suIDStr,
            Password = suPWStr,
            DisplayName = suNickStr,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnIDRegisterSuccess, OnIDRegisterFailure);
    }

    void OnIDRegisterSuccess(RegisterPlayFabUserResult result)
    {
        msgBoxObj.SetActive(true);
        msgBox.SetMessageText("���� ����", "���ο� ������ �� �� �����ؿ� >w<");
        if (loginPanelObj != null) loginPanelObj.SetActive(true);
        if (signupPanelObj != null) signupPanelObj.SetActive(false);
        GlobalValue.g_isFirstLogin = true;
    }

    void OnIDRegisterFailure(PlayFabError error)
    {
        errorStr = "";
        errorReason = "";

        errorReason = error.GenerateErrorReport();
        if (errorReason.Contains("already exists"))
            errorStr = "�ߺ��� �̸����� �־�� ��0��";
        else if (errorReason.Contains("address is not valid"))
            errorStr = "�̸��� ������ ���� �ʾƿ�~!";
        else if (errorReason.Contains("display name entered is not available"))
            errorStr = "�̹� ������ �г����̿��� ��_��";
        else if (errorReason.Contains("DisplayName value was 2 characters long which is outside"))
            errorStr = "�г��� ������ ���� �ʾƿ� ��0��";

        msgBoxObj.SetActive(true);
        msgBox.SetMessageText("���� ���� �˸�", errorStr);
    }

    private bool CheckEmailAddress(string emailStr)
    {
        if (string.IsNullOrEmpty(emailStr)) isValidFormat = false;

        emailStr = Regex.Replace(emailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true �� ��ȯ�� ��, �ùٸ� �̸��� ������.
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
