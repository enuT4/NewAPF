using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeKind
{
    Bonus,
    Fever,
    Super,
    ugKindCount
}

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] internal Image upgradeIconImg;
    [SerializeField] internal Text upgradeLvTxt;
    [SerializeField] internal Image upgradeGuageBar;
    [SerializeField] internal Text upgradeAmountInfoTxt;
    [SerializeField] internal Text upgradeInfoTxt;
    [SerializeField] internal Button upgradeLvUpBtn;
    [SerializeField] internal Image upgradeLvUpCostImg;
    [SerializeField] internal Text upgradeLvUpCostTxt;
    [SerializeField] internal Text upgradeLvMaxTxt;
    [SerializeField] internal Button upgradePanelCloseBtn;

    [SerializeField] internal Sprite[] upgradeSpriteGroup = new Sprite[8];
    [SerializeField] internal Text[] upgradeAmountGroup = new Text[3];

    [SerializeField] internal UpgradeKind ugKind = UpgradeKind.ugKindCount;
    int upgradeCost = 0;

    bool isupgradeDelay = false;
    WaitForSeconds wfsDelay = new WaitForSeconds(0.5f);

    [SerializeField] internal GameObject lvUpImgObj;
    [SerializeField] internal GameObject upgradePanelCanvasObj;
    [SerializeField] internal GameObject messageBoxObj;
    MessageBox msgBox;

    void Awake() => AwakeFunc();

    void AwakeFunc()
    {
        if (!upgradePanelCanvasObj) upgradePanelCanvasObj = transform.GetChild(0).gameObject;
        if (!upgradeIconImg) upgradeIconImg = upgradePanelCanvasObj.transform.Find("UpgradeIconImg").GetComponent<Image>();
        if (!upgradeLvTxt) upgradeLvTxt = upgradePanelCanvasObj.transform.Find("UpgradeLevelText").GetComponent<Text>();
        if (!upgradeGuageBar) upgradeGuageBar = upgradePanelCanvasObj.transform.Find("UpgradeGuageBack").transform.GetChild(0).GetComponent<Image>();
        if (!upgradeAmountInfoTxt) upgradeAmountInfoTxt = upgradePanelCanvasObj.transform.Find("UpgradeAmountInfoText").GetComponent<Text>();
        if (!upgradeInfoTxt) upgradeInfoTxt = upgradePanelCanvasObj.transform.Find("UpgradeInfoText").GetComponent<Text>();
        if (!upgradeLvUpBtn) upgradeLvUpBtn = upgradePanelCanvasObj.transform.Find("UpgradeLevelUpBtn").GetComponent<Button>();
        if (!upgradeLvUpCostImg) upgradeLvUpCostImg = upgradeLvUpBtn.transform.GetChild(1).GetComponent<Image>();
        if (!upgradeLvUpCostTxt) upgradeLvUpCostTxt = upgradeLvUpBtn.transform.GetChild(0).GetComponent<Text>();
        if (!upgradeLvMaxTxt) upgradeLvMaxTxt = upgradeLvUpBtn.transform.GetChild(2).GetComponent<Text>();
        if (!upgradePanelCloseBtn) upgradePanelCloseBtn = upgradePanelCanvasObj.transform.Find("UpgradePanelCloseBtn").GetComponent<Button>();
        if (!lvUpImgObj) lvUpImgObj = upgradePanelCanvasObj.transform.Find("LevelUpImg").gameObject;
        if (!messageBoxObj) messageBoxObj = upgradePanelCanvasObj.transform.Find("MessageBox").gameObject;
        if (messageBoxObj != null) msgBox = messageBoxObj.GetComponent<MessageBox>();

        for (int ii = 0; ii < upgradeAmountGroup.Length; ii++)
        {
            if (!upgradeAmountGroup[ii]) upgradeAmountGroup[ii] = upgradeIconImg.transform.GetChild(ii).GetComponent<Text>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        UpdateUpgradeKind();

        if (upgradeLvUpBtn != null)
            upgradeLvUpBtn.onClick.AddListener(ItemUpgradeFunc);

        if (upgradePanelCloseBtn != null)
            upgradePanelCloseBtn.onClick.AddListener(() =>
            {
                ugKind = UpgradeKind.ugKindCount;
                this.gameObject.SetActive(false);
            });

    }

    void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(ugKind);
            upgradeIconImg.sprite = upgradeSpriteGroup[0];
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(ugKind);
            upgradeIconImg.sprite = upgradeSpriteGroup[1];
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(ugKind);
            upgradeIconImg.sprite = upgradeSpriteGroup[2];
        }

    }

    private void OnEnable()
    {
        UpdateUpgradeKind();
    }

    void UpdateUpgradeKind()
    {
        if (GlobalValue.g_GameKind == GameKind.YSMS) UpdateYSMSUpgradeKind();
        else if (GlobalValue.g_GameKind == GameKind.SDJR) UpdateSDJRUpgradeKind();
    }

    void UpdateYSMSUpgradeKind()
    {
        GlobalValue.YSMSUpgradeAmount();

        if (ugKind == UpgradeKind.Bonus)
        {
            if (upgradeIconImg != null) 
                upgradeIconImg.sprite = upgradeSpriteGroup[0];

            upgradeAmountGroup[0].gameObject.SetActive(true);
            upgradeAmountGroup[1].gameObject.SetActive(false);
            upgradeAmountGroup[2].gameObject.SetActive(false);

            if (GlobalValue.g_YSMSUpgradeLv[0] == 0)
                upgradeAmountGroup[0].gameObject.SetActive(false);
            else
                upgradeAmountGroup[0].text = "+" + GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]].ToString();
            upgradeLvTxt.text = GlobalValue.g_YSMSUpgradeLv[0].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_YSMSUpgradeLv[0] / 15.0f;

            upgradeAmountInfoTxt.text = "";
            if (GlobalValue.g_YSMSUpgradeLv[0] == 15)
            {
                upgradeAmountInfoTxt.text = "���ʽ� ĳ���Ͱ� ù��°�� �����ؿ�~\n" + "���ʽ� ���� " + GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]] + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                if (GlobalValue.g_YSMSUpgradeLv[0] == 0)
                    upgradeAmountInfoTxt.text = "���ʽ� ����� �����ؿ�~\n";
                else if (1 <= GlobalValue.g_YSMSUpgradeLv[0] && GlobalValue.g_YSMSUpgradeLv[0] <= 5)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� �׹�°�� ���� > <color=#ff0000>�׹�°�� ����</color>\n";
                else if (GlobalValue.g_YSMSUpgradeLv[0] == 6)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� �׹�°�� ���� > <color=#ff0000>����°�� ����</color>\n";
                else if (7 <= GlobalValue.g_YSMSUpgradeLv[0] && GlobalValue.g_YSMSUpgradeLv[0] <= 9)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� ����°�� ���� > <color=#ff0000>����°�� ����</color>\n";
                else if (GlobalValue.g_YSMSUpgradeLv[0] == 10)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� ����°�� ���� > <color=#ff0000>�ι�°�� ����</color>\n";
                else if (GlobalValue.g_YSMSUpgradeLv[0] == 11 || GlobalValue.g_YSMSUpgradeLv[0] == 12)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� �ι�°�� ���� > <color=#ff0000>�ι�°�� ����</color>\n";
                else if (GlobalValue.g_YSMSUpgradeLv[0] == 13)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���� �ι�°�� ���� > <color=#ff0000>ù��°�� ����</color>\n";
                else if (GlobalValue.g_YSMSUpgradeLv[0] == 14)
                    upgradeAmountInfoTxt.text = "���ʽ� ĳ���Ͱ� ù��°�� �����ؿ�~\n";

                upgradeAmountInfoTxt.text += "���ʽ� ���� " + GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0]] +
                "�� > <color=#ff0000>" + GlobalValue.bonusAmount[GlobalValue.g_YSMSUpgradeLv[0] + 1] + "��</color>";
                upgradeCost = GlobalValue.bonusUpgradeCost[GlobalValue.g_YSMSUpgradeLv[0]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }
            upgradeInfoTxt.text = "������ ��~�� �ִ� �Ϳ�� ģ���� ���Ϳ�.\n" + "�������Ҽ��� �� ����! ������ ��~��!";
        }
        else if (ugKind == UpgradeKind.Fever)
        {
            if (upgradeIconImg != null)
                upgradeIconImg.sprite = upgradeSpriteGroup[1];

            upgradeAmountGroup[0].gameObject.SetActive(false);
            upgradeAmountGroup[1].gameObject.SetActive(true);
            upgradeAmountGroup[2].gameObject.SetActive(false);

            upgradeAmountGroup[1].text = GlobalValue.feverAmount[GlobalValue.g_YSMSUpgradeLv[1]].ToString("F1");
            upgradeLvTxt.text = GlobalValue.g_YSMSUpgradeLv[1].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_YSMSUpgradeLv[1] / 15.0f;

            if (GlobalValue.g_YSMSUpgradeLv[1] == 15)
            {
                upgradeAmountInfoTxt.text = "�ǹ� ���� " + GlobalValue.feverAmount[GlobalValue.g_YSMSUpgradeLv[1]] + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                upgradeAmountInfoTxt.text = "�ǹ� ���� " + GlobalValue.feverAmount[GlobalValue.g_YSMSUpgradeLv[1]] +
                "�� > <color=#ff0000>" + GlobalValue.feverAmount[GlobalValue.g_YSMSUpgradeLv[1] + 1] + "��</color>";
                upgradeCost = GlobalValue.feverUpgradeCost[GlobalValue.g_YSMSUpgradeLv[1]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }
            upgradeInfoTxt.text = "�ǹ� ���°� �Ǹ� ������ ��~�� �ö��!\n" + "�������Ҽ��� �� ��� ��! ��! ��!";
        }
        else if (ugKind == UpgradeKind.Super)
        {
            if (upgradeIconImg != null) 
                upgradeIconImg.sprite = upgradeSpriteGroup[2];

            upgradeAmountGroup[0].gameObject.SetActive(false);
            upgradeAmountGroup[1].gameObject.SetActive(false);
            upgradeAmountGroup[2].gameObject.SetActive(true);

            if (GlobalValue.g_YSMSUpgradeLv[2] == 0)
                upgradeAmountGroup[2].gameObject.SetActive(false);
            else
                upgradeAmountGroup[2].text = GlobalValue.superAmount[GlobalValue.g_YSMSUpgradeLv[2]].ToString("F1") + "��";
            upgradeLvTxt.text = GlobalValue.g_YSMSUpgradeLv[2].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_YSMSUpgradeLv[2] / 15.0f;

            if (GlobalValue.g_YSMSUpgradeLv[2] == 15)
            {
                upgradeAmountInfoTxt.text = "�̺�Ʈ �ð� " + GlobalValue.superAmount[GlobalValue.g_YSMSUpgradeLv[2]].ToString("F1") + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                upgradeAmountInfoTxt.text = "�̺�Ʈ �ð� " + GlobalValue.superAmount[GlobalValue.g_YSMSUpgradeLv[2]].ToString("F1") +
                "�� > <color=#ff0000>" + GlobalValue.superAmount[GlobalValue.g_YSMSUpgradeLv[2] + 1].ToString("F1") + "��</color>";
                upgradeCost = GlobalValue.superUpgradeCost[GlobalValue.g_YSMSUpgradeLv[2]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }
            upgradeInfoTxt.text = "�� ĳ���ͷ� ��� ���ϵſ�!\n" + "�������Ҽ��� ��~�� ���ӵǾ� ¯ ������!";
        }
    }

    void UpdateSDJRUpgradeKind()
    {
        GlobalValue.SDJRUpgradeAmount();

        if (ugKind == UpgradeKind.Bonus)
        {
            if (upgradeIconImg != null)
                upgradeIconImg.sprite = upgradeSpriteGroup[0];

            upgradeAmountGroup[0].gameObject.SetActive(true);
            upgradeAmountGroup[1].gameObject.SetActive(false);
            upgradeAmountGroup[2].gameObject.SetActive(false);

            if (GlobalValue.g_SDJRUpgradeLv[0] == 0)
                upgradeAmountGroup[0].gameObject.SetActive(false);
            else
                upgradeAmountGroup[0].text = "+" + GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0]].ToString();

            upgradeLvTxt.text = GlobalValue.g_SDJRUpgradeLv[0].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_SDJRUpgradeLv[0] / 15.0f;

            upgradeAmountInfoTxt.text = "";
            if (GlobalValue.g_SDJRUpgradeLv[0] == 15)
            {
                upgradeAmountInfoTxt.text = "���ʽ� ���� �����ؿ�~\n" + "���ʽ� ���� " +
                    GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0]] + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                upgradeAmountInfoTxt.text = "���ʽ� ����� �����ؿ�~\n" + "���ʽ� ���� " +
                    GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0]] + "�� > <color=#ff0000>" +
                    GlobalValue.bonusAmount[GlobalValue.g_SDJRUpgradeLv[0] + 1] + "��</color>";
                upgradeCost = GlobalValue.bonusUpgradeCost[GlobalValue.g_SDJRUpgradeLv[0]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }

            upgradeInfoTxt.text = "�Ѱ��� ���� ��� �����ִ� ����� ���Ϳ�.\n" + "�������Ҽ��� �� ����! ������ ��~��!";
        }
        else if (ugKind == UpgradeKind.Fever)
        {
            if (upgradeIconImg != null)
                upgradeIconImg.sprite = upgradeSpriteGroup[1];

            upgradeAmountGroup[0].gameObject.SetActive(false);
            upgradeAmountGroup[1].gameObject.SetActive(true);
            upgradeAmountGroup[2].gameObject.SetActive(false);

            upgradeAmountGroup[1].text = GlobalValue.feverAmount[GlobalValue.g_SDJRUpgradeLv[1]].ToString("F1");
            upgradeLvTxt.text = GlobalValue.g_SDJRUpgradeLv[1].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_SDJRUpgradeLv[1] / 15.0f;

            if (GlobalValue.g_SDJRUpgradeLv[1] == 15)
            {
                upgradeAmountInfoTxt.text = "�ǹ� ���� " + GlobalValue.feverAmount[GlobalValue.g_SDJRUpgradeLv[1]] + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                upgradeAmountInfoTxt.text = "�ǹ� ���� " + GlobalValue.feverAmount[GlobalValue.g_SDJRUpgradeLv[1]] +
                "�� > <color=#ff0000>" + GlobalValue.feverAmount[GlobalValue.g_SDJRUpgradeLv[1] + 1] + "��</color>";
                upgradeCost = GlobalValue.feverUpgradeCost[GlobalValue.g_SDJRUpgradeLv[1]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }

            upgradeInfoTxt.text = "�ǹ� ���°� �Ǹ� ������ ��~�� �ö��!\n" + "�������Ҽ��� �� ��� ��! ��! ��!";
        }
        else if (ugKind == UpgradeKind.Super)
        {
            if (upgradeIconImg != null)
                upgradeIconImg.sprite = upgradeSpriteGroup[2];

            upgradeAmountGroup[0].gameObject.SetActive(false);
            upgradeAmountGroup[1].gameObject.SetActive(false);
            upgradeAmountGroup[2].gameObject.SetActive(true);

            if (GlobalValue.g_SDJRUpgradeLv[2] == 0)
                upgradeAmountGroup[2].gameObject.SetActive(false);
            else
                upgradeAmountGroup[2].text = GlobalValue.superAmount[GlobalValue.g_SDJRUpgradeLv[2]].ToString("F1") + "��";
            upgradeLvTxt.text = GlobalValue.g_SDJRUpgradeLv[2].ToString();
            upgradeGuageBar.fillAmount = (float)GlobalValue.g_SDJRUpgradeLv[2] / 15.0f;

            if (GlobalValue.g_SDJRUpgradeLv[2] == 15)
            {
                upgradeAmountInfoTxt.text = "�̺�Ʈ �ð� " + GlobalValue.superAmount[GlobalValue.g_SDJRUpgradeLv[2]].ToString("F1") + "��";
                upgradeLvUpBtn.interactable = false;
                upgradeLvUpCostImg.gameObject.SetActive(false);
                upgradeLvUpCostTxt.gameObject.SetActive(false);
                upgradeLvMaxTxt.gameObject.SetActive(true);
            }
            else
            {
                upgradeAmountInfoTxt.text = "�̺�Ʈ �ð� " + GlobalValue.superAmount[GlobalValue.g_SDJRUpgradeLv[2]].ToString("F1") +
                "�� > <color=#ff0000>" + GlobalValue.superAmount[GlobalValue.g_SDJRUpgradeLv[2] + 1].ToString("F1") + "��</color>";
                upgradeCost = GlobalValue.superUpgradeCost[GlobalValue.g_SDJRUpgradeLv[2]];
                upgradeLvUpCostTxt.text = upgradeCost.ToString();
            }
            upgradeInfoTxt.text = "���� �ΰ��� ������ ���ϵſ�!\n" + "�������Ҽ��� ��~�� ���ӵǾ� ¯ ������!";
        }
    }

    void ItemUpgradeFunc()
    {
        if (GlobalValue.g_UserGold - upgradeCost < 0)
        {
            messageBoxObj.SetActive(true);
            msgBox.SetMessageText("��ȭ ���� �˸�", "��ȭ�� �����ؿ� ��0��", MessageState.OK);
            return;
        }
        if (isupgradeDelay) return;

        
        GlobalValue.g_UserGold -= upgradeCost;
        NetworkMgr.inst.PushPacket(PacketType.UserMoney);

        if (GlobalValue.g_GameKind == GameKind.YSMS)
        {
            if (ugKind == UpgradeKind.Bonus && GlobalValue.g_YSMSUpgradeLv[0] < 15)
                GlobalValue.g_YSMSUpgradeLv[0]++;
            else if (ugKind == UpgradeKind.Fever && GlobalValue.g_YSMSUpgradeLv[1] < 15)
                GlobalValue.g_YSMSUpgradeLv[1]++;
            else if (ugKind == UpgradeKind.Super && GlobalValue.g_YSMSUpgradeLv[2] < 15)
                GlobalValue.g_YSMSUpgradeLv[2]++;

            NetworkMgr.inst.PushPacket(PacketType.YSMSUpgradeLv);
        }
        else if (GlobalValue.g_GameKind == GameKind.SDJR)
        {
            if (ugKind == UpgradeKind.Bonus && GlobalValue.g_SDJRUpgradeLv[0] < 15)
                GlobalValue.g_SDJRUpgradeLv[0]++;
            else if (ugKind == UpgradeKind.Fever && GlobalValue.g_SDJRUpgradeLv[1] < 15)
                GlobalValue.g_SDJRUpgradeLv[1]++;
            else if (ugKind == UpgradeKind.Super && GlobalValue.g_SDJRUpgradeLv[2] < 15)
                GlobalValue.g_SDJRUpgradeLv[2]++;

            NetworkMgr.inst.PushPacket(PacketType.SDJRUpgradeLv);
        }
        UpdateUpgradeKind();

        //������ �̹��� �����ֱ�
        if (lvUpImgObj != null)
            lvUpImgObj.SetActive(true);
        isupgradeDelay = true;
        StartCoroutine(DelayTime());
    }

    IEnumerator DelayTime()
    {
        yield return wfsDelay;
        isupgradeDelay = false;
    }

}
