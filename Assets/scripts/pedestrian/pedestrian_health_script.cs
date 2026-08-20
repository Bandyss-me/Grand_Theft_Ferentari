using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class pedestrian_health_script : MonoBehaviour
{
    [SerializeField] 
    float maxHealth;

    [SerializeField] 
    GameObject body;

    CharacterController caracterController;
    Animator animator;
    Rigidbody[] ragdolls;
    float health;

    private void Start()
    {
        caracterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ragdolls = GetComponentsInChildren<Rigidbody>();
        health = maxHealth;
    }
    
    IEnumerator Die(Vector3 direction)
    {
        Coroutine coroutine = GetComponent<pedestrian_script>().coroutine;
        if(coroutine!=null) StopCoroutine(coroutine);
        caracterController.enabled = false;
        yield return new WaitForSeconds(1f);
        animator.enabled = false;
        foreach (Rigidbody ragdoll in ragdolls)
        {
            ragdoll.isKinematic = false;
        }
        body.GetComponent<Rigidbody>().AddForce(direction.normalized*20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(30f);
        foreach (Rigidbody ragdoll in ragdolls)
        {
            ragdoll.isKinematic = true;
        }
        GetComponent<pedestrian_script>().Spawn();
        animator.enabled = true;
        animator.SetTrigger("lost");
        caracterController.enabled = true;
        health = maxHealth;
    }
    
    public void HitMe(float damage, Vector3 direction)
    {
        health -= damage;
        if (health < 0)
            StartCoroutine(Die(direction));
    }
}
