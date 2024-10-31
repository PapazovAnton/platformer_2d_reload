using System;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool IsGround {  get; private set; }
    public event Action<Coin> OnCoinCollected;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Coin coin))
        {
            OnCoinCollected?.Invoke(coin);
        }
    }
}
