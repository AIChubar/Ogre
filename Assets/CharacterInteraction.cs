using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{

    private bool damageAnimation = false;
    private bool colliding = false;

    [HideInInspector]
    public List<DamageableObject> collidingObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* if (colliding && !damageAnimation)
        {
            if (!GameManager.gameManager.Character.healthSystem.Damage(collidingObjects[0].damage))
            {
                colliding = false;
                GameManager.gameManager.PauseMenu.GameOver();
                GetComponent<Collider2D>().enabled = false;
                movement.x = 0;
                movement.y = 0;
                return;
            }
            
            StartCoroutine(DamageReceived(AnimationDuration));
            
        } */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            colliding = true;
            collidingObjects.Add(collidingObject);
            collidingObjects.Sort((o1,o2)=>o1.damage.CompareTo(o2.damage));
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<DamageableObject>(out var collidingObject))
        {
            collidingObjects.Remove(collidingObject);
            if (collidingObjects.Count == 0)
                colliding = false;
        }
       
    }
}
