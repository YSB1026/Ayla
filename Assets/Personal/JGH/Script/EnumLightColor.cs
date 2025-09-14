using UnityEngine;
using System.Collections;

public enum ColorOption
{
    White,
    Red,
    Black,
    Blue,
    Green
}
public class LightColorController : MonoBehaviour
{
    // 색상을 위한 열거형(Enum) 정의

    // SpriteRenderer 컴포넌트 참조
    public SpriteRenderer spriteRenderer;

    // HardLight2D 컴포넌트 참조
    public HardLight2D hardLight2D;

    // 현재 색상
    public ColorOption currentColor { get; private set; } = ColorOption.White;

    // 색상 변경 시간 (초)
    public float colorChangeDuration = 1.0f;

    // 현재 실행 중인 코루틴 참조
    private Coroutine colorChangeCoroutine;
    private Coroutine rangeChangeCoroutine;

    [Header("HardLight2D 범위 변경 설정")]
    [SerializeField] private float rangeChangeDuration = 0.5f;
    [SerializeField] private float targetRange = 12f;

    private bool isFading = false;
    private Color whiteColor = new Color(214f / 255f, 236f / 255f, 248f / 255f);
    private Color greenColor = new Color(155f / 255f, 1f, 73f / 255f);

    // Start는 첫 실행 전 한 번 호출됩니다
    void Start()
    {
        // SpriteRenderer가 할당되지 않았다면 찾기
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // HardLight2D가 없으면 경고 메시지 출력
        if (hardLight2D == null)
        {
            Debug.LogWarning("HardLight2D 컴포넌트가 할당되지 않았습니다. Inspector에서 할당해주세요.");
        }

        // 초기 색상 적용
        ApplyCurrentColor(false); // 페이드 없이 즉시 적용

        GameManager.Instance.RegistLightController(this);
    }

    // Update는 매 프레임마다 호출됩니다
    void Update()
    {
        // 스페이스바를 누르면 색상 변경 (순차적으로 색상 변화)
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     // 다음 색상으로 변경
        //     ColorOption nextColor = (ColorOption)(((int)currentColor + 1) % System.Enum.GetValues(typeof(ColorOption)).Length);
        //     ChangeColorWithFade(nextColor);
        // }
    }

    // 페이드 효과와 함께 색상 변경
    public void ChangeColorWithFade(ColorOption newColor)
    {
        // 현재 실행 중인 코루틴이 있다면 중지
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }

        // 새로운 코루틴 시작
        colorChangeCoroutine = StartCoroutine(FadeToColor(newColor));
    }

    // 색상 페이드 코루틴
    private IEnumerator FadeToColor(ColorOption newColor)
    {
        if (isFading) yield break;
        Color startColor = GetColorFromEnum(currentColor);
        Color targetColor = GetColorFromEnum(newColor);

        float elapsedTime = 0;

        while (elapsedTime < colorChangeDuration)
        {
            // 시간 경과에 따른 보간값 계산 (0~1)
            float t = elapsedTime / colorChangeDuration;

            // 현재 색상 계산 (선형 보간)
            Color currentColorValue = Color.Lerp(startColor, targetColor, t);

            // 색상 적용
            if (spriteRenderer != null)
            {
                spriteRenderer.color = currentColorValue;
            }

            if (hardLight2D != null)
            {
                hardLight2D.Color = currentColorValue;
            }

            // 다음 프레임까지 대기
            yield return null;

            // 경과 시간 갱신
            elapsedTime += Time.deltaTime;
        }

        // 페이드가 완료되면 정확한 대상 색상을 설정
        if (spriteRenderer != null)
        {
            spriteRenderer.color = targetColor;
        }

        if (hardLight2D != null)
        {
            hardLight2D.Color = targetColor;
        }

        // 현재 색상 업데이트
        currentColor = newColor;
    }

    // 현재 선택된 색상을 즉시 적용하는 메서드
    void ApplyCurrentColor(bool withFade = true)
    {
        if (withFade)
        {
            ChangeColorWithFade(currentColor);
        }
        else
        {
            // 페이드 없이 즉시 적용
            Color newColor = GetColorFromEnum(currentColor);

            if (spriteRenderer != null)
            {
                spriteRenderer.color = newColor;
            }

            if (hardLight2D != null)
            {
                hardLight2D.Color = newColor;
            }
        }
    }

    // 열거형에서 Color 가져오기
    private Color GetColorFromEnum(ColorOption color)
    {
        switch (color)
        {
            case ColorOption.White:
                return whiteColor;
            case ColorOption.Red:
                return Color.red;
            case ColorOption.Black:
                return Color.black;
            case ColorOption.Blue:
                return Color.blue;
            case ColorOption.Green:
                return greenColor;
            default:
                return whiteColor;
        }
    }

    public void ChangeRangeWithFade()
    {
        if (hardLight2D == null) return;

        // if (rangeChangeCoroutine != null)
        //     StopCoroutine(rangeChangeCoroutine);

        if(!isFading)
            rangeChangeCoroutine = StartCoroutine(FadeRange());
    }

    private IEnumerator FadeRange()
    {
        isFading = true;

        float originalRange = hardLight2D.Range;
        float elapsedTime = 0f;

        while (elapsedTime < rangeChangeDuration)
        {
            float t = elapsedTime / rangeChangeDuration;
            hardLight2D.Range = Mathf.Lerp(originalRange, targetRange, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hardLight2D.Range = targetRange;

        elapsedTime = 0f;
        while (elapsedTime < rangeChangeDuration)
        {
            float t = elapsedTime / rangeChangeDuration;
            hardLight2D.Range = Mathf.Lerp(targetRange, originalRange, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hardLight2D.Range = originalRange;
        isFading = false;
    }
}