using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
    }
}
