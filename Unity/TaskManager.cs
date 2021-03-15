/** タスクを管理するスクリプト **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    /// テスト用タスクを追加したいときは下のenumのEND前にタスク名を付ける
    public enum Task
    {
        BasicLookForFood,
        BehindWall,
        Ymaze,
        Detour,
        Tmaze,
        UseRamp,
        Escape,
        RadialMaze,
        ToolUse,
        END,
        Training,
    }
    public Task task;
    public bool trainingMode = false;
    LevelManager envTask;
    // Start is called before the first frame update
    void Start()
    {
        ChooseTask();
        //ResetArena();
    }
    void ChooseTask()
    {
        Transform taskGameObj = transform.Find("Task").transform;
        if (!trainingMode)
        {
            /// テスト用タスクを追加したいときは下のIF文にタスク名を追加する.そして，エディターの方に同名のgame objectをTaskオブジェクトの下に置く必要がある
            if (task == Task.BasicLookForFood)
            {
                envTask = taskGameObj.Find("BasicFindFood").gameObject.GetComponent<BasicFindFood>();
            }
            else if (task == Task.BehindWall)
            {
                envTask = taskGameObj.Find("BehindWall").gameObject.GetComponent<BehindWall>();
            }
            else if (task == Task.Ymaze)
            {
                envTask = taskGameObj.Find("Ymaze").gameObject.GetComponent<Ymaze>();
            }
            else if (task == Task.Detour)
            {
                envTask = taskGameObj.Find("Detour").gameObject.GetComponent<Detour>();
            }
            else if (task == Task.Tmaze)
            {
                envTask = taskGameObj.Find("Tmaze").gameObject.GetComponent<Tmaze>();
            }
            else if (task == Task.UseRamp)
            {
                envTask = taskGameObj.Find("UseRamp").gameObject.GetComponent<UseRamp>();
            }
            else if (task == Task.Escape)
            {
                envTask = taskGameObj.Find("Escape").gameObject.GetComponent<Escape>();
            }
            else if (task == Task.RadialMaze)
            {
                envTask = taskGameObj.Find("RadialMaze").gameObject.GetComponent<RadialMaze>();
            }
            else if (task == Task.ToolUse)
            {
                envTask = taskGameObj.Find("ToolUse").gameObject.GetComponent<ToolUse>();
            }
        }
        else
        {
            task = Task.Training;
            envTask = taskGameObj.Find("TraningScene").gameObject.GetComponent<TraningScene>();
        }

    }
    public void ResetArena()
    {
        envTask.ClearObjs();
        envTask.PlaceOtherObjs();
    }
    public float GetBaseReawrd()
    {
        return envTask.GetbaseReward();
    }
    public string GetTaskName()
    {
        return task.ToString();
    }
    public void GoNextTask()
    {
        envTask.ClearObjs();
        if (task != Task.END)
            task++;
        ChooseTask();
    }
    public void GoNextLevel()
    {
        envTask.GoNextLevel();
    }
    public void AutoChangeTask()
    {
        if (task != Task.Training)
        {
            if (envTask.GetCurrentLevel() == envTask.GetLevelNumbers())
            {
                GoNextTask();
            }
        }
        Debug.Log("GetCurrentLevel/GetLevelNumbers:" + (envTask.GetCurrentLevel() + 1) + "/" + envTask.GetLevelNumbers());
    }
    public void AutoChangeLevel()
    {
        envTask.AutoChangeLevel();
    }
}
