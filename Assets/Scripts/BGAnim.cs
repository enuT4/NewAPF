using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnim : MemoryPoolObject
{
    Vector2 startPos;
    Vector2 endPos;
    Vector2 tempPos;
    [SerializeField] float value = 0.0f;

    CreateCharMgr charMgr;

    // Start is called before the first frame update
    void Start()
    {
        if (!charMgr) charMgr = GameObject.Find("CreateCharMgr").GetComponent<CreateCharMgr>();

        startPos = transform.localPosition;
        endPos = new Vector2(startPos.x - 5600.0f, startPos.y - 5600.0f);//수치 바꿔야함
    }

    // Update is called once per frame
    void Update()
    {
        
        BGMove();
    }


    private void OnEnable()
    {
        transform.localPosition = startPos;
        value = 0.0f;
    }

    void BGMove()
    {
        if (value < 1.0f)
        {
            value += Time.deltaTime * 0.05f;
            if (value >= 1.0f)
            {
                value = 1.0f;
                ObjectReturn();
                charMgr.SpawnBG(startPos);
            }
        }
        tempPos = Vector2.Lerp(startPos, endPos, value);
        transform.localPosition = tempPos;
    }
}
