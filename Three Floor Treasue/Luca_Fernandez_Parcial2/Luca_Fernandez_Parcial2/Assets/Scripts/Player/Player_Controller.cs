using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    [Header("CHARACTER PARAMETERS")]
    public float moveSpeed = 5f;
    public float health;
    public float maxHealth;
    public float untouchableDuration;

    private float timerUntouchable = 0;
    private bool isUntouchable = false;

    private bool isAttack = false;
    [Header("ATTACK PARAMETERS")]
    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    float nextAttack;
    public Collider2D[] hitEnemies;

    [Header("ATTACHED")]
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public Vector2 movement;
    public Rigidbody2D rb;
    public Animator animator;
    private GameManager gameManager;
    public AudioSource audioAttack;
    public AudioSource audioHit;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        health = maxHealth;
    }

    void Update()
    {
        if (isUntouchable)
        {
            timerUntouchable += Time.deltaTime;
            if (timerUntouchable > untouchableDuration)
            {
                isUntouchable = false;
                timerUntouchable = 0;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
            }
        }

        if (!isAttack)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        CheckFlick();
        CheckAnimation(false);

        if (Time.time >= nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Attack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void CheckFlick()
    {
        if (moveSpeed > 0)
        {
            if (movement.x > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (movement.x < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
    }

    void CheckAnimation(bool attack)
    {
        if (movement.x > 0 || movement.x < 0 || movement.y > 0 || movement.y < 0 && isAttack == false)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Idle", false);
        }
        else if (movement.x == 0 || movement.x == 0 || movement.y == 0 || movement.y == 0 && isAttack == false)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
        }
        if (attack == true)
        {
            animator.SetBool("Attack", true);
            isAttack = true;
            movement.x = 0;
            movement.y = 0;
        }
    }
    void Attack()
    {
        CheckAnimation(true);

        hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        audioAttack.Play();

        foreach (Collider2D enemy in hitEnemies)
        {
            if (!enemy.isTrigger)
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        Array.Resize(ref hitEnemies, 0);
    }

    public void UpdateHealth(float mod)
    {
        audioHit.Play();
        if (!isUntouchable)
        {
            health += mod;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0)
            {
                health = 0f;
                gameManager.loseLevel = SceneManager.GetActiveScene().buildIndex;
                gameManager.LoseGame();
            }
            if (mod < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f);
                isUntouchable = true;
            }
            gameManager.HearthCheck();
        }
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            isAttack = false;
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }
    }

}
