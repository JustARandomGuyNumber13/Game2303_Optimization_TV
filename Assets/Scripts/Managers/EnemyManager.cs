using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth; // Change to private type since nothing else is accessing the infomations _TV_
    //[SerializeField] GameObject enemy;
    [SerializeField] EnemyHealth enemyPrefab;
    [SerializeField] float spawnTime = 3f;
    //public Transform[] spawnPoints;
    [SerializeField] Transform spawnPoint;    // Optimized since there's only one spawn point plugged in the scene _TV_

    IObjectPool<EnemyHealth> _objectPool;  // Pooling system _TV_
    [Tooltip("The amount of enemy spawned when game start")]
    [SerializeField] int enemyDefaultAmount;
    [Tooltip("The max amount of enemy can be spawn during gameplay")]
    [SerializeField] int enemyMaxAmount;

    private NavMeshAgent _agent;
    int count = 0;

    private void Awake()
    {
        _objectPool = new ObjectPool<EnemyHealth>(CreateEnemy, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, false, enemyDefaultAmount, enemyMaxAmount);
        _agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    private void Spawn()    // _TV_
    {
        if (playerHealth.currentHealth > 0 && count < enemyMaxAmount)
        {
            count++;
            EnemyHealth curEnemy = _objectPool.Get();
            if (curEnemy == null) return;
        }
    }
    private EnemyHealth CreateEnemy() // _TV_
    {
        EnemyHealth enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemyInstance.ObjectPool = _objectPool;
        return enemyInstance;
    }
    private void OnGetFromPool(EnemyHealth poolObject)  // _TV_
    {
        if (!poolObject.gameObject.activeInHierarchy)
        {
            poolObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            poolObject.ReactivateAgent();
        }
        poolObject.gameObject.SetActive (true);
    }
    private void OnReleaseToPool(EnemyHealth poolObject)    // _TV_
    {
        count--;
        poolObject.gameObject.SetActive(false);
    }
    private void OnDestroyPoolObject(EnemyHealth poolObject)    // _TV_
    {
        Destroy(poolObject.gameObject);
    }

    //void Update() // Unessesarry function _TV_
    //{

    //}

    //void Spawn ()
    //{
    //    if(playerHealth.currentHealth <= 0f)
    //        return;

    //    int spawnPointIndex = Random.Range(0, spawnPoints.Length);
    //    Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    //}
}
