using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStat : ScriptableObject
{
    public int startingHealth = 100;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;
}
