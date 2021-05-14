using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Ally
{
    public Vector3 targetOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(Room r) {
        health = maxHealth;
        room = r;

    }

    // Update is called once per frame
    void Update()
    {
        if (laserTimer < laserRechargeTime) {
            laserTimer += Time.deltaTime;
        }

        if (laserTimer >= laserRechargeTime) {

            //timer = 0;
                
            Enemy target = room.TargetEnemy();
                
            if (target) {
                laserTimer = 0;
                Vector3 targetV = target.transform.position + targetOffset;
                Vector3 start = transform.position + laserSpawnOffset;
                Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
                lazor.Initiate(start, targetV, target.gameObject, damage, true, room);        
            }
        }    
    }
}
