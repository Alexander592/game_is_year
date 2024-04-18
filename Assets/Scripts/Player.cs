using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDataPersistence
{

     [Header("Attributes SO")]
    [SerializeField] private AttributesScriptableObject playerAttributesSO;
     
    [Header("Player")]
    public ControlType controlType;
    public Joystick joystick;
    public Text healthDisplay;
    public int health;
    public float speed;
    private Score score;
    
 

    [Header("Attack")]
    public List<GameObject> unlockedWeapons;
    public GameObject[] allWeapons;
    public Image weaponIcon;

    [Header("Key")]
    public GameObject key;
    public GameObject wallEffect;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;

    public enum ControlType
    {
        PC,
        Android
    }

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Animator anim;
    private bool facingRight = true;

    private bool keyButtonPushed;

    public GameObject panel;

    private void Start()
    {
         rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (controlType == ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
        ChangeHealth(0);
 
    }
     public void LoadData(GameData data)
    {
         this.transform.position = data.playerPosition;

          playerAttributesSO.vitality = data.playerAttributesData.vitality;
        playerAttributesSO.strength = data.playerAttributesData.strength;
        playerAttributesSO.intellect = data.playerAttributesData.intellect;
        playerAttributesSO.endurance = data.playerAttributesData.endurance;
           
         health = data.health;
    }

    public void SaveData(GameData data)
    {
        if(this !=null)
    {  
       data.playerPosition = this.transform.position;

        data.playerAttributesData.vitality = playerAttributesSO.vitality;
        data.playerAttributesData.strength = playerAttributesSO.strength;
        data.playerAttributesData.intellect = playerAttributesSO.intellect;
        data.playerAttributesData.endurance = playerAttributesSO.endurance;
        
       
       data.health = health;
    }
    }
 
    private void Update()
    {

        if (controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        moveVelocity = moveInput.normalized * speed;

        if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        if (!facingRight && moveInput.x > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput.x < 0)
        {
            Flip();

        }
        if (health <= 0)
        {
            AudioManager.Instance.PlaySFX("death");
            panel.SetActive(true);
            Destroy(gameObject);
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            for (int i = 0; i < allWeapons.Length; i++)
            {
                if (other.name == allWeapons[i].name)
                {
                    unlockedWeapons.Add(allWeapons[i].gameObject);
                }
            }
            SwitchWeapon();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Key"))
        {
            key.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Potion"))
        {
            ChangeHealth(5);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Shield"))
        {
            if (!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetShield();
                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Door") && key.activeInHierarchy && keyButtonPushed)
        {
            Instantiate(wallEffect, other.transform.position, Quaternion.identity);
            key.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    public void ChangeHealth(int healthValue)
    {
        if (!shield.activeInHierarchy || shield.activeInHierarchy && healthValue > 0)
        {
            health += healthValue;
            healthDisplay.text = "HP: " + health;
        }
        else if (shield.activeInHierarchy && healthValue < 0)
        {
            shieldTimer.ReduceFillAmount(healthValue);
        }
    }
   
    public void OnKeyButtonDown()
    {
        keyButtonPushed = !keyButtonPushed;
    }

    public void SwitchWeapon()
    {
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (unlockedWeapons[i].activeInHierarchy == true)
            {
                unlockedWeapons[i].SetActive(false);
                if (i != 0)
                {
                    unlockedWeapons[i - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                    weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;
                }
                weaponIcon.SetNativeSize();
                break;
            }
        }
    }
     
}
