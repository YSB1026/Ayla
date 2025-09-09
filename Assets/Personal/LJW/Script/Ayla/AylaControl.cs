using UnityEngine;

public class AylaControl : MonoBehaviour
{
    [SerializeField] private Ayla ayla;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ayla != null) ayla.ToggleSide();
        }
    }
}
