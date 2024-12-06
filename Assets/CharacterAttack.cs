using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class CharacterAttack : MonoBehaviour
{



    private PlayerControls playerControls;

    [SerializeField] private GameObject attackObject;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
     private void OnEnable()
    {
        playerControls.Enable();
    }

    private float attackTimer = 0.0f;
    private float attackReactionDelay = 0.4f;


    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (playerControls.Player.Attack.IsPressed())
        {
            attackReactionDelay -= Time.deltaTime;
            if (attackTimer >= GameManager.gameManager.Character.ShootingDelay.Value && (attackReactionDelay < 0f || SystemInfo.deviceType == DeviceType.Desktop))
            {
                StartCoroutine(Attack());
                attackTimer = 0.0f;
            }
        }
    }

    IEnumerator Attack()
    {
        attackObject.SetActive(true);
        float rotationTime = 0.6f; // Total time for 3 rotations
        float elapsedTime = 0f;
        float totalRotation = 1080f; // 3 times 360 degrees
        Quaternion initialRotation = attackObject.transform.rotation; // Store initial rotation

        while (elapsedTime < rotationTime)
        {
            float rotationStep = totalRotation * Time.deltaTime / rotationTime;
            attackObject.transform.Rotate(Vector3.up, rotationStep, Space.World); // Rotate around y-axis in global space
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        attackObject.transform.rotation = initialRotation; // Reset to initial rotation
        attackObject.SetActive(false);
    }
}
