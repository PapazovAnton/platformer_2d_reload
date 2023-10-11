using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;

    public void CreateCoin()
    {
        Instantiate(_coin, transform.position, Quaternion.identity);
    }
}
