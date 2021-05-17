using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallB : Room
{
    public List<GameObject> ventPath;


    public override List<GameObject> GetPathway(Enemy e) { 
        if (e.spawnID == 2) { //IT CRAWLED OUT OF THE VENT!
            return ventPath;
        } else {
            return pathway;
        }
    
    }
}
