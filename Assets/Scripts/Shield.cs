using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float cooldown;

    [HideInInspector] public bool isCooldown;

    private Image shieldIcon;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        shieldIcon = GetComponent<Image>();
        isCooldown = true;
    }

    private void Update()
    {
        if (isCooldown)
        {
            shieldIcon.fillAmount -= 1 / cooldown * Time.deltaTime;
            if (shieldIcon.fillAmount <= 0)
            {
                shieldIcon.fillAmount = 1;
                isCooldown = false;
                player.shield.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ReduceFillAmount(int damage)
    {
        shieldIcon.fillAmount += damage / 5f;
    }
    public void ResetShield()
    {
        shieldIcon.fillAmount = 1;
    }
}
