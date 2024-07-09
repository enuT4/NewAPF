using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] internal Button messageOKBtn;
    [SerializeField] internal Text messageLabel;
    [SerializeField] internal Text messageText;

    MessageBox inst;

    private void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!messageOKBtn) messageOKBtn = transform.Find("MessageOKBtn").GetComponent<Button>();
        if (!messageLabel) messageLabel = transform.Find("MessageLabel").GetComponent<Text>();
        if (!messageText) messageText = transform.Find("MessageText").GetComponent<Text>();

        inst = this;
    }


    private void Start() => StartFunc();

    // Start is called before the first frame update
    void StartFunc()
    {
        if (messageOKBtn != null) messageOKBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    //private void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }


    public void SetMessageText(string label, string text)
    {
        messageLabel.text = "¡Ú " + label + " ¡Ú";
        messageText.text = text;
    }
}
