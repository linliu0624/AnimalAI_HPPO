using UnityEngine;

public class UseRamp : LevelManager
{
    public enum LevelType
    {
        eRamp1,
        eRamp2,
        eRamp3,
        eRampCantSeeFood1,
        eRampCantSeeFood2,
        END,
    }
    public LevelType levelType;
    public GameObject hightFloorPrefab;
    public GameObject rampPrefab;
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
        if (levelType == LevelType.eRamp1)
        {
            Ramp1();
        }
        else if (levelType == LevelType.eRamp2)
        {
            Ramp2();
        }
        else if (levelType == LevelType.eRamp3)
        {
            Ramp3();
        }
        else if (levelType == LevelType.eRampCantSeeFood1)
        {
            RampCantSeeFood1();
        }
        else if (levelType == LevelType.eRampCantSeeFood2)
        {
            RampCantSeeFood2();
        }
        levelTimes++;
    }

    private void Ramp3()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, -14, -10);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //hight floor
        GameObject floor = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor);
        floor.transform.position = transform.position + new Vector3(0f, 0, -5f);
        floor.transform.localScale = new Vector3(10, 0.6f, 5);
        floor.transform.rotation = Quaternion.Euler(0, -90, 0);

        //hight floor
        GameObject floor2 = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor2);
        floor2.transform.position = floor.transform.position + new Vector3(0, 0, Random.Range(2.5f, 7.5f));
        floor2.transform.localScale = new Vector3(5, 0.6f, 0.3f);

        //Ramp
        GameObject ramp1 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp1);
        ramp1.transform.position = floor.transform.position + new Vector3(-floor.transform.localScale.z, floor.transform.localScale.y, 0);
        ramp1.transform.rotation = Quaternion.Euler(0, 90, -20);
        ramp1.transform.localScale = new Vector3(3f, 0.25f, floor.transform.localScale.z);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = floor2.transform.position + new Vector3(4, 0.5f, 0.15f);
    }

    private void RampCantSeeFood2()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, -14, -10);
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //Ramp
        GameObject ramp1 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp1);
        ramp1.transform.position = transform.position + new Vector3(-15, 2, -5);
        ramp1.transform.rotation = Quaternion.Euler(0, 90, -30);
        ramp1.transform.localScale = new Vector3(5f, 0.1f, 30);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -13, 13, 14.5f, 0f) + new Vector3(0, 0.5f, 0);
    }

    private void RampCantSeeFood1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 0.3f, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //hight floor
        GameObject floor = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor);
        floor.transform.position = transform.position + new Vector3(-15f, 0, -5f);
        floor.transform.localScale = new Vector3(30, 1.3f, 20);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, 14.5f, 14.5f) + new Vector3(0, floor.transform.localScale.y + 0.5f, 0);

        //Ramp
        GameObject ramp = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp);
        ramp.transform.position = floor.transform.position + new Vector3(Random.Range(5, 25f), floor.transform.localScale.y, 0);
        ramp.transform.rotation = Quaternion.Euler(0, 90, -30);
        ramp.transform.localScale = new Vector3(2.5f, 0.1f, Random.Range(0.5f, 1.5f));
    }

    private void Ramp2()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //hight floor
        GameObject floor1 = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor1);
        floor1.transform.position = transform.position + new Vector3(-15f, 0, -5f);
        floor1.transform.localScale = new Vector3(30, 1, 20);
        //hight floor
        GameObject floor2 = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor2);
        floor2.transform.position = transform.position + new Vector3(Random.Range(-10, 5f), floor1.transform.localScale.y, 0f);
        floor2.transform.localScale = new Vector3(5, 1, 5);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = new Vector3(floor2.transform.position.x + 2.5f, floor2.transform.localScale.y + 0.5f, floor2.transform.position.z + 2.5f);

        //Ramp
        GameObject ramp1 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp1);
        ramp1.transform.position = floor1.transform.position + new Vector3(Random.Range(5, 25f), floor1.transform.localScale.y, 0);
        ramp1.transform.rotation = Quaternion.Euler(0, 90, -30);
        ramp1.transform.localScale = new Vector3(2, 0.1f, Random.Range(0.5f, 1.5f));
        //Ramp
        GameObject ramp2 = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp2);
        ramp2.transform.position = floor2.transform.position + floor2.transform.localScale;
        ramp2.transform.rotation = Quaternion.Euler(0, -90, -30);
        ramp2.transform.localScale = new Vector3(2, 0.1f, 2.5f);
    }

    private void Ramp1()
    {
        //agent
        Rigidbody rigidbody = agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        agent.transform.position = transform.position + new Vector3(0, 1, Random.Range(-12f, -14f));
        agent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //hight floor
        GameObject floor = Instantiate(hightFloorPrefab) as GameObject;
        objsList.Add(floor);
        floor.transform.position = transform.position + new Vector3(-15f, 0, -5f);
        floor.transform.localScale = new Vector3(30, 0.5f, 20);

        //target food
        Rigidbody targetRigidbody = targetFood.GetComponent<Rigidbody>();
        targetRigidbody.velocity = Vector3.zero;
        targetRigidbody.angularVelocity = Vector3.zero;
        targetFood.transform.position = ChooseRandomRectPosition(transform.position, -10, 10, 0, -4.5f) + new Vector3(0, floor.transform.localScale.y + 0.5f, 0);

        //Ramp
        GameObject ramp = Instantiate(rampPrefab) as GameObject;
        objsList.Add(ramp);
        ramp.transform.position = floor.transform.position + new Vector3(Random.Range(5, 25f), floor.transform.localScale.y, 0);
        ramp.transform.rotation = Quaternion.Euler(0, 90, -15);
        ramp.transform.localScale = new Vector3(2, 0.1f, Random.Range(0.5f, 1.5f));
    }
}
