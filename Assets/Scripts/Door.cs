using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int nextLocationIndex;
    //The int that the PlayerManager script will use to determine the next part of the map.
    // Refer to PlayerManager or my design notes for a list of which index leads to which area.
    // NOTE: Use of nextLocationIndex may no longer be needed, but I'm worried I might break something if I remove it, 
    //       and considering we're less than a week from the submission deadline, I'd rather not risk it.
    public Room nextRoom;
    
}
