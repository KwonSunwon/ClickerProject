using DG.Tweening;
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
    private short _direction = 1;   // 1: 오른쪽, -1: 왼쪽

    [SerializeField] private float _moveSpeed = 100.0f;
    [SerializeField] private float _digInterval = 0.1f;

    private float _digTimer = 0.0f;

    //TODO: Save/Load 함수 제작

    public void Init(MineManager mm)
    {
        _mm ??= mm;
    }

    private void Start()
    {
        _mm ??= _mineManager.GetComponent<MineManager>();

        _target.depth = 0;
        _target.index = 0;

        GetComponent<RectTransform>().anchoredPosition = new Vector2(460.0f, -545.0f);

        //TODO: 임시 초기 타겟 설정, 추후 이 절차 없이 제대로된 타켓을 찾도록 수정 필요
        //_target = new Target() {
        //    depth = 0,
        //    index = 0,
        //    rock = _mm.TryGetRockAt(1, 14),
        //    position = new Vector2(1484.8f, -750.0f)
        //};
    }

    private void Update()
    {
        if (_target.rock == null || _target.rock.IsBroken) {
            UpdateTarget();
            MoveToTarget();
            return;
        }

        if (_isMoving) {
            return;
        }

        _digTimer -= Time.deltaTime;
        if (_digTimer < 0.0f) {
            if (TryDigTarget()) {
                _digTimer = _digInterval;
            }
            else {
                _target.rock = null;
            }
        }
    }

    private void MoveToTarget()
    {
        _isMoving = true;

        UpdateTargetPosition();

        GetComponent<RectTransform>().DOAnchorPos(_target.position, _moveSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                _isMoving = false;
            });
    }

    private bool TryDigTarget()
    {
        if (_target.rock == null || _target.rock.IsBroken)
            return false;

        _mm.TryAttackRockByState(_target.rock, Managers.Stat.workerDamage());

        //TODO: 애니메이션 및 이펙트 재생
        //GetComponent<RectTransform>().DOShakePosition(0.2f, new Vector2(5.0f, 5.0f), 10, 90.0f);

        return true;
    }

    private void UpdateTarget()
    {
        _prevTarget = _target;
        do {
            _target.index += 1 * _direction;
            if (_target.index > MAX_INDEX || _target.index < 0) {
                _target.depth++;
                _direction = (short)(-_direction); // 방향 전환
                continue;
            }
            _target.rock = _mm.TryGetRockAt(_target.depth, _target.index);
        } while (_target.rock == null || _target.rock.IsBroken);
    }

    private void UpdateTargetPosition()
    {
        //FIXME: 맨 처음에 받는 타겟의 y 좌표가 항상 0으로 설정되는 문제가 있음
        // 예상되는 원인으로는 해당 깊이의 LineView의 위치가 제대로 초기화되지 않아서 생기는 문제로 추측 중
        // 상점에서 Worker 를 구매하는 방식으로 하는 경우에도 문제가 생기는지 확인 필요

        var rockView = _mm.TryGetRockViewAt(_target.depth, _target.index);
        var lineView = _mm.TryGetLineView(_target.depth);

        if (rockView != null && lineView != null) {
            _target.position.x = rockView.GetComponent<RectTransform>().anchoredPosition.x;
            _target.position.y = lineView.GetComponent<RectTransform>().anchoredPosition.y;
        }
    }
}