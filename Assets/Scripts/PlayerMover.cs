using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 15f;

    private Rigidbody2D _rigidbody;
    private bool _isGround;

    public event Action OnLanding;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _isGround = true;
    }

    public void Move(float horizontalDirection)
    {
        _rigidbody.velocity = new Vector2(horizontalDirection * _moveSpeed, _rigidbody.velocity.y);
    }

    public void SetView(Vector3 viewDirection)
    {
        transform.localScale = viewDirection;
    }

    public void TryJump()
    {
        if (_isGround)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _isGround = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            _isGround = true;
            OnLanding?.Invoke();
        }
    }
}
