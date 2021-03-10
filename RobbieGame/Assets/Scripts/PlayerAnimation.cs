using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator _anim;
    private PlayerMovement _movement;
    private Rigidbody2D _rb;

    private int _groundID;
    private int _hangingID;
    private int _crouchID;
    private int _speedID;
    private int _fallID;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _movement = GetComponentInParent<PlayerMovement>();
        _rb = GetComponentInParent<Rigidbody2D>();

        _groundID = Animator.StringToHash("isOnGround");
        _speedID = Animator.StringToHash("speed");
        _hangingID = Animator.StringToHash("isHanging");
        _crouchID = Animator.StringToHash("isCrouching");
        _fallID = Animator.StringToHash("verticalVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat(_speedID, Mathf.Abs(_movement.XVelocity));
        //_anim.SetBool("isOnGround",_movement.IsOnGround);
        _anim.SetBool(_groundID, _movement.IsOnGround);
        _anim.SetBool(_crouchID, _movement.IsCrouch);
        _anim.SetBool(_hangingID, _movement.IsHanging);
        _anim.SetFloat(_fallID, _rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootStepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootStepAudio();
    }
}
