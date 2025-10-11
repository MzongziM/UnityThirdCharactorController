using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class TopDownController : MonoBehaviour
{
    private static readonly int PlayerInputVector = Animator.StringToHash("PlayerInputVector");
    private static readonly int Rifle = Animator.StringToHash("Rifle");
    private int _injuredLayerIndex;
    private Animator _animator;
    private Transform _transform;
    private Vector2 _playerInputVec;
    private Vector3 _playerMovement;
    [SerializeField] private float rotateSpeed = 1000;

    [SerializeField] private bool isRun;

    [SerializeField] private float currentSpeed;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float maxDistanceDelta = 10;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 4f;

    /// <summary>
    /// 持枪状态
    /// </summary>
    [SerializeField] private bool armedRifle;

    [SerializeField] private float injuredScale = 0.6f;
    [SerializeField] private bool isInjured;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _injuredLayerIndex = _animator.GetLayerIndex("Injured");
    }

    private void Update()
    {
        RotatePlayer();
        PlayerMove();
        if (Keyboard.current.rKey.wasReleasedThisFrame)
        {
            isInjured = true;
            StopAllCoroutines();
            StartCoroutine(SetInjured());
        }
    }

    [SerializeField] private Transform rightHandTrans;
    [SerializeField] private Transform leftHandGrip;
    private void OnAnimatorIK(int layerIndex)
    {
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandGrip.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandGrip.rotation);
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTrans.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTrans.rotation);
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    }

    private IEnumerator SetInjured()
    {
        var _timer = 0f;
        while (_timer < 1)
        {
            var newTarget = Mathf.Lerp(0, 1, _timer);
            _animator.SetLayerWeight(_injuredLayerIndex, newTarget);
            _timer += Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, 1);
            yield return null;
        }
    }

    public void GetPlayerMoveInput(InputAction.CallbackContext context)
    {
        _playerInputVec = context.ReadValue<Vector2>();
    }

    public void GetPlayerRunningInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRun = true;
        }
        else if (context.canceled)
        {
            isRun = false;
        }
    }

    public void GetArmedRifle(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            armedRifle = !armedRifle;
            _animator.SetBool(Rifle, armedRifle);
        }
    }

    private void RotatePlayer()
    {
        // if (_playerInputVec.Equals(Vector2.zero)) return;
        // _playerMovement = new Vector3(_playerInputVec.x, _playerMovement.y, _playerInputVec.y);
        // var targetRotation = Quaternion.LookRotation(_playerMovement, Vector3.up);
        // _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (_playerInputVec.Equals(Vector2.zero)) return;
        _playerMovement = new Vector3(_playerInputVec.x, 0f, _playerInputVec.y); // Y is zero
        var targetRotation = Quaternion.LookRotation(_playerMovement, Vector3.up);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        // _transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * _playerInputVec.x);
    }

    private void PlayerMove()
    {
        targetSpeed = isRun ? runSpeed : walkSpeed;
        targetSpeed *= _playerInputVec.magnitude;
        if (isInjured)
        {
            targetSpeed *= injuredScale;
        }
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * maxDistanceDelta);
        _animator.SetFloat(PlayerInputVector, currentSpeed);
    }
}