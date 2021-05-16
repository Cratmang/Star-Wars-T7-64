using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Spawns and manages enemies


    //Enemy Spawn Rooms
    public Room hangarBay, mine, vent;
    public bool mineSpawnActive, ventSpawnActive;

    //Enemy Paths
    // Basically, the waypoints for every enemy path. These are meant to be mix-matched-stitched together to create a single path through the base.
    
    public List<Transform> enemyHangarPath;
    public List<Transform> enemyVentPath; //Enemies should be able to SPAWN from the vent, but they need to go back to the Hangar to escape.
    
    //public List<Transform> enemyVault0PathHangar; // As there are three vaults, the enemy will branch one of three ways upon reaching the Atrium.
    public List<Transform> enemyVault1PathHangar;
    public List<Transform> enemyVault2PathHangar;

    public List<Transform> enemyMinePath;
    public List<Transform> enemyVault0PathMine;
    public List<Transform> enemyVault1PathMine;
    public List<Transform> enemyVault2PathMine;

    public List<Transform> enemyVault0Path;
    public List<Transform> enemyVault1Path;
    public List<Transform> enemyVault2Path;

    public List<Transform> enemyEscapePath0;
    public List<Transform> enemyEscapePath1;
    public List<Transform> enemyEscapePath2;
    public List<Transform> enemyEscapePathToTheChopper; //The final rush through the halls!


    //Spawn Timer
    // Spawn enemies over time. Gonna replace this with a Wave-based system.
    public GameObject battleDroid;
    public float spawnTime;
    float timer = 0;

    public List<GameObject> enemies;

    

    // Update is called once per frame
    void Update()
    {

        //TO-DO: Set up Wave spawning system

        timer += Time.deltaTime;
        if (timer >= spawnTime) {

            GameObject newEn = Instantiate(battleDroid, enemyHangarPath[0].position, enemyHangarPath[0].rotation);
            List<Transform> path = new List<Transform>();

            //TO-DO: Set up path randomization. Choose between Vaults 0 - 2, start in the Hangar, the Vent, or the Mine.
            //*path.AddRange(enemyHangarPath);
            //path.AddRange(enemyVault1PathHangar); 
            //path.AddRange(enemyVault1Path);*/
            newEn.GetComponent<Enemy>().Initiate(hangarBay, this, 0);

            timer = 0;
        }
    }


    //Putting this code in Room.cs instead
    /*public GameObject getTargetInRoom(int roomID) {
        List<GameObject> eList = new List<GameObject>();
        for (int i = 0; i < enemies.Count; i++) {
            Enemy e = enemies[i].GetComponent<Enemy>();
            if (e.roomID == roomID) {
                eList.Add(enemies[i].gameObject);
            }
        }

        if (eList.Count == 0) {
            return null;
        } else {
            int t = Random.Range(0, eList.Count);
            return eList[t];
        }
    }*/

}
