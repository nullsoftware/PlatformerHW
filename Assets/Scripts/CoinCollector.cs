using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private string _coinTag;
    [SerializeField] private AudioSource _collectSound;

    private int _coinsCollected;

    public UnityEvent<int> CoinCollectedEvent;

    private void Start()
    {
        CoinCollectedEvent.Invoke(_coinsCollected);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _coinTag)
        {
            Destroy(collision.gameObject);
            _coinsCollected++;
            _collectSound.Play();
            CoinCollectedEvent.Invoke(_coinsCollected);
        }
    }
}
