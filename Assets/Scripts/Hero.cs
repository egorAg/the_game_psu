using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    //скорость передвижения персонажа
    [SerializeField] private float speed = 3f;
    // количество жизней персонажа
    [SerializeField] private long lives = 8;
    // сила прыжка (для новых версий unity - рпиходится ставить столь малое значение 
    // и увеличивать массу в компоненте rigidbody)
    [SerializeField] private float jumpForce = 0.00000000000000000000001f;
    // проверка на возможность прыжка
    private bool isGrounded = false;

    // привязанный объект ригит
    private Rigidbody2D rb;
    // объект аниматора
    private Animator anim;
    // рендерер
    private SpriteRenderer sprite;

    public static Hero Instance {get; set;}

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    // при компиляции согласно lifeCycle окмпонентов
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Instance = this;
    }

    // функция бега
    private void Run()
    {
        // анимация бега
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    
        sprite.flipX = dir.x < 0.0f;
    }

    // постоянные проверки 1 раз за каждый отрисованный кадр
    private void FixedUpdate()
    {
        CheckGround();
    }

    // фуункция отрабатывает при срабатывании любой клавиши на клавиатуре
    private void Update()
    {
        // стоим на земле - анимация idle
        if (isGrounded) State = States.idle;

        // если нажата a/d/<-/-> то run()
        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        // если персонаж на земле и нажата пробел - прыгаем
        if (isGrounded && Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    // простой метод прыжка
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    // проверка, стоит ли персонаж на земле 
    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.jump;
    }

    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log("У игрока: " + lives);

        if (lives < 1)
        {
            Die();
        }
    }
}

public enum States
{
    idle,
    run,
    jump
}