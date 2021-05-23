using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberSentry : Sentry
{
    public float deflectChance; // Chance that the sentry/turret/I-don't-know-what-I'm-calling-it-anymore to deflect the laser. 0 = won't deflect, 1 = always deflect.
    public GameObject strike;
    
    

    public override void TakeDamage(int ow) {

        float roll = Random.Range(0.0F, 1.0F);
        if (roll <= deflectChance) {
            Enemy target = room.TargetEnemy();

            if (target) {
                laserTimer = 0;
                Vector3 targetV = target.transform.position + targetOffset;
                Vector3 start = transform.position + laserSpawnOffset;
                Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
                lazor.Initiate(start, targetV, target.gameObject, ow, true, room);
            }
        } else {
            base.TakeDamage(ow);
        }
    }
}
