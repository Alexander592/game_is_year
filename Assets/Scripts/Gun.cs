using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunType gunType;
    public Joystick joystick;

    public float offset;
    public GameObject bullet;
    public Transform shotPoint;
    public float startTimeBtwShots;
    private float timeBtwShots;

    private Vector2 difference;
    private float rotZ;
    private Animator camAnim;
    private Player player;

    public enum GunType
    {
        Default,
        Enemy
    }
    private void Start()
    {
        //camAnim = Camera.main.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (gunType == GunType.Default && player.controlType == Player.ControlType.PC)
        {
            joystick.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (gunType == GunType.Default)
        {
            if (player.controlType == Player.ControlType.PC)
            {
                difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            else if (player.controlType == Player.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.3f || Mathf.Abs(joystick.Vertical) > 0.3f)
            {
                rotZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
        }
        else if (gunType == GunType.Enemy)
        {
            if (player != null)
            {
                difference = player.transform.position - transform.position;
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
        }
        if(Input.mousePosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
        else{
            transform.rotation = Quaternion.Euler(180f, 0f, -rotZ - offset);
        }

        if (timeBtwShots <= 0)
        {
            if (player.controlType == Player.ControlType.PC && Input.GetMouseButtonDown(0) || gunType == GunType.Enemy)
            {
                 AudioManager.Instance.PlaySFX("shot");
                  Shoot();
            }
            else if (player.controlType == Player.ControlType.Android)
            {
                if (joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                     Shoot();
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    public void Shoot()
    {
         Instantiate(bullet, shotPoint.position, shotPoint.rotation);
        timeBtwShots = startTimeBtwShots;
        // camAnim.SetTrigger("shake");
    }
}
