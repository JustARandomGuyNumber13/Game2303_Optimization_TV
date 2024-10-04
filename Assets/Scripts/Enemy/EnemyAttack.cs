using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyStat stat;    // ScriptableObject _TV_


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    private readonly int playerDieTriggerHash = Animator.StringToHash("PlayerDead");
    private bool _isAttacking;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
            if (!_isAttacking)  // Prevent calling another coroutine when one is active _TV_
            {   
                _isAttacking = true;
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInRange = false;
    }


    IEnumerator AttackCoroutine()   // Change attack update to event/trigger base _TV_
    {
        while (playerInRange)
        {
            if (enemyHealth.currentHealth > 0)
            {
                Attack();
                if (enemyHealth.currentHealth <= 0)
                {
                    _isAttacking = false;
                    anim.SetTrigger(playerDieTriggerHash);
                    yield break;
                }

                yield return new WaitForSeconds(stat.timeBetweenAttacks);   // Attack cool down _TV_
            }
            yield return null;
        }
        _isAttacking = false;
    }
    void Attack()
    {
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(stat.attackDamage);
        }
    }
}
