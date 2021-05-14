using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    protected SpriteRenderer spriteRenderer;

    public GameObject[] drops;
    public int minDrops, maxDrops;

    public float health;
    public float maxHealth;
    protected GameManager gm;
    public List<GameObject> waypoints;
    public List<int> safeWaypoints;
    public int currentWaypoint = 0;
    public float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public bool moving;

    //Gun attack 
    public GameObject laserPrefab;
    public int damage;
    public Vector3 laserSpawnOffset;
    public float attackRecharge;
    float attackTimer;
    public Vector3 targetOffset;

    protected Vector3 startPosition;
    protected Vector3 endPosition;

    protected float pathLength;
    public float totalTimeForPath;
    public float currentTimeOnPath;

    public Room room;
    public int spawnID;
    /* 0 = Came from Hangar Bay
     * 1 = Came from Mine
     * 2 = Came from Vent
     */

    public int targetVault = 3;
    /* 0-2 = Vault 64_
     * 3 = undecided
     */ 

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public float distanceToGoal()
    {
        float distance = 0;
        distance += Vector3.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Count - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector3.Distance(startPosition, endPosition);
        }
        return distance;
    }

    public void Initiate(Room spawnRoom, GameManager gm, int startPoint)
    {
        this.gm = gm;
        lastWaypointSwitchTime = Time.time;
        room = spawnRoom;
        //waypoints = new List<GameObject>();
        //waypoints = spawnRoom.pathway;
        currentWaypoint = startPoint;
        startPosition = spawnRoom.pathway[currentWaypoint].transform.position;
        endPosition = spawnRoom.pathway[currentWaypoint + 1].transform.position;
        /*Vector2 vectorToTarget = waypoints[currentWaypoint + 1].transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;*/
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        health = maxHealth;
        moving = true;
    }


    void Update()
    {
        

        //Movement
        //startPosition = waypoints[currentWaypoint].transform.position;
        //endPosition = waypoints[currentWaypoint + 1].transform.position;

        pathLength = Vector3.Distance(startPosition, endPosition);
        totalTimeForPath = pathLength / speed;//step;
        currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        //TO-DO: Write rotation code to flip sprite when it is walking the opposite direction that it is facing.
        /// put it in SpriteAlwaysFaceCamera.cs
        /// 
        ///Vector3 vectorToTarget = waypoints[currentWaypoint + 1].transform.position - transform.position;
        //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 0.1F);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (gameObject.transform.position.Equals(endPosition))
        {
            if (currentWaypoint < room.GetPathway(this).Count - 2) {
                currentWaypoint++;

                /*//Add check if the next Waypoint is in the next room. If yes, teleport to the next room.
                if (!waypoints[currentWaypoint + 1]) {
                    currentWaypoint += 2;
                    
                }*/

            } else {//Crap! They've reached the end!
                //TO-DO: Enemy needs variable for when they're actively escaping. Just a simple boolean.
               
                
                Door d = room.GetPathway(this)[currentWaypoint+1].GetComponent<Door>();

                if (d) {
                    room.enemiesInRoom.Remove(this);
                    room = d.nextRoom;
                    transform.position = room.GetPathway(this)[0].transform.position;
                    room.enemiesInRoom.Add(this);
                    currentWaypoint = 0;

                } else {
                    Destroy(gameObject);
                }
            }

            startPosition = room.GetPathway(this)[currentWaypoint].transform.position;
            endPosition = room.GetPathway(this)[currentWaypoint + 1].transform.position;
            endPosition = room.GetPathway(this)[currentWaypoint + 1].transform.position;
            lastWaypointSwitchTime = Time.time;

        }

        //Rotate/Point towards next waypoint
        //1
        //Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        //Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        //Vector3 newDirection = (newEndPosition - newStartPosition);
        //2
        //float x = newDirection.x;
        //float y = newDirection.y;
        //float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //3
        //GameObject sprite = (GameObject) gameObject.transform.FindChild("Sprite").gameObject;
        //sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        // Attack!
        Ally target = room.TargetAlly();
        
        if (target) {
            if (attackTimer < attackRecharge) {
                attackTimer += Time.deltaTime;
            }
            if (attackTimer >= attackRecharge) {
                attackTimer = 0;
                Vector3 targetV = target.transform.position + targetOffset;
                Vector3 start = transform.position + laserSpawnOffset;
                Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
                lazor.Initiate(start, targetV, target.gameObject, damage, false, room);
            }
        } else {
            attackTimer = 0;
        }


        
        
    }


    public void TakeDamage(int ow) {
        health -= ow;
        if (health <= 0) {
            Die();
        }
    }


    protected void Die() {
        int a = (int) Random.Range(minDrops, maxDrops+1);
        Vector3 lootSpawn = transform.position + new Vector3(0, 1, 0);

        for (int b = 0; b < a; b++) {
            int d = (int)Random.Range(0, drops.Length);
            Resource loot = Instantiate(drops[d], lootSpawn, transform.rotation).GetComponent<Resource>();
            loot.Initiate();
        }

        room.enemiesInRoom.Remove(this);
        Destroy(gameObject);
    }

}
