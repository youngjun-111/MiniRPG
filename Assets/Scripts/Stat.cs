using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    //프로젝트 창에서 보여주기 위해
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defence;
    [SerializeField]
    protected float _moveSpeed;

    //외부에서 접근할 수 있게 프로퍼티로 생성 해줌
    //즉, 플레이어 적 둘다 사용하는 것들
    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defence = 5;
        _moveSpeed = 5f;
    }

    //데미지및 사망처리
    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defence);
        Hp -= damage;
        if(Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    //실제 사망처리 및 경험치 처리
    protected virtual void OnDead(Stat attacker)
    {
        //attacker를 PlayerStat 캐스팅 하기
        PlayerStat playerStat = attacker as PlayerStat;
        //스탯은 몬스터, 플레이어 둘다 있음. 하지만 플레이어스탯은 플레이어만의 Exp가 존재하기 때문에 attacker에 PlayerStat을 캐스팅해줌
        if(playerStat != null)
        {
            playerStat.Exp += 15;
        }
        Managers.game.Despawn(gameObject);
    }
}
