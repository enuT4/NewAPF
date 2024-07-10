using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    GameObject titleCanvas;

    [SerializeField] private Button gameStartBtn;
    GameObject loginPanelObj;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!titleCanvas) titleCanvas = GameObject.Find("Canvas").gameObject;
        if (!gameStartBtn) gameStartBtn = titleCanvas.transform.Find("GameStartBtn").GetComponent<Button>();

        if (!loginPanelObj) loginPanelObj = titleCanvas.transform.Find("LoginPanelObj").gameObject;
        if (loginPanelObj.activeSelf) loginPanelObj.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (gameStartBtn != null) gameStartBtn.onClick.AddListener(() => loginPanelObj.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
