using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    [field: SerializeField] private string TARGET ="";
    [field: SerializeField] public bool collidedWithTarget { get; set; } = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(TARGET))
        {
            collidedWithTarget = true;
            Debug.Log($"Weapon collided with target: {other.gameObject.name}");
        }
    }
}
