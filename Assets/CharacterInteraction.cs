using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    //private Vector2 movement;
    private bool damageAnimation = false;
    private bool colliding = false;

    private Rigidbody rb;
    private SpriteRenderer sprite;

    [HideInInspector]
    public List<DamageableObject> collidingObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        //controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && !damageAnimation)
        {
            if (!GameManager.gameManager.Character.healthSystem.Damage(collidingObjects[0].damage))
            {
                colliding = false;
                //GameManager.gameManager.PauseMenu.GameOver();
                GetComponent<Collider>().enabled = false;
                //movement.x = 0;
                //movement.y = 0;
                return;
            }
            
            StartCoroutine(DamageReceived(GameManager.gameManager.Character.DamageDelay.Value));
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyAttack")) && other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            colliding = true;
            collidingObjects.Add(collidingObject);
            collidingObjects.Sort((o1,o2)=>o1.damage.CompareTo(o2.damage));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyAttack")) && collision.gameObject.TryGetComponent<DamageableObject>(out var collidingObject) && !colliding && !damageAnimation)
        {
            if (!GameManager.gameManager.Character.healthSystem.Damage(collidingObject.damage))
            {
                colliding = false;
                //GameManager.gameManager.PauseMenu.GameOver();
                GetComponent<Collider>().enabled = false;
                //movement.x = 0;
                //movement.y = 0;
                return;
            }
            StartCoroutine(DamageReceived(GameManager.gameManager.Character.DamageDelay.Value));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyAttack")) && other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            collidingObjects.Remove(collidingObject);
            if (collidingObjects.Count == 0)
                colliding = false;
        }
       
    }

    private IEnumerator DamageReceived(float damageDelay)
    {
        damageAnimation = true;
        //AudioManager.instance.Play(SoundDamageReceived);
        
        for (int j = 0; j < 10; j++)
        {
            for (float t = 0; t < 1; t += Time.deltaTime / damageDelay * 20f)
            {
                sprite.color = new Color(1, Mathf.SmoothStep(1, 0, t), Mathf.SmoothStep(1, 0, t));
                yield return null;
            }
        
            for (float t = 0; t < 1; t += Time.deltaTime / damageDelay * 20f)
            {
                sprite.color = new Color(1, Mathf.SmoothStep(0, 1, t), Mathf.SmoothStep(0, 1, t));
                yield return null;
            }
        }

        sprite.color = Color.white;
        damageAnimation = false;

    }
}
