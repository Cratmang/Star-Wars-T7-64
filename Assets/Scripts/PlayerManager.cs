using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
                Door passage = hit.transform.gameObject.GetComponent<Door>();

                if (passage) {
                    //Change Cursor
                    Cursor.SetCursor(cursorGoTo, hotSpot, cursorMode);
                          

                } else {
                                    
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
