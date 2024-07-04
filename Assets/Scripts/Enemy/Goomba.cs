using System.Collections;
using UnityEngine;

public class Goomba : Enemy
{
    protected override void Start()
    {
        base.Start();
        health = 1;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}