using System;
using UnityEngine;

public class ToolUse : LevelManager
{
    public enum LevelType
    {
        eBoxHideWay1,
        eBoxPushTarget1,
        eBoxPushTarget2,
        eBoxPushTarget3,
        eBoxPushTarget4,
        END,
    }
    public LevelType levelType;
    public GameObject boxPrefab;
    public GameObject wallPrefab;
    public GameObject daedZonePrefab;
    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eBoxHideWay1)
        {
            BoxHideWay1();
        }
        else if (levelType == LevelType.eBoxPushTarget1)
        {
            BoxPushTarget1();
        }
        else if (levelType == LevelType.eBoxPushTarget2)
        {
            BoxPushTarget2();
        }
        else if (levelType == LevelType.eBoxPushTarget3)
        {
            BoxPushTarget3();
        }
        else if (levelType == LevelType.eBoxPushTarget4)
        {
            BoxPushTarget4();
        }
        levelTimes++;
    }

    private void BoxPushTarget4()
    {
        Vector3 wallSize = new Vector3(0.2f, 1f, 0.5f);
        //agent
        float[,] agentPos = { { 10, 0 }, { -10, 0 } };
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomFixedPosition(transform.position, agentPos);
        if (agent.transform.position.x > 0) agent.transform.rotation = Quaternion.Euler(0, -90, 0);
        else agent.transform.rotation = Quaternion.Euler(0, 90, 0);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(0, 0.5f, 0);

        //other
        //Dead Zone
        GameObject deadZone = Instantiate(daedZonePrefab) as GameObject;
        objsList.Add(deadZone);
        deadZone.transform.position = transform.position + new Vector3(-5f, 0, -2.5f);
        deadZone.transform.localScale = new Vector3(10, 0.1f, 5);
        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = transform.position + new Vector3(-0.75f, 0, -8f);
        box.transform.localScale = new Vector3(1.5f, 0.4f, 7);
    }

    private void BoxPushTarget3()
    {
        Vector3 wallSize = new Vector3(0.2f, 1f, 0.5f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = new Vector3(0, 0.5f, -10);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(0, 0.5f, 0);

        //other
        //Dead Zone
        GameObject deadZone = Instantiate(daedZonePrefab) as GameObject;
        objsList.Add(deadZone);
        deadZone.transform.position = transform.position + new Vector3(-5f, 0, -2.5f);
        deadZone.transform.localScale = new Vector3(10, 0.1f, 5);
        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = transform.position + new Vector3(-0.75f, 0, -8f);
        box.transform.localScale = new Vector3(1.5f, 0.4f, 7);
    }

    private void BoxPushTarget2()
    {
        Vector3 wallSize = new Vector3(0.2f, 1f, 0.5f);
        //agent
        float[,] agentPos = { { 10, 0 }, { -10, 0 } };
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomFixedPosition(transform.position, agentPos);
        if (agent.transform.position.x > 0) agent.transform.rotation = Quaternion.Euler(0, -90, 0);
        else agent.transform.rotation = Quaternion.Euler(0, 90, 0);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(0, 0.5f, 0);

        //other
        //Dead Zone
        GameObject deadZone = Instantiate(daedZonePrefab) as GameObject;
        objsList.Add(deadZone);
        deadZone.transform.position = transform.position + new Vector3(-5f, 0, -2.5f);
        deadZone.transform.localScale = new Vector3(10, 0.1f, 5);
        //牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = transform.position + new Vector3(-1, 0, -5.5f);
        //牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = transform.position + new Vector3(1, 0, -5.5f);
        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = transform.position + new Vector3(-0.75f, 0, -8f);
        box.transform.localScale = new Vector3(1.5f, 0.4f, 7);
    }

    private void BoxPushTarget1()
    {
        Vector3 wallSize = new Vector3(0.2f, 1f, 0.5f);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = new Vector3(0, 0.5f, -10);
        agent.transform.rotation = noRotation;

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(0, 0.5f, 0);

        //other
        //Dead Zone
        GameObject deadZone = Instantiate(daedZonePrefab) as GameObject;
        objsList.Add(deadZone);
        deadZone.transform.position = transform.position + new Vector3(-5f, 0, -2.5f);
        deadZone.transform.localScale = new Vector3(10, 0.1f, 5);
        //牆壁
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = transform.position + new Vector3(-1, 0, -5.5f);
        //牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = transform.position + new Vector3(1, 0, -5.5f);
        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = transform.position + new Vector3(-0.75f, 0, -8f);
        box.transform.localScale = new Vector3(1.5f, 0.4f, 7);
    }

    private void BoxHideWay1()
    {
        agent.MaxStep = 1000;
        Vector3 wallSize = new Vector3(13.5f, 3f, 10f);
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
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = wallSize;
        wall1.transform.position = transform.position + new Vector3(-15, 0, -5);
        //牆壁
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = wallSize;
        wall2.transform.position = transform.position + new Vector3(1.5f, 0, -5);
        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = transform.position + new Vector3(-3, 0, -6.5f);
        box.transform.localScale = new Vector3(7, 0.8f, 1);

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
}
