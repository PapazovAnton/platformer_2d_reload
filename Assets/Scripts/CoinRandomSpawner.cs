using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRandomSpawner : MonoBehaviour
{
    private CoinSpawner[] _coinSpawners;
    private int _lastGeneratedSpawnerKey = -1;

    private void Start()
    {
        _coinSpawners = gameObject.GetComponentsInChildren<CoinSpawner>();
        CreateCoin();
    }

    public void CreateCoin()
    {
        int keyRandomSpawner;

        do
        {
            keyRandomSpawner = Random.Range(0, _coinSpawners.Length);
        }
        while (keyRandomSpawner == _lastGeneratedSpawnerKey);

        _lastGeneratedSpawnerKey = keyRandomSpawner;
        CoinSpawner currentSpawner = _coinSpawners[keyRandomSpawner];
        currentSpawner.CreateCoin();
    }
}
