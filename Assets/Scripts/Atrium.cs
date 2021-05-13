using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atrium : Room
{

    public List<GameObject> pathwayHallToVault0, pathwayHallToVault1, pathwayHallToVault2, pathwayMineToVault0, pathwayMineToVault1, pathwayMineToVault2;

    //TO-DO: When enemies reach this room, they need to decide which vault they're aiming for (assuming they haven't already).
    /*new public List<GameObject> EnterRoom(GameObject newEn, bool escaping) {
        enemiesInRoom.Add(newEn);
        newEn.GetComponent<Enemy>().room = this;

        if (escaping) {
            List<GameObject> escapePath = new List<GameObject>();
            escapePath.AddRange(pathwayHallToVault1); // From my understanding of the example, Reverse() does not simply return a reversed list - it reverses the original list, which is something I don't want.
            escapePath.Reverse();
            return escapePath;
        } else {
            return pathwayHallToVault1;
        }
    }*/

    new public List<GameObject> pathway() {
        //Need to determine path for enemy
        return pathwayHallToVault1;
    }
}
