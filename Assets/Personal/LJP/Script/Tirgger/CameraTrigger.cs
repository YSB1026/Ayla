using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cam;
	[SerializeField] private Vector2 cameraScreenPos;
	[SerializeField] private Vector2 damping;//카메라를 더 부드럽고 자연스럽게 이동 값이 높을수록 느려지고 부드러워짐


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
