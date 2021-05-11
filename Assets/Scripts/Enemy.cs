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
    public List<Transform> waypoints;
    public List<int> safeWaypoints;
    protected int currentWaypoint = 0;
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

    //public void Initiate() {
        //gm = game;
    //    lastWaypointSwitchTime = Time.time;
        //waypoints = pw;
    //}

    /*public void Initiate(GameObject[] pw, List<int> sw, GameManager game)
    {
        gm = game;
        lastWaypointSwitchTime = Time.time;
        waypoints = pw;
        safeWaypoints = sw;
        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector2 vectorToTarget = waypoints[currentWaypoint + 1].transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        health = maxHealth[level];
        moving = true;

    }*/

    public void Initiate(List<Transform> pw, GameManager gm, int startPoint)
    {
        this.gm = gm;
        lastWaypointSwitchTime = Time.time;
        waypoints = pw;
        currentWaypoint = startPoint;
        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector2 vectorToTarget = waypoints[currentWaypoint + 1].transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        health = maxHealth;
        moving = true;
    }


    void Update()
    {
        

        //Movement
        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;

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
            if (currentWaypoint < waypoints.Count - 2)
            {
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;

                //Add check if the next Waypoint is in the next room. If yes, teleport to the next room.
                if (!waypoints[currentWaypoint+1]) {
                    currentWaypoint += 2;
                    transform.position = waypoints[currentWaypoint].transform.position;
                }

            }
            else {//Crap! They've reached the end!
                //gameManager.EnemyReachedGoal(bounty[level]);
                Destroy(gameObject);
                
            }
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
        if (attackTimer < attackRecharge) {
            attackTimer += Time.deltaTime;
        }
        if (attackTimer >= attackRecharge) {
            attackTimer = 0;

            //Get Target!
            //Right now, the only target is the player.
            //TO-DO: Add code to detect if player or turrets are in the same room.
            GameObject target = GameObject.Find("T7-64");
            Vector3 targetV = target.transform.position + targetOffset;
            Vector3 start = transform.position + laserSpawnOffset;
            Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
            lazor.Initiate(start, targetV, target, damage, false);
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
            /*float xForce = Random.Range(-2.5F, 2.5F);
            float yForce = Random.Range( 0.1F, 5.0F);
            float zForce = Random.Range(-2.5F, 2.5F);
            loot.GetComponent<Rigidbody>().velocity = (new Vector3(xForce, yForce, zForce));*/
        }

        Destroy(gameObject);
    }

}
