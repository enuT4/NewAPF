using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusText : MemoryPoolObject
{
    float effectTime = 0.0f;
    float moveVelocity = 20.0f;
    float alphaVelocity = 1.0f / (1.0f - 0.5f);

    Vector3 currentPos = Vector3.zero;

    Canvas sortLayerCanvas;

    [SerializeField] internal Text thisText;
    RectTransform textRectTransform;
    Color thisColor;
    Outline thisOutline;
    Color outlineColor;
    float screenScale;

    public static BonusText inst;

    private void Awake()
    {
        if (!thisText) thisText = transform.GetChild(0).GetComponent<Text>();
        if (!thisOutline) thisOutline = thisText.GetComponentInChildren<Outline>();
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisColor = thisText.color;
        thisColor.a = 1.0f;

        outlineColor = thisOutline.effectColor;

        sortLayerCanvas = transform.GetChild(0).GetComponent<Canvas>();
        sortLayerCanvas.overrideSorting = true;
        sortLayerCanvas.sortingOrder = 6;

        if (GlobalValue.g_GameKind == GameKind.SDJR)
            moveVelocity /= 10.0f;


        screenScale = Screen.width / 1440.0f;
        //thisText.gameObject.transform.localPosition = new Vector3(0.0f, 200.0f * screenScale, 0.0f);
        thisText.fontSize = (int)(thisText.fontSize * screenScale);
        
        textRectTransform = thisText.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        EffectTimeFunc();
    }

    public void InitScore(GameKind gameKind)
    {
        if (gameKind == GameKind.YSMS)
            GlobalValue.YSMSUpgradeAmount();
        else if (gameKind == GameKind.SDJR)
            GlobalValue.SDJRUpgradeAmount();
    }

    void EffectTimeFunc()
    {
        effectTime += Time.deltaTime;
        if (effectTime < 1.05f)
        {
            currentPos = textRectTransform.anchoredPosition;
            currentPos.y += Time.deltaTime * moveVelocity;
            textRectTransform.anchoredPosition = currentPos;
        }

        if (0.4f < effectTime)
        {
            thisColor.a -= Time.deltaTime * alphaVelocity;
            if (thisColor.a < 0.0f)
                thisColor.a = 0.0f;
            outlineColor.a -= Time.deltaTime * alphaVelocity;
            thisOutline.effectColor = outlineColor;
            thisText.color = thisColor;
        }

        if (1.05f <= effectTime)
        {
            thisColor.a = 1.0f;
            thisText.color = thisColor;
            outlineColor.a = 1.0f;
            thisOutline.effectColor = outlineColor;
            effectTime = 0.0f;
            ObjectReturn();
        }
    }

    public void SetScore(GameKind gameKind)
    {
        if (gameKind == GameKind.YSMS)
            thisText.text = GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]].ToString();
        else if (gameKind == GameKind.SDJR)
            thisText.text = GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0]].ToString();
    }
}
