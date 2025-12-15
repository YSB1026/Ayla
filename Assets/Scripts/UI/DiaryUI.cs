using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : ViewerUI
{
	[SerializeField] private List<Image> DiaryPieceList = new List<Image>();
	private List<bool> DiaryPieceCollected = new List<bool>();
	private int DiaryPieceIndex = -1;

	protected override void Start()
	{
		base.Start();
		InitDiaryPieceCollected();
	}

	public override  void HideUI()
	{
		HideAllPiece();
		UIManager.Instance.HideViewer(ViewerUIType.Diary);
	}
	
	private void InitDiaryPieceCollected()
	{
		for(int i = 0; i < DiaryPieceList.Count; i++)
		{
			DiaryPieceCollected.Add(false);
		}
	}

	public void UnlockDiaryPiece(int index)
	{
		DiaryPieceCollected[index] = true;
		ShowUnlockDiaryPice(index);
	}

	public void ShowUnlockDiaryPice(int index)
	{
		HideAllPiece();
		DiaryPieceIndex = index;
		DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(true);
	}

	public void HideAllPiece()
	{
		DiaryPieceIndex = -1;

		for(int i = 0; i < DiaryPieceList.Count; i++)
		{
			DiaryPieceList[i].gameObject.SetActive(false);
		}
	}

	public void MoveOnNextPiece()
	{
		if (DiaryPieceIndex >= DiaryPieceList.Count - 1) return;

		 if(DiaryPieceIndex > -1) DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(false);
		DiaryPieceIndex++;

		if (DiaryPieceCollected[DiaryPieceIndex] == true)
			DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(true);
	}

	public void MoveOnPreviousPiece()
	{
		if (DiaryPieceIndex <= -1) return;

		DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(false);
		DiaryPieceIndex--;

		if (DiaryPieceIndex == -1) return;

		if (DiaryPieceCollected[DiaryPieceIndex] == true)
			DiaryPieceList[DiaryPieceIndex].gameObject.SetActive(true);
	}
}
