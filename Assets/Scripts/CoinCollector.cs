using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private AudioSource _collectSound;

    private int _coinsCollected;

    public UnityEvent<int> CoinCollected;

    private void Start()
    {
        CoinCollected?.Invoke(_coinsCollected);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin _))
        {
            Destroy(collision.gameObject);
            _coinsCollected++;
            _collectSound.Play();
            CoinCollected?.Invoke(_coinsCollected);
        }
    }
}
