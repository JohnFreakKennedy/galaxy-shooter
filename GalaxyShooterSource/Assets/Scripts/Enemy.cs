using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;

    private Player _player;
    private Animator _animator;
    private Collider2D _collider;

    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _prefabEnemyLaser;

    private static readonly int EnemyDeath = Animator.StringToHash("EnemyDeath");

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _animator = GetComponent<Animator>();
        if ( _animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("Collider is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.clip = _explosionSound;
        }
        else
        {
            Debug.LogError("AudioSource on the enemy is null");
        }

        StartCoroutine(RandomShootingRoutine());
    }

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }

    private IEnumerator RandomShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Instantiate(_prefabEnemyLaser, transform.position, Quaternion.identity);
        }
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), -transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }

            OnEnemyDeath();
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.DamagePlayer();
            }

            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        _animator.SetTrigger(EnemyDeath);
        _speed = 0;
        Destroy(_collider);
        _audioSource.Play();
        Destroy(gameObject, 1.4f);
    }
}
