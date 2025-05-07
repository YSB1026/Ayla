using UnityEngine;

public class FirstFloorTrigger : BaseTrigger
{
    [SerializeField] GameObject firstFloorObject;

    private void Start()
    {
        if (firstFloorObject.activeSelf)
        {
            firstFloorObject.SetActive(false);
        }
    }

    protected override void OnPlayerEnter()
    {
        firstFloorObject.SetActive(true);
    }
}
