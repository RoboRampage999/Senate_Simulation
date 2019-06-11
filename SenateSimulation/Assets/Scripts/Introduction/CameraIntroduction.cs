using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIntroduction : MonoBehaviour
{
    bool start = false;
    public bool freeCamera = false;
    Vector3 target = new Vector3(0.0f, 1.9f, 6.7f);

    // Use this for initialization
    void Start()
    {
        this.gameObject.transform.position = new Vector3(0.5f, 1.9f, -36.0f);
        this.gameObject.transform.LookAt(new Vector3(0.0f, 1.9f, -8.7f));
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.05f);

            if ((freeCamera == true) && (this.transform.position == target))
            {
                this.GetComponent<CameraScript>().enabled = true;
                GameObject.FindObjectOfType<Canvas>().GetComponentInChildren<UIScript>().SendMessage("BeginResponse");
                this.GetComponent<CameraIntroduction>().enabled = false;
            }
        }
    }

    void CameraStart() { start = true; }
    void FreeCamera() { freeCamera = true; }
}
