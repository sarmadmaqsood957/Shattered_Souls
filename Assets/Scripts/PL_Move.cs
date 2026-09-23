using UnityEditor.Tilemaps;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]


public class PL_Move : MonoBehaviour
{
    [Header("Movement")]
    public float walkspeed = 5f;
    public float sprintspeed = 8f;
    [Header("Jump")]
    public float jumpforce = 12f;
    public Transform groundcheck;  //empty object placed at the charachter feet 
    public LayerMask groundlayer;  // ground use
    public float groundcheckradius = 0.15f;

    [Header("Dodge")]
    public float dodgespeed = 14f;
    public float dodgeDuration = 0.2f;
    public float dodgecooldown = 0.8f;
    public bool nodamageduringdodge = true;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isSprinting;
    private bool isGrounded;
    private bool facingright = true;
    private bool isDodging;
    private float dodgeTimer;
    private float dodgecooldowntimer;
    private float dodgeDirection;

    private bool invincible { get; private set; }

    // Charachter Desingn insertion
    //Sprites + animations

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // ANIMATION Component
    }
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        //Face flip to direction
        if (moveInput > 0 && !facingright) FlipTool();
        else if (moveInput < 0 && facingright) FlipTool();
        //Ground
        isGrounded = groundcheck != null && 
            Physics2D.OverlapCircle(groundcheck.position, groundcheckradius, groundlayer);
        //Jump
        if(Input.GetButtonDown("Jump") && isGrounded && !isDodging)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpforce);
            // ANIM: animator?.SetTrigger("Jump");
        }

        // Dodge
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && dodgecooldowntimer <= 0f)
        {
            StartDodge();
        }

        if (dodgecooldowntimer > 0f)
            dodgecooldowntimer -= Time.deltaTime;

        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0f)
                EndDodge();
        }

        // ANIM: drive walk/sprint/grounded animator parameters here, e.g.
        // animator?.SetBool("IsWalking", Mathf.Abs(moveInput) > 0.01f);
        // animator?.SetBool("IsSprinting", isSprinting);
        // animator?.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        if (isDodging)
        {
            rb.linearVelocity = new Vector2(dodgeDirection * dodgespeed, rb.linearVelocity.y);
        }
        else
        {
            float speed = isSprinting ? sprintspeed : walkspeed;
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
    }

    private void StartDodge()
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgecooldowntimer = dodgecooldown;
        dodgeDirection = facingright ? 1f : -1f;

        if (nodamageduringdodge)
            invincible = true;

        // ANIM: animator?.SetTrigger("Dodge");
    }

    private void EndDodge()
    {
        isDodging = false;
        invincible = false;
    }

    private void Flip()
    {
        facingright = !facingright;
        // Simple flip using scale. If your sprite/rig behaves oddly

        // "Flip X" property instead once character art is attached.
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    // Visualize the ground check radius in the editor
    void OnDrawGizmosSelected()
    {
        if (groundcheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundcheck.position, groundcheckradius);
        }
    }
}
   