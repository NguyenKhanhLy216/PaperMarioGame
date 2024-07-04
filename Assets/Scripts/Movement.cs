using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public partial class Movement : MonoBehaviour
{
    private Vector3 target;
    private bool facingRight;
    public float moveSpeed; // Tốc độ di chuyển
    public float maxJumpHeight; // Chiều cao nhảy tối đa
    public float maxJumpTime; // Thời gian nhảy tối đa
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f); // Lực nhảy

    public float groundCheckRadius; // Bán kính kiểm tra tiếp đất
    public LayerMask whatIsGround; // Lớp định danh mặt đất
    public bool grounded; // Trạng thái tiếp đất

    public Transform sprite; // Transform của sprite
    public Transform feet; // Transform của chân
    public Animator anim; // Bộ điều khiển hoạt hình

    private Rigidbody rb; // Rigidbody của Mario
    public GameObject attackHitbox; // Hitbox tấn công
    public int health = 3; // Số máu của Mario
    public float invincibilityDuration = 1.0f; // Thời gian bất khả xâm phạm
    private bool isInvincible = false; // Trạng thái bất khả xâm phạm
    public AudioSource walksound; // Âm thanh bước đi
    public AudioSource jumpsound; // Âm thanh nhảy
    public AudioSource hammersound; // Âm thanh dùng búa

    // Hàm Start được gọi trước khi frame đầu tiên được cập nhật
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = sprite.forward;
        facingRight = true;
        attackHitbox.SetActive(false);
    }

    private void LateUpdate()
    {
        anim.SetFloat("Velocity", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        anim.SetBool("Grounded", grounded);
    }

    // Hàm Update được gọi một lần mỗi frame
    void Update()
    {
        grounded = Physics.CheckSphere(feet.transform.position, groundCheckRadius, whatIsGround);

        // Nhìn vào mục tiêu
        Vector3 dir = Vector3.RotateTowards(sprite.forward, target, 15 * Time.deltaTime, 0.0f);
        sprite.rotation = Quaternion.LookRotation(dir);

        // Nhập phím phải
        if (Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            // Thay đổi mục tiêu
            if (!facingRight)
            {
                target = -target;
                facingRight = true;
            }
        }
        // Nhập phím trái
        else if (Input.GetAxisRaw("Horizontal") < 0.0f)
        {
            // Thay đổi mục tiêu
            if (facingRight)
            {
                target = -target;
                facingRight = false;
            }
        }

        // Theo dõi nhập để tiếp tục di chuyển người chơi
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y, Input.GetAxis("Vertical") * moveSpeed * 1.50f);

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
    }

    private IEnumerator ResetHammerAnimation()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.8f);

        // Đặt lại trigger trong Animator
        anim.SetBool("Hammer", false);
        attackHitbox.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            StartCoroutine(DelayedTakeDamage(damage));
        }
    }

    private IEnumerator DelayedTakeDamage(int damage)
    {
        yield return new WaitForSeconds(3.0f);
        if (!isInvincible)
        {
            health -= damage;
            Debug.Log("Mario nhận sát thương");
            if (health <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibilityCoroutine());
            }
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    // Hàm xử lý khi Mario chết
    private void Die()
    {
        anim.SetTrigger("Die");
        SceneManager.LoadScene("Dead"); // Chuyển sang scene "Dead"
    }

    // Xử lý va chạm với quái vật
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Nếu va chạm từ phía dưới hoặc ngang
            ContactPoint contact = collision.contacts[0];
            if (contact.point.y < collision.transform.position.y)
            {
                TakeDamage(1);
            }
        }
    }
}

