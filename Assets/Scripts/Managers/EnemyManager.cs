using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    [Header("Script components")]
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] EnemyHealth enemyPrefab;
    [SerializeField] Transform spawnPoint;    // Optimized since there's only one spawn point plugged in the scene _TV_
    [SerializeField] float spawnTime = 3f;


    [Header("Pooling system components")]
    [Tooltip("The amount of enemy spawned when game start")] [SerializeField] int enemyDefaultAmount;
    [Tooltip("The max amount of enemy can be spawn during gameplay")] [SerializeField] int enemyMaxAmount;
    IObjectPool<EnemyHealth> _objectPool;  // Pooling system _TV_
    int count = 0;

    private void Awake()
    {
        _objectPool = new ObjectPool<EnemyHealth>(OnCreateEnemy, OnGetFromPool, OnReleaseToPool, OnDestroyPoolObject, false, enemyDefaultAmount, enemyMaxAmount);
    }
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
    private void Spawn()    
    {
        if (playerHealth.currentHealth > 0 && count < enemyMaxAmount)   // Prevent overspawn, which Unity should have do it, but it doesn't for unknown reason _TV_
        {
            count++;
            EnemyHealth curEnemy = _objectPool.Get();
            if (curEnemy == null) return;
        }
    }

    /* Require methods for Unity's pooling system _TV_ */
    private EnemyHealth OnCreateEnemy() 
    {
        EnemyHealth enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemyInstance.ObjectPool = _objectPool;
        return enemyInstance;
    }
    private void OnGetFromPool(EnemyHealth poolObject)  
    {
        if (!poolObject.gameObject.activeInHierarchy)
        {
            poolObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            poolObject.ReactivateEnemy();
        }
        poolObject.gameObject.SetActive (true);
    }
    private void OnReleaseToPool(EnemyHealth poolObject)    
    {
        count--;
        poolObject.gameObject.SetActive(false);
    }
    private void OnDestroyPoolObject(EnemyHealth poolObject)   
    {
        Destroy(poolObject.gameObject);
    }
}
