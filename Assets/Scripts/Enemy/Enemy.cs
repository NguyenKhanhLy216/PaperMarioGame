using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody rb;
    protected Vector3 home;
    protected Quaternion initRotation;

    public float distance;
    public float moveSpeed;

    private bool isAttacked = false;

    protected Coroutine currentCoroutine;

    protected bool noticeMario;
    protected bool currentState;
    protected bool previousState;

    public Transform marioTransform;
    protected Transform enemyTransform;
    public Transform sprite;
    public Transform feet;
    public Animator anim;
    public int health;
    public AudioSource deathsound;

    protected bool isDead = false;

    public float invincibilityDuration = 1.0f; 
    protected bool isInvincible = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyTransform = GetComponent<Transform>();
        distance = 0;
        noticeMario = false;
        currentState = false;
        previousState = false;
        home = new Vector3(transform.position.x, 1.0f, transform.position.z);
        initRotation = transform.rotation;
        anim = GetComponentInChildren<Animator>();

        currentCoroutine = StartCoroutine(IdleWalking());
    }

    protected virtual void Update()
    {
        distance = Vector3.Distance(marioTransform.position, enemyTransform.position);

        if (distance < 2)
        {
            noticeMario = true;
            previousState = currentState;
            currentState = true;
        }
        else
        {
            noticeMario = false;
            previousState = currentState;
            currentState = false;
        }

        if (StateChange(currentState, previousState))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            if (noticeMario)
            {
                currentCoroutine = StartCoroutine(ChaseMario());
            }
            else
            {
                currentCoroutine = StartCoroutine(GoHome());
            }
        }
    }

    protected bool StateChange(bool state1, bool state2)
    {
        return state1 != state2;
    }

    protected virtual IEnumerator IdleWalking()
    {
        anim.SetBool("isRun", false);
        while (!noticeMario)
        {
            yield return null;
        }
        anim.SetBool("isRun", true);
        currentCoroutine = null;
    }

    protected virtual IEnumerator ChaseMario()
    {
        anim.SetBool("isRun", true);
        while (noticeMario && !isAttacked)
        {
            Vector3 direction = (marioTransform.position - transform.position).normalized;
            if (marioTransform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (marioTransform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            direction.y = 0;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }
        anim.SetBool("isRun", false);
        currentCoroutine = null;
    }

    protected virtual IEnumerator GoHome()
    {
        anim.SetBool("isRun", true);
        float closeEnoughDistance = 0.1f; 

        while (Vector3.Distance(new Vector3(home.x, transform.position.y, home.z), transform.position) > closeEnoughDistance && !isAttacked)
        {

            if (home.x > transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (home.x < transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            Vector3 direction = (home - transform.position).normalized;
            direction.y = 0; 
            transform.position += direction * (moveSpeed / 2) * Time.deltaTime;
            yield return null;
        }

        anim.SetBool("isRun", false);
        currentCoroutine = StartCoroutine(IdleWalking());
    }

    public virtual void Death()
    {
        StopAllCoroutines();
        StartCoroutine(Die());
    }

    protected virtual IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(2.0f); 
        Destroy(this.gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;
        if (!isInvincible)
        {
            StartCoroutine(DelayedTakeDamage(damage));
        }
    }

    protected virtual IEnumerator DelayedTakeDamage(int damage)
    {
        yield return new WaitForSeconds(0.5f);
        if (!isDead && !isInvincible)
        {
            health -= damage;
   
            if (health <= 0)
            {
                Death();
                deathsound.Play();
            }
            else
            {
                StartCoroutine(InvincibilityCoroutine());
                isAttacked = true; 
                yield return new WaitForSeconds(2.0f); 
                isAttacked = false; 
            }
        }
    }

    protected IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

          protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!isInvincible && collision.gameObject.CompareTag("Player"))
        {
            Player mario = collision.gameObject.GetComponent<Player>();

            if (mario != null)
            {
                // Kiểm tra nếu Mario không ở trạng thái Knock Back
                if (!mario.IsKnockback)
                {
                    if (collision.transform.position.y > transform.position.y)
                    {
                        PlayTakeHitAnimation(); // Gọi hàm để chạy animation TakeHit
                        TakeDamage(1);
                    }
                    else
                    {
                        mario.TakeDamage(1);
                    }
                }
                else
                {
                    // Mario đang ở trạng thái Knock Back, không gây sát thương
                    mario.TakeDamage(1);
                }
            }
        }
    }

    // Hàm để chạy animation TakeHit
    protected virtual void PlayTakeHitAnimation()
    {
        if (anim != null)
        {
            anim.SetTrigger("TakeHit"); // Kích hoạt animation "TakeHit"
        }
    }
}