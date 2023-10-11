using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private UnityEvent _destroy;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}