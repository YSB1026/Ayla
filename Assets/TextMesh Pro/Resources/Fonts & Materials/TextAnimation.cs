using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;          // 일반 글자 출력 속도
    public float lineBreakDelay = 0.3f;        // 줄바꿈(\n) 시 대기 시간
    public TMP_Text textComponent;
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
