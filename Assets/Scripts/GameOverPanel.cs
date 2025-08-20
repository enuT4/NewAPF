using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    internal Image gameoverImg;
    internal Image timeupImg;
    [SerializeField]float gameoverShowTimer = 0.0f;


    private void Awake()
    {
        if (!gameoverImg) gameoverImg = transform.Find("GameOverImg").GetComponent<Image>();
        if (!timeupImg) timeupImg = transform.Find("TimeUpImg").GetComponent<Image>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (gameoverImg != null && gameoverImg.gameObject.activeSelf)
            gameoverImg.gameObject.SetActive(false);

        if (timeupImg != null && timeupImg.gameObject.activeSelf)
            timeupImg.gameObject.SetActive(false);
    }

    void Update() => UpdateFunc();
    // Update is called once per frame
    void UpdateFunc()
    {
        if (gameoverShowTimer > 0.0f)
        {
            gameoverShowTimer -= Time.deltaTime;
            if (gameoverShowTimer < 0.0f)
            {
                gameoverShowTimer = 0.0f;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    private void OnEnable()
    {
        gameoverShowTimer = 3.0f;
    }

    public void TimeUpOrGameOver(bool isTimeUp)
    {
        if (isTimeUp)
        {
            gameoverImg.gameObject.SetActive(false);
            timeupImg.gameObject.SetActive(true);
        }
        else
        {
            gameoverImg.gameObject.SetActive(true);
            timeupImg.gameObject.SetActive(false);
        }

    }
}
