using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T764 : Ally
{
    public PlayerManager pm;

    public List<Transform> standHere;
    public List<Transform> travelWaypoints;
    public Transform waypointTo, waypointFro;
    
    public float travelTime = 0.5f;
    public bool traveling = false;
    float timer;

    public HealthBarUI healthBar;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = standHere[0].position;
        waypointTo = standHere[0].transform;
        waypointFro = standHere[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (traveling) {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(waypointFro.position, waypointTo.position, (timer/travelTime));
            
            if (timer >= travelTime) {
                transform.position = waypointTo.position;
                traveling = false;
                waypointFro = waypointTo;
                pm.UnlockInput();
            }
        }

        if (laserTimer < laserRechargeTime) {
            laserTimer += Time.deltaTime;
        }
    }


    // Code to travel between rooms. I may need to rewrite this code entirely to account for the Room object.
    //   Possible solution: include a refrence to the next room in Door.cs.
    //    Gonna go to bed now though, because it is 23:54 (almost midnight!) at the time I'm writing, and I want
    //    to have some semblance of a sleep schedule.
    public void TravelTo (int newRoom){
        traveling = true;
        timer = 0;
        //Teleport straight to the new room, determine which direction you're entering from based on the old room.
        switch (newRoom) {
            case 0:// Command Room
                //There's only one way into the Command Room!
                waypointFro = travelWaypoints[0];
                break;

            case 1:// Atrium
                switch (room.roomID) {
                    case 0:// From Command Room
                        waypointFro = travelWaypoints[1];
                        break;
                    case 2:// From Mine Entrance
                        waypointFro = travelWaypoints[2];
                        break;
                    case 4:// From Hall B
                        waypointFro = travelWaypoints[3];
                        break;
                    case 7:// From Vault 640
                        waypointFro = travelWaypoints[4];
                        break;
                    case 8:// From Vault 641
                        waypointFro = travelWaypoints[5];
                        break;
                    case 9:// From Vault 642
                        waypointFro = travelWaypoints[6];
                        break;
                    default:
                        Debug.LogError("old room index ID = somehow invalid! // noclip = activated.");
                        waypointFro = waypointTo;
                        break;
                }
                break;

            case 2:// Mine Entrance
                switch (room.roomID) {
                    case 1:// From Atrium
                        waypointFro = travelWaypoints[7];
                        break;
                    case 3:// From Mine Entrance
                        waypointFro = travelWaypoints[8];
                        break;
                    default:
                        Debug.LogError("old room index ID = somehow invalid! // noclip = activated.");
                        waypointFro = waypointTo;
                        break;
                }
                break;

            case 3:// Mine
                //There's only one way into the Mine!
                waypointFro = travelWaypoints[9];
                break;

            case 4:// Hall B
                switch (room.roomID) {
                    case 1:// From Atrium
                        waypointFro = travelWaypoints[11];
                        break;
                    case 5:// From Hall A
                        waypointFro = travelWaypoints[10];  //I messed up the order.
                        break;
                    default:
                        Debug.LogError("old room index ID = somehow invalid! // noclip = activated.");
                        waypointFro = waypointTo;
                        break;
                }
                break;

            case 5:// Hall A
                switch (room.roomID) {
                    case 4:// From Hall B
                        waypointFro = travelWaypoints[12];
                        break;
                    case 6:// From Hangar Bay
                        waypointFro = travelWaypoints[13];
                        break;
                    default:
                        Debug.LogError("old room index ID = somehow invalid! // noclip = activated.");
                        waypointFro = waypointTo;
                        break;
                }
                break;

            case 6:// Hangar Bay
                //There's only one way into the Hangar Bay! That said, I'm not sure if I will allow the player to enter the Hangar Bay.
                waypointFro = travelWaypoints[14];
                break;

            // The only way in and out of the vaults is through the Atrium.
            case 7:// Vault 640
                waypointFro = travelWaypoints[15];
                break;

            case 8:// Vault 641
                waypointFro = travelWaypoints[16];
                break;

            case 9:// Vault 642
                waypointFro = travelWaypoints[17];
                break;

            default:
                Debug.LogError("newRoom index ID = invalid! // T7-64 = returning to command room.");
                waypointTo = standHere[0];
                waypointFro = travelWaypoints[0];
                newRoom = 0;
                break;
        }
        waypointTo = standHere[newRoom];
        //roomID = newRoom;
    }

    //TO-DO: Add recharge time on laser attack.
    public void FireLazor(Vector3 targetV, GameObject target) {
        if (laserTimer >= laserRechargeTime) {
            Vector3 start = transform.position + laserSpawnOffset;
            Laser lazor = Instantiate(laserPrefab, start, transform.rotation).GetComponent<Laser>();
            lazor.Initiate(start, targetV, target, damage, true, room);
            laserTimer = 0;
        }
    }

    public override void TakeDamage(int ow) {
        base.TakeDamage(ow);
        if (health > 0) {
            healthBar.SetState(health);
        } else {
            healthBar.SetState(0);
        }
    }

    protected override void Die() {
        Debug.Log("Dead, you should be.");
    }
    
    public void RepairSelf(int heal) {
        health += heal;
        if (health > maxHealth) {
            health = maxHealth;
        }
        healthBar.SetState(health);
    }
}
