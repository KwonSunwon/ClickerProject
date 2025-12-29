using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetOre : MonoBehaviour
{
	[SerializeField] Image image;
	[SerializeField] TextMeshProUGUI textMeshPro;
	[SerializeField] private RectTransform rect;
	public void Init(MineralType type, BigNumber amount)
	{
		image.sprite = Managers.Mineral.GetSlot(type).Sprite;
		textMeshPro.text = amount.ToString();

		PlayAnimation();
	}

	private Tween animTween;
	private Vector2 originalPos;
	private void PlayAnimation()
	{
		animTween?.Kill();

		gameObject.SetActive(true);

		// 시작 위치 저장
		originalPos = rect.anchoredPosition;

		// 목표 위치(위로 이동)
		Vector2 endPos = originalPos + new Vector2(0f, 30f);

		Sequence seq = DOTween.Sequence();

		seq.Append(rect.DOAnchorPos(endPos, 0.8f).SetEase(Ease.OutQuad))
		   .OnComplete(() =>
		   {
			   // 원래 위치로 복귀
			   rect.anchoredPosition = originalPos;

			   // 비활성화
			   gameObject.SetActive(false);
		   });

		animTween = seq;
	}
}
