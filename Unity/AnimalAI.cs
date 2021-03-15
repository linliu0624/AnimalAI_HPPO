/** ML AGENTSの核心 **/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
public class AnimalAI : Agent
{
    public bool humanPlay = false;
    public bool autoRun = true;
    //public int maxStep = 3000;
    [Tooltip("How fast the agent crawl moves forward")]
    public float moveSpeed = 5f;
    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 180f;
    public RayScanner Scanner;
    public Text showReward;
    public Text showTaskName;
    public Text showPassOrNot;
    public Text showPass;
    public Text showFail;
    TaskManager tmArena;
    [HideInInspector] public int passTime = 0;
    private int totalTime = -1;
    private bool isPass;
    new Rigidbody rigidbody;
    [HideInInspector] public List<GameObject> foodsList;

    private Vector3 nowPos;
    private Vector3 beforePos;
    private float distance;

    float cumulativeReward = 0;
    public GameObject subTarget;
    private Vector3 subTargetPos;
    /// <summary>
    /// Initial setup, called when the agent is enabled
    /// </summary>
    public override void Initialize()
    {
        tmArena = GetComponentInParent<TaskManager>();
        rigidbody = GetComponent<Rigidbody>();
        showFail.enabled = false;
        showPass.enabled = false;
        subTargetPos = Vector3.zero;
    }
    /// <summary>
    /// Reset the agent and area
    /// </summary>
    public override void OnEpisodeBegin()
    {
        showFail.enabled = false;
        showPass.enabled = false;
        totalTime++;
        Debug.Log("pass/total: " + passTime + "/" + totalTime);
        // テストモードの時に, このステージやタスクが終わったら, 自動的に次のステージやタスクに入る
        if (autoRun)
        {
            tmArena.AutoChangeLevel();
            tmArena.AutoChangeTask();
        }
        tmArena.ResetArena();
        distance = 0;
        nowPos = transform.position;
        beforePos = transform.position;
    }
    /// <summary>
    /// Perform actions based on a vector of numbers
    /// </summary>
    /// <param name="vectorAction">The list of actions to take</param>
    public override void OnActionReceived(float[] vectorAction)
    {
        Move(vectorAction);
        // Apply a tiny negative reward every step to encourage action
        ReuduceRewardByTime();
    }
    /// <summary>
    /// Collect all non-Raycast observations
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        // agentの座標, 向きと部分目標との内積を収集し, pythonで使う
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(GetDotWithTarget());
    }

    private void FixedUpdate()
    {
        ShowInfo();
        // GetRewardByDistance();
        if (StepCount % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }
        if (humanPlay)
        {
            // R-key押せばエピソードを終了する
            if (Input.GetKeyDown(KeyCode.R))
            {
                EndEpisode();
            }
            // N-key押せば次のタスクに移動する
            if (Input.GetKeyDown(KeyCode.N))
            {
                tmArena.GoNextTask();
                totalTime--;
                EndEpisode();
            }
            // M-key押せば次のステージに移動する
            if (Input.GetKeyDown(KeyCode.M))
            {
                tmArena.GoNextLevel();
                totalTime--;
                EndEpisode();
            }
        }
        // Agentがフィールド外に行ったらエピソードを終了する
        OutField();
    }
    /// <summary>
    /// When the agent collides with something, take action
    /// </summary>
    /// <param name="collision">The collision info</param>
    private void OnCollisionEnter(Collision collision)
    {
        /** unityの衝突判定のところ **/
        if (collision.transform.CompareTag("TargetFood"))
        {
            TakeTarget();
        }
        else if (collision.transform.CompareTag("Food"))
        {
            TakeFood(collision);
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            TouchEnemy();
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        // HotZoneにいれば，報酬を計算する
        if (collision.transform.CompareTag("HotZone"))
        {
            TouchHotZone();
        }
    }
    public override void Heuristic(float[] actionsOut)
    {
        // 環境のテストするとき, 人工でagentを操作することができる
        actionsOut[0] = 0;
        //actionsOut[1] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 3f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 4f;
        }

    }
    /** agnetの移動 **/
    public void Move(float[] act)
    {
        // Convert the first action to forward movement
        float forwardAmount = 0f;
        // Convert the second action to turning left or right
        float turnAmount = 0f;

        if (act[0] == 1f)
        {
            forwardAmount = 1f;
            rigidbody.AddForce(transform.forward * moveSpeed * forwardAmount * Time.fixedDeltaTime * 3.5f, ForceMode.VelocityChange);
        }
        else if (act[0] == 2f)
        {
            forwardAmount = -1f;
            rigidbody.AddForce(transform.forward * moveSpeed * 0.6f * forwardAmount * Time.fixedDeltaTime * 3.5f, ForceMode.VelocityChange);
            // rigidbody.AddForce(transform.forward * moveSpeed * forwardAmount * Time.fixedDeltaTime * 3.5f, ForceMode.VelocityChange);
        }
        else if (act[0] == 3f)
        {
            turnAmount = -1f;
        }
        else if (act[0] == 4f)
        {
            turnAmount = 1f;
        }

        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        // 部分目標を取得する
        subTargetPos = new Vector3(act[1], act[2], act[3]);
        subTarget.transform.position = subTargetPos;
    }
    // stepごとに報酬を減る
    public void ReuduceRewardByTime()
    {
        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
            cumulativeReward += -1f / MaxStep;
        }
    }
    // ターゲットにタッチしたら
    public void TakeTarget()
    {
        AddReward(1f);
        cumulativeReward += 1;
        //ShowPassOrNot();
        //passTime++;
        float reward = GetCumulativeReward();
        if (reward >= tmArena.GetBaseReawrd()) passTime++;
        EndEpisode();
    }
    // ボーナスにタッチしたら
    public void TakeFood(Collision collision)
    {
        AddReward(0.5f);
        cumulativeReward += 0.5f;
        foodsList.Remove(collision.gameObject);
        Destroy(collision.gameObject, 0);
    }
    // 敵にタッチしたら
    public void TouchEnemy()
    {
        AddReward(-1f);
        cumulativeReward += -1;
        EndEpisode();
    }
    // hotzoneにタッチしたら
    public void TouchHotZone()
    {
        AddReward(-0.01f);
        cumulativeReward += -0.01f;
    }
    // agentの移動距離で報酬を計算する(今は使ってない)
    public void GetRewardByDistance()
    {
        //今のagent座標を得る
        nowPos = transform.position;
        if (StepCount % 100 == 0)
        {
            distance = Vector3.Distance(nowPos, beforePos);
            // Debug.Log(StepCount + " distance: " + distance);
            if (distance > 1)
            {
                AddReward(distance * 0.001f);
                cumulativeReward += distance * 0.001f;
            }
            else
            {
                AddReward(-0.01f);
                cumulativeReward += -0.01f;
            }
        }
        // 100ステップ以降の座標を得る
        if (StepCount % 100 == 0 || StepCount == 1)
        {
            beforePos = transform.position;
        }
    }

    public void ShowInfo()
    {
        float reward = GetCumulativeReward();
        showReward.text = "Reward:" + reward.ToString("0.000");
        // showReward.text = "Reward:" + cumulativeReward.ToString("0.000");
        showTaskName.text = "Task Name:" + tmArena.GetTaskName();
        showPassOrNot.text = "passtime/totalTime:" + passTime + "/" + totalTime;

        showReward.enabled = humanPlay == true ? true : false;
        showTaskName.enabled = humanPlay == true ? true : false;
        showPassOrNot.enabled = humanPlay == true ? true : false;
    }
    public void ShowPassOrNot()
    {
        if (humanPlay)
        {
            float reward = GetCumulativeReward();
            if (reward >= tmArena.GetBaseReawrd())
            {
                showPass.enabled = true;
                //StartCoroutine(Coroutine());
            }
            else
            {
                showFail.enabled = true;
                //StartCoroutine(Coroutine());
            }
        }
    }

    // agnetがフィールドの外に行ったら終了する
    public void OutField()
    {
        if (transform.position.y < -5 || transform.position.y > 5)
        {
            EndEpisode();
        }
    }

    // agnetの向きと部分目標座標の内積を計算する
    private float GetDotWithTarget(){
        Vector3 forward = transform.forward;
        Vector3 toOther = (subTargetPos - transform.position).normalized;
        float dot = Vector3.Dot(forward, toOther);
        Debug.Log(toOther);
        Debug.Log(Vector3.Dot(forward, toOther));
        // if (Vector3.Dot(forward, toOther) < 0)
        // {
        //     Debug.Log("The other transform is behind me!");
        // }
        return dot;
    }
}
