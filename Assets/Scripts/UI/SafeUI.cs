using UnityEngine;
using UnityEngine.UI;

public class SafeUI : ViewerUI
{
	public override void HideUI()
	{
		UIManager.Instance.HideViewer(ViewerUIType.Safe);
	}
}
