using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public GameObject window;
    public PlayerManager pm;

    private void OnMouseDown() {
        pm.OpenWindow(window);
        
    }
}
