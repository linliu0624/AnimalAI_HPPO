using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFindFood : LevelManager
{
    public enum LevelType
    {
        eBasicLookforFood,
        END,
    }
    public LevelType levelType;
    public void PlaceAgent()
    {
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        agent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
    public void SpawnTargetFood()
    {
        Rigidbody rigidbody = targetFood.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
        targetFood.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
    public override void PlaceOtherObjs()
    {
        if (levelType == LevelType.eBasicLookforFood)
        {
            PlaceAgent();
            SpawnTargetFood();
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
}
