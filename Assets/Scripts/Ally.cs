using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public int health, maxHealth, damage;

    public GameObject laserPrefab;
    public Room room;
    public Vector3 laserSpawnOffset;
    public float laserRechargeTime;
    protected float laserTimer = 0;

    public virtual void TakeDamage(int ow) {
        health -= ow;
        if (health <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        room.alliesInRoom.Remove(this);
        Destroy(gameObject);
    }
}
