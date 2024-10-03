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
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _playerHealth = _playerTransform.GetComponent<PlayerHealth>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        InvokeRepeating("UpdateDestination", 0f, 0.2f);
    }
    private void UpdateDestination()
    {
        if (_enemyHealth.currentHealth > 0 && _playerHealth.currentHealth > 0)
        {
            if(_agent.isOnNavMesh && _agent.enabled)    // Prevent trying to move when is being reuse/deactivated, and to not disable agent component 
                _agent.SetDestination(_playerTransform.position);
        }
        else
            _agent.enabled = false;
    }
}
