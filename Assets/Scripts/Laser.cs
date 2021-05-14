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
    Room room;
    //int roomID;

    public Sprite[] laserColour;
    // 0 = green
    // 1 = yellow
    // 2 = red

    public SpriteRenderer sr1, sr2;

    
    public void Initiate(Vector3 hv, Vector3 tv, GameObject tar, int d, bool f, Room r) {
        homeV = hv;
        targetV = tv;
        target = tar;
        damage = d;
        spawnTime = Time.time;
        friendly = f;
        room = r;
        //roomID = id;

        gameObject.layer = 2;
        int c = 0;
        //DetermineLaserColour
        if (!friendly) {
            //Laser is being shot at player
            if (tar.tag.Equals("Player")) {
                c = 2;
                gameObject.layer = 0;

            //Laser is being shot at an ally, but not the player
            } else {
                c = 1;
            }
        
        //Laser is being shot by the player, or an ally
        } else {
            c = 0;
        }

        sr1.sprite = laserColour[c];
        sr2.sprite = laserColour[c];

        transform.rotation = Quaternion.LookRotation(targetV - transform.position);
    }
    public void Initiate(Vector3 hv, GameObject tar, int d, bool f, Room r) {
        Initiate(hv, tar.transform.position, tar, d, f, r);
        //Added case in case for some reason I can't get an appropriate target Vector. Just get the targets actual position, and use that.
    }

    // Update is called once per frame
    void Update() {
        float pathLength = Vector3.Distance(homeV, targetV);
        float totalTimeForPath = pathLength / speed;//step;
        float currentTimeOnPath = Time.time - spawnTime;
        transform.position = Vector3.Lerp(homeV, targetV, currentTimeOnPath / totalTimeForPath);


        if (gameObject.transform.position.Equals(targetV)) {
            if (target) { //Check to see if the target hasn't already been destroyed before the bullet landed.
                bool hit = false;
                //Check if the target is still in the same room.
                if (friendly) { 
                    Enemy T = target.GetComponent<Enemy>();
                    if (T.room.Equals(room)) {
                        hit = true;
                    }
                } else {
                    Ally T = target.GetComponent<Ally>();
                    if (T.room.Equals(room)) {
                        hit = true;
                    }
                }

                if (hit) target.SendMessage("TakeDamage", damage);
            }
            Destroy(this.gameObject);
        }
    }
}
