using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Walls")]
    public GameObject[] walls;
    public GameObject wallEffect;
    public GameObject door;

    [Header("Powerups")]
    public GameObject healthPotion;
    public GameObject shield;
     

    [Header("Enemies")]
    public GameObject boss;
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [HideInInspector] public List<GameObject> enemies;
    [HideInInspector] public bool roomBeforeBoss;
    [HideInInspector] public bool bossRoom;

    private RoomVariants variants;
    private bool spawned;
    private bool wallsDestroyed;

    private void Awake() {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void Start()
    {
        variants.rooms.Add(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;

            if (!bossRoom)
            {
                foreach (Transform spawner in enemySpawners)
                {
                    int rand = Random.Range(0, 11);
                    if (rand < 9)
                    {
                        GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                        GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                        enemy.transform.parent = transform;
                        enemies.Add(enemy);
                    }
                    else if (rand == 9)
                    {
                        Instantiate(healthPotion, spawner.position, Quaternion.identity);
                    }
                    else if (rand == 10)
                    {
                        Instantiate(shield, spawner.position, Quaternion.identity);
                    }
                }
                StartCoroutine(CheckEnemies());
            }
            else if (bossRoom)
            {
                boss.SetActive(true);
            }
        }
        else if (other.CompareTag("Player") && spawned)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().playerNotInRoom = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawned)
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().playerNotInRoom = true;
            }
        }
    }
    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => enemies.Count == 0);
        DestroyWalls();
    }

    public void DestroyWalls()
    {
        if (!bossRoom)
        {
            foreach (GameObject wall in walls)
            {
                if (wall.transform.childCount != 0)
                {
                    Instantiate(wallEffect, wall.transform.position, Quaternion.identity);
                    Destroy(wall);
                }
                wallsDestroyed = true;
            }
            
        }
        
    }
     private void OnTriggerStay2D(Collider2D other)
    {
        if(wallsDestroyed && other.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }
    }
    
}