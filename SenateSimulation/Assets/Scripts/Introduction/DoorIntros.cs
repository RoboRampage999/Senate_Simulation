using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIntros : MonoBehaviour
{

    public Transform DoorR, DoorL, Camera;
    bool start = false;

    private void Start()
    {
        Camera = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true)
        {
            if ((Camera.position.z >= -16) && (Camera.position.z < -6))
            {
                DoorR.RotateAround(new Vector3(2.026f, 1.78f, -7.889f), new Vector3(0.0f, 1.0f, 0.0f), 0.39f);
                DoorL.RotateAround(new Vector3(-2.026f, 1.78f, -7.889f), new Vector3(0.0f, 1.0f, 0.0f), -0.39f);
            }
            else if ((Camera.position.z >= -6) && (Camera.position.z < 4))
            {
                DoorR.RotateAround(new Vector3(2.026f, 1.78f, -7.889f), new Vector3(0.0f, 1.0f, 0.0f), -0.39f);
                DoorL.RotateAround(new Vector3(-2.026f, 1.78f, -7.889f), new Vector3(0.0f, 1.0f, 0.0f), 0.39f);
            }

            if (Camera.position.z > 4)
            {
                this.GetComponent<DoorIntros>().enabled = false;
            }
        }
    }

    void DoorStart() { start = true; }
}
