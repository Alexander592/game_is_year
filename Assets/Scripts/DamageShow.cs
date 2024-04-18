using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageShow : MonoBehaviour
{
    [HideInInspector] public float damage;
    private TextMesh tm;

    private void Start()
    {
        tm = GetComponent<TextMesh>();
        tm.text = "-" + damage;
    }
    public void OnAnimationOver()
    {
        Destroy(transform.parent.gameObject);
    }

   
}
