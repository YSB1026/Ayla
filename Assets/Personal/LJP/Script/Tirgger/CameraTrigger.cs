using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cam;
	[SerializeField] private Vector2 cameraScreenPos;
	[SerializeField] private Vector2 damping;//ī�޶� �� �ε巴�� �ڿ������� �̵� ���� �������� �������� �ε巯����


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			var composer = cam.gameObject.TryGetComponent<CinemachinePositionComposer>(out var comp) ? comp : null;

			composer.Composition.ScreenPosition = cameraScreenPos;
			composer.Damping = damping;
		}
	}
}
