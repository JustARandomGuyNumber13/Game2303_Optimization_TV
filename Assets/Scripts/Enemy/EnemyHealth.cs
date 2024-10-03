using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth { get; private set; }
    [SerializeField] AudioClip deathClip;

    [SerializeField] EnemyStat stat;    // ScriptableObject _TV_
    private IObjectPool<EnemyHealth> _objectPool;   // Create pooling system base on this script EnemyHealth.cs
    public IObjectPool<EnemyHealth> ObjectPool { set => _objectPool = value; }
    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Transform _myTransform; 
    private Transform _particalTransform;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;
    private readonly int dieTriggerHash = Animator.StringToHash("Dead");


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        _agent = GetComponent <NavMeshAgent> ();
        _rb = GetComponent <Rigidbody> ();
        _myTransform = transform;
        _particalTransform = hitParticles.transform;
        currentHealth = stat.startingHealth;
    }
    void Update ()
    {
        if(isSinking)
        {
            _myTransform.Translate (-Vector3.up * stat.sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;

        _particalTransform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }
    void Death ()
    {
        _agent.enabled = false;
        _rb.isKinematic = true;
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger (dieTriggerHash);
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }
    public void StartSinking ()
    {
        isSinking = true;
        ScoreManager.UpdateScore(stat.scoreValue); // _TV_
        Invoke("Deactivate", 2f);
    }
    private void Deactivate()
    {
        _objectPool.Release(this);
    }
    public void ReactivateEnemy()   // For pooling system to use _TV_
    {
        isDead = false;
        _rb.isKinematic = false;
        isSinking = false;
        currentHealth = stat.startingHealth;
        capsuleCollider.isTrigger = false;
        _agent.enabled = true;
    }
}
