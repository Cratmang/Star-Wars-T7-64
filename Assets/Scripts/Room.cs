using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomID;
    public Room[] connectedRooms;
    public Room nextRoom, prevRoom;
    public Door[] exits;

    public List<GameObject> enemiesInRoom;
    public List<GameObject> alliesInRoom;
    public List<GameObject> pathway;

    public Transform cameraTransform;

    public GameObject[] sentries;

    public bool Equals(Room r) {
        if (roomID == r.roomID) {
            return true;
        } else {
            return false;
        }
    }


    //This is for the enemies
    //Returns the path for the enemies to follow through the room.
    public List<GameObject> EnterRoom(GameObject newEn, bool escaping) {
        enemiesInRoom.Add(newEn);
        newEn.GetComponent<Enemy>().room.LeaveRoom(newEn);
        newEn.GetComponent<Enemy>().room = this;
        
        if (escaping) {
            List<GameObject> escapePath = new List<GameObject>();
            escapePath.AddRange(pathway); // From my understanding of the example, Reverse() does not simply return a reversed list - it reverses the original list, which is something I don't want.
            escapePath.Reverse();
            return escapePath;
        } else {
            return pathway;
        }
    }
    public void LeaveRoom(GameObject byeEn) {
        enemiesInRoom.Remove(byeEn);
        Debug.Log("Welcome, friend!");
    }

    public List<GameObject> EnemySpawning(GameObject en) {
        enemiesInRoom.Add(en);
        en.GetComponent<Enemy>().room = this;
        return pathway;
    }



    //This is for the player
    public void EnterRoom(T764 them) {
        them.room.alliesInRoom.Remove(them.gameObject);
        them.room = this;
        alliesInRoom.Add(them.gameObject);
        //them.roomID = roomID;
    }





    public bool IsInRoom(GameObject thing, bool friend) {
        if (!friend) {
            if (enemiesInRoom.Contains(thing)) {
                return true;
            } else {
                return false;
            }
        } else {
            if (alliesInRoom.Contains(thing)) {
                return true;
            } else {
                return false;
            }
        }
    }


    public GameObject TargetEnemy() {
        if (enemiesInRoom.Count == 0) {
            return null;
        } else {
            int i = Random.Range(0, enemiesInRoom.Count);
            return enemiesInRoom[i];
        }
    }

    public GameObject TargetAlly() {
        if (alliesInRoom.Count == 0) {
            return null;
        } else {
            int i = Random.Range(0, alliesInRoom.Count);
            return enemiesInRoom[i];
        }
    }
}
