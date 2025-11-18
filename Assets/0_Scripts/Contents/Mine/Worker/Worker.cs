using UnityEngine;

public class Worker : MonoBehaviour
{
    // 작업자는 'ㄹ'자 모양으로 갱도를 파는 역할
    // 현재 라인의 가장 앞쪽의 바위를 캐는 역할
    // Mine 으로부터 가장 앞쪽 바위의 ID를 전달받아 해당 바위를 캐야 함

    private const int MAX_INDEX = 14; // 한 라인 당 바위 개수 - 1

    private short _direction = 1;   // 1: 오른쪽, -1: 왼쪽

    [SerializeField] private GameObject _mineManager;
    private MineManager _mm;

    struct TargetRock
    {
        public int depth;
        public int index;
    }

    [SerializeField] private TargetRock _targetRock;

    private void Start()
    {
        _mm = _mineManager.GetComponent<MineManager>();

        _targetRock.depth = 0;
        _targetRock.index = 0;
    }

    private void Update()
    {
        var rock = _mm.GetRockAt(_targetRock.depth, _targetRock.index);
        if (rock == null || rock.IsBroken) {
            _targetRock.index += 1 * _direction;

            if (_targetRock.index > MAX_INDEX || _targetRock.index < 0) {
                _targetRock.depth++;
                _direction = (short)(-_direction); // 방향 전환
            }
        }
        else {
            _mm.AttackRockAt(_targetRock.depth, _targetRock.index, 1);
        }
    }
}