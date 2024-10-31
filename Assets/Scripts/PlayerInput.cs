using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string AxisName = "Horizontal";

    public float HorizontalDirection { get; private set; }
    public Vector3 ViewDirection { get; private set; }
    public bool TryJump {  get; private set; }

    private void Awake()
    {
        ViewDirection = new Vector3(1, 1, 1);
        TryJump = false;
    }

    private void Update()
    {
        HorizontalDirection = Input.GetAxis(AxisName);

        if (Input.GetKey(KeyCode.D))
            ViewDirection = new Vector3(1, 1, 1);

        if (Input.GetKey(KeyCode.A))
            ViewDirection = new Vector3(-1, 1, 1);

        if (Input.GetKey(KeyCode.Space))
        {
            TryJump = true;
        } 
        else
        {
            TryJump = false;
        }
    }
}
