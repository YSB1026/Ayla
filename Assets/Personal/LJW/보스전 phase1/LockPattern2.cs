using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockPattern2 : MonoBehaviour
{
    public GameObject linePrefab;
    public Canvas canvas;
    public Transform player;
    public float selectionRadius = 1f; // �� ���� �ݰ�

    public List<int> correctPattern = new List<int> { 0, 1, 2, 5, 8 };

    private Dictionary<int, CircleIdentifier> circles;
    private List<CircleIdentifier> lines;
    private List<int> inputPattern = new List<int>();

    private GameObject lineOnEdit;
    private RectTransform lineOnEditRcTs;
    private CircleIdentifier circleOnEdit;

    private bool unLocking;
    private new bool enabled = true;

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
        if (!enabled) return;

        // �÷��̾ E Ű�� ���� ���� ����� ���� ����
        if (Input.GetKeyDown(KeyCode.E))
        {
            TrySelectNearestCircle();
        }

        // �÷��̾� ���� ���� ����
        if (unLocking && circleOnEdit != null && lineOnEditRcTs != null)
        {
            Vector3 playerPos = canvas.transform.InverseTransformPoint(player.position);
            lineOnEditRcTs.sizeDelta = new Vector2(lineOnEditRcTs.sizeDelta.x, Vector3.Distance(playerPos, circleOnEdit.transform.localPosition));
            lineOnEditRcTs.rotation = Quaternion.FromToRotation(Vector3.up, (playerPos - circleOnEdit.transform.localPosition).normalized);
        }

        // ���� �Է� ����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndUnlocking();
        }
    }

    void TrySelectNearestCircle()
    {
        foreach (var pair in circles)
        {
            var circle = pair.Value;
            float dist = Vector3.Distance(player.position, circle.transform.position);

            if (dist <= selectionRadius)
            {
                if (!unLocking)
                {
                    unLocking = true;
                }

                TrySetLineEdit(circle);
                break;
            }
        }
    }

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

        inputPattern.Add(circle.id);
    }

    GameObject CreateLine(Vector3 pos, int id)
    {
        var line = Instantiate(linePrefab, canvas.transform);
        line.transform.localPosition = pos;

        var lineIdf = line.AddComponent<CircleIdentifier>();
        lineIdf.id = id;

        lines.Add(lineIdf);
        return line;
    }

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

    void EnableColorFade(Animator anim, bool isSuccess)
    {
        anim.enabled = true;
        anim.Rebind();

        if (isSuccess)
            anim.SetBool("Success", true);
        else
            anim.SetBool("Fail", true);
    }

    void EndUnlocking()
    {
        if (!unLocking) return;

        bool result = IsCorrectPattern();

        foreach (var line in lines)
        {
            EnableColorFade(circles[line.id].GetComponent<Animator>(), result);
            EnableColorFade(line.GetComponent<Animator>(), result);
        }

        if (lines.Count > 0)
        {
            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);
        }

        StartCoroutine(Release());

        unLocking = false;
    }

    IEnumerator Release()
    {
        enabled = false;

        yield return new WaitForSeconds(3f);

        foreach (var circle in circles)
        {
            circle.Value.GetComponent<Image>().color = Color.white;
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
}
