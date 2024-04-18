using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public GameObject[] faces;
    public GameObject[] guns;
    public GameObject damageShow;
    public GameObject bossEffect;

    public Vector3 stageTwoScale;
    public Vector3 stageThreeScale;

    public float speed;
    public int health;
    public int damage;

    private Slider healthBar;
    private Player player;
    private Vector2 playerPos;

    private int stageNum;

    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        healthBar.value = health;
        healthBar.transform.GetChild(0).gameObject.SetActive(true);
        stageNum = 1;
        StartCoroutine(NewStage());
        StartCoroutine(UpdatePlayerPosition());
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Vector2 damagePopPos = new Vector2(transform.position.x + 3, transform.position.y + 4f);
        Instantiate(damageShow, damagePopPos, Quaternion.identity);
        damageShow.GetComponentInChildren<DamageShow>().damage = damage;
        healthBar.value = health;
        CheckStage();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            player.ChangeHealth(-damage);
        }
    }
    public void CheckStage()
    {
        if (healthBar.value <= 0 && stageNum == 3)
        {
            stageNum = 4;
            faces[2].SetActive(false);
            faces[3].SetActive(true);
            healthBar.gameObject.SetActive(false);
            speed = 0f;
            foreach (GameObject gun in guns)
            {
                gun.SetActive(false);
            }
            Instantiate(bossEffect, transform.position, Quaternion.identity);
        }
        else if (healthBar.value <= 83 && stageNum == 2)
        {
            stageNum = 3;
            faces[1].SetActive(false);
            faces[2].SetActive(true);
            transform.localScale = stageThreeScale;
            foreach (GameObject gun in guns)
            {
                gun.SetActive(true);
            }
            Instantiate(bossEffect, transform.position, Quaternion.identity);
        }
        else if (healthBar.value <= 166 && stageNum == 1)
        {
            stageNum = 2;
            faces[0].SetActive(false);
            faces[1].SetActive(true);
            transform.localScale = stageTwoScale;
            Instantiate(bossEffect, transform.position, Quaternion.identity);
        }
    }
    IEnumerator NewStage()
    {
        while (stageNum != 4)
        {
            if (stageNum == 1)
            {
                speed /= 2;
                yield return new WaitForSeconds(5f);
                speed *= 2;
                yield return new WaitForSeconds(1f);
            }
            else if (stageNum == 2)
            {
                speed /= 5;
                yield return new WaitForSeconds(4f);
                speed *= 5;
                yield return new WaitForSeconds(1f);
            }
            else if (stageNum == 3)
            {
                speed /= 3;
                yield return new WaitForSeconds(2f);
                speed *= 3;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    IEnumerator UpdatePlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            playerPos = player.transform.position;
            
            if (player.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}