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
    private Target _prevTarget;

    private bool _isMoving = false;

    private int _direction = 1;   // 1: 오른쪽, -1: 왼쪽
    private short _directionVert = 1; // 1: 수평, 0: 아래로 이동 중

    private bool _isMoveWithOutTarget = false;
    private bool _isDigging = false;

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
        _digTimer = _digInterval;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(460.0f, -550.0f);

        _target.position.y = -550.0f;
        _target.position.x = 460.0f;
    }

    private void Update()
    {
        //if (_target.rock == null || _target.rock.IsBroken && !_isMoveWithOutTarget) {
        //    UpdateTarget();
        //    MoveToTarget();
        //    return;
        //}

        //if (_isMoving) {
        //    return;
        //}

        //_digTimer -= Time.deltaTime;
        //if (_digTimer < 0.0f) {
        //    if (TryDigTarget()) {
        //        _digTimer = _digInterval;
        //    }
        //    else {
        //        _target.rock = null;
        //        _digTimer = _digInterval;
        //    }
        //}

        if (_isMoving) {
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

    private void MoveToTarget()
    {
        var rt = (RectTransform)transform;

        _isMoving = true;

        UpdateTargetPosition();

        GetComponent<RectTransform>().DOAnchorPos(_target.position, _moveSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                _isMoving = false;
                _isMoveWithOutTarget = false;
                _directionVert = 1;
            });
    }

    private bool TryDigTarget()
    {
        if (_target.rock == null || _target.rock.IsBroken)
            return false;

        _mm.TryAttackRockByState(_target.rock, Managers.Stat.WorkerDamage());

        //TODO: 애니메이션 및 이펙트 재생
        //GetComponent<RectTransform>().DOShakePosition(0.2f, new Vector2(5.0f, 5.0f), 10, 90.0f);

        return true;
    }

    private void UpdateTarget()
    {
        //NOTE: 이동 및 타겟 갱신 로직
        // 1. 현재 타겟이 없는 경우(파괴되거나 소환 직후) 아래 라인이 클리어인지 확인
        //  a. 클리어된 경우 바로 아래로 이동 -> 1
        //  b. 그러지 않은 경우 현재 방향에 따라 가장자리로 이동
        // 2. 바로 아래 Rock이 존재하는지 확인하고 타겟으로 지정
        // 3. 타겟을 공격하고 아래로 이동/바로 이동
        // 4. 방향을 전환하고 현재라인에서 좌/우로 이동하면서 채굴 수행
        // 5. 가장자리에 도달한 경우 1번부터 다시 수행

        if (_mm.IsLineCleared(_target.depth)) {
            //NOTE: 현재 라인을 다 클리어했고, 바로 아래 Rock이 존재하는 경우
            if (_mm.TryGetRockAt(_target.depth + 1, _target.index, out _target.rock)) {
                ++_target.depth;

            }
            //NOTE: 바로 아래 Rock이 존재하지 않는 경우
            else {
                ++_target.depth;
                _directionVert = 0;
                _isMoveWithOutTarget = true;
                return;
            }
        }

        _prevTarget = _target;
        do {
            _target.index += 1 * _direction;
            if (_target.index > MAX_INDEX || _target.index < 0) {
                _target.depth++;
                _direction = -_direction; // 방향 전환
                continue;
            }
            _mm.TryGetRockAt(_target.depth, _target.index, out _target.rock);
        } while (_target.rock == null || _target.rock.IsBroken);
    }

    private void UpdateTargetPosition()
    {
        //FIXME: 맨 처음에 받는 타겟의 y 좌표가 항상 0으로 설정되는 문제가 있음
        // 예상되는 원인으로는 해당 깊이의 LineView의 위치가 제대로 초기화되지 않아서 생기는 문제로 추측 중
        // 상점에서 Worker 를 구매하는 방식으로 하는 경우에도 문제가 생기는지 확인 필요
        // --문제 해결 하지만 계속 추적 필요

        VeinView.Marker ??= new(GameObject.FindGameObjectsWithTag("VeinPositionMarker"));

        //var rockView = _mm.TryGetRockViewAt(_target.depth, _target.index);
        var lineView = _mm.TryGetLineView(_target.depth);

        if (lineView != null) {
            //_target.position.x = rockView.GetComponent<RectTransform>().anchoredPosition.x + (100 * _direction);
            _target.position.x = VeinView.Marker.GetPosition(_target.index).x - (100 * _direction) * _directionVert;
            _target.position.y = lineView.GetComponent<RectTransform>().anchoredPosition.y;
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
        //_target.index -= 1 * _direction;

        if (_mm.TryGetRockAt(_target.depth + 1, _target.index, out _target.rock)) {
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
            });
    }

    private void DigSide()
    {
        if (_mm.TryAttackRockByState(_target.rock, Managers.Stat.WorkerDamage())) {
            return;
        }
        _target.index -= 1 * _direction;
        _current = State.FindSide;
    }

    private void DigDown()
    {
        if (_mm.TryAttackRockByState(_target.rock, Managers.Stat.WorkerDamage())) {
            return;
        }
        _current = State.MoveDown;
    }

    #endregion State Functions
}