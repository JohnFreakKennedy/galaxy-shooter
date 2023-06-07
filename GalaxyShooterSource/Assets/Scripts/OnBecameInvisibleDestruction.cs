using UnityEngine;

public class OnBecameInvisibleDestruction : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
