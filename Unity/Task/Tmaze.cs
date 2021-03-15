using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tmaze : LevelManager
{
    public enum LevelType
    {
        eFoodAndTargetFood,
        eOnlyTarget,
        END,
    }
    public LevelType levelType;
    public GameObject food;
    public GameObject wall;

    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eFoodAndTargetFood)
        {
            FoodAndTargetFood();
            levelTimes++;
        }
        else if (levelType == LevelType.eOnlyTarget)
        {
            OnlyTarget();
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
    private void OnlyTarget()
    {
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        float[,] targetFoodPos = { { 14, 14 }, { -14, 14 } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetFoodPos) + new Vector3(0, 0.5f, 0);

        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = transform.position + new Vector3(-15f, 0, -15f);
        wall1.transform.rotation = Quaternion.Euler(0f, 0, 0);
        wall1.transform.localScale = new Vector3(13.5f, 3, 27.5f);
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = transform.position + new Vector3(1.5f, 0, -15f);
        wall2.transform.rotation = Quaternion.Euler(0f, 0, 0);
        wall2.transform.localScale = new Vector3(13.5f, 3, 27.5f);
    }

    private void FoodAndTargetFood()
    {
        baseReward = 1;

        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        float[,] targetFoodPos = { { 14, 14 }, { -14, 14 } };
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomFixedPosition(transform.position, targetFoodPos) + new Vector3(0, 0.5f, 0);

        GameObject wall1 = Instantiate(wall) as GameObject;
        objsList.Add(wall1);
        wall1.transform.position = transform.position + new Vector3(-15f, 0, -15f);
        wall1.transform.rotation = Quaternion.Euler(0f, 0, 0);
        wall1.transform.localScale = new Vector3(13.5f, 3, 27.5f);
        GameObject wall2 = Instantiate(wall) as GameObject;
        objsList.Add(wall2);
        wall2.transform.position = transform.position + new Vector3(1.5f, 0, -15f);
        wall2.transform.rotation = Quaternion.Euler(0f, 0, 0);
        wall2.transform.localScale = new Vector3(13.5f, 3, 27.5f);

        GameObject food1 = Instantiate(food) as GameObject;
        objsList.Add(food1);
        food1.transform.position = new Vector3(targetFood.transform.position.x * -1, 0.5f, targetFood.transform.position.z);
    }
}
