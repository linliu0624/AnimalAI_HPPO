using UnityEngine;
using System.Collections.Generic;
public class TraningScene : LevelManager
{
    public GameObject wallPrefab;
    public GameObject tWallPrefab;
    public GameObject rampPrefab;
    public GameObject hightLandPrefab;
    public GameObject deadZonePrefab;
    public GameObject enemyPrefab;
    public GameObject hotZonePrefab;
    public GameObject foodPrefab;
    public GameObject boxPrefab;

    public enum LevelType
    {
        eFoodAndTargetFood1 = 0,
        eFoodAndTargetFood2 = 1,
        eFoodAndEnemy1 = 2,
        eShortWallAndDeadZone1 = 3,
        eShortWallAndDeadZone2 = 4,
        eShortWallAndSemiT_Wall1 = 5,
        eWallAndSemiT_Wall1 = 6,
        eHotZoneAndDeadZone1 = 7,
        eHotZoneAndEnemy1 = 8,
        eRampAndHighLand1 = 9,
        eRampAndHighLand2 = 10,
        ePushBox1 = 11,
        ePushBox2 = 12,
        eWallAndSemiT_Wall_Foods1 = 13,
        END,
        eTraningIV,
        eTrainIV_basic,
        eTrainCtrler_basic,
        eTrainCtrler_basic2,
    }
    public bool fixLevel = false;
    public LevelType levelType;
    public bool selectedRandomLevel = false;
    public int[] levels;
    private Vector3 centerPos;
    private Vector3 foodHeight = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        centerPos = transform.position;
    }
    public override void PlaceOtherObjs()
    {
        //隨機選擇關卡
        if (!fixLevel && !selectedRandomLevel)
        {
            int levelNum = Random.Range(0, (int)LevelType.END);
            levelType = (LevelType)levelNum;
        }
        //限定隨機選擇關卡
        if (selectedRandomLevel)
        {
            int levelNum = Random.Range(0, levels.Length);
            levelType = (LevelType)levels[levelNum];
        }

        // agent.MaxStep = Random.Range(1000, 1500);
        // agent.MaxStep = Random.Range(2000000 * 3, 3000000 * 3);
        // agent.MaxStep = Random.Range(2000 * 3, 3000 * 3);
        if (levelType == LevelType.eFoodAndTargetFood1)
        {
            FoodAndTargetFood1();
        }
        else if (levelType == LevelType.eFoodAndTargetFood2)
        {
            FoodAndTargetFood2();
        }
        else if (levelType == LevelType.eFoodAndEnemy1)
        {
            FoodAndEnemy1();
        }
        else if (levelType == LevelType.eShortWallAndDeadZone1)
        {
            ShortWallAndDeadZone1();
        }
        else if (levelType == LevelType.eShortWallAndDeadZone2)
        {
            ShortWallAndDeadZone2();
        }
        else if (levelType == LevelType.eHotZoneAndDeadZone1)
        {
            HotZoneAndDeadZone1();
        }
        else if (levelType == LevelType.eHotZoneAndEnemy1)
        {
            HotZoneAndEnemy1();
        }
        else if (levelType == LevelType.eShortWallAndSemiT_Wall1)
        {
            ShorWallAndSemiT_Wall1();
        }
        else if (levelType == LevelType.eWallAndSemiT_Wall1)
        {
            WallAndSemiT_Wall1();
        }
        else if (levelType == LevelType.eRampAndHighLand1)
        {
            RampAndHighLand1();
        }
        else if (levelType == LevelType.eRampAndHighLand2)
        {
            RampAndHighLand2();
        }
        else if (levelType == LevelType.ePushBox1)
        {
            PushBox1();
        }
        else if (levelType == LevelType.ePushBox2)
        {
            PushBox2();
        }
        else if (levelType == LevelType.eWallAndSemiT_Wall_Foods1)
        {
            WallAndSemiT_Wall_Foods1();
        }
        else if (levelType == LevelType.eTraningIV)
        {
            TrainingIV();
        }
        else if (levelType == LevelType.eTrainIV_basic)
        {
            TrainIVBasic();
        }
        else if (levelType == LevelType.eTrainCtrler_basic)
        {
            TrainCtrler_basic();
        }
        else if (levelType == LevelType.eTrainCtrler_basic2)
        {
            TrainCtrler_basic2();
        }
    }

    private void TrainCtrler_basic2()
    {
        // agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -5, 5, 5, -5);
        //agent.transform.position = new Vector3(0, 1, 0) + centerPos;
        // agent.transform.rotation = Quaternion.Euler(0, 0, 0);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);

        // target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        // targetFood.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-8, 0)) + foodHeight;
        // targetFood.transform.localScale *= 3;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + foodHeight;
    }

    private void TrainCtrler_basic()
    {
        // agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        //agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 0, -10);
        agent.transform.position = new Vector3(0, 1, 0) + centerPos;
        agent.transform.rotation = Quaternion.Euler(0, 0, 0);

        // target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        // targetFood.transform.position = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 0)) + foodHeight;
        targetFood.transform.position = transform.position + new Vector3(2, 0, 10) + foodHeight;
        // targetFood.transform.position = transform.position + new Vector3(2, 0, -10) + foodHeight;
    }
    private void TrainIVBasic()
    {
        agent.moveSpeed = 0;
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = new Vector3(0, 1, 0) + centerPos;
        agent.transform.rotation = Quaternion.Euler(0, 0, 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(-0.5f, 0, 3.5f) + foodHeight;

    }

    private void TrainingIV()
    {
        //agent
        // float[,] agentPos = { { 0, 0 }, { 14, 14 }, { -14, 14 }, { 14, -14 }, { 0, -14 }, { -14, -14 } };
        float[,] agentPos = { { 5, 5 }, { -5, -10 }, { -10, 5 }, { -10, -10 }, { -8, 8 }, { -12, 12 } };
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomFixedPosition(centerPos, agentPos);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + new Vector3(0, 0.5f, 0);

        int foodnum = Random.Range(10, 20);
        if (foodnum > 0)
        {
            for (int i = 0; i < foodnum; i++)
            {
                GameObject food = Instantiate(foodPrefab) as GameObject;
                objsList.Add(food);
                food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + new Vector3(0, 0.5f, 0);
            }
        }
    }

    private void WallAndSemiT_Wall_Foods1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 2, -1);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        float[,] targetPos = { { 14, 14 }, { 0, 14 }, { -14, 14 }, { 14, -14 }, { 0, -14 }, { -14, -14 } };
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(centerPos, targetPos) + foodHeight;

        //foods
        int foodsNum = Random.Range(4, 10);
        for (int i = 0; i < foodsNum; i++)
        {
            int pattern = Random.Range(0, 2);

            GameObject food = Instantiate(foodPrefab) as GameObject;
            objsList.Add(food);
            if (pattern == 0)
            {
                food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14) + foodHeight;
            }
            else
            {
                food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;
            }
        }

        //semiT wall
        int semiT_WallNum = Random.Range(3, 5);
        for (int i = 0; i < semiT_WallNum; i++)
        {
            GameObject tWall = Instantiate(tWallPrefab) as GameObject;
            objsList.Add(tWall);
            tWall.transform.position = ChooseRandomRectPosition(centerPos, -14, 9, 10, 5);
            tWall.transform.rotation = Quaternion.Euler(0, Random.Range(-5, 5f), 0);
            tWall.transform.localScale = new Vector3(Random.Range(3, 6f), 3, Random.Range(0.2f, 2));
        }

        //wall
        int wallNum = Random.Range(3, 5);
        for (int i = 0; i < wallNum; i++)
        {
            GameObject Wall = Instantiate(wallPrefab) as GameObject;
            objsList.Add(Wall);
            Wall.transform.position = ChooseRandomRectPosition(centerPos, -14, 9, -5, -10);
            Wall.transform.rotation = Quaternion.Euler(0, Random.Range(-5, 5f), 0);
            Wall.transform.localScale = new Vector3(Random.Range(3, 6f), 3, Random.Range(0.2f, 2));
        }
    }

    private void PushBox2()
    {

        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = centerPos + new Vector3(Random.Range(-5, 5), 0, -2.5f);
        agent.transform.rotation = Quaternion.Euler(0, 0, 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = foodHeight + centerPos + new Vector3(0, 0, 2.5f);

        //box
        GameObject box = Instantiate(boxPrefab) as GameObject;
        objsList.Add(box);
        box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
        box.transform.position = centerPos + new Vector3(-10, 0, -0.25f);
        box.transform.localScale = new Vector3(20, 0.5f, 0.5f);

    }

    private void PushBox1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = centerPos;
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        float[,] targetPos = { { 14, 14 }, { 14, -14 }, { -14, 14 }, { -14, -14 } };
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = foodHeight + ChooseRandomFixedPosition(centerPos, targetPos);

        //box
        for (int i = 0; i < 6; i++)
        {
            GameObject box = Instantiate(boxPrefab) as GameObject;
            objsList.Add(box);
            box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
            box.transform.position = centerPos + new Vector3(-3.2f + i, 0, -2);
            box.transform.localScale = new Vector3(1, 1.5f, 1);
        }
        for (int i = 0; i < 6; i++)
        {
            GameObject box = Instantiate(boxPrefab) as GameObject;
            objsList.Add(box);
            box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
            box.transform.position = centerPos + new Vector3(-3.2f + i, 0, 2);
            box.transform.localScale = new Vector3(1, 1.5f, 1);
        }
        for (int i = 0; i < 6; i++)
        {
            GameObject box = Instantiate(boxPrefab) as GameObject;
            objsList.Add(box);
            box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
            box.transform.position = centerPos + new Vector3(-3.2f, 0, -2 + i);
            box.transform.localScale = new Vector3(1, 1.5f, 1);
        }
        for (int i = 0; i < 6; i++)
        {
            GameObject box = Instantiate(boxPrefab) as GameObject;
            objsList.Add(box);
            box.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
            box.GetComponentInChildren<Rigidbody>().angularVelocity = Vector3.zero;
            box.transform.position = centerPos + new Vector3(3.2f, 0, -2 + i);
            box.transform.localScale = new Vector3(1, 1.5f, 1);
        }
        //food
        int foodNum = Random.Range(15, 30);
        for (int i = 0; i < foodNum; i++)
        {
            GameObject food = Instantiate(foodPrefab) as GameObject;
            objsList.Add(food);
            while (food.transform.position.x < 4 && food.transform.position.x > -4 && food.transform.position.z < 4 && food.transform.position.z > -4)
                food.transform.position = ChooseRandomRectPosition(centerPos, -13, 13, 13, -13) + foodHeight;
        }
        // //box and food
        // int boxNum = Random.Range(10, 20);
        // for (int i = 0; i < boxNum; i++)
        // {
        //     GameObject box = Instantiate(boxPrefab) as GameObject;
        //     objsList.Add(box);
        //     box.transform.position = ChooseRandomRectPosition(centerPos, -6, 6, 6, -6);
        //     while (box.transform.position.x < 3 && box.transform.position.x > -3 &&
        //            box.transform.position.z < 3 && box.transform.position.z > -3)
        //         box.transform.position = ChooseRandomRectPosition(centerPos, -6, 6, 6, -6);
        //     box.transform.localScale = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.5f, 2f), Random.Range(0.5f, 2f));
        //     GameObject food = Instantiate(foodPrefab) as GameObject;
        //     objsList.Add(food);
        //     food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14);
        // }
    }

    private void RampAndHighLand2()
    {
        agent.MaxStep = Random.Range(1000, 2000);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -12, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        float[,] hightLandPos = { { -14, -5f }, { -7.5f, -5f }, { -1f, -5f }, { 5.5f, -5f }, { 11, -5f } };
        for (int i = 0; i < 5; i++)
        {
            //hight land
            GameObject hightLand = Instantiate(hightLandPrefab) as GameObject;
            objsList.Add(hightLand);
            hightLand.transform.position = centerPos + new Vector3(hightLandPos[i, 0], 0, hightLandPos[i, 1]);
            hightLand.transform.localScale = new Vector3(Random.Range(1.5f, 3.5f), 0.5f, 21.5f);
            //ramp
            GameObject ramp = Instantiate(rampPrefab) as GameObject;
            objsList.Add(ramp);
            ramp.transform.position = centerPos + new Vector3(hightLandPos[i, 0], hightLand.transform.localScale.y, hightLandPos[i, 1]);
            ramp.transform.rotation = Quaternion.Euler(0, 90, -Random.Range(5, 25f));
            ramp.transform.localScale = new Vector3(3, 0.1f, hightLand.transform.localScale.x);
            //food 
            GameObject food = Instantiate(foodPrefab) as GameObject;
            objsList.Add(food);
            food.transform.position = foodHeight + hightLand.transform.position + new Vector3(1, 0, Random.Range(5, 10));
            //bridge
            if (i != 4)
            {
                GameObject bridge = Instantiate(hightLandPrefab) as GameObject;
                objsList.Add(bridge);
                bridge.transform.position = hightLand.transform.position + new Vector3(hightLand.transform.localScale.x, 0, Random.Range(5, 20));
                bridge.transform.localScale = new Vector3(7 - hightLand.transform.localScale.x, hightLand.transform.localScale.y, Random.Range(0.2f, 0.8f));
            }
        }
        //dead zone
        GameObject deadZone = Instantiate(deadZonePrefab) as GameObject;
        objsList.Add(deadZone);
        deadZone.transform.position = new Vector3(-14.5f, 0, -5) + centerPos;
        deadZone.transform.localScale = new Vector3(30, 0.1f, 22f);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(centerPos, hightLandPos) + new Vector3(1, 0, Random.Range(10, 18)) + foodHeight;

    }

    private void RampAndHighLand1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = centerPos + new Vector3(-14, 0, 14) + foodHeight;

        //hight land
        int hightLnadNum = Random.Range(5, 11);
        float firstPosX = Random.Range(-14f, -10);
        float firstPosZ = Random.Range(6f, 10);
        for (int i = 0; i < hightLnadNum; i++)
        {
            float hightLandSize = Random.Range(2f, 5);
            GameObject hightLand = Instantiate(hightLandPrefab) as GameObject;
            objsList.Add(hightLand);
            hightLand.transform.position = centerPos + new Vector3(firstPosX, 0, firstPosZ);
            hightLand.transform.localScale = new Vector3(hightLandSize, 1, hightLandSize);
            firstPosX += Random.Range(5f, 8);
            if (firstPosX > 10)
            {
                firstPosX = Random.Range(-14f, 10);
                firstPosZ -= Random.Range(5f, 8);
            }
            if (firstPosZ < -5)
            {
                firstPosZ = -5;
            }
            //ramp
            GameObject ramp = Instantiate(rampPrefab) as GameObject;
            objsList.Add(ramp);
            ramp.transform.position = hightLand.transform.position + new Vector3(0, 1, 0);
            ramp.transform.rotation = Quaternion.Euler(0, 90, -Random.Range(10, 30f));
            ramp.transform.localScale = new Vector3(5, 0.1f, Random.Range(2, 5));

            //food
            GameObject food = Instantiate(foodPrefab) as GameObject;
            objsList.Add(food);
            food.transform.position = hightLand.transform.position +
                    new Vector3(hightLand.transform.localScale.x / 2, hightLand.transform.localScale.y, hightLand.transform.localScale.z / 2) + foodHeight;

        }
    }

    private void FoodAndEnemy1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -14, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = centerPos + new Vector3(-14, 0, 14) + foodHeight;
        //food
        int foodNum = Random.Range(15, 30);
        for (int i = 0; i < foodNum; i++)
        {
            GameObject food = Instantiate(foodPrefab) as GameObject;
            objsList.Add(food);
            food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -9) + foodHeight;
        }
        //enemy
        int enemyNum = Random.Range(5, 15);
        for (int i = 0; i < enemyNum; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab) as GameObject;
            objsList.Add(enemy);
            enemy.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -12) + foodHeight;
        }
    }

    private void WallAndSemiT_Wall1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 2, -1);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        float[,] targetPos = { { 14, 14 }, { 0, 14 }, { -14, 14 }, { 14, -14 }, { 0, -14 }, { -14, -14 } };
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(centerPos, targetPos) + foodHeight;

        //foods
        // int foodsNum = Random.Range(2, 5);
        // for (int i = 0; i < foodsNum; i++)
        // {
        //     GameObject food = Instantiate(foodPrefab) as GameObject;
        //     objsList.Add(food);
        //     food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + foodHeight;
        // }

        //semiT wall
        int semiT_WallNum = Random.Range(1, 4);
        for (int i = 0; i < semiT_WallNum; i++)
        {
            GameObject tWall = Instantiate(tWallPrefab) as GameObject;
            objsList.Add(tWall);
            tWall.transform.position = ChooseRandomRectPosition(centerPos, -14, 9, i + 5, i + 5);
            tWall.transform.rotation = Quaternion.Euler(0, Random.Range(-5, 5f), 0);
            tWall.transform.localScale = new Vector3(Random.Range(8, 16f), 3, Random.Range(0.2f, 2));
        }

        //wall
        int wallNum = Random.Range(3, 5);
        for (int i = 0; i < wallNum; i++)
        {
            GameObject Wall = Instantiate(wallPrefab) as GameObject;
            objsList.Add(Wall);
            Wall.transform.position = ChooseRandomRectPosition(centerPos, -14, 9, -5, -10);
            Wall.transform.rotation = Quaternion.Euler(0, Random.Range(-5, 5f), 0);
            Wall.transform.localScale = new Vector3(Random.Range(3, 6f), 3, Random.Range(0.2f, 2));
        }
    }

    private void ShorWallAndSemiT_Wall1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;

        //other
        //short wall
        int wallNum = Random.Range(1, 5);
        for (int i = 0; i < wallNum; i++)
        {
            GameObject shortWall = Instantiate(wallPrefab) as GameObject;
            objsList.Add(shortWall);
            shortWall.transform.position = ChooseRandomRectPosition(centerPos, -14, 8, 0, -9);
            shortWall.transform.localScale = new Vector3(Random.Range(2, 8f), Random.Range(0.2f, 0.7f), Random.Range(0.2f, 3));
        }
        //semi-T_ wall
        int semiT_WallNum = Random.Range(1, 4);
        for (int i = 0; i < semiT_WallNum; i++)
        {
            GameObject semiT_Wall = Instantiate(tWallPrefab) as GameObject;
            objsList.Add(semiT_Wall);
            semiT_Wall.transform.position = ChooseRandomRectPosition(centerPos, -14, 8, i + 2, i + 2);
            semiT_Wall.transform.localScale = new Vector3(Random.Range(8, 16f), 3, Random.Range(0.2f, 1));
        }
        GameObject semiT_Wall1 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(semiT_Wall1);
        semiT_Wall1.transform.position = centerPos + new Vector3(-15, 0, 0);
        semiT_Wall1.transform.localScale = new Vector3(Random.Range(2, 3f), 3, Random.Range(0.2f, 1));
        GameObject semiT_Wall2 = Instantiate(tWallPrefab) as GameObject;
        objsList.Add(semiT_Wall2);
        semiT_Wall2.transform.localScale = new Vector3(Random.Range(2, 3f), 3, Random.Range(0.2f, 1));
        semiT_Wall2.transform.position = centerPos + new Vector3(15 - semiT_Wall2.transform.localScale.x, 0, 0);
    }

    private void HotZoneAndEnemy1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -14, -14);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 14) + foodHeight;

        //others
        //hot zone
        int hotZoneNum = Random.Range(3, 6);
        for (int i = 0; i < hotZoneNum; i++)
        {
            GameObject hotZone = Instantiate(hotZonePrefab) as GameObject;
            objsList.Add(hotZone);
            hotZone.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 9, -9);
            hotZone.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
            hotZone.transform.localScale = new Vector3(Random.Range(4, 8), 0.1f, Random.Range(3, 7));
        }
        //enemy
        int deadZoneNum = Random.Range(5, 15);
        for (int i = 0; i < deadZoneNum; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab) as GameObject;
            objsList.Add(enemy);
            enemy.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 12, -12) + foodHeight;
        }
    }

    private void HotZoneAndDeadZone1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;

        //others
        //hot zone 
        int hotZoneNum = Random.Range(1, 4);
        for (int i = 0; i <= hotZoneNum; i++)
        {
            GameObject hotZone = Instantiate(hotZonePrefab) as GameObject;
            objsList.Add(hotZone);
            hotZone.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 9, 0);
            hotZone.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
            hotZone.transform.localScale = new Vector3(Random.Range(4, 8), 0.1f, Random.Range(3, 7));
        }
        //dead zone
        int deadZoneNum = Random.Range(1, 4);
        for (int i = 0; i < deadZoneNum; i++)
        {
            GameObject deadZone = Instantiate(deadZonePrefab) as GameObject;
            objsList.Add(deadZone);
            deadZone.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 0, -9);
            deadZone.transform.localScale = new Vector3(Random.Range(1, 8), 0.1f, Random.Range(0.2f, 8));
            deadZone.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
        }
    }

    private void ShortWallAndDeadZone2()
    {
        int partern = Random.Range(0, 2);
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        //set agent and food pos
        if (partern == 0)
        {
            agent.transform.position = centerPos + new Vector3(14, 0, -14);
            targetFood.transform.position = centerPos + new Vector3(-14, 0, 14) + foodHeight;
        }
        else
        {
            agent.transform.position = centerPos + new Vector3(-14, 0, -14);
            targetFood.transform.position = centerPos + new Vector3(14, 0, 14) + foodHeight;
        }
        //others
        //short wall
        int wallNum = Random.Range(3, 6);
        for (int i = 0; i < wallNum; i++)
        {
            GameObject shortWall = Instantiate(wallPrefab) as GameObject;
            objsList.Add(shortWall);
            shortWall.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 9, -9);
            shortWall.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
            shortWall.transform.localScale = new Vector3(Random.Range(4, 8), Random.Range(0.25f, 0.7f), Random.Range(2, 5));
        }
        //dead zone
        int deadZoneNum = Random.Range(3, 6);
        for (int i = 0; i < deadZoneNum; i++)
        {
            GameObject deadZone = Instantiate(deadZonePrefab) as GameObject;
            objsList.Add(deadZone);
            deadZone.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 4, -9);
            deadZone.transform.localScale = new Vector3(Random.Range(4, 8), 0.1f, Random.Range(4, 8));
            deadZone.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
        }
    }

    //區域限制生成矮牆和死亡區
    private void ShortWallAndDeadZone1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;

        //others
        //short wall
        int wallNum = Random.Range(1, 4);
        for (int i = 0; i < wallNum; i++)
        {
            GameObject shortWall = Instantiate(wallPrefab) as GameObject;
            objsList.Add(shortWall);
            shortWall.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 9, -9);
            shortWall.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
            shortWall.transform.localScale = new Vector3(Random.Range(1, 8), Random.Range(0.25f, 0.7f), Random.Range(0.2f, 5));
        }
        //dead zone
        int deadZoneNum = Random.Range(1, 4);
        for (int i = 0; i < deadZoneNum; i++)
        {
            GameObject deadZone = Instantiate(deadZonePrefab) as GameObject;
            objsList.Add(deadZone);
            deadZone.transform.position = ChooseRandomRectPosition(centerPos, -14, 5, 4, -9);
            deadZone.transform.localScale = new Vector3(Random.Range(1, 8), 0.1f, Random.Range(0.2f, 8));
            deadZone.transform.rotation = Quaternion.Euler(0, Random.Range(-10, 10), 0);
        }
    }

    //區域限制生成
    private void FoodAndTargetFood2()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, -10, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;

        int foodnum = Random.Range(0, 5);
        if (foodnum > 0)
        {
            for (int i = 0; i < foodnum; i++)
            {
                GameObject food = Instantiate(foodPrefab) as GameObject;
                objsList.Add(food);
                food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, 10) + foodHeight;
            }
        }
    }

    //全地圖隨機生成
    private void FoodAndTargetFood1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14);
        agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

        //target food
        Rigidbody targetFoodRigidbody = targetFood.GetComponent<Rigidbody>();
        targetFoodRigidbody.velocity = Vector3.zero;
        targetFoodRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + new Vector3(0, 0.5f, 0);

        int foodnum = Random.Range(5, 20);
        if (foodnum > 0)
        {
            for (int i = 0; i < foodnum; i++)
            {
                GameObject food = Instantiate(foodPrefab) as GameObject;
                objsList.Add(food);
                food.transform.position = ChooseRandomRectPosition(centerPos, -14, 14, 14, -14) + new Vector3(0, 0.5f, 0);
            }
        }
    }
    public override int GetCurrentLevel()
    {
        return 0;//(int)levelType;
    }
    public override int GetLevelNumbers()
    {
        return 0;//(int)LevelType.END;
    }
    public override void SetCurrenLevel()
    {

    }
}
