using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    //������ üũ
    public int Exp 
    {
        get { return _exp; 
    } 
        set 
        { 
            _exp = value;

            int level = 1;
            //�ѹ��� �뷮�� ����ġ�� ���� ��� ��� ������ �� �� �ְ�
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                //�� ���ǿ� �ɸ��� �ʾ����� ��������
                level++; ;
            }
            if(level != Level)
            {
                Debug.Log("������");
                Level = level;
                SetStat(Level);
            }
        } 
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;
        _exp = 0;
        _defence = 5;
        _moveSpeed = 5f;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        //���� ���ٸ� �ൿ(GameOver�� ����ǥ�� ���?) ��� �α׸� �����
        Debug.Log("Player Dead");
    }
}
