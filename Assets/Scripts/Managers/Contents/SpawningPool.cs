using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    //1. ������ ������Ű�� ������ �����Ǿ�
    int _reserveCount = 0;
    [SerializeField]
    int _keepMonsterCount = 0;
    //���� ��ġ
    [SerializeField]
    Vector3 _spawnPos = new Vector3 (10, 0, 20);
    //���� ����
    [SerializeField]
    float _spawnRadius = 15f;
    //���� Ÿ��
    [SerializeField]
    float _spawnTime = 5f;
    public void AddMonsterCount(int value) { _monsterCount += value; }

    //2. ������ ���͸� ������ �� �ְ�. -> GameScene���� ȣ���� ��
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    private void Start()
    {
        Managers.game.OnSpawnEvent -= AddMonsterCount;
        Managers.game.OnSpawnEvent += AddMonsterCount;
    }
    private void Update()
    {
        //������ ���� �� + ���� ���� �� ���� ������ ������ ������ ���� ���
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    //�����ð��� ���� ��ų �� �ְ� �ڷ�ƾ����
    IEnumerator ReserveSpawn()
    {
        //������ �ϴ� ������
        _reserveCount++;
        //�����ð��� ��������
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        //����
        GameObject obj = Managers.game.Spawn(Define.WorldObject.Monster, "Knight");
        NavMeshAgent navAgent = obj.GetOrAddComponent<NavMeshAgent>();
        Vector3 randPos;
        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
            randDir.y = 0;
            randPos = _spawnPos + randDir;
            //���� �ִ� �������� üũ���ֱ� ���� NavMeshPath�� ���� �����ֱ�
            NavMeshPath path = new NavMeshPath();
            //����Ž�������Ʈ�� ���������� ����Ž� ���� ����
            if (navAgent.CalculatePath(randPos, path))
                break;
            obj.transform.position = randPos;
            //����Ȱ� ���� ������ ������ �ٽ� �ʱ�ȭ �̰����� ������ 1+1 < 5�� �Ǿ� �����ϱ�
            _reserveCount--;
        }
    }
}
