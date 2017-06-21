using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour {
    string Name = "Weapons";
    public BASE_Weapon[] weapons;
    public BASE_Weapon weaponEquiped { get { return weapons[choice];} }
    public int choice;
    public int Choice { get { return choice%weapons.Length; } set { choice = value % weapons.Length == -1 ? weapons.Length-1: value % weapons.Length; } }




}
