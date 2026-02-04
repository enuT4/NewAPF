using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCountEffectImage : MonoBehaviour
{
    public enum ComboEffectKind
    {
        Fever,
        Fyah,
        ComboEffectKindCount
    }

    [SerializeField] float effectTime;
    float scaleHorizontalVelocity;
    float alphaVelocity;
    float scaleX;
    float scaleY;

    ComboEffectKind thisEffectKind;
    Image thisImg;
    Color thisColor;

    [SerializeField] internal Image feverImg;
    [SerializeField] internal Image fyahImg;

    bool isOnOff = false;

    private void Awake()
    {
        if (!feverImg) feverImg = transform.GetChild(0).GetComponent<Image>();
        if (!fyahImg) fyahImg = transform.GetChild(1).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetComboEffectNumberFunc(thisEffectKind);
        if (feverImg != null && feverImg.gameObject.activeSelf) feverImg.gameObject.SetActive(false);
        if (fyahImg != null && fyahImg.gameObject.activeSelf) fyahImg.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnOff) return;
        effectTime += Time.deltaTime;
        if (effectTime < 0.5f)
        {
            scaleX = thisImg.transform.localScale.x;
            scaleY = thisImg.transform.localScale.y;
            scaleX += Time.deltaTime * scaleHorizontalVelocity;
            scaleY += Time.deltaTime * scaleHorizontalVelocity;
            if (1.0f <= scaleX)
            {
                scaleX = 1.0f;
                scaleY = 1.0f;
            }
        }
        if (0.7f < effectTime)
        {
            thisColor.a -= Time.deltaTime * alphaVelocity;
            if(thisColor.a < 0.0f)
                thisColor.a = 0.0f;
        }
        if (1.0f < effectTime)
        {
            feverImg.gameObject.SetActive(false);
            fyahImg.gameObject.SetActive(false);
            isOnOff = false;
        }

        thisImg.transform.localScale = new Vector2(scaleX, scaleY);
        thisImg.color = thisColor;
    }


    void SetComboEffectNumberFunc(ComboEffectKind effectKind)
    {
        if (effectKind == ComboEffectKind.Fever)
        {
            scaleHorizontalVelocity = 4.0f / 0.3f;
            alphaVelocity = 1.0f / 0.2f;
            scaleX = 0.1f;
            scaleY = 0.1f;

            thisImg = feverImg;
            
        }
        else if (effectKind == ComboEffectKind.Fyah)
        {
            scaleHorizontalVelocity = 5.0f / 0.3f;
            alphaVelocity = 1.0f / 0.2f;
            scaleX = 0.1f;
            scaleY = 1.0f;

            thisImg = fyahImg;
        }
        else
            return;

        SetComboEffectOnOffFunc(effectKind);
        thisImg.transform.localScale = new Vector2(scaleX, scaleY);
        thisColor = thisImg.color;
        thisColor.a = 1.0f;
    }

    void SetComboEffectOnOffFunc(ComboEffectKind effectOnKind)
    {
        if (effectOnKind == ComboEffectKind.Fever)
        {
            feverImg.gameObject.SetActive(true);
            fyahImg.gameObject.SetActive(false);
        }
        else if (effectOnKind == ComboEffectKind.Fyah)
        {
            feverImg.gameObject.SetActive(false);
            fyahImg.gameObject.SetActive(true);
        }
        else
        {
            feverImg.gameObject.SetActive(false);
            fyahImg.gameObject.SetActive(false);
        }
    }

    public void SetComboEffectKindFunc(ComboEffectKind effectKind)
    {
        thisEffectKind = effectKind;
        SetComboEffectNumberFunc(thisEffectKind);
        effectTime = 0.0f;
        isOnOff = true;
        SetComboEffectOnOffFunc(effectKind);
    }
}
