using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    //1. 생성을 지연시키기 때문에 생성되어
    int _reserveCount = 0;
    [SerializeField]
    int _keepMonsterCount = 0;
    //스폰 위치
    [SerializeField]
    Vector3 _spawnPos = new Vector3 (10, 0, 20);
    //스폰 범위
    [SerializeField]
    float _spawnRadius = 15f;
    //리젠 타임
    [SerializeField]
    float _spawnTime = 5f;
    public void AddMonsterCount(int value) { _monsterCount += value; }

    //2. 유지할 몬스터를 지정할 수 있게. -> GameScene에서 호출할 거
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    private void Start()
    {
        Managers.game.OnSpawnEvent -= AddMonsterCount;
        Managers.game.OnSpawnEvent += AddMonsterCount;
    }
    private void Update()
    {
        //생성될 몬스터 수 + 현재 몬스터 수 보다 생성된 몬스터의 수보다 작을 경우
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    //생성시간을 지연 시킬 수 있게 코루틴으로
    IEnumerator ReserveSpawn()
    {
        //예약을 일단 시켜줌
        _reserveCount++;
        //지연시간을 랜덤으로
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        //생성
        GameObject obj = Managers.game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent navAgent = obj.GetOrAddComponent<NavMeshAgent>();
        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randDir.y = 0;
            randPos = _spawnPos + randDir;
            //갈수 있는 공간인지 체크해주기 위해 NavMeshPath를 생성 시켜주기
            NavMeshPath path = new NavMeshPath();
            //내비매쉬에이전트로 갈수있으면 내비매쉬 위로 생성
            if (navAgent.CalculatePath(randPos, path))
                break;
            obj.transform.position = randPos;
            //예약된게 생성 됐으니 예약은 다시 초기화 이과정이 없으면 1+1 < 5가 되어 버리니까
            _reserveCount--;
        }
    }
}
