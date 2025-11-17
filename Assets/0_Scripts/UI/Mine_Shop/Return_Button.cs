using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Return_Button : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] GameObject Parent_GO;
	public void OnPointerClick(PointerEventData eventData)
	{
		PlayCloseAnimation();
	}

	void PlayCloseAnimation()
	{
		CanvasGroup cg = Parent_GO.GetComponent<CanvasGroup>();
		if (cg == null)
			cg = Parent_GO.AddComponent<CanvasGroup>();

		Sequence seq = DOTween.Sequence();

		// 1) 축소 + 페이드 아웃 동시에
		seq.Join(Parent_GO.transform.DOScale(0.7f, 0.25f).SetEase(Ease.InBack));
		seq.Join(cg.DOFade(0f, 0.25f));

		// 2) 끝나면 삭제
		seq.OnComplete(() =>
		{
			Destroy(Parent_GO);
		});
	}
}
