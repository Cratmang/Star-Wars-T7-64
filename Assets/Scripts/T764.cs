﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T764 : MonoBehaviour
{
    public int hp = 6;

    public PlayerManager pm;

    public List<Transform> standHere;
    public List<Transform> travelWaypoints;
    public Transform waypointTo, waypointFro;
    int room;
    //Refer to list of CameraPos


    public float travelTime = 0.5f;
    public bool traveling = false;
    float timer;

    
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
    }

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
                switch (room) {
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
                switch (room) {
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
                switch (room) {
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
                switch (room) {
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
        room = newRoom;
    }
}