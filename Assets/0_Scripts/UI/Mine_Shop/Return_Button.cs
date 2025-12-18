using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Return_Button : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject Parent_GO;

    [SerializeField] private MineEventChannel _OnReturnMine;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayCloseAnimation();

        _OnReturnMine.Raise();
    }

    void PlayCloseAnimation()
    {
        CanvasGroup cg = Parent_GO.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = Parent_GO.AddComponent<CanvasGroup>();

        Vector3 defaultScale = Parent_GO.transform.localScale;
        float defaultAlpha = cg.alpha;

        Sequence seq = DOTween.Sequence();

        // 1) 축소 + 페이드 아웃 동시에
        seq.Join(Parent_GO.transform.DOScale(0.7f, 0.25f).SetEase(Ease.InBack));
        seq.Join(cg.DOFade(0f, 0.25f));

        // 2) 끝나면 삭제
        seq.OnComplete(() => {
            Parent_GO.transform.localScale = defaultScale;
            cg.alpha = defaultAlpha;
            Parent_GO.SetActive(false);
        });
    }
}
