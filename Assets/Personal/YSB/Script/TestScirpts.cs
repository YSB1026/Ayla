using System.Collections;
using UnityEngine;

public class TestScirpts : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private bool isFacingLeft = true;

    //�ϴ� �ӽ÷� �ڷ�ƾ���� 0.5�ʸ��� ������׽��ϴ�.
    //�ִϸ��̼ǿ��� add event�� ���ݴ� ������ ���̳׿�.
    public bool isWalking = false;
    private Coroutine walkSoundCoroutine;


    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        HandleInput();
        FlipControl();
        Move();
        SoundHandler();
    }

    void HandleInput()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        isWalking = moveDir == Vector2.zero ? false : true;
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

    private void SoundHandler()
    {
        if (isWalking && walkSoundCoroutine == null)
        {
            walkSoundCoroutine = StartCoroutine(PlayWalkSoundPeriodically());
        }
        else if (!isWalking && walkSoundCoroutine != null)
        {
            StopCoroutine(walkSoundCoroutine);
            walkSoundCoroutine = null;
        }
    }

    private IEnumerator PlayWalkSoundPeriodically()
    {
        while (isWalking)
        {
            SoundManager.Instance.PlayPlayerSFX("Walk");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
