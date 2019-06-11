using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class SenatorTraits : MonoBehaviour
{

    public List<string> TraitName = new List<string>();
    public List<int> TraitValue = new List<int>();
    List<List<string>> TraitEffects = new List<List<string>>();
    public int Compromising = 0, Reasoning = 0, OverInf = 0, PopInf = 0, OpiInf = 0, ID = 0, Party = -1; //Opi = 0, Pop = 1
    public float PlayerInf = 0; 
    bool CompEff = false, ReasonEff = false, AllEff = false, PopEff = false, OpiEff = false, OwnPartyEff = false, OppPartyEff = false, NegEff = false, RelationEff = false, InfEff = false;
    public List<int> SenateRelation = new List<int>();
    Transform PlayerInfBar;

    private void Start()
    {
        Transform[] TempTrans = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform Trans in TempTrans) { if (Trans.tag == "SenInfBar") { PlayerInfBar = Trans; } }

        PlayerInfBar.localScale += new Vector3((PlayerInf / 20), 0.0f, 0.0f);
                
    }

    void AssignTraits(int TraitCount)
    {
        int rand = UnityEngine.Random.Range(1, 4);

        List<int> Assigned = new List<int>();
        for (int i = 0; i < rand; i++)
        {
            int TraitRand = UnityEngine.Random.Range(0, TraitCount);

            for (int j = 0; j < Assigned.Count; j++)
            {
                if (Assigned[j] == TraitRand)
                {
                    j = 0;
                    TraitRand = UnityEngine.Random.Range(0, TraitCount);
                }
            }

            TraitName.Add(GetComponentInParent<SpawnSenators>().TraitsNames[TraitRand]);

            string traitStat = GetComponentInParent<SpawnSenators>().TraitsStats[TraitRand];
            string[] traitStatParts = Regex.Split(traitStat, ",");

            List<string> temp = new List<string>();

            foreach (string s in traitStatParts)
            {
                switch (s)
                {
                    case "5":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "10":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "15":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "20":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "25":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "30":
                        TraitValue.Add(Int32.Parse(s));
                        break;
                    case "-":
                        temp.Add(s);
                        break;
                    case "r":
                        temp.Add(s);
                        break;
                    case "o":
                        temp.Add(s);
                        break;
                    case "i":
                        temp.Add(s);
                        break;
                    case "c":
                        temp.Add(s);
                        break;
                    case "#":
                        temp.Add(s);
                        break;
                    case "s":
                        temp.Add(s);
                        break;
                    case "p":
                        temp.Add(s);
                        break;
                    case "q":
                        temp.Add(s);
                        break;
                    case "u":
                        temp.Add(s);
                        break;
                    case "l":
                        temp.Add(s);
                        break;
                }
            }

            TraitEffects.Add(temp);
            Assigned.Add(TraitRand);
            switch (GetComponentInParent<SpawnSenators>().TraitTreePos[TraitRand])
            {
                case 1:
                    Assigned.Add(TraitRand + 1);
                    Assigned.Add(TraitRand + 2);
                    break;
                case 2:
                    Assigned.Add(TraitRand - 1);
                    Assigned.Add(TraitRand + 1);
                    break;
                case 3:
                    Assigned.Add(TraitRand - 1);
                    Assigned.Add(TraitRand - 2);
                    break;
            }
        }
    }

    void SortStats()
    {
        int rand = UnityEngine.Random.Range(-80, 80);
        Compromising = rand;

        rand = UnityEngine.Random.Range(-80, 80);
        Reasoning = rand;

        if (Party == 0) { OpiInf = UnityEngine.Random.Range(-20, 80); PopInf = UnityEngine.Random.Range(-80, 20); }
        else if (Party == 1) { OpiInf = UnityEngine.Random.Range(-80, 20); PopInf = UnityEngine.Random.Range(-20, 80); }

        SenatorTraits[] ChildScripts = transform.parent.GetComponentsInChildren<SenatorTraits>();
        int counter = 0;

        foreach (SenatorTraits script in ChildScripts)
        {
            if (script.ID == ID) { continue; }
            else
            {
                if (script.Party == Party) { rand = UnityEngine.Random.Range(-20, 80); }
                else { rand = UnityEngine.Random.Range(-80, 20); }
                SenateRelation.Add(rand);
            }
            counter++;
        }

        OverInf = (OpiInf + PopInf) / 2;

        int outerCount = TraitName.Count; //How many traits this Senator has

        rand = UnityEngine.Random.Range(-5, 20);
        PlayerInf = PopInf + rand;

        for (int j = 0; j < outerCount; j++)
        {
            int count = TraitEffects[j].Count; //How many parameters each trait has
            for (int i = 0; i < count; i++)
            {
                SetBools(j, i);
            }
            //Calculate Changes in here

            CalculateChanges(j, ChildScripts);

            ResetBools();
        }
    }

    void SetBools(int j, int i)
    {
        switch (TraitEffects[j][i])
        {
            case "-":
                NegEff = true;
                break;
            case "r":
                RelationEff = true;
                break;
            case "o":
                OppPartyEff = true;
                break;
            case "i":
                InfEff = true;
                break;
            case "c":
                OpiEff = true;
                break;
            case "#":
                OwnPartyEff = true;
                break;
            case "s":
                AllEff = true;
                break;
            case "p":
                CompEff = true;
                break;
            case "q":
                ReasonEff = true;
                break;
            case "u":
                OwnPartyEff = true;
                break;
            case "l":
                PopEff = true;
                break;
        }
    }

    void ResetBools()
    {
        CompEff = false;
        ReasonEff = false;
        AllEff = false;
        PopEff = false;
        OpiEff = false;
        OwnPartyEff = false;
        OppPartyEff = false;
        NegEff = false;
        RelationEff = false;
        InfEff = false;
    }

    void CalculateChanges(int j, SenatorTraits[] ChildScripts)
    {
        int tempValue = 0;

        if (NegEff) { tempValue = -(TraitValue[j]); }
        else { tempValue = TraitValue[j]; }

        tempValue *= 2;

        if (CompEff) { Compromising += tempValue; }
        else if (ReasonEff) { Reasoning += tempValue; }

        bool RorI = false;
        if (RelationEff) { RorI = true; }
        else if (InfEff) { RorI = false; }

        int Parties = -1;
        if (AllEff) { Parties = 2; }
        else if (PopEff) { Parties = 1; }
        else if (OpiEff) { Parties = 0; }
        else if (OwnPartyEff) {
            if (Party == 0) { Parties = 0; } else { Parties = 1; } }
        else if (OppPartyEff) {
            if (Party == 0) { Parties = 1; } else { Parties = 0; } }

        RelationLoop(Parties, RorI, tempValue, ChildScripts);
        CapStats();        
    }

    void RelationLoop(int Parties, bool RorI, int tempValue, SenatorTraits[] ChildScripts)
    {
        if (RorI)
        {         
            for (int i = 0; i < SenateRelation.Count; i++)
            {
                if (ChildScripts[i].Party == Parties) { SenateRelation[i] += tempValue; }
                else if (Parties == 2) { SenateRelation[i] += tempValue; }
            }
        }
        else if (!RorI)
        {
            switch (Parties)
            {
                case 0: OpiInf += tempValue; break;
                case 1: PopInf += tempValue; break;
                case 2: OpiInf += tempValue; PopInf += tempValue; break;
            }
            OverInf = (OpiInf + PopInf) / 2;
        }
    }

    void CapStats()
    {
        if (Compromising > 100) { Compromising = 100; } else if (Compromising < -100) { Compromising = -100; }
        if (Reasoning > 100) { Reasoning = 100; } else if (Reasoning < -100) { Reasoning = -100; }
        if (PopInf > 100) { PopInf = 100; } else if (PopInf < -100) { PopInf = -100; }
        if (OpiInf > 100) { OpiInf = 100; } else if (OpiInf < -100) { OpiInf = -100; }
        OverInf = (PopInf + OpiInf) / 2;
        for (int i = 0; i < SenateRelation.Count; i++) {
            if (SenateRelation[i] > 100) { SenateRelation[i] = 100; } else if (SenateRelation[i] < -100) { SenateRelation[i] = -100; } }
        if (PlayerInf > 100) { PlayerInf = 100; } else if (PlayerInf < -100) { PlayerInf = -100; }
    }

    void PIBChange(float Dif)
    {
       if (PlayerInfBar.localScale.x + Dif >= 10) { PlayerInfBar.localScale -= new Vector3(PlayerInfBar.localScale.x, 0.0f, 0.0f); PlayerInfBar.localScale += new Vector3(10.1f, 0.0f, 0.0f); Dif = 0;  }
       else if (PlayerInfBar.localScale.x + Dif <= 0) { PlayerInfBar.localScale -= new Vector3(PlayerInfBar.localScale.x, 0.0f, 0.0f); PlayerInfBar.localScale += new Vector3(0.1f, 0.0f, 0.0f); Dif = 0; }
       PlayerInfBar.localScale += new Vector3((Dif), 0.0f, 0.0f);
    }
}
