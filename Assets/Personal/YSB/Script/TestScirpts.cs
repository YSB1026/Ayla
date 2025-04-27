using System.Collections;
using UnityEngine;

public class TestScirpts : MonoBehaviour
{
    [Header("이동 정보")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private bool isFacingLeft = true;

    //일단 임시로 코루틴으로 0.5초마다 실행시켰습니다.
    //애니메이션에서 add event가 조금더 괜찮아 보이네요.
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
