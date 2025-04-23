using UnityEngine;
using System.Collections;

public class LightColorController : MonoBehaviour
{
    // 색상을 위한 열거형(Enum) 정의
    public enum ColorOption
    {
        White,
        Red,
        Black,
        Blue,
        Green
    }

    // SpriteRenderer 컴포넌트 참조
    public SpriteRenderer spriteRenderer;
    
    // HardLight2D 컴포넌트 참조
    public HardLight2D hardLight2D;

    // 현재 색상
    public ColorOption currentColor;
    
    // 색상 변경 시간 (초)
    public float colorChangeDuration = 1.0f;
    
    // 현재 실행 중인 코루틴 참조
    private Coroutine colorChangeCoroutine;

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
    }

    // Update는 매 프레임마다 호출됩니다
    void Update()
    {
        // 스페이스바를 누르면 색상 변경 (순차적으로 색상 변화)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 다음 색상으로 변경
            ColorOption nextColor = (ColorOption)(((int)currentColor + 1) % System.Enum.GetValues(typeof(ColorOption)).Length);
            ChangeColorWithFade(nextColor);
        }
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
                return Color.white;
            case ColorOption.Red:
                return Color.red;
            case ColorOption.Black:
                return Color.black;
            case ColorOption.Blue:
                return Color.blue;
            case ColorOption.Green:
                return Color.green;
            default:
                return Color.white;
        }
    }
}