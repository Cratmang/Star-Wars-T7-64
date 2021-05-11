using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public int indexID;
    public bool ripe = false;
    float timer = 0, ripeTime = 0.5F;

    // "Ripeness" prevents the resources from being collected as soon as they spawn, allowing them to burst from their source in an explosion effect first.

    private void Update() {
        if (!ripe) {
            timer += Time.deltaTime;
            if (timer >= ripeTime) {
                ripe = true;
            }
        }
    }

    public void Initiate() {
        timer = 0;
        ripe = false;
        float xForce = Random.Range(-2.5F, 2.5F);
        float yForce = Random.Range(0.1F, 5.0F);
        float zForce = Random.Range(-2.5F, 2.5F);
        gameObject.GetComponent<Rigidbody>().velocity = (new Vector3(xForce, yForce, zForce));
    }
}
