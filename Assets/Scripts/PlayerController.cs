using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 0.5f;

    public Vector2 Velocity;
    public LayerMask LayerMask;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.01f;
    protected const float shellRadius = 0.01f;

    private Animator _animator;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();

        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(LayerMask);
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        float axisHorizontal = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed", Math.Abs(axisHorizontal));
        targetVelocity = new Vector2(axisHorizontal, 0);

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            _animator.SetBool("Grounded", false);
            _animator.SetBool("Jump", true);
            Velocity.y = 5;
        }

        if (Input.GetKey(KeyCode.D))
            transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetKey(KeyCode.A))
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        Velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        Velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = Velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float speed = (yMovement) ? _speed / 1.5f : _speed; 
        float distance = move.magnitude * speed;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    grounded = true;
                    _animator.SetBool("Grounded", true);

                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    } 
                }

                float projection = Vector2.Dot(Velocity, currentNormal);

                if (projection < 0)
                {
                    Velocity = Velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            coin.Destroy();
        }
    }
}