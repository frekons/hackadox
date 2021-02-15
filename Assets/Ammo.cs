using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float Speed;

    public GameObject HitEffect;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    public void Shoot(Vector2 direction)
    {
        _rigidbody2D.velocity = direction.normalized * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (!collision.collider.CompareTag("Player"))
        //    return;

        if (HitEffect)
        {
            Instantiate(HitEffect, collision.contacts[0].point, Quaternion.identity);
        }

        collision.collider.SendMessage("KillPlayer", GameManager.DamageTypes.Weapon, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }

}
