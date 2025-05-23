﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Coroutine autoReturnRoutine;
    public float lifetime = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        
        autoReturnRoutine = StartCoroutine(AutoReturnToPool());
    }

    private void OnDisable()
    {
        
        if (autoReturnRoutine != null)
            StopCoroutine(autoReturnRoutine);
    }

    private IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPool.Instance.ReturnObject(gameObject);
        Debug.Log(" หมดเวลา → บอลกลับ Pool");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Targetitem"))
        {
            
            TargetItem target = collision.collider.GetComponent<TargetItem>();
            if (target != null)
            {
                target.OnHit(); 
            }

            ObjectPool.Instance.ReturnObject(gameObject);
            Debug.Log(" ชนกับ Targetitem → บอลหาย");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BallThrower thrower = other.GetComponent<BallThrower>();
            if (thrower != null)
            {
                thrower.CollectBall(1);
                ObjectPool.Instance.ReturnObject(gameObject);
                Debug.Log(" ผู้เล่นเก็บบอลคืน");
            }
        }
    }
}
