using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D triggerCol;
    private Transform target;
    public GameObject attackPoint;
    public GameObject triggerArea;

    public int maxHealth = 100;
    public float attackRate = 1f;
    private float coolDownAttack;
    public float attackDamage = 10f;
    public float moveSpeed = 3f;
    public bool isBoss = false;
    int currentHealth;

    float normalizedTime = 0;

    private bool isHitting = false;
    private GameManager gameManager;

    #region BOSS PARAMETERS
    int deathCount = 0;
    public int recoverHealth;
    #endregion

    void Start()
    {
        coolDownAttack = attackRate;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        //CHECK Si el personaje esta en el area de patrulla
        if (triggerArea.GetComponent<TriggerAreaScript>().haveTarget == true)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else if (triggerArea.GetComponent<TriggerAreaScript>().haveTarget == false)
        {
            target = null;
        }

        switch (animator.GetBool("Die"))
        {
            case false:
                if (target != null && !animator.GetBool("Attack") && isHitting == false && !animator.GetBool("Hurt") && !!animator.GetBool("Idle") && !animator.GetBool("Recover"))
                {
                    CheckAnimation(false, false, true, false, false);
                    float step = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                    if (target.position.x > transform.position.x)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                    }
                    else
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                    }
                }
                if (target == null)
                {
                    CheckAnimation(false, false, false, false, true);
                }
                if (attackRate >= coolDownAttack)
                {
                    coolDownAttack += Time.deltaTime;
                }

                if (target != null && isHitting == true)
                {
                    Attack();
                }
                break;
        }
    }

    public void Attack()
    {
        if (attackRate <= coolDownAttack)
        {
            while (normalizedTime <= 1f)
            {
                normalizedTime += Time.deltaTime / attackRate;
            }
            CheckAnimation(false, false, false, true, false);
            if (attackPoint.GetComponent<AttackEnemyScript>().hittingPlayer == true)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>().UpdateHealth(-attackDamage);
            }
            coolDownAttack = 0;
            normalizedTime = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        this.GetComponent<AudioSource>().Play();
        currentHealth -= damage;
        CheckAnimation(true, false, false, false, false);
        if (currentHealth <= 0)
        {
            Die();
            if (!isBoss)
                Destroy(gameObject, 3);
        }
    }

    void Die()
    {
        CheckAnimation(false, true, false, false, false);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        if (isBoss == false)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameManager.enemyList.Remove(gameObject);
            if (gameManager.level == 1 || gameManager.level == 2)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Pike"))
                {
                    g.GetComponent<PikesScript>().TurnOff();
                }
            }
        }
    }

    void CheckAnimation(bool hurt, bool die, bool run, bool attack, bool idle)
    {
        if (hurt == true)
        {
            animator.SetBool("Hurt", true);
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
        }

        if (die == true)
        {
            animator.SetBool("Die", true);
        }

        if (run == true)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true);
        }

        if (attack == true)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            animator.SetBool("Idle", true);
        }

        if (idle == true)
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Hurt", false);
        }

        if (message.Equals("HurtAnimationEnded"))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Hurt", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Run", false);
        }


        if (message.Equals("DieAnimationEnded") && deathCount == 0)
        {
            animator.SetBool("Recover", true);
        }

        if (message.Equals("RecoverAnimationEnded") && deathCount == 0)
        {
            animator.SetBool("Recover", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Hurt", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Run", false);
            animator.SetBool("Die", false);
            currentHealth = recoverHealth;
            deathCount++;
        }
        else if (message.Equals("DieAnimationEnded") && deathCount == 1)
        {
            Destroy(gameObject, 3);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameManager.enemyList.Remove(gameObject);
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Pike"))
            {
                g.GetComponent<SpriteRenderer>().sprite = g.GetComponent<PikesScript>().pikeStates[1];
            }
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Chest"))
            {
                g.GetComponent<Animator>().SetBool("Win", true);
            }
            gameManager.ChangeLevel(3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHitting = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isHitting = false;
        }
    }
}
