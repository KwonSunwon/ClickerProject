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

    struct TargetIndex
    {
        public int depth;
        public int index;
    }

    private TargetIndex _targetIndex;
    private RockState _targetRock;
    private Vector2 _targetPosition;

    private bool _isMoving = false;
    private short _direction = 1;   // 1: 오른쪽, -1: 왼쪽

    private float _moveDuration = 1.0f;
    private float _digInterval = 0.1f;

    private float _digTimer = 0.0f;

    private void Start()
    {
        _mm = _mineManager.GetComponent<MineManager>();

        _targetIndex.depth = 0;
        _targetIndex.index = 0;

        UpdateTarget();
        MoveToTarget();
    }


    //TODO: 대상이 깨진거 확인해서 다음 대상으로 변경 아니면 공격
    // 다음 대상을 찾은 뒤에는 해당 대신으로 위치 이동(이동 속도 설정 필요)
    // 앞에 도달하면 공격 시작
    // 반복
    private void Update()
    {
        if (_isMoving) {
            return;
        }

        if (_targetRock == null || _targetRock.IsBroken) {
            UpdateTarget();
            MoveToTarget();
            return;
        }

        _digTimer -= Time.deltaTime;
        if (_digTimer < 0.0f) {
            if (TryDigTarget()) {
                _digTimer = _digInterval;
            }
            else {
                _targetRock = null;
            }
        }
    }

    private void MoveToTarget()
    {
        _isMoving = true;

        UpdateTargetPosition();

        GetComponent<RectTransform>().DOAnchorPos(_targetPosition, _moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                _isMoving = false;
            });
    }

    private bool TryDigTarget()
    {
        if (_targetRock == null || _targetRock.IsBroken)
            return false;

        _mm.TryAttackRockByState(_targetRock, 1);
        //TODO: 애니메이션 및 이펙트 재생
        return true;
    }

    private void UpdateTarget()
    {
        do {
            _targetIndex.index += 1 * _direction;
            if (_targetIndex.index > MAX_INDEX || _targetIndex.index < 0) {
                _targetIndex.depth++;
                _direction = (short)(-_direction); // 방향 전환
                continue;
            }
            _targetRock = _mm.TryGetRockAt(_targetIndex.depth, _targetIndex.index);
        } while (_targetRock == null || _targetRock.IsBroken);
    }

    private void UpdateTargetPosition()
    {
        var rockView = _mm.TryGetRockViewAt(_targetIndex.depth, _targetIndex.index);
        var lineView = _mm.TryGetLineView(_targetIndex.depth);

        if (rockView != null && lineView != null) {
            _targetPosition.x = rockView.GetComponent<RectTransform>().anchoredPosition.x;
            _targetPosition.y = lineView.GetComponent<RectTransform>().anchoredPosition.y;
        }
    }
}