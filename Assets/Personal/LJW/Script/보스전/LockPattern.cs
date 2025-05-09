using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LockPattern : MonoBehaviour
{
    public GameObject linePrefab;
    public Canvas canvas;

    public Dictionary<int, CircleIdentifier> circles;

    private List<CircleIdentifier> lines;

    private List<int> inputPattern = new List<int>(); // 입력된 패턴 저장용

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRcTs;
    private CircleIdentifier circleOnEdit;

    private bool unLocking;

    new bool enabled = true;

    public List<int> correctPattern = new List<int> { 0, 1, 2, 5, 8 }; // 정답 패턴 예시

    void Start()
    {
        circles = new Dictionary<int, CircleIdentifier>();
        lines = new List<CircleIdentifier>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var circle = transform.GetChild(i);

            var identifier = circle.GetComponent<CircleIdentifier>();

            identifier.id = i;

            circles.Add(i, identifier);
        }
    }


    void Update()
    {
        if (enabled == false)
        {
            return;
        }

        // 줄을 마우스로 늘리는 코드
        if (unLocking)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);

            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(mousePos, circleOnEdit.transform.localPosition));

            // 줄이 마우스에 따라 회전
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
            Vector3.up, (mousePos - circleOnEdit.transform.localPosition).normalized);

        }
    }

    // 줄 제거 및 초기화
    IEnumerator Release()
    {
        enabled = false;

        // 3초 대기
        yield return new WaitForSeconds(3);

        foreach (var circle in circles)
        {
            circle.Value.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            circle.Value.GetComponent<Animator>().enabled = false;
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();
        inputPattern.Clear();

        lineOnEdit = null;
        lineOnEditRcTs = null;
        circleOnEdit = null;

        enabled = true;
    }

    // 줄 잇기
    GameObject CreateLine(Vector3 pos, int id)
    {
        var line = GameObject.Instantiate(linePrefab, canvas.transform);

        line.transform.localPosition = pos;

        var lineIdf = line.AddComponent<CircleIdentifier>();

        lineIdf.id = id;

        lines.Add(lineIdf);

        return line;
    }

    // 중복되는 줄 제거
    void TrySetLineEdit(CircleIdentifier circle)
    {
        foreach (var line in lines)
        {
            if (line.id == circle.id)
            {
                return;
            }
        }

        lineOnEdit = CreateLine(circle.transform.localPosition, circle.id);
        lineOnEditRcTs = lineOnEdit.GetComponent<RectTransform>();
        circleOnEdit = circle;

        inputPattern.Add(circle.id); // 입력된 순서 기록 
    }

    void EnableColorFade(Animator anim, bool isSuccess) // 추가
    {
        anim.enabled = true;
        anim.Rebind();

        // 추가
        if (isSuccess)
            anim.SetBool("Success", isSuccess);
        else
            anim.SetBool("Fail", !isSuccess);
    }

    // 정확한 패턴인지 확인하는 bool
    bool IsCorrectPattern()
    {
        if (inputPattern.Count != correctPattern.Count)
            return false;

        for (int i = 0; i < correctPattern.Count; i++)
        {
            if (inputPattern[i] != correctPattern[i])
                return false;
        }

        FindAnyObjectByType<Phase1_Manager>()?.SolvePuzzle();

        return true;
    }

    public void OnMouseEnterCircle(CircleIdentifier idf)
    {
        if (enabled == false)
        {
            return;
        }

        // 줄이 동그라미 안에 들어갔을 때 자동 가운데 정렬
        if (unLocking)
        {
            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(circleOnEdit.transform.localPosition, idf.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
                Vector3.up, (idf.transform.localPosition - circleOnEdit.transform.localPosition).normalized);

            TrySetLineEdit(idf);
        }
    }
    public void OnMouseOutCircle(CircleIdentifier idf)
    {
        if (enabled == false)
        {
            return;
        }
    }
    public void OnMouseDownCircle(CircleIdentifier idf)
    {
        if (enabled == false)
        {
            return;
        }

        unLocking = true;

        TrySetLineEdit(idf);


    }
    public void OnMouseUpCircle(CircleIdentifier idf)
    {
        if (enabled == false)
        {
            return;
        }

        // 동그라미 fade
        if (unLocking)
        {
            /*foreach (var line in lines)
            {
                EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>());
            }*/



            // 줄 fade
            /*foreach (var line in lines)
            {
                EnableColorFade(line.GetComponent<Animator>());
            }*/

            if (IsCorrectPattern())
            {
                Debug.Log("정답입니다!");
                foreach (var line in lines)
                    EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>(), true);
                foreach (var line in lines)
                    EnableColorFade(line.GetComponent<Animator>(), true);
            }
            else
            {
                Debug.Log("오답입니다!");
                foreach (var line in lines)
                    EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>(), false);
                foreach (var line in lines)
                    EnableColorFade(line.GetComponent<Animator>(), false);
            }

            // 마지막 줄 지우기
            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);

            StartCoroutine(Release());
        }

        unLocking = false;
    }
}
