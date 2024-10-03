using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    public int currentHealth { get; private set; }
    [SerializeField] Slider healthSlider;
    [SerializeField] Image damageImage;
    [SerializeField] AudioClip deathClip;
    [SerializeField] float flashSpeed = 5f;
    [SerializeField] Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    private readonly int dieTriggerHash = Animator.StringToHash("Die");

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    private IEnumerator TakeDamageColorChangeCoroutine()    // Event/trigger type for color change instead of keep asking for it to change in update _TV_
    {
        damageImage.color = flashColour;

        while (damageImage.color != Color.clear)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            yield return null;  
        }
    }
    public void TakeDamage (int amount) // Not sure what to do since this already is an event/trigger type _TV_
    {
        StopCoroutine(TakeDamageColorChangeCoroutine());
        StartCoroutine(TakeDamageColorChangeCoroutine());
        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }
    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger (dieTriggerHash);

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }
    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
