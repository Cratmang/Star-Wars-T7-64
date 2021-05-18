using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    Image image;
    public float[] state;

    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 1f;        
    }

    public void SetState(int s) {
        {
            image.fillAmount = state[s];
        } 
    }
}
