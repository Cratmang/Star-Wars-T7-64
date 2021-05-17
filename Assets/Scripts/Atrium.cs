using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atrium : Room
{

    public List<GameObject> pathwayHallToVault0, pathwayHallToVault1, pathwayHallToVault2, pathwayMineToVault0, pathwayMineToVault1, pathwayMineToVault2;

    public override List<GameObject> GetPathway(Enemy e) {

        if (e.spawnID == 1) { //Enemy came from mine
            switch (e.targetVault) {
                case 640:
                    return pathwayMineToVault0;
                case 641:
                    return pathwayMineToVault1;
                case 642:
                    return pathwayMineToVault2;
                default:  //In case somebody (me) messed up...
                    return pathwayMineToVault1;
            }
        } else {
            switch (e.targetVault) {
                case 640:
                    return pathwayHallToVault0;
                case 641:
                    return pathwayHallToVault1;
                case 642:
                    return pathwayHallToVault2;
                default:  //In case somebody (me) messed up...
                    return pathwayHallToVault1;
            }
        }
    }


}
