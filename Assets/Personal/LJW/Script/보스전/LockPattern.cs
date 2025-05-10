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

    private List<int> inputPattern = new List<int>(); // �Էµ� ���� �����

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRcTs;
    private CircleIdentifier circleOnEdit;

    private bool unLocking;

    new bool enabled = true;

    public List<int> correctPattern = new List<int> { 0, 1, 2, 5, 8 }; // ���� ���� ����

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

        // ���� ���콺�� �ø��� �ڵ�
        if (unLocking)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);

            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(mousePos, circleOnEdit.transform.localPosition));

            // ���� ���콺�� ���� ȸ��
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(
            Vector3.up, (mousePos - circleOnEdit.transform.localPosition).normalized);

        }
    }

    // �� ���� �� �ʱ�ȭ
    IEnumerator Release()
    {
        enabled = false;

        // 3�� ���
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

    // �� �ձ�
    GameObject CreateLine(Vector3 pos, int id)
    {
        var line = GameObject.Instantiate(linePrefab, canvas.transform);

        line.transform.localPosition = pos;

        var lineIdf = line.AddComponent<CircleIdentifier>();

        lineIdf.id = id;

        lines.Add(lineIdf);

        return line;
    }

    // �ߺ��Ǵ� �� ����
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

        inputPattern.Add(circle.id); // �Էµ� ���� ��� 
    }

    void EnableColorFade(Animator anim, bool isSuccess) // �߰�
    {
        anim.enabled = true;
        anim.Rebind();

        // �߰�
        if (isSuccess)
            anim.SetBool("Success", isSuccess);
        else
            anim.SetBool("Fail", !isSuccess);
    }

    // ��Ȯ�� �������� Ȯ���ϴ� bool
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

        // ���� ���׶�� �ȿ� ���� �� �ڵ� ��� ����
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

        // ���׶�� fade
        if (unLocking)
        {
            /*foreach (var line in lines)
            {
                EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>());
            }*/



            // �� fade
            /*foreach (var line in lines)
            {
                EnableColorFade(line.GetComponent<Animator>());
            }*/

            if (IsCorrectPattern())
            {
                Debug.Log("�����Դϴ�!");
                foreach (var line in lines)
                    EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>(), true);
                foreach (var line in lines)
                    EnableColorFade(line.GetComponent<Animator>(), true);
            }
            else
            {
                Debug.Log("�����Դϴ�!");
                foreach (var line in lines)
                    EnableColorFade(circles[line.id].gameObject.GetComponent<Animator>(), false);
                foreach (var line in lines)
                    EnableColorFade(line.GetComponent<Animator>(), false);
            }

            // ������ �� �����
            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);

            StartCoroutine(Release());
        }

        unLocking = false;
    }
}
