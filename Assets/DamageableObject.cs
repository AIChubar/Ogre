using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script that is attached to any enemy or object that can do damage.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DamageableObject : MonoBehaviour
{
    [Tooltip("Base object damage")]
    [SerializeField]
    public float damage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
}
