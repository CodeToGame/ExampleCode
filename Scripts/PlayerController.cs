using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    private float horizontal;  
    private float _coyoteTime;
    private bool isJumpCut;
    private bool canSetIsJumping;

    [Header("Movement")]
    public float maxSpeed;
    public float currentSpeed;
    [Range(0.00f, 1.00f)] public float speedUp;
    [Range(0.00f, 1.00f)] public float speedDown;

    [Header("Jumping")]
    public float jumpingPower;
    public bool isJumping;
    [Range(0.00f, 1.00f)] public float jumpCut;

    [Header("Coyote Time")]
    public float coyoteTime;

    [Header("Hang Time (Jump Buffer)")]
    public float hangTime; 
    private float _hangTime;

    [Header("Apex Modifiers")]
    public float apexThreshold;      
    public float apexGravityMultiplier;

    [Header("Gravity")]
    public float gravity;
    public float fallGravity;

    [Header("References")]
    public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundLayer;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    void Update(){
        horizontal = Input.GetAxisRaw("Horizontal");
        float targetSpeed = horizontal * maxSpeed;
        float acceleration = (horizontal != 0f) ? speedUp : speedDown;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);

        if (IsGrounded()){
            if(canSetIsJumping){
                _coyoteTime = coyoteTime;
                isJumping = false;
            }
            rb.gravityScale = gravity;
        }
        else{
            if (rb.linearVelocity.y < 0f && !isJumping){
                _coyoteTime -= Time.deltaTime;
            }
            canSetIsJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.C)){
            _hangTime = hangTime;
        }
        else{
            _hangTime -= Time.deltaTime;
        }
        if (_coyoteTime > 0f && _hangTime > 0f){
            if(!isJumping){
                isJumpCut = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                isJumping = true;
                canSetIsJumping = false;
                _coyoteTime = 0f;
                _hangTime = 0f;
            }    
        }
        if (Mathf.Abs(rb.linearVelocity.y) < apexThreshold && !IsGrounded()){
            rb.gravityScale = gravity * apexGravityMultiplier;
        }
        else if(rb.linearVelocity.y < 0f){
            rb.gravityScale = fallGravity;
        }
        if (Input.GetKeyUp(KeyCode.C) || rb.linearVelocity.y < 0f){
            if(isJumping && isJumpCut){
                rb.gravityScale = fallGravity;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCut);
                isJumpCut = false;
            }    
        }
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }
    private bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
}
