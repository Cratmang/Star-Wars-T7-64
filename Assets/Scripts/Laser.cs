using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    public bool friendly = false;
    Vector3 homeV, targetV;
    GameObject target;
    public float speed = 1.0f;
    float spawnTime;

    // A lot of initiates, when I could easily just use prefabs... ^v^;
    public void Initiate(Vector3 hv, Vector3 tv, GameObject tar) {
        homeV = hv;
        targetV = tv;
        target = tar;
        spawnTime = Time.time;
    }
    public void Initiate(Vector3 hv, GameObject tar) {
        homeV = hv;
        targetV = tar.transform.position;
        target = tar;
        spawnTime = Time.time;
    }
    public void Initiate(Vector3 hv, Vector3 tv, GameObject tar, int d) {
        homeV = hv;
        targetV = tv;
        target = tar;
        damage = d;
        spawnTime = Time.time;
    }
    public void Initiate(Vector3 hv, GameObject tar, int d) {
        homeV = hv;
        targetV = tar.transform.position;
        target = tar;
        damage = d;
        spawnTime = Time.time;
    }


    // Update is called once per frame
    void Update() {
        float pathLength = Vector3.Distance(homeV, targetV);
        float totalTimeForPath = pathLength / speed;//step;
        float currentTimeOnPath = Time.time - spawnTime;
        transform.position = Vector3.Lerp(homeV, targetV, currentTimeOnPath / totalTimeForPath);

        transform.rotation = Quaternion.LookRotation(targetV-transform.position);

        if (gameObject.transform.position.Equals(targetV)) {
            target.SendMessage("TakeDamage", damage);
            Destroy(this.gameObject);
        }
    }
}
