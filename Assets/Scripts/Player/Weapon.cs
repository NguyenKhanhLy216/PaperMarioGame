using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float destroyDelay = 3f;

    private void Start()
    {
        StartCoroutine(DestroyWeaponAfterDelay());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyWeaponAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

}


