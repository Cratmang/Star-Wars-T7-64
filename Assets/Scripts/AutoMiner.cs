using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public GameObject[] drops;

    float timer = 0;
    int digs = 0;
    public float digTime;
    public int digsPerFuel;

    public GameObject spawnPoint;

    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (digs > 0) {
            timer += Time.deltaTime;
            if (timer >= digTime) {
                int d = Random.Range(0, drops.Length);
                Resource loot = Instantiate(drops[d], spawnPoint.transform.position, transform.rotation).GetComponent<Resource>();
                loot.Initiate();
                digs--;
                timer = 0;
                if (digs <= 0) {
                    anim.SetBool("Powered", false);
                }
            }
        }
    }

    public void Power() {
        digs += digsPerFuel;
        anim.SetBool("Powered", true);
    }
}
