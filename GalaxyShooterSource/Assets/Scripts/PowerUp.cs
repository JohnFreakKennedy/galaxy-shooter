using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private AudioClip _powerUpSound;

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUpSound, transform.position);

            if (player != null)
            {
                switch (tag)
                {
                    case "TripleShotPowerUp":
                        player.TripleShotActive();
                        break;
                    case "SpeedPowerUp":
                        player.SpeedActive();
                        break;
                    case "ShieldPowerUp":
                        player.ShieldActive();
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
