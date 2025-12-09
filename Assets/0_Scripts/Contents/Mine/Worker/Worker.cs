using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    // 작업자는 'ㄹ'자 모양으로 갱도를 파는 역할
    // 현재 라인의 가장 앞쪽의 바위를 캐는 역할
    // Mine 으로부터 가장 앞쪽 바위의 ID를 전달받아 해당 바위를 캐야 함

    private const int MAX_INDEX = 14; // 한 라인 당 바위 개수 - 1

    [SerializeField] private GameObject _mineManager;
    private MineManager _mm;

    private struct Target
    {
        public int depth;
        public int index;
        public RockState rock;
        public Vector2 position;
    }

    private Target _target;

    private bool _isMoving = false;

    private int _direction = 1;   // 1: 오른쪽, -1: 왼쪽

    [SerializeField] private float _moveSpeed = 100.0f;
    [SerializeField] private float _digInterval = 10.0f;

    private float _digTimer;

    //TODO: Worker의 수 추가가 아닌 단계 업그레이드 방식으로 변경함으로 레벨 추가
    // 단계 업그레이드 적용 작업 필요
    [SerializeField] private int _level = 1;
    public int Level {
        get => _level;
        set => _level = value;
    }

    private RectTransform _rt;

    //TODO: Save/Load 함수 제작

    public void Init(MineManager mm)
    {
        _mm ??= mm;
        _rt ??= GetComponent<RectTransform>();
    }

    private void Start()
    {
        _mm ??= _mineManager.GetComponent<MineManager>();
        _rt ??= GetComponent<RectTransform>();

        _target.depth = -1;
        _target.index = 4;
        _digTimer = Managers.Stat.WorkerSpeed(_digInterval);

        GetComponent<RectTransform>().anchoredPosition = new Vector2(460.0f, -550.0f);

        _target.position.y = -550.0f;
        _target.position.x = 460.0f;
    }

    private void Update()
    {
        if (_isMoving) {
            return;
        }

        _digTimer -= Time.deltaTime;
        if (_current == State.DigSide || _current == State.DigDown) {
            if (_digTimer > 0.0f)
                return;
        }

        switch (_current) {
            case State.FindSide:
                FindSide();
                break;
            case State.FindDown:
                FindDown();
                break;
            case State.MoveSide:
                MoveSide();
                break;
            case State.MoveDown:
                MoveDown();
                break;
            case State.DigSide:
                DigSide();
                break;
            case State.DigDown:
                DigDown();
                break;
        }
    }

    #region State Functions
    private enum State
    {
        FindSide,
        FindDown,
        MoveSide,
        MoveDown,
        DigSide,
        DigDown
    }

    private State _current = State.FindSide;

    private void FindSide()
    {
        if (_mm.IsLineCleared(_target.depth)) {
            _current = State.FindDown;
            return;
        }

        HashSet<int> visited = new();
        while (visited.Count <= MAX_INDEX) {
            if (_mm.TryGetRockAt(_target.depth, _target.index, out _target.rock)) {
                _current = State.MoveSide;
                return;
            }
            visited.Add(_target.index);
            _target.index += 1 * _direction;
            if (_target.index > MAX_INDEX || _target.index < 0) {
                _direction = -_direction;
                _target.index += 1 * _direction;
            }
        }

        _current = State.FindDown;
    }

    private void FindDown()
    {
        if (_mm.TryGetRockAt(_target.depth + 1, _target.index, out _target.rock)) {
            _digTimer = Managers.Stat.WorkerSpeed(_digInterval);
            _current = State.DigDown;
            return;
        }

        _current = State.MoveDown;
    }

    private void MoveSide()
    {
        VeinView.Marker ??= new(GameObject.FindGameObjectsWithTag("VeinPositionMarker"));

        _target.position.x = VeinView.Marker.GetPosition(_target.index).x - (100 * _direction);

        _isMoving = true;

        _rt.DOAnchorPos(_target.position, _moveSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                _isMoving = false;
                _current = State.DigSide;
                _digTimer = Managers.Stat.WorkerSpeed(_digInterval);
            });
    }

    private void MoveDown()
    {
        _target.depth++;

        _target.position.y -= 100.0f;

        _isMoving = true;

        _rt.DOAnchorPos(_target.position, _moveSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                _isMoving = false;
                _current = State.FindSide;
                _digTimer = Managers.Stat.WorkerSpeed(_digInterval);
            });
    }

    private void DigSide()
    {
        _mm.TryAttackRockByState(_target.rock, Managers.Stat.WorkerDamage());

        if (_target.rock == null || _target.rock.IsBroken) {
            _target.index -= 1 * _direction;
            _current = State.FindSide;
        }
        else
            _digTimer = Managers.Stat.WorkerSpeed(_digInterval);
    }

    private void DigDown()
    {
        _mm.TryAttackRockByState(_target.rock, Managers.Stat.WorkerDamage());

        if (_target.rock == null || _target.rock.IsBroken)
            _current = State.MoveDown;
        else
            _digTimer = Managers.Stat.WorkerSpeed(_digInterval);
    }

    #endregion State Functions
}