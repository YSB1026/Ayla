using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TimelineTransitionMulti : MonoBehaviour
{
    public Transform player1;
    public Transform ayla;

    public Vector3 startPos1;
    public Vector3 startPos2;

    public PlayableDirector timelineDirector;
    public float transitionDuration = 1.5f;

    public void StartTimelineWithTransition()
    {
        StartCoroutine(MoveBothPlayersAndPlayTimeline());
    }

    private IEnumerator MoveBothPlayersAndPlayTimeline()
    {
        // ���ÿ� �� �÷��̾ �̵���Ű�� �ڷ�ƾ ����
        Coroutine move1 = StartCoroutine(MovePlayer(player1, startPos1));
        Coroutine move2 = StartCoroutine(MovePlayer(ayla, startPos2));

        // �� �ڷ�ƾ�� ��� ���� ������ ���
        yield return move1;
        yield return move2;

        // �̵� �Ϸ� �� Timeline ���
        timelineDirector.Play();
    }

    private IEnumerator MovePlayer(Transform player, Vector3 destination)
    {
        float speed = Vector3.Distance(player.position, destination) / transitionDuration;

        while (Vector3.Distance(player.position, destination) > 0.01f)
        {
            player.position = Vector3.MoveTowards(player.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        player.position = destination;
    }

}
