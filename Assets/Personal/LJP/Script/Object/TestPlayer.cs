using System;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float faceDir = 1;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private BoxCollider2D boxCol => GetComponent<BoxCollider2D>();

    void Update()
    {
        KeyEvent();
    }

	private void KeyEvent()
	{
		if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.linearVelocity = new Vector2(moveSpeed * -faceDir, rb.linearVelocityY);
        }

		if (Input.GetKey(KeyCode.RightArrow))
		{
            rb.linearVelocity = new Vector2(moveSpeed * faceDir, rb.linearVelocityY);
		}
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }

        if(Input.GetKey(KeyCode.X))
        {
            boxCol.offset = new Vector2(0, -0.25f);
            boxCol.size = new Vector2(1, 0.5f);
        }

        if(Input.GetKeyUp(KeyCode.X))
        {
			boxCol.offset = new Vector2(0, 0);
			boxCol.size = new Vector2(1, 1);
		}
	}
}
