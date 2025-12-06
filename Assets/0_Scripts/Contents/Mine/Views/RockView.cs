using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RockView : MonoBehaviour, IPointerClickHandler
{
    private Image _img;
    private BoxCollider2D _collider;

    [SerializeField] private OreSpriteSet _spriteSet;

    [SerializeField] private int _id = 0;
    public int Id => _id;

    private Tweener _tw = null;

    Action<int> OnClick;

    //TODO: 테스트 모니터링용
    [SerializeField] private int _hp;
    [SerializeField] private int _imageIndex = 5;

    public void Bind(RockState data, Action<int> onClick)
    {
        _img = GetComponent<Image>();
        _img.sprite = _spriteSet.sprites[5];

        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = true;

        _id = data.Id;
        OnClick = onClick;

        //TODO: 테스트 모니터링용
        _hp = data.Hp;

        Refresh(data);
    }

    public void Refresh(RockState data)
    {
        Debug.Log($"Rock {_id} Refreshed: Hp={data.Hp}, IsBroken={data.IsBroken}");

        //if (data.Hp < 6) _img.sprite = _spriteSet.sprites[data.Hp];

        _hp = data.Hp;

        if (data.IsBroken) PlayBreak();
        else {
            _img.sprite = _spriteSet.sprites[Math.Clamp((data.Hp * 6) / data.MaxHp, 0, 5)];

            _imageIndex = Math.Clamp((data.Hp * 6) / data.MaxHp, 0, 5);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"Rock {_id} Clicked");

        OnClick?.Invoke(_id);
    }

    public void PlayBreak()
    {
        //Debug.Log($"Rock {_id} Broken");

        _img.enabled = false;
        _collider.enabled = false;
    }

    public void PlayDoTween()
    {
        // DoTween
        _tw ??= GetComponent<RectTransform>()?.DOShakeAnchorPos(
             duration: 0.1f,
             strength: new Vector2(5, 5),
             vibrato: 100,
             randomness: 180,
             snapping: false,
             fadeOut: false
         ).SetAutoKill(false).SetLink(gameObject).Pause();

        _tw?.Restart();
    }
}
