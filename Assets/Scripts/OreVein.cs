using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreVein : MonoBehaviour
{
    public GameObject[] drops;

    public int minDrop, maxDrop;
    public float mineTime;

    public GameObject spawnPoint;

    public void SpawnResources() {
        float m = Random.Range(minDrop, maxDrop);
        int r;
        if (m >= 2) {
            r = Mathf.FloorToInt(m/2);
        } else {
            r = Mathf.FloorToInt(m);
        }
        

        if (r > 0) {
            for (int o = 0; o < r; o++) {
                int d = Random.Range(0, drops.Length);
                Resource loot = Instantiate(drops[d], spawnPoint.transform.position, transform.rotation).GetComponent<Resource>();
                loot.Initiate();
            }
        } else {
            Debug.Log("Resources found = none");
        }
    }
}
