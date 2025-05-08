using UnityEngine;

public class AylaPhase1Controller : MonoBehaviour
{
    public float moveSpeed = 6f;
    private Rigidbody2D rb;
    private bool active = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("AylaPhase1Controller 초기화됨");
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    private void FixedUpdate()
    {
        if (!active) return;
        Debug.Log("Phase1 이동 중...");

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y).normalized;

        Vector2 moveDelta = input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }
}
