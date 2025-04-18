using UnityEngine;

public class TestScirpts : MonoBehaviour
{
    [Header("이동 정보")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private bool isFacingLeft = true;

    void Start()
    {
        
    }

    void Update()
    {
        HandleInput();
        FlipControl();
        Move();
    }

    void HandleInput()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    private void FlipControl()
    {
        Vector3 scale = transform.localScale;
        if(moveDir.x > 0 && isFacingLeft)
        {
            scale.x = -scale.x;
            isFacingLeft = false;
        }
        else if (moveDir.x <0 && !isFacingLeft)
        {
            scale.x = -scale.x;
            isFacingLeft = true;
        }

        transform.localScale = scale;
    }

    private void Move()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

}
