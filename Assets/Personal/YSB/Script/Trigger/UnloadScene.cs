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
        Debug.Log("����ũ����");
        CustomSceneManager.Instance.LoadScene("EndingCreditScene");
    }

    protected override void OnPlayerEnter()//�갡 �� �ʿ��޴��� ����̾ȳ���..
    {
        UnloadMemoryScene();
    }
}
