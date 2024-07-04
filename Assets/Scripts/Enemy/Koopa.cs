using System.Collections;
using UnityEngine;

public class Koopa : Enemy
{
    public GameObject theShuriken;
    public float shurikenSpeed;
    public float timeBtwShuriken;
    private float timeCoolDown;
    public AudioSource throwsound;

    protected override void Start()
    {
        base.Start();
        health = 1;
        timeCoolDown = timeBtwShuriken; // Initialize the cooldown timer
    }

    protected override void Update()
    {
        base.Update();
        distance = Vector3.Distance(marioTransform.position, enemyTransform.position); // Update the distance

        if (distance >= 4 && distance <= 8)
        {
            timeCoolDown -= Time.deltaTime;
            if (timeCoolDown < 0)
            {
                timeCoolDown = timeBtwShuriken;
                ShootShuriken();
                throwsound.Play();
            }
        }
    }

    void ShootShuriken()
    {
        if (marioTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (marioTransform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        var shurikenTmp = Instantiate(theShuriken, transform.position, Quaternion.identity);
        Rigidbody shurikenRb = shurikenTmp.GetComponent<Rigidbody>();
        Vector3 direction = (marioTransform.position - transform.position).normalized;
        shurikenRb.AddForce(direction * shurikenSpeed, ForceMode.Impulse);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (!isInvincible && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.y > transform.position.y)
            {
                TakeDamage(1);
            }
        }
    }
}