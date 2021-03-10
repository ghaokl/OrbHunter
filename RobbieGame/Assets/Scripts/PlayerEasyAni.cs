using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEasyAni : MonoBehaviour
{
    private Animator _anim;
    private PlayerEasyPatern _playerEasyPatern;
    private Rigidbody2D _rb;

    private int _groundID; 
    private int _speedID;
    private int _fallID;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _playerEasyPatern = GetComponentInParent<PlayerEasyPatern>();

        _rb = GetComponentInParent<Rigidbody2D>();

        _groundID = Animator.StringToHash("isOnGround");
        _speedID = Animator.StringToHash("speed");    
        _fallID = Animator.StringToHash("verticalVelocity");

    }

    // Update is called once per frame
    void Update()
    {
        _anim.SetFloat(_speedID, Mathf.Abs(_playerEasyPatern.XVelocity));
        //_anim.SetBool("isOnGround",_movement.IsOnGround);
        _anim.SetBool(_groundID, _playerEasyPatern.IsOnGround);      
        _anim.SetFloat(_fallID, _rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootStepAudio();
    }
}
