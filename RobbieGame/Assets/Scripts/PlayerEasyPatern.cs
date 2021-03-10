using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEasyPatern : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _coll;

    [Header("移动参数")]
    public float Speed = 8.0f;

    [Header("跳跃参数")]
    public float JumpForce = 6.3f;    //跳跃的力
    public float JumpHoldForce = 1.9f;   //持续跳跃的力
    public float JumpHoldDuration = 0.1f;

    private float _jumpTime;

    [Header("状态")]
    public bool IsOnGround;
    public bool IsJump;

    [Header("环境检测")]
    public float FootOffset = 0.4f;
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
    }

    // Update is called once per frame
    void Update()
    {     
        _jumpPressed = Input.GetButtonDown("Jump");
        _jumpHeld = Input.GetButton("Jump");
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    void PhysicsCheck()
    {
        //左右脚射线
        RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffset, 0f), Vector2.down, GroundDistance, GroundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, 0f), Vector2.down, GroundDistance, GroundLayer);
        RaycastHit2D leftCheckToLeft = Raycast(new Vector2(-FootOffset, 0f), Vector2.left, GroundDistance, GroundLayer);
        RaycastHit2D rightCheckToright = Raycast(new Vector2(FootOffset, 0f), Vector2.right, GroundDistance, GroundLayer);

        if (leftCheck || rightCheck)//|| leftCheckToLeft|| rightCheckToright
        {
            IsOnGround = true;        
        }
        else
        {
            IsOnGround = false;
        }   
    }

    //跳跃状态
    void MidAirMovement()
    {
        if (_jumpPressed)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
        }
        //地面上跳跃时
        if (_jumpPressed && IsOnGround)
        {          
            if ( IsOnGround)
            {            
                _rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            }
            IsOnGround = false;
            IsJump = true;

            //开始计算跳跃时间
            _jumpTime = Time.time + JumpHoldDuration;
            _rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);  //突然给了一个力的模式

         //   AudioManager.PlayJumpAudio();
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


    void GroundMovement()
    {                
        XVelocity = Input.GetAxis("Horizontal");
     
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
  
    void StandUp()
    {    
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
