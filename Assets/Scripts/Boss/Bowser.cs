using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : Enemy
{
    public bool isFire = false;
    public GameObject explosionPrefab;
    public GameObject fireBullet;
    public float fireSpeed;
    public float timeBtwFire;
    private float timeCoolDown;

    public float jumpDuration = 1f;
    public float waitTime = 3f;
    public float jumpHeight = 50f;

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private bool isJumping = false;
    private Coroutine jumpCoroutine;

    public AudioSource flamesound;    
    public AudioSource jumpsmash;
    public int numberOfBullets = 5; 
    public float bulletSpacing = 1.0f;
    public GameObject objectToHide; 

    private List<GameObject> fireBullets = new List<GameObject>();

    private Renderer bowserRenderer;
    public float blinkDuration = 0.1f;

    public ParticleSystem explosionEffect;

    protected override void Start()
    {
        base.Start();
        health = 3; // Set initial health to 3 for testing
        startPosition = transform.position; // Initialize start position
        bowserRenderer = GetComponentInChildren<Renderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (health == 1)
        {
            StartCoroutine(WaitAndHandleJumping());
        }
        else if (health == 2)
        {
            StartCoroutine(WaitAndHandleFiring());
        }
        else if (health == 3)
        {
            StartCoroutine(WaitAndHandleChasing());
        }
        else if (health == 0){
            objectToHide.SetActive(false);
        }
    }

    private IEnumerator WaitAndHandleJumping()
    {
        yield return new WaitForSeconds(1f);
        HandleJumping();
    }

    private IEnumerator WaitAndHandleChasing()
    {
        yield return new WaitForSeconds(1f);
        HandleChasing();
    }

    private IEnumerator WaitAndHandleFiring()
    {
        yield return new WaitForSeconds(1f);
        HandleFiring();
    }

    private void HandleJumping()
    {
        if (!isJumping)
        {
            targetPosition = marioTransform.position;
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
            jumpCoroutine = StartCoroutine(ParabolicJump(targetPosition));
            jumpsmash.Play();
        }
    }

    private void HandleFiring()
    {
        if (isJumping)
        {
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
            isJumping = false;
        }

        timeCoolDown -= Time.deltaTime;
        if (timeCoolDown < 0)
        {
            timeCoolDown = timeBtwFire;
            FireMultipleBullets();
            flamesound.Play();
        }
    }

    private void HandleChasing()
    {
        if (isJumping)
        {
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
            isJumping = false;
        }

        if (Vector3.Distance(transform.position, marioTransform.position) <= 2f)
        {
             Vector3 direction = (marioTransform.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            Vector3 homeDirection = (startPosition - transform.position).normalized;
            homeDirection.y = 0;
            transform.position += homeDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void FireMultipleBullets()
    {
        Vector3 direction = (marioTransform.position - transform.position).normalized;
        Vector3 startPosition = transform.position;
        for (int i = 0; i < numberOfBullets; i++)
        {
            var fireBulletTmp = Instantiate(fireBullet, startPosition, Quaternion.identity);
            Rigidbody fireBulletRb = fireBulletTmp.GetComponent<Rigidbody>();
            fireBulletRb.AddForce(direction * fireSpeed, ForceMode.Impulse);

             fireBullets.Add(fireBulletTmp);
            startPosition += direction * bulletSpacing;
        }
    }

    private IEnumerator ParabolicJump(Vector3 target)
    {
        isJumping = true;

        // Calculate the jump peak
        Vector3 startPosition = transform.position;
        Vector3 jumpPeak = new Vector3((startPosition.x + target.x) / 2, jumpHeight, (startPosition.z + target.z) / 2);

        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            // Calculate the next position in the parabola
            float t = elapsedTime / jumpDuration;
            Vector3 currentPosition = Parabola(startPosition, target, jumpPeak, t);
            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;

        yield return new WaitForSeconds(waitTime);

        isJumping = false;
    }

    private IEnumerator BlinkEffect()
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        // Save the original color of the character
        Color originalColor = bowserRenderer.material.color;
        Color blinkColor = originalColor;
        blinkColor.a = 0.5f; // Dim color

        while (elapsedTime < invincibilityDuration)
        {
            elapsedTime += blinkDuration;
            isVisible = !isVisible;

            // Change the color of the character
            if (isVisible)
            {
                bowserRenderer.material.color = blinkColor; // Change to dim color
            }
            else
            {
                bowserRenderer.material.color = originalColor; // Restore the original color
            }

            yield return new WaitForSeconds(blinkDuration);
        }

        // Restore the original color of the character after the blink effect ends
        bowserRenderer.material.color = originalColor;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(BlinkEffect());
    }

    private Vector3 Parabola(Vector3 start, Vector3 end, Vector3 peak, float t)
    {
        Vector3 m1 = Vector3.Lerp(start, peak, t);
        Vector3 m2 = Vector3.Lerp(peak, end, t);
        return Vector3.Lerp(m1, m2, t);
    }

    private void DestroyAllFireBullets()
    {
        foreach (GameObject bullet in fireBullets)
        {
            Destroy(bullet);
        }
        // Clear the list
        fireBullets.Clear();
    }

    protected override IEnumerator Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        Explode();
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        // Instantiate and play the explosion effect
        ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.Play();
        Destroy(explosion.gameObject,3); 
    }
}
