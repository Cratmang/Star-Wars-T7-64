using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomID;
    public Room nextRoom, prevRoom;

    public List<GameObject> enemiesInRoom;
    public List<GameObject> alliesInRoom;
    public List<GameObject> pathway;

    public GameObject[] sentries;

    //This is for the enemies
    public List<GameObject> EnterRoom(GameObject newEn) {
        enemiesInRoom.Add(newEn);
        newEn.GetComponent<Enemy>().room = this;
        return pathway;
    }
    public Room LeaveRoom(GameObject byeEn, bool escaping) {
        enemiesInRoom.Remove(byeEn);
        if (escaping) {
            return prevRoom;
        } else {
            return nextRoom;
        }
    }

    //This is for the player
    public void EnterRoom(T764 them) {
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
