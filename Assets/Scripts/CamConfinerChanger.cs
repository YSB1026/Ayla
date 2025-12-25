using Unity.Cinemachine;
using UnityEngine;

public class CamConfinerChanger : MonoBehaviour
{
    [SerializeField] private GameObject Cam;
    [SerializeField] private Collider2D ExitColider;

	[Header("ScreenPosition")]
	[SerializeField] private Vector2 cameraScreenPos;
	[SerializeField] private Vector2 damping;

	private CinemachineCamera vcam;
	private CinemachinePositionComposer positionComposer;
	private CinemachineConfiner2D confiner;

	private void Start()
	{
		InitComponent();
	}

	private void InitComponent()
	{
		vcam = Cam.GetComponent<CinemachineCamera>();
		positionComposer = vcam.TryGetComponent<CinemachinePositionComposer>(out var comp) ? comp : null;
		confiner = vcam.GetComponent<CinemachineConfiner2D>();
	}

	public void ChangeCam(GameObject player)
	{
		SetScreenPosition();
		ChangeConfiner(player);
	}

	private void SetScreenPosition()
	{
		positionComposer.Composition.ScreenPosition = cameraScreenPos;
		positionComposer.Damping = damping;
	}

	private void ChangeConfiner(GameObject player)
	{
		confiner.enabled = false;

		confiner.BoundingShape2D = ExitColider;
		confiner.InvalidateBoundingShapeCache();

		Vector3 clampedPos = ExitColider.ClosestPoint(player.transform.position);
		vcam.ForceCameraPosition(clampedPos, Quaternion.identity);

		confiner.enabled = true;
	}

}
