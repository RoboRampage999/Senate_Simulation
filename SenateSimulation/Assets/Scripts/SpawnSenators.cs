using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class SpawnSenators : MonoBehaviour
{

    public int SenatorCount, Excess, PopCount = 0, ConCount = 0;
    public GameObject ConYoungSenator;
    public GameObject PopYoungSenator;
    public GameObject ConOldSenator;
    public GameObject PopOldSenator;
    public GameObject ConLargeSenator;
    public GameObject PopLargeSenator;
    GameObject ToInstantiate;

    Vector3 Position;
    Quaternion Rotation;
    float Modifier = 6.84f, Translate = 7.0f, Spacing = 0.174533f;
    int Counter = 0;
    Vector3 LookAtV = new Vector3(0.0f, 0.5f, 6.5f);

    public List<string> TraitsNames = new List<string>();
    public List<string> TraitsStats = new List<string>();
    public List<int> TraitTreePos = new List<int>();
    int TraitCount;

    // Use this for initialization
    //Top Seating Y = 2.36 // Middle Seating Y = 1.68 // Lower Seating Y = 0.93
    //Top Co-ords (-5.6, 2.36, -4.4) Rotation Y = 25
    //Mid Co-ords (-4.5, 1.68, -2.2) Rotation Y = 25
    //Lower Co-ords (-3.7, 0.93, -0.6) Rotation Y = 25   Next (-4.8, y, 0.1) RotY = 35
    //Floor Co-ords (y = 0.28)

    // REMEMBER RADIANS, NEITHER DID I
    void Start()
    {
        string path = (Application.dataPath + "/StreamingAssets/Textfiles/SenatorTraits.txt");
        //Name. Description. Effect. Stat.
        string[] AllLines = System.IO.File.ReadAllLines(path);

        //Formatting
        for (int i = 0; i < AllLines.Length; i++)
        {
            string value = AllLines[i];
            string[] parts = Regex.Split(value, "~~name~~");
            TraitsNames.Add(parts[1]);
            i += 3;
            value = AllLines[i];
            parts = Regex.Split(value, "~~stat~~");
            TraitsStats.Add(parts[1]);
            i++;
            value = AllLines[i];
            parts = Regex.Split(value, "~~treepos~~");
            TraitTreePos.Add(Int32.Parse(parts[1]));
        }

        TraitCount = TraitsNames.Count;
        int SenatorID = 0;

        Position.y = 0.28f;
        Rotation.y = -2.78526f;
        Rotation.w = 1.0f;

        Position.x = (Modifier * Mathf.Sin(Rotation.y));
        Position.z = (Modifier * Mathf.Cos(Rotation.y)) + Translate;

        for (int i = 0; i < SenatorCount; i++)
        {
            int Senator = UnityEngine.Random.Range(0, 6);

            switch (Senator)
            {
                case 0:
                    ToInstantiate = ConYoungSenator;
                    ConCount++;
                    break;
                case 1:
                    ToInstantiate = ConLargeSenator;
                    ConCount++;
                    break;
                case 2:
                    ToInstantiate = ConOldSenator;
                    ConCount++;
                    break;
                case 3:
                    ToInstantiate = PopYoungSenator;
                    PopCount++;
                    break;
                case 4:
                    ToInstantiate = PopLargeSenator;
                    PopCount++;
                    break;
                case 5:
                    ToInstantiate = PopOldSenator;
                    PopCount++;
                    break;
                default:
                    break;
            }

            if (Position.z < 1)
            {
                if ((Position.x < 2.45) && (Position.x > -2.3) && (Counter == 0))
                {
                    Modifier = 8.5f;
                    Rotation.y = -2.70526f;
                    Position.y = 0.93f;
                    Counter++;
                    LookAtV = new Vector3(0.0f, 0.8f, 6.5f);
                }
                else if ((Position.x < 4.5) && (Position.x > -3.4) && (Counter == 1))
                {
                    Modifier = 10.18f;
                    Rotation.y = -2.70526f;
                    Position.y = 1.68f;
                    Spacing = 0.104533f;
                    Counter++;
                    LookAtV = new Vector3(0.0f, 1.5f, 6.5f);
                }
                else if ((Position.x < 5) && (Position.x > -4.3) && (Counter == 2))
                {
                    Modifier = 11.08f;
                    Rotation.y = -2.62526f;
                    Position.y = 1.68f;
                    LookAtV = new Vector3(0.0f, 1.6f, 6.5f);
                    Counter++;
                }
                else if ((Position.x < 4.8) && (Position.x > -4.3) && (Counter == 3))
                {
                    Modifier = 12.26f;
                    Rotation.y = -2.70526f;
                    Position.y = 2.36f;
                    Spacing = 0.084533f;
                    LookAtV = new Vector3(0.0f, 2.1f, 6.5f);
                    Counter++;
                }
                else if ((Position.x < 6.0) && (Position.x > -5.1) && (Counter == 4))
                {
                    Modifier = 13.5f;
                    Rotation.y = -2.70526f;
                    Position.y = 2.36f;
                    Counter++;
                    LookAtV = new Vector3(0.0f, 2.2f, 6.5f);
                }
                else if ((Position.x < 6.4) && (Position.x > -5.1) && (Counter == 5))
                {
                    Excess = SenatorCount - i;
                    i = SenatorCount;
                    continue;
                }
            }

            Position.x = (Modifier * Mathf.Sin(Rotation.y));
            Position.z = (Modifier * Mathf.Cos(Rotation.y)) + Translate;

            GameObject Temp = Instantiate(ToInstantiate, Position, Rotation, this.transform);
            Temp.GetComponent<SenatorTraits>().ID = SenatorID;
            SenatorID++;
            if (Senator < 3) { Temp.GetComponent<SenatorTraits>().Party = 0; }
            else { Temp.GetComponent<SenatorTraits>().Party = 1; }
            Temp.SendMessage("AssignTraits", TraitCount);
            Temp.transform.LookAt(LookAtV);

            //Spacing inbetween each Senator
            Rotation.y += Spacing;
        }

        this.gameObject.BroadcastMessage("SortStats");
    }

    void RecCanvasMessage(Canvas can)
    {
        can.GetComponent<UIScript>().SendMessage("InitInf", PopCount);
    }
}
