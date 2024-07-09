using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [Header("-------- Login --------")]
    [SerializeField] internal GameObject loginPanelObj;
    [SerializeField] internal InputField loginIDInputField;
    [SerializeField] internal InputField loginPWInputField;
    [SerializeField] internal Button loginBtn;
    [SerializeField] internal Button loginSignUpBtn;
    [SerializeField] internal Button loginCloseBtn;

    [Header("-------- SignUp --------")]
    [SerializeField] internal GameObject signupPanelObj;
    [SerializeField] internal InputField signupIDInputField;
    [SerializeField] internal InputField signupPWInputField;
    [SerializeField] internal InputField signupPWCfmInputField;
    [SerializeField] internal Text signupIDCfmText;
    [SerializeField] internal Button signupIDCfmBtn;
    [SerializeField] internal Button signupCreateIDBtn;
    [SerializeField] internal Button signupCloseBtn;

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
    }

    // Update is called once per frame
    void Update()
    {
        //탭(쉬프트탭)키를 누를 때 인풋필드간 이동
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
        Debug.Log("로그인 함수");
    }

    void IDConfirmFunc()        //아이디 확인 함수
    {
        Debug.Log("아이디 확인 함수");
    }

    void CreateIDFunc()         //아이디 생성 함수
    {
        Debug.Log("아이디 생성 함수");
    }
}
