using UnityEngine;
using UnityEngine.Playables;

public class ObjectTimelineTrigger : MonoBehaviour
{
    public PlayableDirector timelineDirector; // Ÿ�Ӷ��� �����
    public GameObject playerObject; // �÷��̾� ������Ʈ (�ִϸ����Ͱ� �ִ� ������Ʈ)

    // �浹 �� Ÿ�Ӷ��� ����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹���� ��
        {

            // �浹ü (�ڱ� �ڽ�)�� ��Ȱ��ȭ
            gameObject.SetActive(false);

            // Ÿ�Ӷ��� ����
            timelineDirector.Play();

            // Ÿ�Ӷ��� ���� ��, �÷��̾��� �������� �����ϴ� �̺�Ʈ ����
            timelineDirector.stopped += OnTimelineFinished;
        }
    }

    // Ÿ�Ӷ��� ���� �� �ٽ� �÷��̾� ������ ����
    void OnTimelineFinished(PlayableDirector director)
    {
        // �̺�Ʈ ���� (�ٽ� ȣ����� �ʵ���)
        director.stopped -= OnTimelineFinished;
    }
}
