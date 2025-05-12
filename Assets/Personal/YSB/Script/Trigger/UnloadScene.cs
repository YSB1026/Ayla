using UnityEngine;

public class UnloadScene : BaseTrigger //Ŭ���� �̸� �ٲٱ�
{
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
    
    public void LoadEndingCredit()
    {
        CustomSceneManager.Instance.LoadScene("EndingCredit");
    }

    protected override void OnPlayerEnter()//�갡 �� �ʿ��޴��� ����̾ȳ���..
    {
        UnloadMemoryScene();
    }
}
