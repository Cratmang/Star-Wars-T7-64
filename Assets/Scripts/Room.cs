using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomID;

    public List<Enemy> enemiesInRoom;
    public List<Ally> alliesInRoom;
    public List<GameObject> pathway;

    public Transform cameraTransform;

    public Transform[] sentryTransforms;
    private GameObject[] sentries;
    

    public bool Equals(Room r) {
        if (roomID == r.roomID) {
            return true;
        } else {
            return false;
        }
    }

    public virtual List<GameObject> GetPathway(Enemy e) {
        return pathway;
    }

    public bool PlaceSentry(GameObject sen) {
        int g = System.Array.IndexOf(sentries, null);
        if (g == -1) { //If my understanding of Array.IndexOf is correct, then it SHOULD return -1 if the index is full. But I could be wrong...
            return false;
        } else {
            GameObject newSen = Instantiate(sen, sentryTransforms[g]);
            sentries[g] = newSen;
            return true;
        }

    }



    public Enemy TargetEnemy() {
        if (enemiesInRoom.Count == 0) {
            return null;
        } else {
            int i = Random.Range(0, enemiesInRoom.Count);
            return enemiesInRoom[i];
        }
    }

    public Ally TargetAlly() {
        if (alliesInRoom.Count == 0) {
            return null;
        } else {
            int i = Random.Range(0, alliesInRoom.Count);
            return alliesInRoom[i];
        }
    }
}
