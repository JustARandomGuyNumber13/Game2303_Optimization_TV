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


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        _myTransform = transform;
        _particalTransform = hitParticles.transform;
        _agent = GetComponent <NavMeshAgent> ();
        _rb = GetComponent <Rigidbody> ();

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
        anim.SetTrigger ("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        isSinking = true;
        //ScoreManager.score += stat.scoreValue;
        ScoreManager.UpdateScore(stat.scoreValue); // _TV_
        //Destroy (gameObject, 2f);
        Deactivate();
    }

    private void Deactivate() // _TV_
    {
        StartCoroutine(DeactivateRoutine(2f));
    }

    /* Deactivate and reset this enemy stats for pooling system */ //_TV_
    IEnumerator DeactivateRoutine(float delay)
    { 
        yield return new WaitForSeconds(delay);
        _objectPool.Release(this);
    }
    public void ReactivateAgent()
    {
        isDead = false;
        _rb.isKinematic = false;
        isSinking = false;
        currentHealth = stat.startingHealth;
        capsuleCollider.isTrigger = false;
        _agent.enabled = true;
    }
}
