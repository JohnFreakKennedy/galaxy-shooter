using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3f;
    [SerializeField]
    private GameObject _prefabExplosion;
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate( _rotateSpeed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            GameObject explosion = Instantiate(_prefabExplosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(gameObject);
            Destroy(explosion, 3f);
        }
    }
}
