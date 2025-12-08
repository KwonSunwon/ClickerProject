using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
	public Image image;
	public Sprite[] frames;
	public float frameDuration = 0.08f;

	void Start()
	{
		Sequence seq = DOTween.Sequence();

		foreach (var f in frames)
		{
			seq.AppendCallback(() => image.sprite = f);
			seq.AppendInterval(frameDuration);
		}

		seq.SetLoops(-1); // 반복
	}
}
