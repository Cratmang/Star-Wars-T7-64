using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public int roomID;

    public List<Enemy> enemiesInRoom;
    public List<Ally> alliesInRoom;
    public List<GameObject> pathway;

    public Transform cameraTransform;

    public Transform[] sentryTransforms;
    public int sentries = 0;

    public Image mapImage;
    public Sprite[] mapSprite = new Sprite[3];
    /* 0 = Normal
     * 1 = Enemies here
     * 2 = Player here
     */

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
        
        if (sentries >= sentryTransforms.Length) { 
            return false;
        } else {
            int g = 0;
            while (sentryTransforms[g].childCount != 0) {
                if (g >= sentryTransforms.Length) {
                    return false; //We shouldn't get here, but just in case...
                } else {
                    g++;
                }
            }
            Sentry newSen = Instantiate(sen, sentryTransforms[g]).GetComponent<Sentry>();
            newSen.Initiate(this);
            sentries++;
            alliesInRoom.Add(newSen);
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

    public bool playerHere;

    private void Update() {
        if (playerHere) {
            mapImage.sprite = mapSprite[2];
        } else if (enemiesInRoom.Count > 0) {
            mapImage.sprite = mapSprite[1];
        } else {
            mapImage.sprite = mapSprite[0];
        }
    }
}
