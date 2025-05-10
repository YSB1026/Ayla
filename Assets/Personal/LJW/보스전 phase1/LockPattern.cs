using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPattern : MonoBehaviour
{
    [Header("프리팹 · 캔버스")]
    public GameObject linePrefab;
    public Canvas canvas;  // World Space Canvas

    Dictionary<int, CircleIdentifier> circles;
    List<CircleIdentifier> lines;
    List<int> inputPattern = new List<int>();

    CircleIdentifier lastCircle;

    bool enabled = true;

    [Header("정답 패턴")]
    public List<int> correctPattern = new List<int> { 0, 1, 2, 5, 8 };

    void Start()
    {
        circles = new Dictionary<int, CircleIdentifier>();
        lines = new List<CircleIdentifier>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var c = transform.GetChild(i).GetComponent<CircleIdentifier>();
            c.id = i;
            circles[i] = c;
        }
    }

    IEnumerator Release()
    {
        enabled = false;
        yield return new WaitForSeconds(3f);

        foreach (var c in circles.Values)
        {
            var img = c.GetComponent<Image>(); if (img) img.color = Color.white;
            var anim = c.GetComponent<Animator>(); if (anim) anim.enabled = false;
        }
        foreach (var l in lines)
            Destroy(l.gameObject);

        lines.Clear();
        inputPattern.Clear();
        lastCircle = null;
        enabled = true;
    }

    GameObject CreateLineBetween(CircleIdentifier start, CircleIdentifier end, int id)
    {
        var go = Instantiate(linePrefab, canvas.transform);
        var rect = go.GetComponent<RectTransform>();

        RectTransform canvasRT = canvas.GetComponent<RectTransform>();
        Vector2 startLocal = canvasRT.InverseTransformPoint(start.GetComponent<RectTransform>().position);
        Vector2 endLocal = canvasRT.InverseTransformPoint(end.GetComponent<RectTransform>().position);

        Vector2 mid = (startLocal + endLocal) * 0.5f;
        rect.localPosition = mid;

        Vector2 dir = (endLocal - startLocal).normalized;
        float dist = Vector2.Distance(startLocal, endLocal);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, dist);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rect.localRotation = Quaternion.Euler(0, 0, angle);

        var idf = go.AddComponent<CircleIdentifier>();
        idf.id = id;
        lines.Add(idf);
        return go;
    }

    bool IsCorrectPattern()
    {
        if (inputPattern.Count != correctPattern.Count) return false;
        for (int i = 0; i < correctPattern.Count; i++)
            if (inputPattern[i] != correctPattern[i]) return false;
        FindAnyObjectByType<Phase1_Manager>()?.SolvePuzzle();
        return true;
    }

    void EnableColorFade(Animator anim, bool isSuccess)
    {
        anim.enabled = true;
        anim.Rebind();
        anim.SetBool("Success", isSuccess);
        anim.SetBool("Fail", !isSuccess);
    }

    public void OnMouseDownCircle(CircleIdentifier idf)
    {
        if (!enabled) return;
        lastCircle = idf;
        inputPattern.Clear();
        lines.ForEach(l => Destroy(l.gameObject));
        lines.Clear();
        inputPattern.Add(idf.id);
    }

    public void OnMouseEnterCircle(CircleIdentifier idf)
    {
        if (!enabled || lastCircle == null || inputPattern.Contains(idf.id)) return;
        CreateLineBetween(lastCircle, idf, idf.id);
        inputPattern.Add(idf.id);
        lastCircle = idf;
    }

    public void OnMouseUpCircle(CircleIdentifier idf)
    {
        if (!enabled) return;
        bool correct = IsCorrectPattern();
        Debug.Log(correct ? "정답입니다!" : "오답입니다!");

        foreach (var circle in circles.Values)
        {
            if (inputPattern.Contains(circle.id))
            {
                var anim = circle.GetComponent<Animator>();
                if (anim) EnableColorFade(anim, correct);
            }
        }

        foreach (var line in lines)
        {
            var anim = line.GetComponent<Animator>();
            if (anim) EnableColorFade(anim, correct);
        }

        StartCoroutine(Release());
    }
}

