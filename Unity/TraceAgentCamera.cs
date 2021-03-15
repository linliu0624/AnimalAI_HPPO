using UnityEngine;

public class TraceAgentCamera : MonoBehaviour
{   
    // dist：與攝像機之間的距離
    public float dist = 3f;
    // height ： 設定攝像機的高度
	public float height = 2f;
    // dampTrace ： 實現平滑追蹤的變數
	public float dampTrace = 20.0f;
    GameObject agent;
    Vector3 myPos;
    Transform agentTransform;
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.transform.parent.gameObject;
        agentTransform = agent.GetComponent<Transform>();
        myPos = new Vector3(0, -2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 agentPos = agentTransform.position;
        transform.LookAt(agentPos);
        transform.position = Vector3.Lerp (transform.position, agentPos - (agentTransform.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);
    }
}
