using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateCharMgr : MonoBehaviour
{
    [SerializeField] GameObject bgObj;
    Vector2[] spawnPos = new Vector2[4];
    float prefabSize = 5600.0f;

    void Awake() => AwakeFunc();

    private void AwakeFunc()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPos();
        InitSpawnBG();
    }

    //void Update() => UpdateFunc();

    // Update is called once per frame
    void UpdateFunc()
    {
        
    }

    void InitPos()
    {
        spawnPos[0] = new Vector2(0.0f, 0.25f) * prefabSize;
        spawnPos[1] = new Vector2(1.0f, 0.25f) * prefabSize;
        spawnPos[2] = new Vector2(1.0f, 1.25f) * prefabSize;
        spawnPos[3] = new Vector2(0.0f, 1.25f) * prefabSize;
    }

    void InitSpawnBG()
    {
        for (int ii = 0; ii < spawnPos.Length; ii++)
        {
            bgObj = MemoryPoolManager.instance.GetObject("ImageParent");
            bgObj.transform.position = spawnPos[ii];
            bgObj.SetActive(true);
        }
    }

    internal void SpawnBG(Vector2 spawnPos)
    {
        bgObj = MemoryPoolManager.instance.GetObject("ImageParent");
        bgObj.transform.position = spawnPos;
        bgObj.SetActive(true);
    }
}
