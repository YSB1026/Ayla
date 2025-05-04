using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 레거시 Text 컴포넌트를 위한 네임스페이스

public class TypewriterEffectLegacy : MonoBehaviour
{
    public float typingSpeed = 0.05f;          // 일반 글자 출력 속도
    public float lineBreakDelay = 0.3f;        // 줄바꿈(\n) 시 대기 시간
    public Text textComponent;                 // TMP_Text 대신 레거시 Text 사용
    [TextArea] public string fullText;

    private Coroutine typingCoroutine;

    void Start()
    {
        StartTyping();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;

            if (c == '\n')
                yield return new WaitForSeconds(lineBreakDelay);
            else
                yield return new WaitForSeconds(typingSpeed);
        }
    }
}