using UnityEngine;

public class SafeDoorAnimator : MonoBehaviour
{
    private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		animator.speed = 0f;
	}

	public void PlayAnim()
	{
		animator.speed = 1f;
	}
}
