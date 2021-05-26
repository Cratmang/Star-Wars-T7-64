using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droideka : Enemy
{
    public float burstFireGap;
    public float minRollTime;
    public int shotNumber;
    public Ally target;
    public bool moving = true;
    float timer;
    Animator anim;

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /*public override void Initiate(Room spawnRoom, GameManager gm, int sID) {
        base.Initiate(spawnRoom, gm, sID);
        moving = true;
        anim.SetBool("Moving", true);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            //Movement, copied from Enemy.cs

            pathLength = Vector3.Distance(startPosition, endPosition);
            totalTimeForPath = pathLength / speed;//step;
            currentTimeOnPath = Time.time - lastWaypointSwitchTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

            //TO-DO: Write rotation code to flip sprite when it is walking the opposite direction that it is facing.

            if (gameObject.transform.position.Equals(endPosition)) {

                if (currentWaypoint < room.GetPathway(this).Count - 2 && !escaping) {
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
                            currentWaypoint = room.GetPathway(this).Count - 1;
                        }
                    }
                }

                startPosition = room.GetPathway(this)[currentWaypoint].transform.position;
                if (escaping) {
                    endPosition = room.GetPathway(this)[currentWaypoint - 1].transform.position;
                } else {
                    endPosition = room.GetPathway(this)[currentWaypoint + 1].transform.position;
                }
                lastWaypointSwitchTime = Time.time;
            }

            if (room.alliesInRoom.Count > 0) {
                timer += Time.deltaTime;
                if (timer >= minRollTime) {
                    //TO DO Swap to stand animation
                    timer = 0;
                    moving = false;
                    anim.SetBool("Moving", false);
                    target = room.TargetAlly();
                }
            } else {
                timer = 0;
            }
        } else {
            timer += Time.deltaTime;
            bool targetSighted = false;
            if (target) {
                if (target.room.Equals(room)) {
                    targetSighted = true;
                    float loadtime;
                    if (shotNumber <= 0 || shotNumber >= 2) {
                        loadtime = attackRecharge;
                        shotNumber = 0;
                    } else {
                        loadtime = burstFireGap;
                    }
                    if (timer >= loadtime) {
                        timer = 0;
                        Vector3 targetV = target.transform.position + targetOffset;
                        Vector3 start = transform.position + laserSpawnOffset;
                        Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
                        lazor.Initiate(start, targetV, target.gameObject, damage, false, room);
                        shotNumber++;
                    }
                }
            }
            
            if (!targetSighted){
                if (timer >= attackRecharge) {
                    //TO DO Swap to rolling animation
                    timer = 0;
                    moving = true;
                    anim.SetBool("Moving", true);
                }
            }
        }
    }


    public override void TakeDamage(int ow) {
        if (moving) {
            ow *= 2; //Droideka takes more damage when it's moving, unshielded.
        }
        base.TakeDamage(ow);
    }

    protected override void Die() { //It's the exact same method. The only reason it's here is to fix a bug.
        int a = (int)Random.Range(minDrops, maxDrops + 1);
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
