using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    public int hp;
    public int damage;
    public GameObject laserPrefab;

    public Vector3 targetOffset, laserSpawnOffset;

    public float attackRechargeTime;
    float timer = 0;

    public GameManager gm;
    public Room room;

    //public int roomID;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < attackRechargeTime) {
            timer += Time.deltaTime;

            if (timer >= attackRechargeTime) {
                timer = 0;
                GameObject target = room.TargetEnemy();
                Vector3 targetV = target.transform.position + targetOffset;
                Vector3 start = transform.position + laserSpawnOffset;
                Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
                lazor.Initiate(start, targetV, target, damage, false, room);


            }
        }    
    }
}
