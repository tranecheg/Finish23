using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Death Race Enemy")]
public class DeathRaceEnemy : ScriptableObject
{

    public string playerName;
    public Sprite playerSprite;

    [Header("Weapon Properties")]
    public string weaponName;
    public float damage;
    public float fireRate;
    public float bulletSpeed;
    public float speedMove;
    public float shootDelay;
    
}
