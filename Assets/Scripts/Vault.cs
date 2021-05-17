using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : Room
{
    public int loot = 6;
    public bool looted = false; //This is to indicate that the vault has been looted, AND all enemies with the loot have brought the loot back.
    public GameObject lootPrefab;
}
