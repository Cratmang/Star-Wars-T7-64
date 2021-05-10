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

    bool locked = false;

    public Texture2D cursorNormal;
    public Texture2D cursorGoTo;
    public Texture2D cursorInteract;
    public Texture2D cursorShoot;
    public Texture2D cursorMine;
    public Texture2D cursorCraft;
    public Texture2D cursorLocked;

    //public GameManager gm;
    
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public Text[] resourceCounters;
    private int[] resourceCount = new int[6];

    float timer;
    public float frameTime;


    private void Start() {
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        SwitchPosition(prevLocationIndex);
    }


    // Update is called once per frame
    void Update() {
        //TO DO: Change cursor when it hovers over interactable object or passageway

        if (locked) {

           Cursor.SetCursor(cursorLocked, hotSpot, cursorMode);
           
        } else {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

               
            if (Physics.Raycast(ray, out hit)) {
                bool f = false;
                Door passage = hit.transform.gameObject.GetComponent<Door>();
                if (passage) {
                    Cursor.SetCursor(cursorGoTo, hotSpot, cursorMode);
                    f = true;

                }

                Resource re = hit.transform.gameObject.GetComponent<Resource>();
                if (re) { 
                    //Gather Resources
                    resourceCount[re.indexID]++;
                    resourceCounters[re.indexID].text = resourceCount[re.indexID].ToString();
                    Destroy(re.gameObject);
                    //TO-DO: Add animation of resource being added to inventory.
                } 
                
                if (!f) {             
                    //TO DO - Interactable object, enemy, etc. 
                    Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
                }

            } else {
                Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
            }

            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(ray, out hit)) {
                    Door passage = hit.transform.gameObject.GetComponent<Door>();

                    if (passage) {

                        nextLocationIndex = passage.nextLocationIndex;
                        SwitchPosition(nextLocationIndex);

                        //Give Previous location and Next Location to T7-64
                        tee7.TravelTo(nextLocationIndex);

                        prevLocationIndex = nextLocationIndex;
                        LockInput();
                    }
                }
            }
        }
    }



    /*int CurrentLockedFrame(bool dia) {
        timer += Time.deltaTime;
        float maxTime;

        if (dia) {
            maxTime = frameTime * cursorDia.Length;
        } else {
            maxTime = frameTime * cursorLocked.Length;
        }

        if (timer >= maxTime) {
            timer = 0;
        }

        int frameNum = Mathf.FloorToInt(timer / frameTime);
        return frameNum;

    }*/

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
}
