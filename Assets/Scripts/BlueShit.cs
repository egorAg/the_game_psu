using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShit : Entity
{

    private float speed = 0.1f;
    private Vector3 direction;
    private SpriteRenderer sprite;
    [SerializeField] private int lives = 1;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        direction = transform.right;
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * direction.x * 0.7f, speed);

        if (colliders.Length > 0) direction *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime);
        sprite.flipX = direction.x > 0.0f;

    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("У синего говна остается: " + lives + "HP");
        }

        if (lives < 1)
        {
            Die();
        }
    }

}
