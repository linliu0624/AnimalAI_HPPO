using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject agentBackCamera;
    // private GameObject agentCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        agentBackCamera = GameObject.Find("TraceBackCam");
        // agentCamera = GameObject.Find("AgentCam");
        // agentCamera.SetActive(true);
        mainCamera.SetActive(true);
        agentBackCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.SetActive(true);
            agentBackCamera.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainCamera.SetActive(false);
            agentBackCamera.SetActive(true);
        }
    }
}
