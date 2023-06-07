using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _prefabLaser;
    [SerializeField]
    private GameObject _prefabTripleShot;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _tripleShotActive = false;
    private bool _shieldActive = false;
    [SerializeField]
    private GameObject _playerShield;

    [SerializeField]
    private GameObject _leftEngineDamaged, _rightEngineDamaged;

    [SerializeField]
    private float _speedBoost = 3f;
    [SerializeField]
    private float _powerUpCooldown = 5f;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.clip = _laserSound;
        }
        else
        {
            Debug.LogError("AudioSource on the player is null");
        }
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        if (_horizontalInput != 0 || _verticalInput != 0)
        {
            MovePlayer(_horizontalInput, _verticalInput);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Shoot();
        }
    }

    /// <summary>
    /// Shoot method spawns a laser beam
    /// </summary>
    private void Shoot()
    {
        Vector3 position = transform.position;
        if (_tripleShotActive)
        {
            Instantiate(_prefabTripleShot, position, Quaternion.identity);
        }
        else
        {
            position += new Vector3(0, 1.05f, 0);
            Instantiate(_prefabLaser, position, Quaternion.identity);
        }

        _audioSource.Play();
    }

    /// <summary>
    /// MovePlayer method moves the player by horizontal and vertical axis
    /// </summary>
    private void MovePlayer(float horizontalInput, float verticalInput)
    {
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(_speed * Time.deltaTime * direction);
        ScreenWrapper();
    }

    /// <summary>
    /// ScreenWrapper makes the player stay in camera sight
    /// </summary>
    private void ScreenWrapper()
    {
        Vector3 position = transform.position;
        if (position.y >= 7)
        {
            position = new Vector3(position.x, -7f, 0);
        }
        else if (position.y <= -7)
        {
            position = new Vector3(position.x, 7f, 0);
        }
        if (position.x >= 11.5f)
        {
            position = new Vector3(-11.5f, position.y, 0);
        }
        else if (position.x <= -11.5f)
        {
            position = new Vector3(11.5f, position.y, 0);
        }
        transform.position = position;
    }

    public void DamagePlayer()
    {
        if (!_shieldActive)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            if (_lives == 2)
            {
                _leftEngineDamaged.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngineDamaged.SetActive(true);
            }
            else if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
            }
        }
        else
        {
            _shieldActive = false;
            _playerShield.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyLaser"))
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
            Destroy(other.gameObject);
            DamagePlayer();
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpCooldown);
        _tripleShotActive = false;
    }

    public void SpeedActive()
    {
        _speed += _speedBoost;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpCooldown);
        _speed -= _speedBoost;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _playerShield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
