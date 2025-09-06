using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("��Ÿ��")]
    [SerializeField] private float cooldownTime = 10f;          // �ɷ� ���� ��� �ð�
    [SerializeField] private TextMeshProUGUI cooldownText;      // ���� ��Ÿ�� ǥ�ÿ�

    [Header("����Ʈ ����")]
    [SerializeField] private Light2D spotLight;                 

    [Header("����Ʈ ��ǥġ (���� �� ���ް�)")]
    [SerializeField] private float targetInnerAngle = 82.19f;      // ���� ���� ����
    [SerializeField] private float targetOuterAngle = 82.19f;      // ���� �ܺ� ����
    [SerializeField] private float targetIntensity = 5f;    // ���� ���
    [SerializeField] private float targetOuterRadius = 8f;      // ���� �ݰ�
    [SerializeField] private float edgeGapDegrees = 4f; // �ܰ��� - ������ ����


    [Header("����Ʈ ���۰� (���� ����)")]
    [SerializeField] private float startInnerAngle = 0f;        // ���� ���� ����
    [SerializeField] private float startOuterAngle = 5f;        // ���� �ܺ� ����
    [SerializeField] private float startIntensity = 0.2f;     // ���� ���
    [SerializeField] private float startOuterRadius = 3f;       // ���� �ݰ�

    [Header("Ÿ�̹�")]
    [SerializeField] private float openDuration = 0.25f;       // ���� ���� �ð�

    [Header("�÷��� ����")]
    [SerializeField] private float flashIntensityMultiplier = 2f; // Ÿ�� ��⿡ ���ؼ� ���������� �� ���
    [SerializeField] private bool animateOuterRadius = true;     // ���� �� �ݰ浵 �Բ� �ø���
    [SerializeField] private UnityEngine.UI.Image flashImage; // ��ü ȭ�� �÷��ÿ�
    [SerializeField] private float flashHold = 0.12f; // ȭ���� �Ͼ�� ������ �ð�
    [SerializeField] private float flashFade = 0.4f;      // �ܻ� ���̵� �ð�

    [Header("�̺�Ʈ ��")]
    public UnityEvent onFlashBurst;   // �÷��� ������ ������ ȣ���� ����Ʈ

    [Header("���� �ɼ�")]
    [SerializeField] private bool animateIntensity = true;    // ��� �ִϸ��̼� ���� ����
    [SerializeField] private bool disableLightWhenIdle = true;  // ��� �� ����Ʈ ���� ��Ȱ��ȭ

    private float cooldownTimer;            // ���� ��Ÿ��
    private bool isActive;                 // ���� �ɷ� ���� ������
    private Coroutine playRoutine;

    private void Awake()
    {
        // ó������ ���� ���·� ����
        if (spotLight != null)
        {
            ApplyLightValues(startInnerAngle, startOuterAngle, targetIntensity, startOuterRadius);
            if (disableLightWhenIdle)
                spotLight.enabled = false;
        }

        // �÷��� ������ �����ϰ� + �÷��� �� ����
        if (flashImage) flashImage.color = new Color(1, 0, 0, 0); 
    }

    private void Update()
    {
        UpdateFacing();

        // ��Ÿ�� ����
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f) cooldownTimer = 0f;
        }

        // ��Ÿ�� UI
        UpdateCooldownUI();

        // ���� Ŭ������ �ߵ�
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
            // �÷��̾� �����ʿ� ���� �� ������ �ٶ󺸱�
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            // �÷��̾� ���ʿ� ���� �� �⺻�� ����
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void TryUseAbility()
    {
        if (isActive) return;            // ���� �߿��� ����
        if (cooldownTimer > 0f) return;  // ��Ÿ�� ���̸� ����

        // ���� ����
        playRoutine = StartCoroutine(PlayLightRoutine());
    }

    private IEnumerator PlayLightRoutine()
    {
        isActive = true;

        if (spotLight == null)
        {
            Debug.LogWarning("����Ʈ ���� X");
            isActive = false;
            yield break;
        }

        // ���۰����� ���� �� �ѱ�
        ApplyLightValues(startInnerAngle, startOuterAngle, targetIntensity, startOuterRadius);
        spotLight.enabled = true;

        // ����
        yield return AnimateLight(
            startInnerAngle, targetInnerAngle,
            startOuterAngle, targetOuterAngle,
            startIntensity, targetIntensity,
            startOuterRadius, targetOuterRadius,
            openDuration, easeOutCubic: true
        );

        // �÷���
        float prevIntensity = spotLight.intensity;
        spotLight.intensity = targetIntensity * Mathf.Max(1f, flashIntensityMultiplier);
        onFlashBurst?.Invoke(); // ����, ����ũ �� ����

        // ȭ�� ���� �÷���
        if (flashImage)
        {
            flashImage.color = new Color(1, 0, 0, 1); // �� ������ ���� ������
            StartCoroutine(HideFlashAfter());
        }

        // ��Ÿ�� ����
        cooldownTimer = cooldownTime;

        // ���� ����� ���� �ʱ�ȭ
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

            // ��¡
            if (easeOutCubic)
            {
                float oneMinus = 1f - k;
                k = 1f - (oneMinus * oneMinus * oneMinus);
            }

            float outerA = Mathf.Lerp(fromOuterAngle, toOuterAngle, k);
            float innerA = Mathf.Max(0f, outerA - edgeGapDegrees);

            // ���� ����(�߽� ���� ����)
            float inten = targetIntensity;

            // �ݰ��� �ʿ�ø� ����
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
        // ����
        yield return new WaitForSeconds(flashHold);

        // ������ �������
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / flashFade;
            float alpha = Mathf.Lerp(1f, 0f, t);

            if (flashImage)
                flashImage.color = new Color(1, 0, 0, alpha);

            yield return null;
        }

        // ������ ������ �� ����Ʈ�� ����
        if (disableLightWhenIdle && spotLight)
            spotLight.enabled = false;
    }

    private void ApplyLightValues(float innerAngle, float outerAngle, float intensity, float outerRadius)
    {
        // Light2D ������ ����/�ܺ� ������ �ܺ� �ݰ� ���
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

        cooldownText.text = (cooldownTimer > 0f) ? $"{cooldownTimer:F1}" : "�غ� �Ϸ�";
    }
}