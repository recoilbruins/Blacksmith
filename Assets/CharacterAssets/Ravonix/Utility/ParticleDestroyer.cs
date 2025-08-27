using UnityEngine;

namespace Ravonix.Utility
{
    public class ParticleDestroyer : MonoBehaviour
    {
        [Range(1, 15)] public int autoDestroyTime = 5;

        private void Start()
        {
            Destroy(gameObject, autoDestroyTime);
        }
    }
}