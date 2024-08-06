using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpImage : MonoBehaviour
{
    float effTime = 0.0f;
    float scHorVel = 5.0f / 0.3f;
    float scVerVel = 0.3f / 0.1f;
    float apVel = 1.0f / 0.2f;
    float scaleX = 0.1f;
    float scaleY = 1.0f;

    Image thisImg;

    Color imgColor;
    //Color outlineColor1;
    //Color outlineColor2;
    //Outline[] outlineArr;

    // Start is called before the first frame update
    void Start()
    {
        thisImg = GetComponent<Image>();
        thisImg.transform.localScale = new Vector2(scaleX, scaleY);
        imgColor = thisImg.color;
        imgColor.a = 1.0f;

        //outlineArr = GetComponentsInChildren<Outline>();
        //if (outlineArr[0] != null && outlineArr[1] != null)
        //{
        //    outlineColor1 = outlineArr[0].effectColor;
        //    outlineColor2 = outlineArr[1].effectColor;
        //    outlineColor1.a = 0.5f;
        //    outlineColor2.a = 0.5f;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (effTime < 1.0f)
        {
            effTime += Time.deltaTime;

            if (effTime < 0.3f)
            {
                scaleX = thisImg.transform.localScale.x;
                scaleX += Time.deltaTime * scHorVel;
                if (1.2f <= scaleX) scaleX = 1.2f;
            }

            if (0.1f < effTime)
            {
                scaleY = thisImg.transform.localScale.y;
                scaleY -= Time.deltaTime * scVerVel;
                if (scaleY <= 0.8f) scaleY = 0.8f;
            }
            if (0.2f < effTime)
            {
                scaleX = 1.0f;
                scaleY = 1.0f;
            }
            if (0.3f < effTime)
            {
                imgColor.a -= Time.deltaTime * apVel;
                //outlineColor1.a -= Time.deltaTime * apVel;
                //outlineColor2.a -= Time.deltaTime * apVel;

                if (imgColor.a < 0.0f) imgColor.a = 0.0f;
                //if (outlineColor1.a < 0.0f) outlineColor1.a = 0.0f;
                //if (outlineColor2.a < 0.0f) outlineColor2.a = 0.0f;
            }
            if (0.5f < effTime)
            {
                this.gameObject.SetActive(false);
            }

            thisImg.transform.localScale = new Vector2(scaleX, scaleY);
            thisImg.color = imgColor;
            //outlineArr[0].effectColor = outlineColor1;
            //outlineArr[1].effectColor = outlineColor2;
        }
        
    }

    private void OnEnable()
    {
        effTime = 0.0f;
        imgColor.a = 1.0f;
    }
}
