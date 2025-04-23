using System;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float faceDir = 1;

    private bool isCol = false;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private BoxCollider2D boxCol => GetComponent<BoxCollider2D>();

    private InteractiveObject interactive;
    private void Update()
    {
        KeyEvent();

        if(interactive != null)
        {
            CollisionKeyEvent();

		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Object"))
        {
            interactive = collision.gameObject.GetComponent<InteractiveObject>();
        }
	}



	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Object"))
        {
            isCol = false;
			interactive.FreezeObject(true);
            interactive = null;
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
            }

            if(Input.GetKeyUp(KeyCode.X))
            {
			    boxCol.offset = new Vector2(0, 0);
			    boxCol.size = new Vector2(1, 1);
		    }
        }    

	}

    private void CollisionKeyEvent()
    {
		if (Input.GetKeyDown(KeyCode.F))
        {
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
			interactive?.FreezeObject(true);
            isCol = false;
		}
	}
}
