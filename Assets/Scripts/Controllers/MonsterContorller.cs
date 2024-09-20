using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContorller : BaseController
{
    Stat _stat;

    //탐지 범위
    [SerializeField]
    float _scanRange = 10;

    //공격 범위
    [SerializeField]
    float _attackRange = 2;
    public override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();

        WorldObjetTpye = Define.WorldObject.Monster;
        //if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
        //{
        //    Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        //}
    }

    protected override void UpdateDie()
    {
        Debug.Log("몬스터 사망");
    }

    protected override void UpdateIdle()
    {
        Debug.Log("몬스터 대기");
        //태그로 찾기보단 GameManager의 프로퍼티를 가져오기
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject player = Managers.game.GetPlayer();
        if (player == null)
        {
            return;
        }

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        Debug.Log("몬스터 이동");
        //플레이어가 널이 아니라면
        if (_lockTarget != null)
        {
            //위치를 플레이어를향해 이동
            _destPos = _lockTarget.transform.position;
            //거리 계산
            float distance = (_destPos - transform.position).magnitude;
            //계산한 거리가 공격 사거리보다 크다면 navAgent를 이용하여 플레이어를 향해 이동
            if (distance <= _attackRange)
            {
                NavMeshAgent navAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
                navAgent.SetDestination(transform.position);
                State = Define.State.Skill;
                return;
            }
        }
        //
        Vector3 dir = _destPos - transform.position;

        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent navAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
            navAgent.SetDestination(_destPos);
            navAgent.speed = _stat.MoveSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        Debug.Log("몬스터 공격");

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();

            //int damage = Mathf.Max(0, _stat.Attack - targetStat.Defence);

            //Debug.Log("Damage");

            //targetStat.Hp -= damage;
            ////플레이어 사망처리
            //if (targetStat.Hp <= 0)
            //{
            //    Managers.game.Despawn(targetStat.gameObject);
            //}
            //주석 처리한 부분이 Stat에 virtual로 만들어 줬으니 새롭게 작성
            targetStat.OnAttacked(_stat);

            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    State = Define.State.Skill;
                }
                else
                {
                    State = Define.State.Moving;
                }
            }
            else
            {
                //_Temp
                Managers.game.Despawn(targetStat.gameObject);
                State = Define.State.Idle;
            }

        }

    }
}
