using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class PlayMoveTest : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    [SerializeField] private Animator animator;
    
    // [SerializeField] private float threshold = 0.1f;
    // [SerializeField] private float forwardSpeed = 2.0f;
    // [SerializeField] private float backSpeed = -1.5f;
    // [SerializeField] private float currentSpeed;
    // [SerializeField] private float targetSpeed = 0.0f;
    // [SerializeField] private Vector3 movement = Vector3.zero;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // private void Update()
    // {
    //     Move();
    // }
    //
    // private void Move()
    // {
    //     currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 5* Time.deltaTime);
    //     movement = new Vector3(0, 0, currentSpeed * Time.deltaTime);
    //     transform.position += movement;
    // }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>().normalized;
        animator.SetFloat(Speed, value.y);
    }
}