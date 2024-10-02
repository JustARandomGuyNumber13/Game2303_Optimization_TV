using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    PlayerHealth _playerHealth;
    Transform _playerTransform;
    NavMeshAgent _agent;
    EnemyHealth _enemyHealth;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;  // Lighter than FindObjectOfType<> since tag was used in the Inspector, could have used GameObject.Find() but I prefer tag _TV_
        _playerHealth = _playerTransform.GetComponent<PlayerHealth>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        InvokeRepeating("UpdateDestination", 0f, 0.2f); // Custom update function for this case _TV_
    }
    private void UpdateDestination()
    {
        if (_enemyHealth.currentHealth > 0 && _playerHealth.currentHealth > 0)
        {
            if(_agent.isOnNavMesh && _agent.enabled)
                _agent.SetDestination(_playerTransform.position);
        }
        else
            _agent.enabled = false;
    }

    //void Update ()
    //{
    //    Transform player = FindObjectOfType<PlayerMovement>().transform;

    //    if (GetComponent<EnemyHealth>().currentHealth > 0 && player.GetComponent<PlayerHealth>().currentHealth > 0)
    //    {
    //        GetComponent<NavMeshAgent>().SetDestination (player.position);
    //    }
    //    else
    //    {
    //        GetComponent<NavMeshAgent>().enabled = false;
    //    }
    //}
}
