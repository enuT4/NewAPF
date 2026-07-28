using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public enum SettingKind
    {
        Music,
        Sound,
        SettingCount
    }

    [SerializeField] internal GameObject settingPanelImage;

    [SerializeField] internal Button applyBtn;
    [SerializeField] internal Button closeBtn;
    [SerializeField] internal Button musicOnOffBtn;
    [SerializeField] internal Button soundOnOffBtn;
    [SerializeField] internal Text musicBtnText;
    [SerializeField] internal Text soundBtnText;
    bool tempMusicMute = false;
    bool currentMusicMute = false;
    bool tempSoundMute = false;
    bool currentSoundMute = false;

    internal GameObject[] musicOnOffObjs = new GameObject[2];
    internal GameObject[] soundOnOffObjs = new GameObject[2];

    //드랍다운
    [SerializeField] internal Dropdown resolutionDropDown;
    int resolutionIdx = -1;
    int tempResolutionIdx = -1;
    int screenWidth = -1;
    int screenHeight = -1;


    void Awake() => AwakeFunc();
    private void AwakeFunc()
    {
        if (!settingPanelImage) settingPanelImage = transform.GetChild(0).gameObject;

        if (!applyBtn) applyBtn = settingPanelImage.transform.Find("ApplyBtn").GetComponent<Button>();
        if (!closeBtn) closeBtn = settingPanelImage.transform.Find("CloseBtn").GetComponent<Button>();
        if (!musicOnOffBtn) musicOnOffBtn = settingPanelImage.transform.Find("MusicObj").transform.Find("MusicOnOffBtn").GetComponent<Button>();
        if (!soundOnOffBtn) soundOnOffBtn = settingPanelImage.transform.Find("SoundObj").transform.Find("SoundOnOffBtn").GetComponent<Button>();
        if (!musicBtnText) musicBtnText = musicOnOffBtn.transform.Find("MusicBtnTxt").GetComponent<Text>();
        if (!soundBtnText) soundBtnText = soundOnOffBtn.transform.Find("SoundBtnTxt").GetComponent<Text>();

        musicOnOffObjs[0] = musicOnOffBtn.transform.Find("MusicImgObj").GetChild(0).gameObject;
        musicOnOffObjs[1] = musicOnOffBtn.transform.Find("MusicImgObj").GetChild(1).gameObject;
        soundOnOffObjs[0] = soundOnOffBtn.transform.Find("SoundImgObj").GetChild(0).gameObject;
        soundOnOffObjs[1] = soundOnOffBtn.transform.Find("SoundImgObj").GetChild(1).gameObject;

        if (!resolutionDropDown) resolutionDropDown = settingPanelImage.transform.Find("ResolutionObj").transform.Find("ResolutionDropDown").GetComponent<Dropdown>();
    }

    // Start is called before the first frame update
    void Start()
    {
        tempMusicMute = GlobalValue.musicMute;
        currentMusicMute = GlobalValue.musicMute;
        tempSoundMute = GlobalValue.soundMute;
        currentSoundMute = GlobalValue.soundMute;
        ApplyUserSettingsFunc();


        if (applyBtn != null)
            applyBtn.onClick.AddListener(ApplyBtnFunc);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                GlobalValue.musicMute = currentMusicMute;
                MusicManager.instance.ApplyMute();
                GlobalValue.soundMute = currentSoundMute;
                SoundManager.instance.ApplyMute();

                SoundManager.instance.PlayerSound("Button");
                this.gameObject.SetActive(false);
            });

        if (musicOnOffBtn != null)
            musicOnOffBtn.onClick.AddListener(() =>
            {
                tempMusicMute = !tempMusicMute;
                GlobalValue.musicMute = tempMusicMute;
                MusicManager.instance.ApplyMute();
                SoundManager.instance.PlayerSound("Button");
                ApplyUserSettingsFunc();
            });

        if (soundOnOffBtn != null)
            soundOnOffBtn.onClick.AddListener(() =>
            {
                tempSoundMute = !tempSoundMute;
                GlobalValue.soundMute = tempSoundMute;
                SoundManager.instance.ApplyMute();
                SoundManager.instance.PlayerSound("Button");
                ApplyUserSettingsFunc();
            });

        if (resolutionDropDown != null) 
            resolutionDropDown.onValueChanged.AddListener(OnResolutionChangedFunc);

        
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    private void OnEnable()
    {
        if (currentMusicMute != GlobalValue.musicMute)
            currentMusicMute = GlobalValue.musicMute;
        if (currentSoundMute != GlobalValue.soundMute)
            currentSoundMute = GlobalValue.soundMute;
        tempMusicMute = currentMusicMute;
        tempSoundMute = currentSoundMute;
        tempResolutionIdx = resolutionDropDown.value;
        ApplyUserSettingsFunc();
    }

    void ApplyUserSettingsFunc()
    {   //유저의 세팅값 적용하는 함수
        musicOnOffObjs[0].SetActive(!tempMusicMute);
        musicOnOffObjs[1].SetActive(tempMusicMute);
        if (tempMusicMute) musicBtnText.text = "OFF";
        else musicBtnText.text = "ON";

        soundOnOffObjs[0].SetActive(!tempSoundMute);
        soundOnOffObjs[1].SetActive(tempSoundMute);
        if (tempSoundMute) soundBtnText.text = "OFF";
        else soundBtnText.text = "ON";

        if (tempResolutionIdx != resolutionDropDown.value)
        {
            Screen.SetResolution(screenWidth, screenHeight, false);
            Debug.Log(screenWidth + " x " + screenHeight);
            tempResolutionIdx = resolutionDropDown.value;
        }

        applyBtn.interactable = HasSettingsChanged();
    }

    void ApplyBtnFunc()
    {   //세팅 값 적용하는 함수
        currentMusicMute = tempMusicMute;
        currentSoundMute = tempSoundMute;
        
        SoundManager.instance.PlayerSound("Button");
        ApplyUserSettingsFunc();
        tempResolutionIdx = resolutionDropDown.value;
    }

    bool HasSettingsChanged()
    {
        if (tempMusicMute != currentMusicMute ||
            tempSoundMute != currentSoundMute)
            return true;
        if (tempResolutionIdx != resolutionDropDown.value)
            return true;

        return false;
    }

    void OnResolutionChangedFunc(int idx)
    {
        switch(idx)
        {
            case 0:
                screenWidth = 720;
                screenHeight = 1280;
                break;
            case 1:
                screenWidth = 1080;
                screenHeight = 1920;
                break;
            case 2:
                screenWidth = 1080;
                screenHeight = 2160;
                break;
            case 3:
                screenWidth = 1080;
                screenHeight = 2340;
                break;
            case 4:
                screenWidth = 1080;
                screenHeight = 2400;
                break;
            case 5:
                screenWidth = 1170;
                screenHeight = 2532;
                break;
            case 6:
                screenWidth = 412;
                screenHeight = 915;
                break;
            case 7:
                screenWidth = 393;
                screenHeight = 852;
                break;
        }
        resolutionIdx = idx;
        applyBtn.interactable = HasSettingsChanged();
        SoundManager.instance.PlayerSound("Button");
    }

}
