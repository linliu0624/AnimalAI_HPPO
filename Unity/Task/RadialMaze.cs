using System.Collections.Generic;
using UnityEngine;

public class RadialMaze : LevelManager
{
    public enum LevelType
    {
        e2ArmsMaze1,
        e2ArmsMaze2,
        e4ArmsMaze1,
        e6ArmsMaze1,
        e8ArmsMaze1,
        END,
    }
    public LevelType levelType;
    public GameObject mazePrefab;
    public GameObject wallPrefab;
    public GameObject rampPrefab;
    public GameObject foodPrefab;
    private Vector3 blockWallSize = new Vector3(2, 3, 0.2f);
    private void Update()
    {
        //如果食物沒了就結束遊戲
        if (agent.foodsList.Count == 0 && isOnlyFood)
        {
            agent.passTime++;
            agent.EndEpisode();
        }
    }
    private void BuildRadialMaze()
    {
        GameObject maze = Instantiate(mazePrefab) as GameObject;
        objsList.Add(maze);
        maze.transform.position = transform.position;
    }
    public override void PlaceOtherObjs()
    {
        isOnlyFood = true;
        BuildRadialMaze();
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, 0);
        agent.transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);

        if (levelType == LevelType.e2ArmsMaze1)
        {
            TwoArmsMaze1();
        }
        else if (levelType == LevelType.e2ArmsMaze2)
        {
            TwoArmsMaze2();
        }
        else if (levelType == LevelType.e4ArmsMaze1)
        {
            FourArmsMaze1();
        }
        else if (levelType == LevelType.e6ArmsMaze1)
        {
            SixArmsMaze1();
        }
        else if (levelType == LevelType.e8ArmsMaze1)
        {
            EightArmsMaze1();
        }
        levelTimes++;
    }

    private void TwoArmsMaze2()
    {
        baseReward = 2;
        //agent.MaxStep = 1000;
        //block unuse zone
        //左上
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = blockWallSize;
        wall1.transform.position = transform.position + new Vector3(-3, 0, 1.5f);
        wall1.transform.rotation = Quaternion.Euler(0, -45, 0);
        //右上
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = blockWallSize;
        wall2.transform.position = transform.position + new Vector3(3, 0, 1.5f);
        wall2.transform.rotation = Quaternion.Euler(0, -135, 0);
        //左下
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.localScale = blockWallSize;
        wall3.transform.position = transform.position + new Vector3(-3, 0, -1.5f);
        wall3.transform.rotation = Quaternion.Euler(0, 45, 0);
        //右下
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.localScale = blockWallSize;
        wall4.transform.position = transform.position + new Vector3(3, 0, -1.5f);
        wall4.transform.rotation = Quaternion.Euler(0, 135, 0);
        //上
        GameObject wall5 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall5);
        wall5.transform.localScale = new Vector3(3, 3, 0.2f);
        wall5.transform.position = transform.position + new Vector3(-1.5f, 0, 3f);
        wall5.transform.rotation = Quaternion.Euler(0, 0, 0);
        //下
        GameObject wall6 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall6);
        wall6.transform.localScale = new Vector3(3, 3, 0.2f);
        wall6.transform.position = transform.position + new Vector3(-1.5f, 0, -3f);
        wall6.transform.rotation = Quaternion.Euler(0, 0, 0);

        //target food
        //float[,] targetPos = { { -10f, 0 }, { 10f, 0 }, { 0, 10f }, { 0, -10f } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(14, 0, 14);//ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //food
        GameObject food1 = Instantiate(foodPrefab) as GameObject;
        food1.transform.position = transform.position + new Vector3(10, 0.5f, 0);
        agent.foodsList.Add(food1);
        objsList.Add(food1);
        GameObject food2 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food2);
        objsList.Add(food2);
        food2.transform.position = transform.position + new Vector3(-10, 0.5f, 0);

        //ramp
        //右邊
        GameObject ramp1_1 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp1_1);
        ramp1_1.transform.position = transform.position + new Vector3(5, 1f, -1.5f);
        ramp1_1.transform.localScale = new Vector3(3, 0.2f, 3);
        ramp1_1.transform.rotation = Quaternion.Euler(0, 0, -18);
        GameObject ramp1_2 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp1_2);
        ramp1_2.transform.position = transform.position + new Vector3(5, 1f, -1.5f);
        ramp1_2.transform.localScale = new Vector3(3, 0.2f, 3);
        ramp1_2.transform.rotation = Quaternion.Euler(0, 0, 210-12);
        //左邊
        GameObject ramp2_1 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp2_1);
        ramp2_1.transform.position = transform.position + new Vector3(-5, 1f, -1.5f);
        ramp2_1.transform.localScale = new Vector3(3, 0.2f, 3);
        ramp2_1.transform.rotation = Quaternion.Euler(0, 0, -18);
        GameObject ramp2_2 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp2_2);
        ramp2_2.transform.position = transform.position + new Vector3(-5, 1f, -1.5f);
        ramp2_2.transform.localScale = new Vector3(3, 0.2f, 3);
        ramp2_2.transform.rotation = Quaternion.Euler(0, 0, 210-12);
    }

    private void TwoArmsMaze1()
    {
        baseReward = 2;
        //agent.MaxStep = 1000;
        //block unuse zone
        //左上
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = blockWallSize;
        wall1.transform.position = transform.position + new Vector3(-3, 0, 1.5f);
        wall1.transform.rotation = Quaternion.Euler(0, -45, 0);
        //右上
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = blockWallSize;
        wall2.transform.position = transform.position + new Vector3(3, 0, 1.5f);
        wall2.transform.rotation = Quaternion.Euler(0, -135, 0);
        //左下
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.localScale = blockWallSize;
        wall3.transform.position = transform.position + new Vector3(-3, 0, -1.5f);
        wall3.transform.rotation = Quaternion.Euler(0, 45, 0);
        //右下
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.localScale = blockWallSize;
        wall4.transform.position = transform.position + new Vector3(3, 0, -1.5f);
        wall4.transform.rotation = Quaternion.Euler(0, 135, 0);
        //上
        GameObject wall5 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall5);
        wall5.transform.localScale = new Vector3(3, 3, 0.2f);
        wall5.transform.position = transform.position + new Vector3(-1.5f, 0, 3f);
        wall5.transform.rotation = Quaternion.Euler(0, 0, 0);
        //下
        GameObject wall6 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall6);
        wall6.transform.localScale = new Vector3(3, 3, 0.2f);
        wall6.transform.position = transform.position + new Vector3(-1.5f, 0, -3f);
        wall6.transform.rotation = Quaternion.Euler(0, 0, 0);

        //target food
        //float[,] targetPos = { { -10f, 0 }, { 10f, 0 }, { 0, 10f }, { 0, -10f } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(14, 0, 14);//ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //food
        GameObject food1 = Instantiate(foodPrefab) as GameObject;
        food1.transform.position = transform.position + new Vector3(10, 0.5f, 0);
        agent.foodsList.Add(food1);
        objsList.Add(food1);
        GameObject food2 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food2);
        objsList.Add(food2);
        food2.transform.position = transform.position + new Vector3(-10, 0.5f, 0);
    }

    private void EightArmsMaze1()
    {
        baseReward = 6;
        agent.MaxStep = 6000;
        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(14, 0, 14);

        //food
        GameObject food1 = Instantiate(foodPrefab) as GameObject;
        food1.transform.position = transform.position + new Vector3(10, 0.5f, 0);
        agent.foodsList.Add(food1);
        objsList.Add(food1);
        GameObject food2 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food2);
        objsList.Add(food2);
        food2.transform.position = transform.position + new Vector3(-10, 0.5f, 0);
        GameObject food3 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food3);
        objsList.Add(food3);
        food3.transform.position = transform.position + new Vector3(0, 0.5f, 10);
        GameObject food4 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food4);
        objsList.Add(food4);
        food4.transform.position = transform.position + new Vector3(0, 0.5f, -10);
        GameObject food5 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food5);
        objsList.Add(food5);
        food5.transform.position = transform.position + new Vector3(8.5f, 0.5f, 7);
        GameObject food6 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food6);
        objsList.Add(food6);
        food6.transform.position = transform.position + new Vector3(-7, 0.5f, 7);
        GameObject food7 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food7);
        objsList.Add(food7);
        food7.transform.position = transform.position + new Vector3(8.5f, 0.5f, -7);
        GameObject food8 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food8);
        objsList.Add(food8);
        food8.transform.position = transform.position + new Vector3(-7, 0.5f, -7);
    }

    //沒有左下右下的arms
    private void SixArmsMaze1()
    {
        baseReward = 4.5f;
        agent.MaxStep = 4500;
        //block unuse zone
        //左下
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.localScale = blockWallSize;
        wall3.transform.position = transform.position + new Vector3(-3, 0, -1.5f);
        wall3.transform.rotation = Quaternion.Euler(0, 45, 0);
        //右下
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.localScale = blockWallSize;
        wall4.transform.position = transform.position + new Vector3(3, 0, -1.5f);
        wall4.transform.rotation = Quaternion.Euler(0, 135, 0);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(14, 0, 14);

        //food
        GameObject food1 = Instantiate(foodPrefab) as GameObject;
        food1.transform.position = transform.position + new Vector3(10, 0.5f, 0);
        agent.foodsList.Add(food1);
        objsList.Add(food1);
        GameObject food2 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food2);
        objsList.Add(food2);
        food2.transform.position = transform.position + new Vector3(-10, 0.5f, 0);
        GameObject food3 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food3);
        objsList.Add(food3);
        food3.transform.position = transform.position + new Vector3(0, 0.5f, 10);
        GameObject food4 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food4);
        objsList.Add(food4);
        food4.transform.position = transform.position + new Vector3(0, 0.5f, -10);
        GameObject food5 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food5);
        objsList.Add(food5);
        food5.transform.position = transform.position + new Vector3(8.5f, 0.5f, 7);
        GameObject food6 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food6);
        objsList.Add(food6);
        food6.transform.position = transform.position + new Vector3(-7, 0.5f, 7);
    }

    //只有上下左右的arms
    private void FourArmsMaze1()
    {
        baseReward = 3;
        agent.MaxStep = 3000;
        //block unuse zone
        //左上
        GameObject wall1 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall1);
        wall1.transform.localScale = blockWallSize;
        wall1.transform.position = transform.position + new Vector3(-3, 0, 1.5f);
        wall1.transform.rotation = Quaternion.Euler(0, -45, 0);
        //右上
        GameObject wall2 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall2);
        wall2.transform.localScale = blockWallSize;
        wall2.transform.position = transform.position + new Vector3(3, 0, 1.5f);
        wall2.transform.rotation = Quaternion.Euler(0, -135, 0);
        //左下
        GameObject wall3 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall3);
        wall3.transform.localScale = blockWallSize;
        wall3.transform.position = transform.position + new Vector3(-3, 0, -1.5f);
        wall3.transform.rotation = Quaternion.Euler(0, 45, 0);
        //右下
        GameObject wall4 = Instantiate(wallPrefab) as GameObject;
        objsList.Add(wall4);
        wall4.transform.localScale = blockWallSize;
        wall4.transform.position = transform.position + new Vector3(3, 0, -1.5f);
        wall4.transform.rotation = Quaternion.Euler(0, 135, 0);

        //target food
        //float[,] targetPos = { { -10f, 0 }, { 10f, 0 }, { 0, 10f }, { 0, -10f } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(14, 0, 14);//ChooseRandomFixedPosition(transform.position, targetPos) + new Vector3(0, 0.5f, 0);

        //food
        GameObject food1 = Instantiate(foodPrefab) as GameObject;
        food1.transform.position = transform.position + new Vector3(10, 0.5f, 0);
        agent.foodsList.Add(food1);
        objsList.Add(food1);
        GameObject food2 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food2);
        objsList.Add(food2);
        food2.transform.position = transform.position + new Vector3(-10, 0.5f, 0);
        GameObject food3 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food3);
        objsList.Add(food3);
        food3.transform.position = transform.position + new Vector3(0, 0.5f, 10);
        GameObject food4 = Instantiate(foodPrefab) as GameObject;
        agent.foodsList.Add(food4);
        objsList.Add(food4);
        food4.transform.position = transform.position + new Vector3(0, 0.5f, -10);
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
        if (levelType == LevelType.END)
            isOnlyFood = false;

    }
}
