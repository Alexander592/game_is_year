using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public GameObject deathEffect;
    public GameObject damageShow;
    public float speed;
    public int health;
    public int damage;
    public float startTimeBtwAttack;
    public float startStopTime;

    [HideInInspector] public bool playerNotInRoom;
    private bool stopped;

    private Player player;
    private Animator anim;
    private AddRoom room;
    private float timeBtwAttack;
    private float stopTime;
    private Score score;
 


    public enum EnemyType
    {
        Green,
        Red
    }

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        room = GetComponentInParent<AddRoom>();
        score = FindObjectOfType<Score>();
    }

    private void Update()
    {
        if (!playerNotInRoom)
        {
            if (stopTime <= 0)
            {
                stopped = false;
            }
            else
            {
                stopped = true;
                stopTime -= Time.deltaTime;
            }
        }
        else{
            stopped = true;
        }

        if (health <= 0)
        {
            AudioManager.Instance.PlaySFX("death");
              score.Kill();
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }
        if ( player != null && player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (!stopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
         
    }
    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
        Vector2 damagePopPos = new Vector2(transform.position.x, transform.position.y + 2.75f);
        Instantiate(damageShow, damagePopPos, Quaternion.identity);
        damageShow.GetComponentInChildren<DamageShow>().damage = damage;
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyType == EnemyType.Green)
        {
            if (timeBtwAttack <= 0)
            {
                anim.SetTrigger("enemyAttack");
                timeBtwAttack = startTimeBtwAttack;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }
    public void OnEnemyAttack()
    {
        Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        player.ChangeHealth(-damage);
    }
}

