using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that is attached to any enemy or object that can be killed.
/// </summary>
[RequireComponent(typeof(Collider))]
public class KillableEnemy : MonoBehaviour
{
    [SerializeField]
    private float MaxHealth;

    [HideInInspector]
    public HealthSystem healthSystem;
    
    private bool damageAnimationPlaying = false;
    [Header("Default death animation defined in this script")]
    [SerializeField]
    private bool DeafultDeath;
    [Header("When damage is done instantly dies")]
    [SerializeField]
    private bool OneShotEnemy;


    private SpriteRenderer sprite;

    // Start is called before the first frame update
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        healthSystem = new HealthSystem(MaxHealth);
        if (DeafultDeath)
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttack") && other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            if (!damageAnimationPlaying)
                    healthSystem.Damage(collidingObject.damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("PlayerAttack") && collision.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            if (!damageAnimationPlaying)
                    healthSystem.Damage(collidingObject.damage);
        }
    }
    
    
    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        if (healthSystem.Health == 0 || OneShotEnemy)
        {
            StartCoroutine(DeathAnimation(0.5f));
        }
        else
        {
            StartCoroutine(DamageReceivedAnimation(0.5f));
        }
    }

    private IEnumerator DamageReceivedAnimation(float duration)
        {
            damageAnimationPlaying = true;
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 4)
            {
                sprite.color = new Color(Mathf.SmoothStep(1, 0.1f, t), Mathf.SmoothStep(1, 0.1f, t), Mathf.SmoothStep(1, 0.1f, t));
                yield return null;
            }
            yield return new WaitForSeconds(duration * 1/2);
            for (float t = 0; t < 1; t += Time.deltaTime / duration * 4)
            {
                sprite.color = new Color(Mathf.SmoothStep(0.1f, 1, t), Mathf.SmoothStep(0.1f, 1, t), Mathf.SmoothStep(0.1f, 1, t));
                yield return null;
            }
            sprite.color = Color.white;
            damageAnimationPlaying = false;
    }
    
    private IEnumerator DeathAnimation(float duration)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        for (float t = 0; t < 1; t += Time.deltaTime/duration)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        Destroy(gameObject);
    }
    
}
