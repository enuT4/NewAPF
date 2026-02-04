using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    [SerializeField] internal Image gameoverImg;
    [SerializeField] internal Image timeupImg;
    [SerializeField] float gameoverShowTimer = 0.0f;


    private void Awake()
    {
        if (!gameoverImg) gameoverImg = transform.Find("GameOverImg").GetComponent<Image>();
        if (!timeupImg) timeupImg = transform.Find("TimeUpImg").GetComponent<Image>();

        if (gameoverImg != null && gameoverImg.gameObject.activeSelf)
            gameoverImg.gameObject.SetActive(false);

        if (timeupImg != null && timeupImg.gameObject.activeSelf)
            timeupImg.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
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

    public void TimeUpOrGameOver(bool isTimeUp)
    {
        if (gameoverImg == null || timeupImg == null) return;
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
        gameoverShowTimer = 3.0f;
    }
}
