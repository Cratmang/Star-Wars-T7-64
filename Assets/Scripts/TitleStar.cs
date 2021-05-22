using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStar : MonoBehaviour
{
    public Vector3 speed;

    private void Update() {
        transform.position += speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
