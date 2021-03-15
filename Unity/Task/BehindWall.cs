using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehindWall : LevelManager
{
    public enum LevelType
    {
        eBehindWall1,
        eBehindWall2,
        eBehindThreeWall1,
        eBehindThreeWall2,
        eYamzeLike1,
        END,
    }
    public LevelType levelType;
    public GameObject wallPrefab;
    public GameObject hightLandPrefab;
    public GameObject housePrefab;
    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eBehindWall1)
        {
            BehindWall1();
        }
        else if (levelType == LevelType.eBehindWall2)
        {
            BehindWall2();
        }
        else if (levelType == LevelType.eBehindThreeWall1)
        {
            BehindThreeWall1();
        }
        else if (levelType == LevelType.eBehindThreeWall2)
        {
            BehindThreeWall2();
        }
        else if (levelType == LevelType.eYamzeLike1)
        {
            YmazeLike1();
        }
        levelTimes++;
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
    private void YmazeLike1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1.3f, -14);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //target food
        float[,] targetPos = { { -7.5f, 0 }, { 7.5f, 0 } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //wall
        GameObject wall = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall);
        wall.transform.position = transform.position + new Vector3(-0.1f, 0, -12);
        wall.transform.localScale = new Vector3(0.2f, 3, 27);
        //hight floor
        GameObject hightLand = Instantiate(hightLandPrefab) as GameObject;
        objsList.Add(hightLand);
        hightLand.transform.position = transform.position + new Vector3(-1.5f, 0f, -15f);
        hightLand.transform.localScale = new Vector3(3f, 1f, 3f);

        //house
        GameObject house = Instantiate(housePrefab) as GameObject;
        objsList.Add(house);
        house.transform.position = targetFood.transform.position + new Vector3(0, -0.5f, 0);
    }
    //隨機生成目標在左上或右上, 一面牆大小隨機的壁擋住
    private void BehindWall1()
    {
        Vector3 wallSize = new Vector3(Random.Range(4, 9f), 3, 0.2f);
        float[,] targetPos = { { -13, 13 }, { 13, 13 } };

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
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);
        targetFood.transform.rotation = noRotation;
        //wall
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        if (targetFood.transform.position.x > 0)
            wall1.transform.position = targetFood.transform.position + new Vector3(15 - wallSize.x - targetFood.transform.position.x, -0.5f, -3);
        else
            wall1.transform.position = targetFood.transform.position + new Vector3(-15f - targetFood.transform.position.x, -0.5f, -3);
        wall1.transform.rotation = noRotation;
        wall1.transform.localScale = wallSize;
    }
    private void BehindWall2()
    {
        Vector3 wallSize = new Vector3(Random.Range(10, 20f), 3, 0.2f);

        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -5, 5, -11, -14) + new Vector3(0, 0.5f, 0);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = transform.position + new Vector3(0, 0.5f, Random.Range(-7.5f, 10f));
        targetFood.transform.rotation = noRotation;
        //wall
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = targetFood.transform.position + new Vector3(-wallSize.x / 2, -0.5f, -3f);
        wall1.transform.rotation = Quaternion.Euler(0, Random.Range(-12.5f, 12.5f), 0);
    }

    //隨機生成一個篇或偏右的目標, 
    private void BehindThreeWall1()
    {
        Vector3 shortWallSize = new Vector3(5, 3, 0.2f);
        Vector3 longWallSize = new Vector3(Random.Range(4, 10f), 3, 0.2f);
        float[,] targetPos = { { 2.5f, -3 }, { -2.5f, -3 } };

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
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);
        targetFood.transform.rotation = noRotation;

        //others
        //短牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = targetFood.transform.position + new Vector3(-3, -0.5f, 0);
        wall1.transform.rotation = Quaternion.Euler(0, 45f, 0);
        wall1.transform.localScale = shortWallSize;
        //左長牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = wall1.transform.position;
        wall2.transform.rotation = Quaternion.Euler(0, -45f, 0);
        wall2.transform.localScale = longWallSize;
        //右長牆壁
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = targetFood.transform.position + new Vector3(0, -0.5f, -3f);
        wall3.transform.rotation = Quaternion.Euler(0, -45f, 0);
        wall3.transform.localScale = wall2.transform.localScale;

    }
    private void BehindThreeWall2()
    {
        Vector3 shortWallSize = new Vector3(5, 3, 0.2f);
        Vector3 longWallSize = new Vector3(Random.Range(4, 10f), 3, 0.2f);
        float[,] targetPos = { { 2.5f, -3 }, { -2.5f, -3 } };

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
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);
        targetFood.transform.rotation = noRotation;

        //others
        //短牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = targetFood.transform.position + new Vector3(3, -0.5f, 0);
        wall1.transform.rotation = Quaternion.Euler(0, 135f, 0);
        wall1.transform.localScale = shortWallSize;
        //左長牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = wall1.transform.position;
        wall2.transform.rotation = Quaternion.Euler(0, -135f, 0);
        wall2.transform.localScale = longWallSize;
        //右長牆壁
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = targetFood.transform.position + new Vector3(0, -0.5f, -3f);
        wall3.transform.rotation = Quaternion.Euler(0, -135f, 0);
        wall3.transform.localScale = wall2.transform.localScale;

    }
}
