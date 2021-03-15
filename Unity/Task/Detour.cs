using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detour : LevelManager
{
    public enum LevelType
    {
        eShortWall1,
        eShortWall2,
        eSemiT_Wall1,
        eSemiT_Wall2,
        e2SemiT_Wall1,
        eDeadZone1,
        e2DeadZone1,
        END,
    }
    public LevelType levelType;
    public GameObject wall;
    public GameObject tWall;
    public GameObject deadZone;
    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eShortWall1)
        {
            ShortWall1();
            levelTimes++;
        }
        else if (levelType == LevelType.eShortWall2)
        {
            ShortWall2();
            levelTimes++;
        }
        else if (levelType == LevelType.eSemiT_Wall1)
        {
            SemiT_Wall1();
            levelTimes++;
        }
        else if (levelType == LevelType.eSemiT_Wall2)
        {
            SemiT_Wall2();
            levelTimes++;
        }
        else if (levelType == LevelType.e2SemiT_Wall1)
        {
            TwoSemiT_Wall1();
            levelTimes++;
        }
        else if (levelType == LevelType.eDeadZone1)
        {
            DeadZone1();
            levelTimes++;
        }
        else if (levelType == LevelType.e2DeadZone1)
        {
            TwoDeadZone1();
            levelTimes++;
        }
    }
    public override int GetCurrentLevel()
    {
        return (int)levelType;
    }
    public override int GetLevelNumbers()
    {
        return (int)LevelType.END;
    }
    public override void SetCurrenLevel()
    {
        levelType++;

    }
    //只有一面矮牆壁
    private void ShortWall1()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 10f), 0.3f, 0.2f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -9, 9, -9, -13) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -9f, 9f, 9f, 14f) + new Vector3(0, 0.5f, 0);

        //other
        //牆壁
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-wallSize.x / 2, -0.5f, -2);
    }
    //矮牆壁左右再隨機生成一面矮牆
    private void ShortWall2()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 10f), 0.3f, 0.2f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -9, 9, -9, -13) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -9f, 9f, 9f, 14f) + new Vector3(0, 0.5f, 0);

        //other
        //牆壁
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-wallSize.x / 2, -0.5f, -2);
        //左或右牆壁
        float[,] wall2Pos = { { 1f, 0 }, //左邊
                                { wallSize.x-1, 0} }; //右邊
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = ChooseRandomFixedPosition(wall1.transform.position, wall2Pos);
        wall2.transform.rotation = Quaternion.Euler(0, -90f, 0);
    }
    private void SemiT_Wall1()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 10f), 3f, 0.2f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -9, 9, -9, -13) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -9f, 9f, 9f, 14f) + new Vector3(0, 0.5f, 0);

        //other
        //牆壁
        GameObject wall1 = Instantiate(tWall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-wallSize.x / 2, -0.5f, -2);
    }
    private void SemiT_Wall2()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 10f), 3f, 0.2f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -9, 9, -9, -13) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -9f, 9f, 9f, 14f) + new Vector3(0, 0.5f, 0);

        //other
        //牆壁
        GameObject wall1 = Instantiate(tWall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-wallSize.x / 2, -0.5f, -2);
        //左或右牆壁
        float[,] wall2Pos = { { 1f, 0 }, //左邊
                                { wallSize.x-1, 0} }; //右邊
        GameObject wall2 = Instantiate(tWall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = ChooseRandomFixedPosition(wall1.transform.position, wall2Pos);
        wall2.transform.rotation = Quaternion.Euler(0, -90f, 0);
    }
    private void TwoSemiT_Wall1()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 8f), 3f, 0.2f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, 0f, 0f, -11, -15) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, 0f, 0f, 7.5f, -10f) + new Vector3(0, 0.5f, 0);

        //other
        //左牆壁
        GameObject wall1 = Instantiate(tWall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-0.3f, -0.5f, -1);
        wall1.transform.rotation = Quaternion.Euler(0, -135f, 0);
        //右牆壁
        GameObject wall2 = Instantiate(tWall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = wall1.transform.position + new Vector3(0.3f, 0, 0);
        wall2.transform.rotation = Quaternion.Euler(0, -45f, 0);
    }
    private void DeadZone1()
    {
        Vector3 deadZoneSize = new Vector3(Random.Range(4, 10f), 0.1f, Random.Range(0.2f, 1f));
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -9, 9, -9, -13) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -9f, 9f, 9f, 14f) + new Vector3(0, 0.5f, 0);

        //other
        //Lava
        GameObject deadZone1 = Instantiate(deadZone) as GameObject;
        objsList.Add(deadZone1);
        deadZone1.transform.localScale = deadZoneSize;
        deadZone1.transform.position = targetFood.transform.position + new Vector3(-deadZoneSize.x / 2, -0.5f, -1.5f);
    }
    private void TwoDeadZone1()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 8f), 0.1f, Random.Range(0.2f, 1f));
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, 0f, 0f, -11, -15) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, 0f, 0f, 7.5f, -10f) + new Vector3(0, 0.5f, 0);

        //other
        //左牆壁
        GameObject deadZone1 = Instantiate(deadZone) as GameObject;
        objsList.Add(deadZone1);
        deadZone1.transform.localScale = wallSize;
        deadZone1.transform.position = targetFood.transform.position + new Vector3(-0.3f, -0.5f, -1.5f);
        deadZone1.transform.rotation = Quaternion.Euler(0, -135f, 0);
        //右牆壁
        GameObject deadZone2 = Instantiate(deadZone) as GameObject;
        objsList.Add(deadZone2);
        deadZone2.transform.localScale = wallSize;
        deadZone2.transform.position = targetFood.transform.position + new Vector3(0.3f, -0.5f, -2f);
        deadZone2.transform.rotation = Quaternion.Euler(0, -45f, 0);

    }
}
