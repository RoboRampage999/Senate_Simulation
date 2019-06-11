using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    //0-4 Emotions, 0-3 Responses
    int Bill = -1, Challenge = 0, AIType = 0;
    bool DescRead = false;
    GameObject BillText, DescText;
    SaveAIType AISingleton;
    Scrollbar InfluenceBar;
    public GameObject SenatorsObj;
    public Text MainText, AIText;
    public Text[] ResponseText = new Text[4];
    string[] AllLines;
    string finalDecision;


    // Use this for initialization
    void Start()
    {
        AISingleton = (SaveAIType)FindObjectOfType(typeof(SaveAIType));
        AIType = AISingleton.AIType;
        InfluenceBar = GameObject.FindGameObjectWithTag("InfluenceBar").GetComponent<Scrollbar>();
        //string path = (Application.dataPath + "/../Assets/Textfiles/DebateTopics.txt");
        string path = (Application.dataPath + "/StreamingAssets/Textfiles/DebateTopics.txt");
        //Name. Description. Option 1. Option 2. Challenge1a. Response1a. Response1b etc etc.
        AllLines = System.IO.File.ReadAllLines(path);
        BillText = GameObject.FindWithTag("BillText");
        DescText = GameObject.FindWithTag("DescText");
        SetAIType();
        SenatorsObj.GetComponent<SpawnSenators>().SendMessage("RecCanvasMessage", this.gameObject.GetComponent<Canvas>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (DescRead == true)
        {
            if (Bill == 0) //Land Reform
            {
                switch (Challenge)
                {
                    case 0:
                        break;
                    case 1:
                        SetText(4);
                        break;
                    case 2:
                        SetText(9);
                        break;
                    case 3:
                        SetText(14);
                        break;
                    case 4:
                        SetText(19);
                        break;
                    case 5:
                        SetText(24);
                        break;
                    case 6:
                        SetText(29);
                        break;
                    case 7:
                        SetText(34);
                        break;
                    case 8:
                        EndGame();
                        break;
                    default:
                        SetText(78);
                        break;
                }
            }
            else if (Bill == 1) //Hannibal
            {
                switch (Challenge)
                {
                    case 0:
                        break;
                    case 1:
                        SetText(43);
                        break;
                    case 2:
                        SetText(48);
                        break;
                    case 3:
                        SetText(53);
                        break;
                    case 4:
                        SetText(58);
                        break;
                    case 5:
                        SetText(63);
                        break;
                    case 6:
                        SetText(68);
                        break;
                    case 7:
                        SetText(73);
                        break;
                    case 8:
                        EndGame();
                        break;
                    default:
                        SetText(78);
                        break;
                }
            }
        }
    }

    void SetChallenges() { DescRead = true; }
    void NextResponse() { Challenge++; }

    void AdjustInfluence(float inf)
    {
        InfluenceBar.size += inf;

        if (InfluenceBar.size == 0) { InfluenceBar.GetComponentInChildren<Image>().enabled = false; }
        else { InfluenceBar.GetComponentInChildren<Image>().enabled = true; }
    }

    void InitInf(float PopCount)
    {
        PopCount = (PopCount - 150.0f)/100;
        InfluenceBar.size = (0.5f + PopCount);
    }

    void SetAIType()
    {
        if (AIType == 1)
        {
            SenatorsObj.GetComponent<ComplexAI>().enabled = false;
            SenatorsObj.GetComponent<SimpleAI>().enabled = true;
            AIText.text = "AI Type: A";
        }
        else if (AIType == 2)
        {
            SenatorsObj.GetComponent<SimpleAI>().enabled = false;
            SenatorsObj.GetComponent<ComplexAI>().enabled = true;
            AIText.text = "AI Type: B";
        }
    }

    void SetText(int startNum)
    {
        MainText.text = AllLines[startNum];
        ResponseText[0].text = AllLines[startNum + 1];
        ResponseText[1].text = AllLines[startNum + 2];
        ResponseText[2].text = AllLines[startNum + 3];
        ResponseText[3].text = AllLines[startNum + 4];
    }

    void SetBill1()
    {
        Bill = 0;
        BillText.GetComponent<Text>().text = "Bill: " + AllLines[0];
        GameObject.FindWithTag("DescPanel").GetComponent<Image>().enabled = true;
        GameObject.FindWithTag("DescText").GetComponent<Text>().enabled = true;
        DescText.GetComponent<Text>().text = ("Debate Topic: " + AllLines[0] + "\n\n" + AllLines[1] + " between the options of: " + AllLines[2] + " or " + AllLines[3]);
    }

    void SetBill2()
    {
        Bill = 1;
        BillText.GetComponent<Text>().text = "Bill: " + AllLines[39];
        GameObject.FindWithTag("DescPanel").GetComponent<Image>().enabled = true;
        GameObject.FindWithTag("DescText").GetComponent<Text>().enabled = true;
        DescText.GetComponent<Text>().text = ("Debate Topic: " + AllLines[39] + "\n\n" + AllLines[40] + " between the options of: " + AllLines[41] + " or " + AllLines[42]);
    }

    void BeginResponse()
    {
        Challenge = 1;
        ResponseText[0].GetComponentInParent<Button>().interactable = true;
        ResponseText[1].GetComponentInParent<Button>().interactable = true;
        ResponseText[2].GetComponentInParent<Button>().interactable = true;
        ResponseText[3].GetComponentInParent<Button>().interactable = true;
    }

    void EndGame()
    {
        if (Bill == 0) {
            if (InfluenceBar.size < 0.5f) { finalDecision = AllLines[3]; }
            else if (InfluenceBar.size >= 0.5f) { finalDecision = AllLines[2]; }
        } else if (Bill == 1) {
            if (InfluenceBar.size < 0.5f) { finalDecision = AllLines[41]; }
            else if (InfluenceBar.size >= 0.5f) { finalDecision = AllLines[42]; }
        }

        ResponseText[0].text = "";
        ResponseText[1].text = "";
        ResponseText[2].text = "";
        ResponseText[3].text = "";

        MainText.text = "The Final outcome has been decided, the Senate has voted in favour of: " + finalDecision + "\n\nPress any key to Restart";
        ResponseText[0].GetComponentInParent<Button>().interactable = false;
        ResponseText[1].GetComponentInParent<Button>().interactable = false;
        ResponseText[2].GetComponentInParent<Button>().interactable = false;
        ResponseText[3].GetComponentInParent<Button>().interactable = false;

        //any key press
        if (Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
