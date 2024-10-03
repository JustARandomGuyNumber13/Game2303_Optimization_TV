using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note_TV
{
    /*
    Things to do and script that contain update:
    Pooling system => EnemyManager.cs && EnemyHealth.cs                                                                                                                   // Done
    One class heavy in Update loop => EnemyMovement.cs                                                                                                                        // Done
    New input system                                                                                                
    Hasing for reference to animation names => PlayerHealth.cs && EnemyAttack.cs && EnemyHealth.cs                                            // Done
    Reduce dependencies between PlayerHealth and UI by using Scriptable Object or event => PlayerHealth.cs                                      // Done
    Reduce dependencies between ScoreManager and UI by using Scriptable Object or event => ScoreManager.cs                                 // Done

    + Player Health: 
    Set serializeField and private set for variables
    Change color shifting to event/trigger type
    
    + PlayerMovement:

    + PlayerShooting:

    + EnemyMovement:
    Create new variables to preven using getComponent for optimizing pooling system
    Add more conditions to move logic to match pooling system 
    Change Update method to increase performance

    + EnemyHealth:  * Need to update SINKING to event or something to reduce waiting call by update//////////////////////////////////////////////////////////
    Set serializeField and private set for variables
    Use ScriptableObject to store startingHealth, sinkSpeed, and scoreValue
    Add some variables to prevent using getComponent for optimizing pooling system
    Add pooling system helper methods for Deactivate and Reactivate object for reuse => Call by main pooling system at EnemyManager.cs

    + EnemyAttack: * Need to update ATTACK to event or something to reduce waiting call by update//////////////////////////////////////////////////////////
    Use ScriptableObject to store attackDamage and timeBetweenAttacks

    + EnemyManager:
    Set serializeField for variables
    Create a pooling system base on EnemyHealth.cs
    Create require methods to support pooling system
    Change spawn mechanic to match pooling system behavior

    + Score Manager:
    Change update score to event/trigger type base instead of constantly update
    Create a string to store score text to reuse instead of keep creating a new string to assign (Solve prevent creating GC Allocates => Consume more memories)

     */
}
