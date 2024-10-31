using System;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    public event Action<Coin> Destory;

    public void Destroy()
    {
        Destory?.Invoke(this);
    }
}