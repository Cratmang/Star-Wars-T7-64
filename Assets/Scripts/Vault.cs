using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : Room
{
    public int loot = 6;
    public bool looted = false; //This is to indicate that the vault has been looted, AND all enemies with the loot have brought the loot back.
    public GameObject lootPrefab;

    private void Update() {
        if (playerHere) {
            mapImage.sprite = mapSprite[2];
        } else if (enemiesInRoom.Count > 0) {
            mapImage.sprite = mapSprite[1];
        } else if (loot <= 0) {
            mapImage.sprite = mapSprite[3];
        } else { 
            mapImage.sprite = mapSprite[0];
        }
    }
}
