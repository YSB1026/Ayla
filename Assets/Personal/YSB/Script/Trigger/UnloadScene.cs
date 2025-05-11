using UnityEngine;

public class UnloadScene : BaseTrigger //Ŭ���� �̸� �ٲٱ�
{
    [SerializeField] bool isLobby = false;
    public void UnloadMemoryScene()
    {
        CustomSceneManager.Instance.UnloadAdditiveScene();
    }

    public void LoadLobbyScene()
    {
        CustomSceneManager.Instance.LoadScene("Lobby");
    }

    public void LoadEndingScene()
    {
        CustomSceneManager.Instance.LoadScene("Ending");
    }

    protected override void OnPlayerEnter()//�갡 �� �ʿ��޴��� ����̾ȳ���..
    {
        UnloadMemoryScene();
    }
}
