using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : LevelManager
{
    public enum LevelType
    {
        eRoomEscape1,
        eRoomEscape2,
        eMazeEscape1,
        eMazeEscape2,
        eMazeEscapeWithBox1,
        END,
    }
    public LevelType levelType;
    public GameObject tWallPrefab;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
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
    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eRoomEscape1)
        {
            RoomEscape1();
        }
        else if (levelType == LevelType.eRoomEscape2)
        {
            RoomEscape2();
        }
        else if (levelType == LevelType.eMazeEscape1)
        {
            MazeEscape1();
        }
        else if (levelType == LevelType.eMazeEscape2)
        {
            MazeEscape2();
        }
        else if (levelType == LevelType.eMazeEscapeWithBox1)
        {
            MazeEscapeWithBox1();
        }
        levelTimes++;
    }

    private void MazeEscape2()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 0, -2);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(13, 0.5f, -3);

        //maze
        //內側左邊牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall1.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall1.transform.localScale = new Vector3(8, 3, 0.2f);
        //內側後方牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall2.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall2.transform.localScale = new Vector3(5f, 3, 0.2f);
        //內側右方牆壁
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = transform.position + new Vector3(2.5f, 0, -3f);
        wall3.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall3.transform.localScale = new Vector3(6f, 3, 0.2f);
        //前方牆壁
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.position = transform.position + new Vector3(-2.5f, 0, 5f);
        wall4.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall4.transform.localScale = new Vector3(8f, 3, 0.2f);
        //外側右方斜牆壁
        GameObject wall5 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall5);
        wall5.transform.position = transform.position + new Vector3(5f, 0, 5f);
        wall5.transform.rotation = Quaternion.Euler(0, 45, 0);
        wall5.transform.localScale = new Vector3(8f, 3, 0.2f);
        //內外中間右方斜牆壁
        GameObject wall6 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall6);
        wall6.transform.position = transform.position + new Vector3(5f, 0, 3f);
        wall6.transform.rotation = Quaternion.Euler(0, 45, 0);
        wall6.transform.localScale = new Vector3(8f, 3, 0.2f);
        //橫短牆壁
        GameObject wall7 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall7);
        wall7.transform.position = transform.position + new Vector3(2.5f, 0, 3f);
        wall7.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall7.transform.localScale = new Vector3(2.5f, 3, 0.2f);
    }

    private void MazeEscapeWithBox1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 0, -2);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(13, 0.5f, -3);

        //maze
        //內側左邊牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall1.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall1.transform.localScale = new Vector3(8, 3, 0.2f);
        //內側後方牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall2.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall2.transform.localScale = new Vector3(5f, 3, 0.2f);
        //內側右方牆壁
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = transform.position + new Vector3(2.5f, 0, -3f);
        wall3.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall3.transform.localScale = new Vector3(6f, 3, 0.2f);

        //box
        Vector3 boxSize = new Vector3(1, 1, 1);
        GameObject box1 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box1);
        box1.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box1.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box2 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box2);
        box2.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box2.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box3 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box3);
        box3.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box3.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;

        box1.transform.position = transform.position + new Vector3(2f, 0, 3.3f);
        box2.transform.position = transform.position + new Vector3(2f, 1f, 3.3f);
        box3.transform.position = transform.position + new Vector3(2f, 2f, 3.3f);
        box1.transform.localScale = boxSize;
        box2.transform.localScale = boxSize;
        box3.transform.localScale = boxSize;

        //前方牆壁
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.position = transform.position + new Vector3(-2.5f, 0, 5f);
        wall4.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall4.transform.localScale = new Vector3(8f, 3, 0.2f);
        //外側右方斜牆壁
        GameObject wall5 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall5);
        wall5.transform.position = transform.position + new Vector3(5f, 0, 5f);
        wall5.transform.rotation = Quaternion.Euler(0, 30, 0);
        wall5.transform.localScale = new Vector3(8f, 3, 0.2f);
        //內外中間右方斜牆壁
        GameObject wall6 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall6);
        wall6.transform.position = transform.position + new Vector3(5f, 0, 3f);
        wall6.transform.rotation = Quaternion.Euler(0, 60, 0);
        wall6.transform.localScale = new Vector3(8f, 3, 0.2f);
        //橫短牆壁
        GameObject wall7 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall7);
        wall7.transform.position = transform.position + new Vector3(2.5f, 0, 3f);
        wall7.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall7.transform.localScale = new Vector3(2.5f, 3, 0.2f);
    }

    private void MazeEscape1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 0, -2);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(13, 0.5f, -3);

        //maze
        //內側左邊牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall1.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall1.transform.localScale = new Vector3(8, 3, 0.2f);
        //內側後方牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = transform.position + new Vector3(-2.5f, 0, -3f);
        wall2.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall2.transform.localScale = new Vector3(5f, 3, 0.2f);
        //內側右方牆壁
        GameObject wall3 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.position = transform.position + new Vector3(2.5f, 0, -3f);
        wall3.transform.rotation = Quaternion.Euler(0, -90, 0);
        wall3.transform.localScale = new Vector3(6f, 3, 0.2f);
        //前方牆壁
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.position = transform.position + new Vector3(-2.5f, 0, 5f);
        wall4.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall4.transform.localScale = new Vector3(8f, 3, 0.2f);
        //外側右方斜牆壁
        GameObject wall5 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall5);
        wall5.transform.position = transform.position + new Vector3(5f, 0, 5f);
        wall5.transform.rotation = Quaternion.Euler(0, 45, 0);
        wall5.transform.localScale = new Vector3(8f, 3, 0.2f);
        //內外中間右方斜牆壁
        GameObject wall6 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall6);
        wall6.transform.position = transform.position + new Vector3(5f, 0, 3f);
        wall6.transform.rotation = Quaternion.Euler(0, 45, 0);
        wall6.transform.localScale = new Vector3(8f, 3, 0.2f);
        //橫短牆壁
        GameObject wall7 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall7);
        wall7.transform.position = transform.position + new Vector3(2.5f, 0, 3f);
        wall7.transform.rotation = Quaternion.Euler(0, 0, 0);
        wall7.transform.localScale = new Vector3(2.5f, 3, 0.2f);
    }

    private void RoomEscape1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + Vector3.zero;
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //target food
        float[,] targetPos = { { -7.5f, 2.5f }, { 7.5f, -2.5f }, { 7.5f, 2.5f }, { -7.5f, -2.5f } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //other
        //room
        Vector3 wallSize = new Vector3(5, 3, 0.2f);
        //左邊牆壁
        GameObject tWall1 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall1);
        tWall1.transform.position = transform.position + new Vector3(-2.5f, 0, 2.5f);
        tWall1.transform.rotation = Quaternion.Euler(0, 90, 0);
        tWall1.transform.localScale = wallSize;

        //右邊牆壁
        GameObject tWall2 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall2);
        tWall2.transform.position = transform.position + new Vector3(2.5f, 0, 2.5f);
        tWall2.transform.rotation = Quaternion.Euler(0, 90, 0);
        tWall2.transform.localScale = wallSize;

        //box
        //下面的box
        Vector3 boxSize = new Vector3(4.5f, 0.5f, 0.5f);
        GameObject box1 = Instantiate(boxPrefab) as GameObject;
        box1.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box1.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box2 = Instantiate(boxPrefab) as GameObject;
        box2.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box2.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box3 = Instantiate(boxPrefab) as GameObject;
        box3.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box3.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        objsList.Add(box1);
        objsList.Add(box2);
        objsList.Add(box3);
        box1.transform.localScale = boxSize;
        box2.transform.localScale = boxSize;
        box3.transform.localScale = boxSize;
        box1.transform.position = transform.position + new Vector3(-2.25f, 0, -2.5f);
        box2.transform.position = transform.position + new Vector3(-2.25f, 0.5f, -2.5f);
        box3.transform.position = transform.position + new Vector3(-2.25f, 1, -2.5f);

        //上面的box
        GameObject box4 = Instantiate(boxPrefab) as GameObject;
        box4.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box4.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box5 = Instantiate(boxPrefab) as GameObject;
        box5.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box5.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box6 = Instantiate(boxPrefab) as GameObject;
        box6.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box6.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        objsList.Add(box4);
        objsList.Add(box5);
        objsList.Add(box6);
        box4.transform.localScale = boxSize;
        box5.transform.localScale = boxSize;
        box6.transform.localScale = boxSize;
        box4.transform.position = transform.position + new Vector3(-2.25f, 0, 2.5f);
        box5.transform.position = transform.position + new Vector3(-2.25f, 0.5f, 2.5f);
        box6.transform.position = transform.position + new Vector3(-2.25f, 1, 2.5f);
    }

    private void RoomEscape2()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + Vector3.zero;
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        //target food
        float[,] targetPos = { { -7.5f, 7.5f }, { 7.5f, -7.5f }, { 7.5f, 7.5f }, { -7.5f, -7.5f } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //other
        //room
        Vector3 wallSize = new Vector3(5, 3, 0.2f);
        //左邊牆壁
        GameObject tWall1 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall1);
        tWall1.transform.position = transform.position + new Vector3(-2.5f, 0, 2.5f);
        tWall1.transform.rotation = Quaternion.Euler(0, 90, 0);
        tWall1.transform.localScale = wallSize;
        //右邊牆壁
        GameObject tWall2 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall2);
        tWall2.transform.position = transform.position + new Vector3(2.5f, 0, 2.5f);
        tWall2.transform.rotation = Quaternion.Euler(0, 90, 0);
        tWall2.transform.localScale = wallSize;
        //上下牆壁
        GameObject tWall3 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall3);
        tWall3.transform.localScale = wallSize;
        if (targetFood.transform.position.z > 0)
        {
            tWall3.transform.position = transform.position + new Vector3(-2.5f, 0, -2.5f);
        }
        else
        {
            tWall3.transform.position = transform.position + new Vector3(-2.5f, 0, 2.5f);
        }
        //兩片小牆壁
        Vector3 tinyWallSize = new Vector3(1, 3, 0.2f);
        GameObject tWall4 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall4);
        GameObject tWall5 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(tWall5);
        tWall4.transform.localScale = tinyWallSize;
        tWall5.transform.localScale = tinyWallSize;
        if (targetFood.transform.position.z > 0)
        {
            tWall4.transform.position = transform.position + new Vector3(-2.5f, 0, 2.5f);
            tWall5.transform.position = transform.position + new Vector3(2.5f - 1, 0, 2.5f);
        }
        else
        {
            tWall4.transform.position = transform.position + new Vector3(-2.5f, 0, -2.5f);
            tWall5.transform.position = transform.position + new Vector3(2.5f - 1, 0, -2.5f);
        }
        //box
        Vector3 boxSize = new Vector3(2.8f, 0.5f, 0.5f);
        GameObject box1 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box1);
        box1.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box1.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box2 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box2);
        box2.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box2.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        GameObject box3 = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box3);
        box3.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box3.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;

        box1.transform.localScale = boxSize;
        box2.transform.localScale = boxSize;
        box3.transform.localScale = boxSize;
        if (targetFood.transform.position.z > 0)
        {
            box1.transform.position = transform.position + new Vector3(-1.5f, 0, 2.5f);
            box2.transform.position = transform.position + new Vector3(-1.5f, 0.5f, 2.5f);
            box3.transform.position = transform.position + new Vector3(-1.5f, 1, 2.5f);
        }
        else
        {
            box1.transform.position = transform.position + new Vector3(-1.5f, 0, -2.5f);
            box2.transform.position = transform.position + new Vector3(-1.5f, 0.5f, -2.5f);
            box3.transform.position = transform.position + new Vector3(-1.5f, 1, -2.5f);
        }
    }
}
