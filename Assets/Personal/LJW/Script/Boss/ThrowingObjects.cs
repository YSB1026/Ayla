using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ThrowingObjects : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D cd;
    private Transform target;

    public Action<bool> onHitPlayerCallback;

    private bool hasHit = false;
    private bool canRotate = true;

    private Vector2 lastDirection;

    [Header("꽂히기 관련")]
    public float freezeTimeDuration = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    public void Setup(Vector2 dir, Transform _target, float speed = 15f)
    {
        target = _target;
        rb.gravityScale = 0f;
        rb.linearVelocity = dir.normalized * speed;

        Invoke("DestroyMe", 7f);
    }

    private void Update()
    {
        if (canRotate)
        {
            if (canRotate && rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                transform.right = rb.linearVelocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;
        hasHit = true;

        canRotate = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cd.enabled = false;

        transform.parent = collision.transform;

        // 콜백은 무조건 한 번만 호출됨
        if (collision.CompareTag("Player"))
        {
            onHitPlayerCallback?.Invoke(true);
        }
        else
        {
            onHitPlayerCallback?.Invoke(false);
        }

        /*if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            hasHit = true;
            StuckInto(collision);

            if (collision.CompareTag("Player"))
                onHitPlayerCallback?.Invoke(true);
            else
                onHitPlayerCallback?.Invoke(false); // 실패로 처리
        }*/
    }

    /*private void StuckInto(Collider2D collision)
    {
        // 꽂힘 처리
        canRotate = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        cd.enabled = false;

        transform.parent = collision.transform;

        if (collision.CompareTag("Player"))
        {
            transform.right = lastDirection;

            float offset = 0.3f;
            Vector3 offsetVector = transform.right * offset;

            offsetVector.y = 0f; // Y축 눌림 제거
            transform.position -= offsetVector;

            Debug.Log("Player에 닿아 멈춤");
        }
        else if (collision.CompareTag("Ground"))
        {
            Debug.Log("Ground에 닿아 멈춤");
        }
    }*/

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
