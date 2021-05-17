using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Spawns and manages enemies

    


    //Enemy Spawn Rooms
    public Room hangarBay, mine;
    public HallB vent;
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
    public GameObject battleDroid, 
                      bdCommander/*, 
                      superDroid, 
                      droideka*/;
    public float spawnTime;
    float timer = 0;

    public List<Enemy> enemies;
    List<GameObject> enemiesToSpawn = new List<GameObject>();
    List<float> waitTimes = new List<float>();
    public int wave;
    bool waveInProgress = false;
    public float timeBetweenWaves;
    //Retrieve wave info from file
    public TextAsset waveInfoFile;
    string[] waves;

    private void Awake() {
        waves = waveInfoFile.text.Split('\n');
        timer = timeBetweenWaves;
        waveInProgress = false;
    }
    //TO DO: Wave counter UI, Time Until Next Wave UI

    // Update is called once per frame
    void Update()
    {
        if (vaults[0].looted && vaults[1].looted && vaults[2].looted) {
            // GAME OVER!
        } else {

            if (waveInProgress) {

                timer += Time.deltaTime;
                if (timer >= spawnTime && enemiesToSpawn.Count > 0) {
                    

                    //Determine new enemy's spawn
                    Transform spawnPoint;
                    Room spawnRoom = hangarBay; // Default spawn location.
                    int spawnID = Random.Range(0, 3);
                    switch (spawnID) {
                        case 1:
                            if (mineSpawnActive) { // Could probably replace this with (wave >= mineSpawnWave) if I have no intention of turning it back off.
                                spawnRoom = mine;
                            } else {
                                spawnID = 0;
                            }
                            break;
                        case 2:
                            if (ventSpawnActive) {
                                spawnRoom = vent;
                            } else {
                                spawnID = 0;
                            }
                            break;
                    }
                    if (spawnID == 2) { // WHY ARE THERE ROBOTS IN THE VENTS!?
                        spawnPoint = vent.ventPath[0].transform;
                    } else {
                        spawnPoint = spawnRoom.pathway[0].transform;
                    }

                    //Spawn new  enemy
                    Enemy newEn = Instantiate(enemiesToSpawn[0], spawnPoint.position, spawnPoint.rotation).GetComponent<Enemy>();
                    newEn.Initiate(spawnRoom, this, spawnID);
                    enemies.Add(newEn);
                    timer = 0;
                    enemiesToSpawn.RemoveAt(0);

                    //Time to spawn next enemy
                    if (waitTimes.Count > 0) {
                        spawnTime = waitTimes[0];
                        waitTimes.RemoveAt(0);
                    }
                }

                if (enemies.Count == 0 && enemiesToSpawn.Count == 0) {
                    //All enemies have been defeated. You've succesfully cleared the wave! Give yourself a pat on the back.
                    waveInProgress = false;
                    timer = timeBetweenWaves;
                }

            } else {

                timer -= Time.deltaTime;
                
                //Update Time Until Next Wave UI Counter (TO DO)
                
                if (timer <= 0) {
                    //Hide Time Until Next Wave

                    waveInProgress = true;
                    wave++;
                    ParseWave();
                    spawnTime = 0;
                }

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
        string[] waveContentStrings = waves[wave - 1].Split(',');
        int nubOEns = 0;
        for (int i = 0; i < waveContentStrings.Length; i += 2) {

            string enemyType = waveContentStrings[i].ToUpper();
            switch (enemyType) {
                case "A":// Standard Battle Droid 
                    enemiesToSpawn.Add(battleDroid);
                    break;
                case "B":// Battle Droid Commander
                    enemiesToSpawn.Add(bdCommander);
                    //TO DO
                    break;
                case "C":// Super Battle Droid
                    //TO DO
                    break;
                case "D":// Droideka
                    //TO DO (lowest priority, but would be cool)
                    break;
                default: // This should not happen ever, but just in case...
                    //enemiesToSpawn.Add(battleDroid);
                    break;
            }
            nubOEns++;

            // The number immediately afterword should be the amount of time (in seconds) until the next enemy will spawn.
            if (i < waveContentStrings.Length - 1) {
                float timeToNextEnemy = 0;
                float.TryParse(waveContentStrings[i + 1], out timeToNextEnemy);
                waitTimes.Add(timeToNextEnemy);
            }

        }

        Debug.Log(nubOEns);
    }
}
