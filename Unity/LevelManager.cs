/** ステージを管理するスクリプト **/
/** level = stage **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{
    public GameObject arena;
    public AnimalAI agent;
    public GameObject targetFood;
    [HideInInspector] public bool isOnlyFood = false;
    protected float baseReward = 0;
    protected int levelTimes = 1;
    protected List<GameObject> objsList;
    protected Quaternion noRotation = Quaternion.Euler(0f, 0f, 0f);
    private int maxRoopTimes = 10;
    public abstract void PlaceOtherObjs();
    public abstract int GetCurrentLevel();
    public abstract int GetLevelNumbers();
    public abstract void SetCurrenLevel();

    public void GoNextLevel()
    {
        ClearObjs();
        SetCurrenLevel();
        Debug.Log("GoNextLevel");
    }
    public void AutoChangeLevel()
    {
        if (levelTimes > maxRoopTimes)
        {
            levelTimes = 1;
            SetCurrenLevel();
        }
        Debug.Log("levelTimes/maxRoopTimes:" + (levelTimes) + "/" + maxRoopTimes);

    }
    /// フィールド内のすべての要素や物体を削除する
    public void ClearObjs()
    {
        if (objsList != null)
        {
            for (int i = 0; i < objsList.Count; i++)
            {
                Destroy(objsList[i]);
            }
        }
        if (agent.foodsList != null)
        {
            for (int i = 0; i < agent.foodsList.Count; i++)
            {
                Destroy(agent.foodsList[i]);
            }
        }
        agent.foodsList = new List<GameObject>();
        objsList = new List<GameObject>();
    }

    public float GetbaseReward()
    {
        return baseReward;
    }
    /// 座標配列を入力すれば，配列にある座標でランダムに生成する
    public Vector3 ChooseRandomFixedPosition(Vector3 center, float[,] posX_Z)
    {
        int rand = Random.Range(0, posX_Z.Length / 2);
        float positionX = posX_Z[rand, 0];
        float positionZ = posX_Z[rand, 1];

        return center + new Vector3(positionX, 0, positionZ);
    }
    /// 機能は上記と同じ，Y軸の生成を追加下だけです.ただし，Y軸は乱数なし.
    public Vector3 ChooseRandomFixedPosition(Vector3 center, float[,] posX_Z, float posY)
    {
        int rand = Random.Range(0, posX_Z.Length / 2);
        float positionX = posX_Z[rand, 0];
        float positionZ = posX_Z[rand, 1];

        return center + new Vector3(positionX, posY, positionZ);
    }
    /// 円形範囲内にランダムの位置に生成する
    public Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;
        float angle = minAngle;

        if (maxRadius > minRadius)
        {
            // Pick a random radius
            radius = Random.Range(minRadius, maxRadius);
        }

        if (maxAngle > minAngle)
        {
            // Pick a random angle
            angle = Random.Range(minAngle, maxAngle);
        }

        // Center position + forward vector rotated around the Y axis by "angle" degrees, multiplies by "radius"
        return center + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * radius;
    }
    /// 方形範囲内にランダムの位置に生成する
    public Vector3 ChooseRandomRectPosition(Vector3 center, float left, float right, float forward, float back)
    {
        float positionX = Random.Range(center.x + left, center.x + right);
        float positionZ = Random.Range(center.z + back, center.z + forward);
        return new Vector3(positionX, 0, positionZ);
    }

}
