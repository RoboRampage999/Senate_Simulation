using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAI : MonoBehaviour {

    public int Emotion = 2, Response = -1;
    float Influence = 0.0f;
    float[] EmotionWeight = new float[5];
    int AIType = -1;
    SaveAIType AISingleton;
    Canvas canvas;
    SenatorTraits[] STScripts; 

    // Use this for initialization
    void Start ()
    {
        AISingleton = (SaveAIType)FindObjectOfType(typeof(SaveAIType));
        AIType = AISingleton.AIType;
        canvas = GameObject.FindObjectOfType<Canvas>();
        STScripts = GetComponentsInChildren<SenatorTraits>();

        for (int j = 0; j < 4; j++)
        {
            EmotionWeight[j] = -1;
        }

        int TotalWeighted = 0;
        float InfluenceValue = 40.0f;

        //While not all emotions given weight
        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, 5);

            //If already weighted, select another emotion to weight
            if (EmotionWeight[rand] != -1)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (EmotionWeight[k] == -1)
                    {
                        rand = k;
                        k = 5;
                    }
                }
            }

            //Mark the emotion as weighted
            TotalWeighted++;

            if (TotalWeighted >= 2)
            {
                InfluenceValue *= -1;
            }

            //Weight the emotion
            switch (rand)
            {
                case 0:
                    EmotionWeight[rand] = InfluenceValue / (TotalWeighted * 2);
                    break;
                case 1:
                    EmotionWeight[rand] = InfluenceValue / (TotalWeighted * 2);
                    break;
                case 2:
                    EmotionWeight[rand] = InfluenceValue / (TotalWeighted * 2);
                    break;
                case 3:
                    EmotionWeight[rand] = InfluenceValue / (TotalWeighted * 2);
                    break;
                case 4:
                    EmotionWeight[rand] = InfluenceValue / (TotalWeighted * 2);
                    break;
            }
        }
		
	}

	// Update is called once per frame
	void Update ()
    {
        if (Response != -1)
        {
            Influence = (EmotionWeight[Emotion] / 100);
            float PIC = 0.0f;
            int RangeHigh = 0, RangeLow = 0;

            if (Influence < 0) { RangeHigh = 0; RangeLow = -5; }
            else if (Influence > 0) { RangeHigh = 6; RangeLow = 1; }

            foreach(SenatorTraits STS in STScripts)
            {
                PIC = (Random.Range(RangeLow, RangeHigh)/5);
                STS.SendMessage("PIBChange", PIC);
            }
            SendInfluence();
        }
    }

    void SendInfluence()
    {
        canvas.GetComponent<UIScript>().SendMessage("AdjustInfluence", Influence);
        Influence = 0;
        Response = -1;
    }

    void SetAggressive() { Emotion = 0; }
    void SetAssertive() { Emotion = 1; }
    void SetCalm() { Emotion = 2; }
    void SetCautious() { Emotion = 3; }
    void SetPassionate() { Emotion = 4; }
    void SetResponse1() { if (AIType == 1) { Response = 0; } }
    void SetResponse2() { if (AIType == 1) { Response = 1; } }
    void SetResponse3() { if (AIType == 1) { Response = 2; } }
    void SetResponse4() { if (AIType == 1) { Response = 3; } }
}
