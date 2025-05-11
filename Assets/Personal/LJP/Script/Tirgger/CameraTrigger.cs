using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cam;

	private CinemachineCamera vcam;
	private CinemachinePositionComposer positionComposer;
	private CinemachineConfiner2D confiner;

	[Header("ScreenPosition")]
	[SerializeField] private Vector2 cameraScreenPos;
	[SerializeField] private Vector2 damping;//ī�޶� �� �ε巴�� �ڿ������� �̵� ���� �������� �������� �ε巯����

	[Header("BoundingShape")]
	[SerializeField] private Collider2D boundingShape;
	//[SerializeField] private bool isTeleport;

    private void Start()
	{
		InitComponent();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
            SetScreenPosition();
			SetBoundingShape(collision);
		}
	}

	private void InitComponent()
	{
        vcam = cam.GetComponent<CinemachineCamera>();
        positionComposer =vcam.TryGetComponent<CinemachinePositionComposer>(out var comp) ? comp : null;
		confiner = vcam.GetComponent<CinemachineConfiner2D>();
    }

	private void SetScreenPosition()
	{
		positionComposer.Composition.ScreenPosition = cameraScreenPos;
		positionComposer.Damping = damping;
	}

	private void SetBoundingShape(Collider2D collision)
	{
        confiner.enabled = false;

        confiner.BoundingShape2D = boundingShape;
        confiner.InvalidateBoundingShapeCache();


        Vector3 clampedPos = boundingShape.ClosestPoint(collision.transform.position);
        vcam.ForceCameraPosition(clampedPos, Quaternion.identity);

        confiner.enabled = true;
  //      if (isTeleport)
		//{
  //          //vcam.ForceCameraPosition(collision.transform.position, Quaternion.identity);

  //          Vector3 clampedPos = boundingShape.ClosestPoint(collision.transform.position);
  //          vcam.ForceCameraPosition(clampedPos, Quaternion.identity);
		//}
		////else
		////{
		////	confiner.BoundingShape2D = boundingShape;
		////}
	}
}
