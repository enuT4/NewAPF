using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleMgr : MonoBehaviour
{
    GameObject titleCanvas;

    [SerializeField] private Button gameStartBtn;
    GameObject loginPanelObj;

    bool isTest = false;

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

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isTest = !isTest;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(Convert.ToInt32(isTest));
        }


    }
}
