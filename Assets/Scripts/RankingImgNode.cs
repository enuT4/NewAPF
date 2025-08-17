using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingImgNode : MonoBehaviour
{
    [SerializeField] internal Image buttonSprite;
    [SerializeField] internal Text rankTxt;
    [SerializeField] internal Text nicknameTxt;
    [SerializeField] internal Image profileImg;
    [SerializeField] internal Text bestScoreTxt;

    [HideInInspector] public int rankUniqueID = -1;
    [HideInInspector] public string rankNickname = "";
    [HideInInspector] public int rankBestScore = -1;
    [HideInInspector] public int rankRanking = -1;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!rankTxt) rankTxt = transform.Find("RankText").GetComponent<Text>();
        if (!nicknameTxt) nicknameTxt = transform.Find("NicknameText").GetComponent<Text>();
        if (!bestScoreTxt) bestScoreTxt = transform.Find("ScoreText").GetComponent<Text>();
        if (!buttonSprite) buttonSprite = GetComponent<Image>();
    }

    //void Start() => StartFunc();
    // Start is called before the first frame update
    void StartFunc()
    {
        
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }


    public void InitInfo(int rank, string nick, int score)
    {
        rankRanking = rank;
        rankNickname = nick;
        rankBestScore = score;
        rankTxt.text = rank.ToString();
        nicknameTxt.text = nick.ToString();
        bestScoreTxt.text = score.ToString();
    }
}
