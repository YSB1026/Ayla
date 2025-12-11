using UnityEngine;
using UnityEngine.UI;

public class SafeUI : MonoBehaviour
{
    [SerializeField] private Button BackBtn;

    void Start()
    {
        BackBtn.onClick.AddListener(HideUI);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HideUI();
		}
    }

    void HideUI()
    {
		UIManager.Instance.HideViewer(ViewerUIType.Safe);
	}
}
