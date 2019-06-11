using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAIType : MonoBehaviour {

    public static SaveAIType Instance;
    public int AIType = 0;

    void Awake()
    {
        this.InstantiateController();
        Instance.ChooseType();
    }

    private void ChooseType()
    {
        if (AIType == 2) { AIType = 1; }
        else if (AIType == 1) { AIType = 2; }
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                AIType = 2;
            }
            else
            {
                AIType = 1;
            }
        }
    }

    private void InstantiateController()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);         
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
    }
}
