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
    public int currentWaypoint = 0;
    public float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public bool escaping;

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

    public int targetVault;
    bool hasLoot;
    public Transform lootHoldTransform;
    GameObject loot;
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //lootHoldTransform = gameObject.GetComponentInChildren<Transform>();
    }


    public float distanceToGoal() //The turrets should probably be using this to determine which robots have roamed the furthest. I think if I decide to use it, I will need to rewrite it.
    {
        float distance = 0;
        /*distance += Vector3.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Count - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector3.Distance(startPosition, endPosition);
        }*/
        return distance;
    }

    public void Initiate(Room spawnRoom, GameManager gm, int sID)
    {
        this.gm = gm;
        lastWaypointSwitchTime = Time.time;
        room = spawnRoom;
        currentWaypoint = 0;
        health = maxHealth;
        escaping = false;
        spawnID = sID;
        targetVault = gm.DetermineTargetVault();
        startPosition = spawnRoom.GetPathway(this)[currentWaypoint].transform.position;
        endPosition = spawnRoom.GetPathway(this)[currentWaypoint + 1].transform.position;
    }

    void Update()
    {
        
        //Movement

        pathLength = Vector3.Distance(startPosition, endPosition);
        totalTimeForPath = pathLength / speed;//step;
        currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        //TO-DO: Write rotation code to flip sprite when it is walking the opposite direction that it is facing.

        if (gameObject.transform.position.Equals(endPosition))
        {
            /*int p;
            if (escaping) {
                p = currentWaypoint - 1;
            } else {
                p = currentWaypoint + 1;
            }*/

            if (currentWaypoint < room.GetPathway(this).Count - 2  && !escaping) {
                currentWaypoint++;
            } else if (currentWaypoint > 1 && escaping) {
                currentWaypoint--;

            } else {//They've reached the end of the room!

                Door d;
                if (escaping) {
                    d = room.GetPathway(this)[0].GetComponent<Door>();

                } else {
                    d = room.GetPathway(this)[room.GetPathway(this).Count - 1].GetComponent<Door>();
                }

                if (d) {
                    room.enemiesInRoom.Remove(this);
                    room = d.nextRoom;
                    room.enemiesInRoom.Add(this);
                    if (escaping) {
                        transform.position = room.GetPathway(this)[room.GetPathway(this).Count - 1].transform.position;
                        currentWaypoint = room.GetPathway(this).Count - 1;
                    } else {
                        transform.position = room.GetPathway(this)[0].transform.position;
                        currentWaypoint = 0;
                    }

                    

                } else {
                    //You've reached the vault! Or, you're back at the ship!
                    if (escaping) {
                        if (hasLoot) {
                            hasLoot = false;
                            Destroy(loot);
                            gm.CheckVaultEmpty(targetVault);
                        }
                        hasLoot = false;
                        escaping = false;
                        targetVault = gm.DetermineTargetVault();
                        currentWaypoint = 0;
                    } else {
                        spawnID = 0; // All bots will always escape through the Atrium.
                        Vault v = room.GetComponent<Vault>();
                        if (v.loot > 0) {
                            hasLoot = true;
                            loot = Instantiate(v.lootPrefab, lootHoldTransform);
                            v.loot--;
                        }
                        escaping = true;
                        currentWaypoint = room.GetPathway(this).Count-1;
                    }
                    //Destroy(gameObject);
                }
            }

            startPosition = room.GetPathway(this)[currentWaypoint].transform.position;
            if (escaping) {
                endPosition = room.GetPathway(this)[currentWaypoint-1].transform.position;
            } else {
                endPosition = room.GetPathway(this)[currentWaypoint+1].transform.position;
            }
            //endPosition = room.GetPathway(this)[p].transform.position;
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

        if (hasLoot) {
            gm.ReturnLoot(targetVault);
        }

        room.enemiesInRoom.Remove(this);
        gm.enemies.Remove(this);
        gm.UpdateEnemyCounter();
        Destroy(gameObject);
    }

}
