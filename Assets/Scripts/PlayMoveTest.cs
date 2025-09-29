using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayMoveTest : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float currentSpeed;
    [SerializeField] private float targetSpeed;

    private void Start()
    {
        animator.SetFloat(Speed, 1 / animator.humanScale);
    }

    private void Update()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * 5);
        animator.SetFloat(Speed, currentSpeed);
    }

    private void OnAnimatorMove()
    {
        Move();
    }

    private void Move()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * 5);
        animator.SetFloat(Speed, currentSpeed);
        rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>().normalized;
        // animator.SetFloat(Speed, value.y);
        targetSpeed = value.y;
    }

    private void Reset()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
}