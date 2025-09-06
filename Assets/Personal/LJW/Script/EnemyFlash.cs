using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("쿨타임")]
    [SerializeField] private float cooldownTime = 10f;          // 능력 재사용 대기 시간
    [SerializeField] private TextMeshProUGUI cooldownText;      // 남은 쿨타임 표시용

    [Header("라이트 참조")]
    [SerializeField] private Light2D spotLight;                 

    [Header("라이트 목표치 (열릴 때 도달값)")]
    [SerializeField] private float targetInnerAngle = 82.19f;      // 최종 내부 각도
    [SerializeField] private float targetOuterAngle = 82.19f;      // 최종 외부 각도
    [SerializeField] private float targetIntensity = 5f;    // 최종 밝기
    [SerializeField] private float targetOuterRadius = 8f;      // 최종 반경
    [SerializeField] private float edgeGapDegrees = 4f; // 외곽각 - 내곽각 간격


    [Header("라이트 시작값 (닫힌 상태)")]
    [SerializeField] private float startInnerAngle = 0f;        // 시작 내부 각도
    [SerializeField] private float startOuterAngle = 5f;        // 시작 외부 각도
    [SerializeField] private float startIntensity = 0.2f;     // 시작 밝기
    [SerializeField] private float startOuterRadius = 3f;       // 시작 반경

    [Header("타이밍")]
    [SerializeField] private float openDuration = 0.25f;       // 각도 여는 시간

    [Header("플래시 설정")]
    [SerializeField] private float flashIntensityMultiplier = 2f; // 타겟 밝기에 곱해서 순간적으로 더 밝게
    [SerializeField] private bool animateOuterRadius = true;     // 열릴 때 반경도 함께 늘릴지
    [SerializeField] private UnityEngine.UI.Image flashImage; // 전체 화면 플래시용
    [SerializeField] private float flashHold = 0.12f; // 화면이 하얗게 유지될 시간
    [SerializeField] private float flashFade = 0.4f;      // 잔상 페이드 시간

    [Header("이벤트 훅")]
    public UnityEvent onFlashBurst;   // 플래시 터지는 순간에 호출할 이펙트

    [Header("세부 옵션")]
    [SerializeField] private bool animateIntensity = true;    // 밝기 애니메이션 포함 여부
    [SerializeField] private bool disableLightWhenIdle = true;  // 대기 시 라이트 완전 비활성화

    private float cooldownTimer;            // 남은 쿨타임
    private bool isActive;                 // 현재 능력 연출 중인지
    private Coroutine playRoutine;

    private void Awake()
    {
        // 처음에는 닫힌 상태로 세팅
        if (spotLight != null)
        {
            ApplyLightValues(startInnerAngle, startOuterAngle, targetIntensity, startOuterRadius);
            if (disableLightWhenIdle)
                spotLight.enabled = false;
        }

        // 플래시 시작은 투명하게 + 플래시 색 빨강
        if (flashImage) flashImage.color = new Color(1, 0, 0, 0); 
    }

    private void Update()
    {
        UpdateFacing();

        // 쿨타임 감소
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f) cooldownTimer = 0f;
        }

        // 쿨타임 UI
        UpdateCooldownUI();

        // 왼쪽 클릭으로 발동
        if (Input.GetMouseButtonDown(0))
        {
            TryUseAbility();
        }
    }

    private void UpdateFacing()
    {
        if (player == null || spotLight == null) return;

        float dx = transform.position.x - player.position.x;

        if (dx < 0f)
        {
            // 플레이어 오른쪽에 있음 → 오른쪽 바라보기
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            // 플레이어 왼쪽에 있음 → 기본값 유지
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void TryUseAbility()
    {
        if (isActive) return;            // 연출 중에는 무시
        if (cooldownTimer > 0f) return;  // 쿨타임 중이면 무시

        // 연출 시작
        playRoutine = StartCoroutine(PlayLightRoutine());
    }

    private IEnumerator PlayLightRoutine()
    {
        isActive = true;

        if (spotLight == null)
        {
            Debug.LogWarning("라이트 연결 X");
            isActive = false;
            yield break;
        }

        // 시작값으로 세팅 후 켜기
        ApplyLightValues(startInnerAngle, startOuterAngle, targetIntensity, startOuterRadius);
        spotLight.enabled = true;

        // 열기
        yield return AnimateLight(
            startInnerAngle, targetInnerAngle,
            startOuterAngle, targetOuterAngle,
            startIntensity, targetIntensity,
            startOuterRadius, targetOuterRadius,
            openDuration, easeOutCubic: true
        );

        // 플래시
        float prevIntensity = spotLight.intensity;
        spotLight.intensity = targetIntensity * Mathf.Max(1f, flashIntensityMultiplier);
        onFlashBurst?.Invoke(); // 사운드, 셰이크 등 연결

        // 화면 빨간 플래시
        if (flashImage)
        {
            flashImage.color = new Color(1, 0, 0, 1); // 한 프레임 동안 빨간색
            StartCoroutine(HideFlashAfter());
        }

        // 쿨타임 시작
        cooldownTimer = cooldownTime;

        // 다음 사용을 위한 초기화
        ApplyLightValues(startInnerAngle, startOuterAngle, startIntensity, startOuterRadius);

        isActive = false;
    }

    private IEnumerator AnimateLight(
        float fromInnerAngle, float toInnerAngle,
        float fromOuterAngle, float toOuterAngle,
        float fromIntensity, float toIntensity,
        float fromRadius, float toRadius,
        float duration, bool easeOutCubic = true)
    {
        if (duration <= 0f)
        {
            ApplyLightValues(toInnerAngle, toOuterAngle, toIntensity, toRadius);
            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float k = Mathf.Clamp01(t);

            // 이징
            if (easeOutCubic)
            {
                float oneMinus = 1f - k;
                k = 1f - (oneMinus * oneMinus * oneMinus);
            }

            float outerA = Mathf.Lerp(fromOuterAngle, toOuterAngle, k);
            float innerA = Mathf.Max(0f, outerA - edgeGapDegrees);

            // 밝기는 고정(중심 번짐 방지)
            float inten = targetIntensity;

            // 반경은 필요시만 보간
            float radius = animateOuterRadius ? Mathf.Lerp(fromRadius, toRadius, k) : spotLight.pointLightOuterRadius;

            ApplyLightValues(innerA, outerA, inten, radius);
            yield return null;
        }
        float finalOuter = toOuterAngle;
        float finalInner = Mathf.Max(0f, finalOuter - edgeGapDegrees);
        ApplyLightValues(finalInner, finalOuter, targetIntensity, animateOuterRadius ? toRadius : spotLight.pointLightOuterRadius);
    }

    private IEnumerator HideFlashAfter()
    {
        // 유지
        yield return new WaitForSeconds(flashHold);

        // 서서히 사라지게
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / flashFade;
            float alpha = Mathf.Lerp(1f, 0f, t);

            if (flashImage)
                flashImage.color = new Color(1, 0, 0, alpha);

            yield return null;
        }

        // 완전히 꺼졌을 때 라이트도 끄기
        if (disableLightWhenIdle && spotLight)
            spotLight.enabled = false;
    }

    private void ApplyLightValues(float innerAngle, float outerAngle, float intensity, float outerRadius)
    {
        // Light2D 스팟은 내부/외부 각도와 외부 반경 사용
        spotLight.pointLightInnerAngle = Mathf.Clamp(innerAngle, 0f, 360f);
        spotLight.pointLightOuterAngle = Mathf.Clamp(outerAngle, 0f, 360f);

        if (animateIntensity)
            spotLight.intensity = Mathf.Max(0f, intensity);

        if (animateOuterRadius)
            spotLight.pointLightOuterRadius = Mathf.Max(0f, outerRadius);
    }

    private void UpdateCooldownUI()
    {
        if (cooldownText == null) return;

        cooldownText.text = (cooldownTimer > 0f) ? $"{cooldownTimer:F1}" : "준비 완료";
    }
}