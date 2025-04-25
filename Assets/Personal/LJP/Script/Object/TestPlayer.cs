using System;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float pushMoveSpeed;
    [SerializeField] private float sitMoveSpeed;
    [SerializeField] private float jumpForce;
    private float moveSpeed;
    private float faceDir = 1;

    private bool isCol = false;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol;

    private InteractiveObject interactive;

	private void Start()
	{
        moveSpeed = defaultMoveSpeed;

        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
	}

	private void Update()
    {
        KeyEvent();

        if(interactive != null)
        {
            //CollisionKeyEvent();

		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Object"))
        {
            // = collision.gameObject.GetComponent<InteractiveObject>();
        }
	}



	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Object"))
        {
            /*isCol = false;
			interactive.FreezeObject(true);
            interactive = null;*/
        }
	}

	private void KeyEvent()
	{
        if (!isCol)
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
                moveSpeed = sitMoveSpeed;
            }

            if(Input.GetKeyUp(KeyCode.X))
            {
			    boxCol.offset = new Vector2(0, 0);
			    boxCol.size = new Vector2(1, 1);
                moveSpeed = defaultMoveSpeed;
		    }
        }    

	}

    private void CollisionKeyEvent()
    {
		if (Input.GetKeyDown(KeyCode.F))
        {
            moveSpeed = pushMoveSpeed;
            rb.linearVelocity = new Vector2(0, 0);
			interactive?.FreezeObject(false);
            isCol = true;
        }

        if(isCol)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
				interactive?.MoveObject(faceDir);
				transform.position += new Vector3(moveSpeed * faceDir * Time.deltaTime, 0);
			}

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				interactive?.MoveObject(-faceDir);
				transform.position += new Vector3(moveSpeed * -faceDir * Time.deltaTime, 0);
			}
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
            moveSpeed = defaultMoveSpeed;
			interactive?.FreezeObject(true);
            isCol = false;
		}
	}
}
