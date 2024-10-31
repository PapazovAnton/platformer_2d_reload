using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;

    private void OnEnable()
    {
        _coin.Destory += DestroyCoin;
    }

    private void OnDisable()
    {
        _coin.Destory -= DestroyCoin;
    }

    public void CreateCoin()
    {
        Instantiate(_coin, transform.position, Quaternion.identity);
    }

    private void DestroyCoin(Coin coin)
    {
        Destroy(coin);
    }
}
