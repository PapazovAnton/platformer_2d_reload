using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnerController : MonoBehaviour
{
    private CoinSpawner[] _coinSpawners;
    private CoinSpawner _lastGeneratedSpawner;

    void Start()
    {
        _coinSpawners = gameObject.GetComponentsInChildren<CoinSpawner>();
        CreateCoin();
    }

    void Update()
    {
        CreateCoin();
    }

    public void CreateCoin()
    {
        int keyRandomSpawner = Random.Range(0, _coinSpawners.Length);
        CoinSpawner currentSpawner = _coinSpawners[keyRandomSpawner];

        if (currentSpawner != _lastGeneratedSpawner && CoinOnStage() == false)
        {
            _lastGeneratedSpawner = currentSpawner;
            currentSpawner.CreateCoin();
        }
    }

    private bool CoinOnStage()
    {
        Coin coin = GameObject.FindObjectOfType<Coin>();
        return (coin == null) ? false : true;
    }
}
