using UnityEngine;

public class PlayerSelectCircleUI : MonoBehaviour
{
    public LockPattern lockPattern;
    private CircleIdentifier currentCircle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCircle != null)
        {
            lockPattern.TrySelectCircle(currentCircle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var idf = other.GetComponent<CircleIdentifier>();
        if (idf != null)
        {
            currentCircle = idf;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var idf = other.GetComponent<CircleIdentifier>();
        if (idf != null && currentCircle == idf)
        {
            currentCircle = null;
        }
    }
}
