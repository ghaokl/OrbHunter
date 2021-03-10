using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _coll;

    [Header("移动参数")]
    public float Speed = 8.0f;
    public float CrouchSpeedDivisor = 3.0f;

    [Header("跳跃参数")]
    public float JumpForce = 6.3f;    //跳跃的力
    public float JumpHoldForce = 1.9f;   //持续跳跃的力
    public float JumpHoldDuration = 0.1f;
    public float CrounchJumpBoost = 2.5f;
    public float HangingJumpForce = 15.0f;

    private float _jumpTime;

    [Header("状态")]
    public bool IsCrouch;
    public bool IsOnGround;
    public bool IsJump;
    public bool IsHeadBlocked;
    public bool IsHanging;


    [Header("环境检测")]
    public float FootOffset = 0.4f;
    public float HeadClearance = 0.5f;
    public float GroundDistance = 0.2f;

    //攀爬条件
    private float _playerHeight;
    public float EyeHeight = 1.5f;
    public float GrabDistance = 0.4f;
    public float ReachOffset = 0.7f;

    public LayerMask GroundLayer;

    public float XVelocity;

    //按键设置
    private bool _jumpPressed;  //单次按下
    private bool _jumpHeld;    //长按跳跃
    private bool _crouchHeld;   //长按下蹲
    private bool _crouchPressed;  //单次下蹲

    //碰撞体尺寸
    private Vector2 _colliderStandSize;
    private Vector2 _colliderStandOffset;
    private Vector2 _colliderCrouchSize;
    private Vector2 _colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();

        _playerHeight = _coll.size.y;
        _colliderStandSize = _coll.size;
        _colliderStandOffset = _coll.offset;
        _colliderCrouchSize = new Vector2(_coll.size.x, _coll.size.y / 2.0f);
        _colliderCrouchOffset = new Vector2(_coll.offset.x, _coll.offset.y / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameOver()) return;

        _jumpPressed = Input.GetButtonDown("Jump");
        _jumpHeld = Input.GetButton("Jump");
        _crouchHeld = Input.GetButton("Crouch");
        _crouchPressed = Input.GetButtonDown("Crouch");
    }

    void FixedUpdate()
    {
        if (GameManager.GameOver()) return;

        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    void PhysicsCheck()
    {
        //左右脚射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffset, 0f), Vector2.down, GroundDistance, GroundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, 0f), Vector2.down, GroundDistance, GroundLayer);

        if (leftCheck || rightCheck)
        {
            IsOnGround = true;
        }
        else
        {
            IsOnGround = false;
        }

        //头顶射线
        RaycastHit2D headCheck = Raycast(new Vector2(0f, _coll.size.y), Vector2.up, HeadClearance, GroundLayer);

        if (headCheck)
            IsHeadBlocked = true;
        else IsHeadBlocked = false;

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0);

        RaycastHit2D blockCheck = Raycast(new Vector2(FootOffset * direction, _playerHeight), grabDir, GrabDistance,
            GroundLayer);

        RaycastHit2D wallCheck = Raycast(new Vector2(FootOffset * direction, EyeHeight), grabDir, GrabDistance,
           GroundLayer);


        RaycastHit2D ledgeCheck = Raycast(new Vector2(ReachOffset * direction, _playerHeight), Vector2.down, GrabDistance,
           GroundLayer);

        //考虑到悬挂在墙壁上时
        if (!IsOnGround && _rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockCheck)
        {
            Vector3 pos = transform.position;

            pos.x += (wallCheck.distance - 0.05f) * direction;
            pos.y -= ledgeCheck.distance;

            transform.position = pos;

            _rb.bodyType = RigidbodyType2D.Static;
            IsHanging = true;
        }

    }

    //跳跃状态
    void MidAirMovement()
    {
        if (IsHanging)
        {
            if (_jumpPressed)
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.velocity = new Vector2(_rb.velocity.x, HangingJumpForce);
                IsHanging = false;
            }

            if (_crouchPressed)
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;
                IsHanging = false;
            }
        }
        //地面上跳跃时
        if (_jumpPressed && IsOnGround && !IsJump && !IsHeadBlocked)
        {
            if (IsCrouch && IsOnGround)
            {
                StandUp();
                _rb.AddForce(new Vector2(0f, CrounchJumpBoost), ForceMode2D.Impulse);
            }
            IsOnGround = false;
            IsJump = true;

            //开始计算跳跃时间
            _jumpTime = Time.time + JumpHoldDuration;
            _rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);  //突然给了一个力的模式

            AudioManager.PlayJumpAudio();
        }
        else if (IsJump)
        {
            if (_jumpHeld)
            {
                _rb.AddForce(new Vector2(0f, JumpHoldForce), ForceMode2D.Impulse);
            }
            if (_jumpTime < Time.time) //与实际时间进行判断
            {
                IsJump = false;
            }
        }
    }

    //void MovementDuringAir()
    //{

    //}

    void GroundMovement()
    {
        if (IsHanging) return;

        if (_crouchHeld && !IsCrouch && IsOnGround)
            Crouch();
        else if (!_crouchHeld && IsCrouch && !IsHeadBlocked)
            StandUp();
        else if (!IsOnGround && IsCrouch)
            StandUp();

        XVelocity = Input.GetAxis("Horizontal");

        if (IsCrouch)
            XVelocity /= CrouchSpeedDivisor;

        _rb.velocity = new Vector2(XVelocity * Speed, _rb.velocity.y);
        FilpDirection();
    }

    void FilpDirection()
    {
        if (XVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (XVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Crouch()
    {
        IsCrouch = true;
        _coll.size = _colliderCrouchSize;
        _coll.offset = _colliderCrouchOffset;
    }

    void StandUp()
    {
        IsCrouch = false;
        _coll.size = _colliderStandSize;
        _coll.offset = _colliderStandOffset;
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDir, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDir, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDir * length, color);

        return hit;
    }

}
