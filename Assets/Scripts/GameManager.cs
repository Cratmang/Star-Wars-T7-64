using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Spawns and manages enemies

    public TextAsset waveInfo;


    //Enemy Spawn Rooms
    public Room hangarBay, mine, vent;
    public bool mineSpawnActive, ventSpawnActive;
    public int mineSpawnWave, ventSpawnWave; // The waves at which the mine and vents will start spawning enemies.

    public Vault[] vaults = new Vault[3];
    
    public int DetermineTargetVault() {
        bool de = false; //de means nothing, I just hit two keys at random.
        int tar = 0;
        
        if (!vaults[0].looted || !vaults[1].looted || !vaults[2].looted) {
            while (!de) {
                tar = Random.Range(0, 3);
                if (!vaults[tar].looted) {
                    tar += 640;
                    de = true;
                }
            }
            return tar;
        } else {
            return 640;
        }
    }

    //When an enemy with loot dies, return the loot to the vault it belongs to.
    public void ReturnLoot(int v) {
        v -= 640;
        vaults[v].loot++;
    }

    //Spawn Timer
    // Spawn enemies over time. Gonna replace this with a Wave-based system.
    public GameObject battleDroid;
    public float spawnTime;
    float timer = 0;

    public List<GameObject> enemies;
    List<string> enemiesToSpawn;
    List<float> waitTimes;
    public int wave;

    // Update is called once per frame
    void Update()
    {
        if (vaults[0].looted && vaults[1].looted && vaults[2].looted) {
            // GAME OVER!
        } else {
            //TO-DO: Set up Wave spawning system

            timer += Time.deltaTime;
            if (timer >= spawnTime) {
                Transform spawnPoint = hangarBay.pathway[0].transform;
                //TO-DO: Set up path randomization. Choose between Vaults 0 - 2, start in the Hangar, the Vent, or the Mine.

                GameObject newEn = Instantiate(battleDroid, spawnPoint.position, spawnPoint.rotation);
                newEn.GetComponent<Enemy>().Initiate(hangarBay, this, 0);
                enemies.Add(newEn);
                timer = 0;
            }
        }
    }

    //When an enemy returns to the Hangar Bay with the last of the vault's treasure, mark that vault as looted, and it's treasure gone.
    public void CheckVaultEmpty(int v) {
        v -= 640;
        if (vaults[v].loot <= 0) {
            vaults[v].looted = true;
        }
    }

    void ParseWave() { 
    
    }
}
