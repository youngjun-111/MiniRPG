using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    //레벨업 체크
    public int Exp 
    {
        get { return _exp; 
    } 
        set 
        { 
            _exp = value;

            int level = 1;
            //한번에 대량의 경험치가 들어온 경우 모두 레벨업 될 수 있게
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                //위 조건에 걸리지 않았으면 레벨업임
                level++;
            }
            //만약 올라간 레벨이 현재 레벨이 아닐경우 레벨업을 해줘서 갱신시켜줌
            if(level != Level)
            {
                Debug.Log("레벨업");
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
        //아직 별다른 행동(GameOver나 점수표시 등등?) 없어서 로그만 찍어줌
        Debug.Log("Player Dead");
    }
}
