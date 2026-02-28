using Enut4LJR;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] internal Button applyBtn;
    [SerializeField] internal Button closeBtn;
    [SerializeField] internal Button musicOnOffBtn;
    [SerializeField] internal Button soundOnOffBtn;
    [SerializeField] internal Text musicBtnText;
    [SerializeField] internal Text soundBtnText;
    bool tempMusicMute = false;
    bool tempSoundMute = false;

    internal GameObject[] musicOnOffObjs = new GameObject[2];
    internal GameObject[] soundOnOffObjs = new GameObject[2];

    void Awake() => AwakeFunc();
    private void AwakeFunc()
    {
        if (!applyBtn) applyBtn = transform.Find("ApplyBtn").GetComponent<Button>();
        if (!closeBtn) closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        if (!musicOnOffBtn) musicOnOffBtn = transform.Find("MusicObj").transform.Find("MusicOnOffBtn").GetComponent<Button>();
        if (!soundOnOffBtn) soundOnOffBtn = transform.Find("SoundObj").transform.Find("SoundOnOffBtn").GetComponent<Button>();
        if (!musicBtnText) musicBtnText = musicOnOffBtn.transform.Find("MusicBtnTxt").GetComponent<Text>();
        if (!soundBtnText) soundBtnText = soundOnOffBtn.transform.Find("SoundBtnTxt").GetComponent<Text>();

        musicOnOffObjs[0] = musicOnOffBtn.transform.Find("MusicImgObj").GetChild(0).gameObject;
        musicOnOffObjs[1] = musicOnOffBtn.transform.Find("MusicImgObj").GetChild(1).gameObject;
        soundOnOffObjs[0] = soundOnOffBtn.transform.Find("SoundImgObj").GetChild(0).gameObject;
        soundOnOffObjs[1] = soundOnOffBtn.transform.Find("SoundImgObj").GetChild(1).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        tempMusicMute = GlobalValue.musicMute;
        tempSoundMute = GlobalValue.soundMute;
        ApplyUserSettingsFunc();


        if (applyBtn != null)
            applyBtn.onClick.AddListener(ApplyBtnFunc);

        if (closeBtn != null)
            closeBtn.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
                MusicManager.instance.ApplyMute();
            });

        if (musicOnOffBtn != null)
            musicOnOffBtn.onClick.AddListener(() =>
            {
                tempMusicMute = !tempMusicMute;
                ApplyUserSettingsFunc();
            });

        if (soundOnOffBtn != null)
            soundOnOffBtn.onClick.AddListener(() =>
            {
                tempSoundMute = !tempSoundMute;
                ApplyUserSettingsFunc();
            });

    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    private void OnEnable()
    {
        tempMusicMute = GlobalValue.musicMute;
        tempSoundMute = GlobalValue.soundMute;
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

        applyBtn.interactable = HasSettingsChanged();
    }

    void ApplyBtnFunc()
    {   //세팅 값 적용하는 함수
        GlobalValue.musicMute = tempMusicMute;
        GlobalValue.soundMute = tempSoundMute;
        ApplyUserSettingsFunc();
    }

    bool HasSettingsChanged()
    {
        if (tempMusicMute != GlobalValue.musicMute ||
            tempSoundMute != GlobalValue.soundMute)
            return true;
        return false;
    }

}
