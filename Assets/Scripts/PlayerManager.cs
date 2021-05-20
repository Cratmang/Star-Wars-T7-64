using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //Handles Player movement and UI.

    public T764 tee7;
    
    public List<Transform> cameraPos;
    /*List of Camera Positions:
     * 0 = Command Room
     * 1 = Atrium
     * 2 = Mine Entrance
     * 3 = Mine
     * 4 = Hall B
     * 5 = Hall A
     * 6 = Hangar
     * 7 = Vault 640
     * 8 = Vault 641
     * 9 = Vault 642
     */

    public int prevLocationIndex = 0; //This will start us in the Command room.
    public int nextLocationIndex = 0;

    //What follows is largely recycled code from another project that I worked on a long time ago, then shelved indefinately.

    public Transform[] transforms;

    bool locked = false, mining = false, windowOpen = false;

    public Texture2D cursorNormal;
    public Texture2D cursorGoTo;
    public Texture2D cursorInteract;
    public Texture2D cursorShoot;
    public Texture2D cursorMine;
    public Texture2D cursorCraft;
    public Texture2D cursorLocked;

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero, targetHotspot = new Vector2(14, 14);


    public Text[] resourceCounters;
    private int[] resourceCount = new int[6];
    /* 0 = Kyber Crystals
     * 1 = Scrap
     * 2 = Circuits
     * 3 = Oil
     * 4 = Fuel Cells
     * 5 = Metal Ore
     */

    public GameObject[] sentryPrefabs = new GameObject[3];
    private int[] sentriesPocketed = new int[3];
    public GameObject[] sentryButtons = new GameObject[3];
    /* 0 = Scrap Turret
     * 1 = Standard Turret
     * 2 = Phaux Jedi
     */

    float timer;
    public float frameTime, interactTime;
    GameObject interactTarget;

    public GameObject craftingWindow;

    public void OpenWindow(GameObject window) {
        window.SetActive(true);
        windowOpen = true;
    }
    public void CloseWindow(GameObject window) {
        window.SetActive(false);
        windowOpen = false;
    }

    private void Start() {
        //SwitchPosition(0);
        //In case I somehow make this mistake again, these statements set the POSITION, then the ROTATION of the camera.
        //  If they look the same, it's because a) they have the same number of characters, and b) your brain has enabled
        //  dyslexic mode.
        transform.position = tee7.room.cameraTransform.position;
        transform.rotation = tee7.room.cameraTransform.rotation;
        UpdateTurretCounter(0);
        UpdateTurretCounter(1);
        tee7.room.playerHere = true;
    }

    void UpdateTurretCounter(int i) {
        if (sentriesPocketed[i] <= 0) {
            sentryButtons[i].SetActive(false);
        } else {
            sentryButtons[i].SetActive(true);
            sentryButtons[i].GetComponentInChildren<Text>().text = sentriesPocketed[i].ToString();
        }
    }

    // Update is called once per frame
    void Update() {
        //TO DO: Change cursor when it hovers over interactable object or passageway

        if (locked) {
            Cursor.SetCursor(cursorLocked, hotSpot, cursorMode);

            if (mining) {
                timer += Time.deltaTime;

                if (timer >= interactTime) {
                    interactTarget.GetComponent<OreVein>().SpawnResources();
                    mining = false;
                    locked = false;
                }
            }


        } else {

            bool f = false;
            if (!windowOpen) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {

                    Resource re = hit.transform.gameObject.GetComponent<Resource>();
                    if (re) {
                        if (re.ripe) {
                            //Gather Resources
                            resourceCount[re.indexID]++;
                            resourceCounters[re.indexID].text = resourceCount[re.indexID].ToString();
                            Destroy(re.gameObject);
                            //TO-DO: Add animation of resource being added to inventory.
                        }
                    } 
                
                    
                    Door passage = hit.transform.gameObject.GetComponent<Door>();
                    if (passage) {
                        Cursor.SetCursor(cursorGoTo, hotSpot, cursorMode);
                        f = true;
                    }
                
                    Enemy en = hit.transform.gameObject.GetComponent<Enemy>();
                    if (en) {
                        Cursor.SetCursor(cursorShoot, targetHotspot, cursorMode);
                        f = true;

                    }

                    OreVein ov = hit.transform.gameObject.GetComponent<OreVein>();
                    if (ov) {
                        // TO-DO: Draw mining cursor
                        Cursor.SetCursor(cursorGoTo, hotSpot, cursorMode);
                        f = true;
                    }

                    Terminal tem = hit.transform.gameObject.GetComponent<Terminal>();
                    if (tem) {
                        // TO-DO: Draw terminal interact cursor
                        Cursor.SetCursor(cursorGoTo, hotSpot, cursorMode);
                        f = true;
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    if (Physics.Raycast(ray, out hit)) {

                        //Door to next room
                        Door passage = hit.transform.gameObject.GetComponent<Door>();
                        if (passage) {

                            //nextLocationIndex = passage.nextLocationIndex;
                            //SwitchPosition(nextLocationIndex);

                            //Again, POSITION, then ROTATION.
                            transform.position = passage.nextRoom.cameraTransform.position;
                            transform.rotation = passage.nextRoom.cameraTransform.rotation;

                            //Remove T7-64 from the previous room, add him to the next room.
                            tee7.room.alliesInRoom.Remove(tee7);
                            tee7.room.playerHere = false;
                            tee7.TravelTo(passage.nextRoom.roomID);
                            tee7.room = passage.nextRoom;
                            tee7.room.alliesInRoom.Add(tee7);
                            tee7.room.playerHere = true;

                            //prevLocationIndex = nextLocationIndex;
                            LockInput();
                        }

                        Enemy en = hit.transform.gameObject.GetComponent<Enemy>();
                        if (en) {
                            tee7.FireLazor(hit.point, hit.transform.gameObject);
                        }

                        OreVein ov = hit.transform.gameObject.GetComponent<OreVein>();
                        if (ov) {
                            LockInput();
                            interactTime = ov.mineTime;
                            timer = 0;
                            mining = true;
                            interactTarget = ov.gameObject;
                        }
                    }
                }
            }

            //The mouse isn't hovering over anything special.
            if (!f) {
                Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
            }
        }
    }

    public void CraftTurret(int type) {
        int[] cost = new int[6];
        
        //Get cost of turret to craft
        switch (type) {
            case 0:// Scrap Turret
                cost[0] = 0;
                cost[1] = 4;
                cost[2] = 2;
                cost[3] = 0;
                cost[4] = 0;
                cost[5] = 0;
                break;
            case 1:// Defence Turret
                cost[0] = 0;
                cost[1] = 6;
                cost[2] = 4;
                cost[3] = 2;
                cost[4] = 1;
                cost[5] = 6;
                break;
            case 2:// JED1 Prototype Training Droid
                cost[0] = 1;
                cost[1] = 7;
                cost[2] = 6;
                cost[3] = 4;
                cost[4] = 2;
                cost[5] = 10;
                break;
        }

        //Check if you have the resources to craft the turret
        bool afford = true;
        for (int i = 0; i < cost.Length; i++) {
            if (cost[i] > resourceCount[i]) {
                afford = false;
            }
        }

        //If yes, craft the turret requested.
        if (afford) {
            for (int i = 0; i < cost.Length; i++) {
                resourceCount[i] -= cost[i];
                resourceCounters[i].text = resourceCount[i].ToString();
            }
            sentriesPocketed[type]++;
            UpdateTurretCounter(type);
        } else {
            Debug.Log(sentryPrefabs[type].name + " = cannot craft. // Resources = insufficient quantity.");
        }

    }

    public void PlaceTurret(int type) {

        if (sentriesPocketed[type] > 0) {
            if (tee7.room.PlaceSentry(sentryPrefabs[type])) {
                sentriesPocketed[type]--;
                UpdateTurretCounter(type);
            } else {
                Debug.Log("Here = cannot place sentry.");
            }
        } else {
            Debug.Log("Requested sentry = 0 available.");
        }
    } 

    public void LockInput(bool dia = false) {
        locked = true;
    }

    public void UnlockInput() {
        locked = false;
    }

    public void SwitchPosition(int index) {
        transform.position = cameraPos[index].position;
        transform.rotation = cameraPos[index].rotation;
    }

    public void PowerMiner(AutoMiner am) {
        
        if (resourceCount[4] > 0) {
            resourceCount[4]--;
            resourceCounters[4].text = resourceCount[4].ToString();
            am.Power();

        } else {
            // "Error = no Fuel Rods available"
        }
    }

    public void RepairSelf(int what) {
        int healAmount;
        switch (what) {
            case 1: // Repair with scrap - 1 scrap = 1 health
                healAmount = 1;
                break;
            case 5: // Repair with ore - 1 ore = 3 health
                healAmount = 3;
                break;
            default: // Repairs should not be possible with any other material
                Debug.Log("Selected resource = cannot be used to heal");
                return;
        }

        if (resourceCount[what] > 0 && tee7.health < tee7.maxHealth) {
            resourceCount[what]--;
            tee7.RepairSelf(healAmount);
            resourceCounters[what].text = resourceCount[what].ToString();
        }
    }
}
