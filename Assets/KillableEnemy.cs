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
    
    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        if (healthSystem.Health == 0 || OneShotEnemy)
        {
            StartCoroutine(DeathAnimation(0.03f));
        }
    }
    
    private IEnumerator DeathAnimation(float duration)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        for (float t = 0; t < 1; t += Time.deltaTime/duration)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        Destroy(gameObject);
    }
    
}
