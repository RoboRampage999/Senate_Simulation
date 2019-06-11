using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComplexAI : MonoBehaviour
{

    public int Emotion = 2, Response = -1;
    public float StartInf = 0.0f, FinalInf = 0.0f, RunningInf = 0.0f;
    int AIType = -1;
    SaveAIType AISingleton;
    Canvas canvas;

    void Start() { AISingleton = (SaveAIType)FindObjectOfType(typeof(SaveAIType)); AIType = AISingleton.AIType; }

    void HandleInfUpdate()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        StartInf = (GameObject.FindGameObjectWithTag("InfluenceBar").GetComponent<Scrollbar>().size) * 1000;

        CalculateInf();

        SendInf(); //Overall Influence Change
    }

    void CalculateInf()
    {
        SenatorTraits[] STScripts = GetComponentsInChildren<SenatorTraits>();

        for (int i = 0; i < STScripts.Length; i++)
        {
            int TempReason = STScripts[i].Reasoning;
            int TempComp = STScripts[i].Compromising;
            int ReasonMod = 0, CompMod = 0;
            float Multiplier = 1.0f;
            Multiplier = CalRepMult();

            if (Emotion == 0) //Aggressive
            {
                if (TempReason < -50) { RunningInf += 5; ReasonMod += -10; }
                else if ((TempReason >= -50) && (TempReason <= 0)) { RunningInf += 10; ReasonMod += -5; }
                else if (TempReason > 50) { RunningInf += -5; ReasonMod += 5; }

                if (TempComp < -50) { RunningInf += 10; CompMod += -10; }
                else if ((TempComp >= -50) && (TempComp < 0)) { RunningInf += 5; CompMod += -5; }
                else if ((TempComp >= 0) && (TempComp <= 50)) { RunningInf += -5; CompMod += 5; }
                else if (TempComp > 50) { RunningInf += -10; CompMod += 10; }
            }
            else if (Emotion == 1) //Assertive
            {
                if (TempReason < -50) { RunningInf += -10; ReasonMod += 10; }
                else if ((TempReason >= -50) && (TempReason <= 0)) { RunningInf += -5; ReasonMod += 5; }
                else if (TempReason > 50) { RunningInf += 5; }

                if (TempComp < -50) { RunningInf += 10; CompMod += -10; }
                else if ((TempComp >= -50) && (TempComp < 0)) { RunningInf += 5; CompMod += -5; }
                else if ((TempComp >= 0) && (TempComp <= 50)) { RunningInf += -5; CompMod += -5; }
                else if (TempComp > 50) { RunningInf += -10; CompMod += 5; }
            }
            else if (Emotion == 2) //Calm
            {
                if (TempReason < -50) { RunningInf += -5; }
                else if (TempReason > 50) { RunningInf += 5; ReasonMod += 5; }

                if (TempComp < -50) { RunningInf += -10; }
                else if ((TempComp >= -50) && (TempComp < 0)) { RunningInf += -5; }
                else if ((TempComp >= 0) && (TempComp <= 50)) { RunningInf += 10; CompMod += 5; }
                else if (TempComp > 50) { RunningInf += 20; CompMod += 10; }
            }
            else if (Emotion == 3) //Cautious
            {
                if (TempReason < -50) { RunningInf += -15; ReasonMod += 10; }
                else if ((TempReason >= -50) && (TempReason <= 0)) { RunningInf += -10; ReasonMod += 5; }
                else if ((TempReason > 0) && (TempReason <= 50)) { RunningInf += 10; }
                else if (TempReason > 50) { RunningInf += 25; ReasonMod -= 5; }

                if (TempComp < -50) { RunningInf += -15; CompMod += 10; }
                else if ((TempComp >= -50) && (TempComp < 0)) { RunningInf += -5; CompMod += 5; }
                else if ((TempComp >= 0) && (TempComp <= 50)) { RunningInf += 10; }
                else if (TempComp > 50) { RunningInf += 25; CompMod += -5; }
            }
            else if (Emotion == 4) // Passionate
            {
                if (TempReason < -75) { RunningInf += -15; ReasonMod += 15; }
                else if ((TempReason >= -75) && (TempReason <= -50)) { RunningInf += -10; ReasonMod += 10; }
                else if ((TempReason > -50) && (TempReason <= -25)) { RunningInf += -5; ReasonMod += 5; }
                else if ((TempReason >= 0) && (TempReason <= 25)) { RunningInf += 5; }
                else if ((TempReason > 25) && (TempReason <= 50)) { RunningInf += 10; ReasonMod += -5; }
                else if ((TempReason > 50) && (TempReason < 75)) { RunningInf += 15; ReasonMod += -10; }
                else if (TempReason >= 75) { RunningInf += 25; ReasonMod += -15; }

                if (TempComp < -75) { RunningInf += -15; CompMod += 25; }
                else if ((TempComp >= -75) && (TempComp <= -50)) { RunningInf += -10; CompMod += 20; }
                else if ((TempComp > -50) && (TempComp <= -25)) { RunningInf += -5; CompMod += 15; }
                else if ((TempComp >= 0) && (TempComp <= 25)) { RunningInf += 5; CompMod += 10; }
                else if ((TempComp > 25) && (TempComp <= 50)) { RunningInf += 10; CompMod += 5; }
                else if ((TempComp > 50) && (TempComp < 75)) { RunningInf += 15; }
                else if (TempComp >= 75) { RunningInf += 25; CompMod += -5; }
            }

            //Update Senators stats here (Because in for loop)
            STScripts[i].Compromising += CompMod;
            STScripts[i].Reasoning += ReasonMod;

            if (STScripts[i].Compromising > 100) { STScripts[i].Compromising = 100; }
            else if (STScripts[i].Compromising < -100) { STScripts[i].Compromising = -100; }

            if (STScripts[i].Reasoning > 100) { STScripts[i].Reasoning = 100; }
            else if (STScripts[i].Reasoning < -100) { STScripts[i].Reasoning = -100; }

            float PI = STScripts[i].PlayerInf;

            if (RunningInf < 0) {
                if (PI >= 75) { RunningInf *= 0.5f; STScripts[i].PlayerInf += -1; }
                else if ((PI < 75) && (PI >= 50)) { RunningInf *= 0.6f; STScripts[i].PlayerInf += -1.5f; }
                else if ((PI < 50) && (PI >= 25)) { RunningInf *= 0.75f; STScripts[i].PlayerInf += -2; }
                else if ((PI < 25) && (PI >= 0)) { RunningInf *= 0.9f; STScripts[i].PlayerInf += -2.5f; }
                else if ((PI < 0) && (PI >= -25)) { RunningInf *= 1.1f; STScripts[i].PlayerInf += -3; }
                else if ((PI < -25) && (PI >= -50)) { RunningInf *= 1.25f; STScripts[i].PlayerInf += -3.5f; }
                else if ((PI < -50) && (PI >= -75)) { RunningInf *= 1.4f; STScripts[i].PlayerInf += -4; }
                else if (PI < -75) { RunningInf *= 1.5f; STScripts[i].PlayerInf += -5; }
            }
            else {
                if (PI >= 75) { RunningInf *= 1.5f; STScripts[i].PlayerInf += 5; }
                else if ((PI < 75) && (PI >= 50)) { RunningInf *= 1.4f; STScripts[i].PlayerInf += 4.0f; }
                else if ((PI < 50) && (PI >= 25)) { RunningInf *= 1.25f; STScripts[i].PlayerInf += 3.5f; }
                else if ((PI < 25) && (PI >= 0)) { RunningInf *= 1.1f; STScripts[i].PlayerInf += 3; }
                else if ((PI < 0) && (PI >= -25)) { RunningInf *= 0.9f; STScripts[i].PlayerInf += 2.5f; }
                else if ((PI < -25) && (PI >= -50)) { RunningInf *= 0.75f; STScripts[i].PlayerInf += 2; }
                else if ((PI < -50) && (PI >= -75)) { RunningInf *= 0.6f; STScripts[i].PlayerInf += 1.5f; }
                else if (PI < -75) { RunningInf *= 0.5f; STScripts[i].PlayerInf += 1; }
            }     
            
            if (PI > STScripts[i].PlayerInf) { STScripts[i].SendMessage("PIBChange", (-
                (PI - STScripts[i].PlayerInf)/5)); }
            else if (PI < STScripts[i].PlayerInf) { STScripts[i].SendMessage("PIBChange", ((STScripts[i].PlayerInf - PI)/5)); }

            //Tally overall influence change here (Adding each value to a float)
            RunningInf = ((RunningInf / 100) * Multiplier);
        }

        FinalInf = RunningInf;
    }

    float CalRepMult()
    {
        float Multiplier = 1.0f;
        if (Response == 0) {
            if (Emotion == 0) { Multiplier = 0.8f; }
            else if (Emotion == 1) { Multiplier = 1.1f; }
            else if (Emotion == 2) { Multiplier = 1.0f; }
            else if (Emotion == 3) { Multiplier = 0.8f; }
            else if (Emotion == 4) { Multiplier = 1.1f; } }
        else if (Response == 1) { 
            if (Emotion == 0) { Multiplier = 0.75f; }
            else if (Emotion == 1) { Multiplier = 1.1f; }
            else if (Emotion == 2) { Multiplier = 1.0f; }
            else if (Emotion == 3) { Multiplier = 0.75f; }
            else if (Emotion == 4) { Multiplier = 1.1f; } }
        else if (Response == 2) { 
            if (Emotion == 0) { Multiplier = 0.9f; }
            else if (Emotion == 1) { Multiplier = 1.2f; }
            else if (Emotion == 2) { Multiplier = 1.1f; }
            else if (Emotion == 3) { Multiplier = 0.8f; }
            else if (Emotion == 4) { Multiplier = 1.25f; } }
        else if (Response == 3) { 
            if (Emotion == 0) { Multiplier = 1.5f; }
            else if (Emotion == 1) { Multiplier = 1.25f; }
            else if (Emotion == 2) { Multiplier = 1.1f; }
            else if (Emotion == 3) { Multiplier = 0.7f; }
            else if (Emotion == 4) { Multiplier = 1.0f; } } return Multiplier;
    }

    void SendInf()
    {
        if (FinalInf > 1) { FinalInf = 1; }
        else if (FinalInf < -1) { FinalInf = -1; }
        canvas.GetComponent<UIScript>().SendMessage("AdjustInfluence", FinalInf);
    }

    void SetAggressive() { Emotion = 0; }
    void SetAssertive() { Emotion = 1; }
    void SetCalm() { Emotion = 2; }
    void SetCautious() { Emotion = 3; }
    void SetPassionate() { Emotion = 4; }
    void SetResponse1() { if (AIType == 2) { Response = 0; HandleInfUpdate(); } }
    void SetResponse2() { if (AIType == 2) { Response = 1; HandleInfUpdate(); } }
    void SetResponse3() { if (AIType == 2) { Response = 2; HandleInfUpdate(); } }
    void SetResponse4() { if (AIType == 2) { Response = 3; HandleInfUpdate(); } }

}
