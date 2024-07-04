using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 target;
    private bool facingRight;
    public float moveSpeed;
    public float maxJumpHeight;
    public float maxJumpTime;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);

    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform sprite;
    public Transform feet;
    public Animator anim;

    private Rigidbody rb;
    public GameObject attackHitbox;
    public int health;
    public float invincibilityDuration = 3.0f; // Thời gian chờ giữa các lần nhận sát thương
    private bool isInvincible = false;
    public AudioSource walksound;
    public AudioSource jumpsound;
    public AudioSource hammersound;
    public AudioSource losehp;
    public GameObject player;

    public float knockbackForce = 10f; // Lực áp dụng cho đẩy lùi
    public float upwardForce = 10f; // Lực đẩy lên để tạo hình dạng Parabol
    public float horizontalForce = 10f; // Lực đẩy ngang để tạo hình dạng Parabol
    public float knockbackDuration = 0.5f; // Thời gian nhân vật bị khóa chuyển động khi bị đẩy lùi

    private bool isKnockback = false; // Biến trạng thái để kiểm soát trạng thái Knock Back

    // Thuộc tính công khai để truy cập trạng thái Knock Back
    public bool IsKnockback => isKnockback;

    private Renderer playerRenderer;
    public Joystick joystick; // Thêm tham chiếu đến joystick

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = sprite.forward;
        facingRight = true;
        attackHitbox.SetActive(false);
        playerRenderer = sprite.GetComponent<Renderer>(); // Lấy Renderer của nhân vật để điều chỉnh độ hiển thị
    }

    private void LateUpdate()
    {
        if (!isKnockback)  // Chỉ cập nhật animator khi không trong trạng thái KnockBack
        {
            anim.SetFloat("Velocity", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
            anim.SetBool("Grounded", grounded);
            anim.SetBool("isMoving", rb.velocity.magnitude > 0.1f && grounded);
        }
    }

    void Update()
    {
        grounded = Physics.CheckSphere(feet.transform.position, groundCheckRadius, whatIsGround);

        if (isKnockback) return;

        Vector3 dir = Vector3.RotateTowards(sprite.forward, target, 15 * Time.deltaTime, 0.0f);
        sprite.rotation = Quaternion.LookRotation(dir);

        float horizontalInput = Input.GetAxis("Horizontal") + joystick.Horizontal; // Kết hợp đầu vào phím và joystick
        float verticalInput = Input.GetAxis("Vertical") + joystick.Vertical; // Kết hợp đầu vào phím và joystick

        if (horizontalInput > 0.0f)
        {
            if (!facingRight)
            {
                target = -target;
                facingRight = true;
            }
        }
        else if (horizontalInput < 0.0f)
        {
            if (facingRight)
            {
                target = -target;
                facingRight = false;
            }
        }

        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, verticalInput * moveSpeed * 1.50f);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpsound.Play();
        }

        if (Input.GetKeyDown(KeyCode.E) && grounded)
        {
            anim.SetBool("Hammer", true);
            hammersound.Play();
            StartCoroutine(ResetHammerAnimation());
        }
        if (rb.velocity.magnitude > 0.1f && grounded)
        {
            if (!walksound.isPlaying)
            {
                walksound.Play();
            }
        }
        else
        {
            walksound.Stop();
        }

        if (gameObject.transform.position.y < -6.0f)
        {
            Application.LoadLevel("Dead");
        }
    }

    private IEnumerator ResetHammerAnimation()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.8f);

        anim.SetBool("Hammer", false);
        attackHitbox.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible && !isKnockback)
        {
            StartCoroutine(DelayedTakeDamage(damage));
        }
    }

    private IEnumerator DelayedTakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.0f);
        if (!isInvincible)
        {
            health -= damage;
            ScoreSystem.hp -= damage;
            if (health <= 0)
            {
                Die();
                Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
                playerRigidbody.isKinematic = true;
                StartCoroutine(DelayedAction());
                IEnumerator DelayedAction()
                {
                    yield return new WaitForSeconds(5f);
                    Application.LoadLevel("Dead");
                }
            }
            else
            {
                StartCoroutine(InvincibilityCoroutine());
                ApplyKnockback();
                losehp.Play();
                SetPlayerVisibility(0.5f); // Làm mờ nhân vật khi bị dính đòn
            }
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        Debug.Log("Invincible");
        isInvincible = true;
        StartCoroutine(BlinkEffect());
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        StopCoroutine(BlinkEffect());
        SetPlayerVisibility(1f);
    }

    private IEnumerator BlinkEffect()
    {
        while (isInvincible)
        {
            SetPlayerVisibility(0.5f);
            yield return new WaitForSeconds(0.1f);
            SetPlayerVisibility(1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetPlayerVisibility(float alpha)
    {
        Color color = playerRenderer.material.color;
        color.a = alpha;
        playerRenderer.material.color = color;
    }

    private void ApplyKnockback()
    {
        Vector3 knockbackDirection = facingRight ? Vector3.left : Vector3.right;
        StartCoroutine(KnockbackCoroutine(knockbackDirection));
    }

    private IEnumerator KnockbackCoroutine(Vector3 knockbackDirection)
    {
        isKnockback = true;
        attackHitbox.SetActive(false);

        knockbackDirection = facingRight ? Vector3.left : Vector3.right;

        anim.SetTrigger("KnockBack"); // Kích hoạt animation KnockBack ngay lập tức

        float timer = 0f;

        while (timer < knockbackDuration)
        {
            float t = timer / knockbackDuration;

            Vector3 knockbackForce = (knockbackDirection * horizontalForce) + (Vector3.up * upwardForce * (1 - (4 * (t - 0.5f) * (t - 0.5f))));

            rb.velocity = knockbackForce;

            timer += Time.deltaTime;

            yield return null;
        }

        rb.velocity = Vector3.zero;
        attackHitbox.SetActive(true);
        isKnockback = false;
    }

    private void Die()
    {
        Debug.Log("Player died");
        anim.SetTrigger("Die");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.contacts[0];
            if (contact.point.y < collision.transform.position.y)
            {
                TakeDamage(1);
            }
        }
    }

    public void Jump()
    {
        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpsound.Play();
        }
    }

    public void UseHammer()
    {
        if (grounded)
        {
            anim.SetBool("Hammer", true);
            hammersound.Play();
            StartCoroutine(ResetHammerAnimation());
        }
    }

    public void Heal(int amount)
    {
        if (health < 3)
        {
            health += amount;
            ScoreSystem.hp += amount;
            if (health > 3)
            {
                health = 3;
                ScoreSystem.hp = 3;
            }
        }
        else
        {
            health = 3;
            ScoreSystem.hp = 3;
        }
    }
}
