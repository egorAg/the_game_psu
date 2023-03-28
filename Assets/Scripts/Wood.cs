using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Entity
{
    [SerializeField] private int lives = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("У бревна остается: " + lives + " жизней.");
        }

        if (lives < 1)
        {
            Die();
        }
    }
}
