using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ymaze : LevelManager
{
    public enum LevelType
    {
        eOnlyHightFloor,
        eOnlyHightFloor2,
        eOnlyWall,
        eOnlyWall2,
        eWallAndHightFloor,
        eOnlyLava,
        eLavaWithWall,
        eLavaWithWall2,
        END,

    }
    public LevelType levelType;
    public float targetPositionX = 3.5f;
    public float targetPositionZ = 11f;
    public GameObject hightFloor;
    public GameObject wall;
    public GameObject hotZone;

    public override void PlaceOtherObjs()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (levelType == LevelType.eOnlyHightFloor)
        {
            OnlyHightFloor();
            levelTimes++;
        }
        else if (levelType == LevelType.eOnlyHightFloor2)
        {
            OnlyHightFloor2();
            levelTimes++;
        }
        else if (levelType == LevelType.eOnlyWall)
        {
            OnlyWall();
            levelTimes++;
        }
        else if (levelType == LevelType.eOnlyWall2)
        {
            OnlyWall2();
            levelTimes++;
        }
        else if (levelType == LevelType.eWallAndHightFloor)
        {
            WallAndHightFloor();
            levelTimes++;
        }
        else if (levelType == LevelType.eOnlyLava)
        {
            OnlyLava();
            levelTimes++;
        }
        else if (levelType == LevelType.eLavaWithWall)
        {
            LavaWithWall();
            levelTimes++;
        }
        else if (levelType == LevelType.eLavaWithWall2)
        {
            LavaWithWall2();
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
    private void LavaWithWall2()
    {
        Vector3 centerPos = transform.position;
        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(Random.Range(-5, 5f), 0.5f, Random.Range(12f, 14f));
        //maze
        GameObject lava = Instantiate(hotZone) as GameObject;
        objsList.Add(lava);
        lava.transform.position = targetFood.transform.position + new Vector3(-7.5f, -0.5f, -20f);
        lava.transform.localScale = new Vector3(15, 0.1f, 15);

        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = lava.transform.position + new Vector3(0, 0, 0);
        wall1.transform.rotation = Quaternion.Euler(0f, -90, 0);
        wall1.transform.localScale = new Vector3(15, 3, 0.2f);
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = lava.transform.position + new Vector3(lava.transform.localScale.x, 0, 0);
        wall2.transform.rotation = Quaternion.Euler(0f, -90, 0);
        wall2.transform.localScale = new Vector3(15, 3, 0.2f);

    }

    private void LavaWithWall()
    {
        Vector3 centerPos = transform.position;
        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(0, 0.5f, Random.Range(12f, 14f));
        //maze
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = targetFood.transform.position + new Vector3(0.5f, -0.5f, -18f);
        wall1.transform.rotation = Quaternion.Euler(0f, -90, 0);
        wall1.transform.localScale = new Vector3(15, 3, 1);

        float[,] lavaPos = { { -15, 0 }, { 0, 0 } };
        GameObject lava = Instantiate(hotZone) as GameObject;
        objsList.Add(lava);
        lava.transform.position = ChooseRandomFixedPosition(wall1.transform.position, lavaPos);
        lava.transform.localScale = new Vector3(15, 0.1f, 15);
    }

    private void OnlyLava()
    {
        Vector3 centerPos = transform.position;
        float[,] targetPos = { { -7.5f, 13 }, { 0, 13 }, { 7.5f, 13 } };
        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(centerPos, targetPos) + new Vector3(0, 0.5f, 0);
        //maze
        GameObject lava = Instantiate(hotZone) as GameObject;
        objsList.Add(lava);
        lava.transform.position = targetFood.transform.position + new Vector3(-7.5f, -0.5f, -20);
        lava.transform.localScale = new Vector3(15, 0.1f, 15);
    }

    private void OnlyHightFloor()
    {
        //Target Food 
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -14, 14, 5, 9f) + new Vector3(0, 0.5f, 0);
        while (targetFood.transform.position.x >= -10 && targetFood.transform.position.x <= 10)
            targetFood.transform.position = ChooseRandomRectPosition(transform.position, -14, 14, 5, 9f) + new Vector3(0, 0.5f, 0);
        //maze
        Vector3 centerPos = transform.position;
        GameObject floor = Instantiate(hightFloor) as GameObject;
        objsList.Add(floor);
        floor.transform.position = centerPos + new Vector3(-0.5f, 0f, 15f);
        floor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        floor.transform.localScale = new Vector3(30f, 1f, 1f);
    }
    private void OnlyHightFloor2()
    {
        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -14, 14, 5, 9f) + new Vector3(0, 0.5f, 0);
        while (targetFood.transform.position.x >= -10 && targetFood.transform.position.x <= 10)
            targetFood.transform.position = ChooseRandomRectPosition(transform.position, -14, 14, 5, 9f) + new Vector3(0, 0.5f, 0);
        //maze
        Vector3 centerPos = transform.position;
        GameObject floor = Instantiate(hightFloor) as GameObject;
        objsList.Add(floor);
        floor.transform.position = centerPos + new Vector3(-0.25f, 0f, 15f);
        floor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        floor.transform.localScale = new Vector3(30f, 1f, 0.5f);
    }
    private void OnlyWall()
    {
        //maze
        Vector3 centerPos = transform.position;
        //橫牆壁
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = centerPos + new Vector3(-15f, 0f, 10);
        wall1.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        wall1.transform.localScale = new Vector3(30f, 3, 0.2f);
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = centerPos + new Vector3(-10, 0f, wall1.transform.position.z);
        wall2.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        wall2.transform.localScale = new Vector3(15, 3, 0.2f);
        GameObject wall3 = Instantiate(wall) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = centerPos + new Vector3(10, 0f, wall1.transform.position.z);
        wall3.transform.rotation = Quaternion.Euler(0f, 135f, 0f);
        wall3.transform.localScale = new Vector3(15, 3, 0.2f);

        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(
            centerPos, -14, 14, wall1.transform.position.z - 1, wall1.transform.position.z - 5) + new Vector3(0, 0.5f, 0);
        while (targetFood.transform.position.x > wall2.transform.position.x && targetFood.transform.position.x < wall3.transform.position.x)
            targetFood.transform.position = ChooseRandomRectPosition(
                centerPos, -14, 14, wall1.transform.position.z - 1, wall1.transform.position.z - 5) + new Vector3(0, 0.5f, 0);
    }
    private void OnlyWall2()
    {
        //maze
        Vector3 centerPos = transform.position;
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = centerPos + new Vector3(0, 0f, -10);
        wall1.transform.rotation = Quaternion.Euler(0f, -100f, 0f);
        wall1.transform.localScale = new Vector3(25, 3, 0.2f);
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = centerPos + new Vector3(0, 0f, -10);
        wall2.transform.rotation = Quaternion.Euler(0f, -80f, 0f);
        wall2.transform.localScale = new Vector3(25, 3, 0.2f);

        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(
            transform.position, -14, 14, targetPositionZ, targetPositionZ) + new Vector3(0, 0.5f, 0);
        while (targetFood.transform.position.x > -7.5f && targetFood.transform.position.x < 7.5f)
            targetFood.transform.position = ChooseRandomRectPosition(
                transform.position, -14, 14, targetPositionZ, targetPositionZ) + new Vector3(0, 0.5f, 0);

    }
    private void WallAndHightFloor()
    {
        //Target Food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, 8, 14f) + new Vector3(0, 0.5f, 0);
        while (targetFood.transform.position.x >= -2 && targetFood.transform.position.x <= 2)
            targetFood.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, 8, 14f) + new Vector3(0, 0.5f, 0);

        //maze
        Vector3 centerPos = transform.position;
        GameObject floor = Instantiate(hightFloor) as GameObject;
        objsList.Add(floor);
        floor.transform.position = centerPos + transform.position + new Vector3(-1f, 0f, 5f);
        floor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        floor.transform.localScale = new Vector3(20f, 1f, 2f);

        //分割地圖的牆壁
        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = centerPos + new Vector3(1f, 0f, 5f);
        wall1.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        wall1.transform.localScale = new Vector3(10f, 3, 2f);
        //高台左邊牆壁
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = centerPos + new Vector3(floor.transform.position.x - 0.2f, 0f, 0);//0.2為自己的寬度
        wall2.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        wall2.transform.localScale = new Vector3(15f, 3, 0.2f);
        //高台右邊牆壁
        GameObject wall3 = Instantiate(wall) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = centerPos + new Vector3(floor.transform.position.x + 2f, 0f, 0);//1高台的寬度
        wall3.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        wall3.transform.localScale = new Vector3(15f, 3, 0.2f);
        //高台左邊斜牆壁
        GameObject wall4 = Instantiate(wall) as GameObject;
        objsList.Add(wall4);
        wall4.transform.position = centerPos + new Vector3(wall2.transform.position.x, 0f, 0f);
        wall4.transform.rotation = Quaternion.Euler(0f, -135f, 0f);
        wall4.transform.localScale = new Vector3(20f, 3, 0.2f);
        //高台右邊斜牆壁
        GameObject wall5 = Instantiate(wall) as GameObject;
        objsList.Add(wall5);
        wall5.transform.position = centerPos + new Vector3(wall3.transform.position.x + 0.2f, 0f, 0f);
        wall5.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
        wall5.transform.localScale = new Vector3(20f, 3, 0.2f);
    }
}
