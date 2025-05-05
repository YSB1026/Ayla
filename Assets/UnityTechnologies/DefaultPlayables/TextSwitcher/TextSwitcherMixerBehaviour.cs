using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TextSwitcherMixerBehaviour : PlayableBehaviour
{
    Color m_DefaultColor;
    int m_DefaultFontSize;
    string m_DefaultText;

    Text m_TrackBinding;
    bool m_FirstFrameHappened;

    private string m_TargetText = "";
    private float m_TypingStartTime;
    private float m_CharactersPerSecond = 45f; // 원하는 속도
    private ScriptPlayable<TextSwitcherBehaviour> m_PreviousPlayable;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Text;

        if (m_TrackBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultColor = m_TrackBinding.color;
            m_DefaultFontSize = m_TrackBinding.fontSize;
            m_DefaultText = m_TrackBinding.text;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount();
        Color blendedColor = Color.clear;
        float blendedFontSize = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        string strongestText = null;
        ScriptPlayable<TextSwitcherBehaviour> strongestPlayable = default;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TextSwitcherBehaviour> inputPlayable = (ScriptPlayable<TextSwitcherBehaviour>)playable.GetInput(i);
            TextSwitcherBehaviour input = inputPlayable.GetBehaviour();

            blendedColor += input.color * inputWeight;
            blendedFontSize += input.fontSize * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                greatestWeight = inputWeight;
                strongestText = input.text;
                strongestPlayable = inputPlayable;
            }
        }

        // 💡 새로운 클립이 시작되었는지 확인
        if (!strongestPlayable.Equals(m_PreviousPlayable))
        {
            m_TargetText = strongestText ?? "";
            m_TypingStartTime = (float)playable.GetTime(); // 정확히 현재 시간
            m_PreviousPlayable = strongestPlayable;
        }

        // ⌨️ 타이핑 처리
        float elapsedTime = (float)playable.GetTime() - m_TypingStartTime;
        int charactersToShow = Mathf.Clamp(Mathf.FloorToInt(elapsedTime * m_CharactersPerSecond), 0, m_TargetText.Length);
        m_TrackBinding.text = m_TargetText.Substring(0, charactersToShow);

        // 혼합된 스타일 적용
        m_TrackBinding.color = blendedColor + m_DefaultColor * (1f - totalWeight);
        m_TrackBinding.fontSize = Mathf.RoundToInt(blendedFontSize + m_DefaultFontSize * (1f - totalWeight));
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        m_FirstFrameHappened = false;

        if (m_TrackBinding == null)
            return;

        m_TrackBinding.color = m_DefaultColor;
        m_TrackBinding.fontSize = m_DefaultFontSize;
        m_TrackBinding.text = m_DefaultText;
    }
}
