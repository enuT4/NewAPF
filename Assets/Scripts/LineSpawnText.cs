using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSpawnText : MemoryPoolObject
{
    float effectTime = 0.0f;
    float moveVelocity = 100.0f / 1.05f;
    float alphaVelocity = 1.0f / (1.0f - 0.5f);

    Vector2 currentPosition = Vector2.zero;

    Canvas sortLayerCanvas;

    [SerializeField] internal Text thisText;
    Color thisColor;
    [SerializeField] Outline thisOutline;
    Color thisOutlineColor;

    private void Awake()
    {
        if(!thisText)
            thisText = this.GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        thisColor = thisText.color;
        thisColor.a = 1.0f;

        thisOutline = thisText.GetComponentInChildren<Outline>();
        thisOutlineColor = thisOutline.effectColor;

        sortLayerCanvas = GetComponent<Canvas>();
        sortLayerCanvas.overrideSorting = true;
        sortLayerCanvas.sortingOrder = 6;

        //if (GlobalValue.g_GameKind == GameKind.SDJR)
        //    moveVelocity /= 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        effectTime += Time.deltaTime;
        if (effectTime < 1.05f)
        {
            currentPosition = transform.localPosition;
            currentPosition.y += Time.deltaTime * moveVelocity;
            transform.localPosition = currentPosition;
        }
        if (0.4f < effectTime)
        {
            thisColor.a -= Time.deltaTime * alphaVelocity;
            if (thisColor.a < 0.0f)
                thisColor.a = 0.0f;
            thisOutlineColor.a -= Time.deltaTime * alphaVelocity;
        }
        if (1.05f <= effectTime)
            ObjectReturn();

        thisOutline.effectColor = thisOutlineColor;
        thisText.color = thisColor;

    }

    private void OnEnable()
    {
        InitLineSpawnTextFunc();
    }

    void InitLineSpawnTextFunc()
    {
        effectTime = 0.0f;
        thisColor.a = 1.0f;
        thisOutlineColor.a = 1.0f;
    }
}
