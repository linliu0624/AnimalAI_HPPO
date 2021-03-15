using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayScanner : MonoBehaviour
{
    //0: targetfood, 1:food, 2:wall, 3:tWall, 4:ramp, 5:hightland, 6:box, 7:enemy, 8:hotzone 9: nothing
    public GameObject agent;
    [HideInInspector] private List<float> hitList;
    // private List<Ray> rayList;
    // private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        // without nothing
        // hitList = new List<float>() { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        // main version
        hitList = new List<float>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, 1 };
        //hitList = new List<float>() { 100, 100, 100, 100, 100, 100, 100, 100, 100, 1 };

        // hitList = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
    }
    // Update is called once per frame
    void Update()
    {
        //RayCast();
        //Scan();
    }
    public List<float> GetHitList()
    {
        return hitList;
    }
    public void ResetHitList()
    {
        // without nothing
        //hitList = new List<float>() { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        // main version
        hitList = new List<float>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, 1 };
        // hitList = new List<float>() { 100, 100, 100, 100, 100, 100, 100, 100, 100, 1 };

        // hitList = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
    }
    public void RayCast()
    {
        Ray rayCast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        // Debug.Log(hit.collider.name);
        if (Physics.Raycast(rayCast, out hit, 50, LayerMask.GetMask("Default")))
        {
            if (hit.collider.CompareTag("TargetFood"))
            {
                //Debug.Log("distance:" + 1 / Vector3.Distance(this.transform.position, hit.transform.position));
                if (hitList[0] == 0 || hitList[0] < 1 / Vector3.Distance(this.transform.position, hit.transform.position))
                // if (hitList[0] > Vector3.Distance(this.transform.position, hit.transform.position))
                {

                    hitList[0] = 1 / Vector3.Distance(this.transform.position, hit.transform.position);
                    // hitList[0] = Vector3.Distance(this.transform.position, hit.transform.position);
                    // 鎖住最大值
                    if (hitList[0] > 1) hitList[0] = 1;
                }
            }
            else if (hit.collider.CompareTag("Food"))
            {
                if (hitList[1] == 0 || hitList[1] < 1 / Vector3.Distance(this.transform.position, hit.transform.position))
                // if (hitList[1] > Vector3.Distance(this.transform.position, hit.transform.position))
                {
                    hitList[1] = 1 / Vector3.Distance(this.transform.position, hit.transform.position);
                    // hitList[1] = Vector3.Distance(this.transform.position, hit.transform.position);
                    if (hitList[1] > 1) hitList[1] = 1;
                }
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                if (hitList[2] == 0 || hitList[2] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[2] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[2] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[2] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[2] > 1) hitList[2] = 1;
                }
            }
            // else if (hit.collider.CompareTag("TWall"))
            // {
            //     if (hitList[3] == 0 || hitList[3] < 1 / Vector3.Distance(this.transform.position, hit.transform.position))
            //         hitList[3] = Vector3.Distance(this.transform.position, hit.transform.position);
            // }
            else if (hit.collider.CompareTag("Ramp"))
            {
                if (hitList[4] == 0 || hitList[4] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[4] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[4] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[4] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[4] > 1) hitList[4] = 1;
                }
            }
            else if (hit.collider.CompareTag("HightLand"))
            {
                if (hitList[5] == 0 || hitList[5] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[5] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[5] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[5] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[5] > 1) hitList[5] = 1;
                }
            }
            else if (hit.collider.CompareTag("Pushable"))
            {
                if (hitList[6] == 0 || hitList[6] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[6] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[6] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[6] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[6] > 1) hitList[6] = 1;
                }
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                if (hitList[7] == 0 || hitList[7] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[7] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[7] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[7] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[7] > 1) hitList[7] = 1;
                }
            }
            else if (hit.collider.CompareTag("HotZone"))
            {
                if (hitList[8] == 0 || hitList[8] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[8] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[8] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    // hitList[8] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[8] > 0.5f) hitList[8] += 0.4f;
                    if (hitList[8] > 1) hitList[8] = 1;
                }
            }
            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(this.transform.position, hit.transform.position), Color.red);
            goto end; //這樣才可以跳過對透明嗆壁的判斷，也就是紅雷射出現就不會重複出現綠雷射
        }
        if (Physics.Raycast(rayCast, out hit, 50, LayerMask.GetMask("RayCastPass")))
        {
            if (hit.collider.CompareTag("TWall"))
            {
                if (hitList[3] == 0 || hitList[3] < 1 / Vector3.Distance(this.transform.position, hit.point))
                // if (hitList[3] > Vector3.Distance(this.transform.position, hit.point))
                {
                    hitList[3] = 1 / Vector3.Distance(this.transform.position, hit.point);
                    //hitList[3] = Vector3.Distance(this.transform.position, hit.point);
                    if (hitList[3] > 1) hitList[3] = 1;
                }
            }
            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(this.transform.position, hit.transform.position), Color.green);

        }
    end:
        {

            for (int i = 0; i < hitList.Count - 1; i++)
            {
                if (hitList[i] > 0)
                {
                    // 什麼都沒看見的時候
                    hitList[hitList.Count - 1] = -1;
                }
            }


        }
    }
    public void Scan()
    {
        //左右擺動範圍 +55~-55
        //上下擺動範圍 +15~-15
        for (int i = -55; i <= 55; i++)
        {
            for (int j = -20; j <= 20; j++)
            {
                // 兩個角度相乘就會讓這個物件跟著父物件旋轉了
                // 不然他只會看向固定方向
                transform.rotation = agent.transform.rotation * Quaternion.Euler(j, i, 0);
                RayCast();

            }
        }
        // 如果不取倒數會用到
        // for (int i = 0; i < hitList.Count; i++)
        // {
        //     hitList[i] /= 10;
        // }
        Debug.Log(hitList[0] + " " + hitList[1] + " " + hitList[2] + " " + hitList[3] + " " +
             hitList[4] + " " + hitList[5] + " " + hitList[6] + " " + hitList[7] + " " + hitList[8] + " " + hitList[9]);

    }
}
