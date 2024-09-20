using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContorller : BaseController
{
    Stat _stat;

    //Ž�� ����
    [SerializeField]
    float _scanRange = 10;

    //���� ����
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
        Debug.Log("���� ���");
    }

    protected override void UpdateIdle()
    {
        Debug.Log("���� ���");
        //�±׷� ã�⺸�� GameManager�� ������Ƽ�� ��������
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
        Debug.Log("���� �̵�");
        //�÷��̾ ���� �ƴ϶��
        if (_lockTarget != null)
        {
            //��ġ�� �÷��̾���� �̵�
            _destPos = _lockTarget.transform.position;
            //�Ÿ� ���
            float distance = (_destPos - transform.position).magnitude;
            //����� �Ÿ��� ���� ��Ÿ����� ũ�ٸ� navAgent�� �̿��Ͽ� �÷��̾ ���� �̵�
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
        Debug.Log("���� ����");

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
            ////�÷��̾� ���ó��
            //if (targetStat.Hp <= 0)
            //{
            //    Managers.game.Despawn(targetStat.gameObject);
            //}
            //�ּ� ó���� �κ��� Stat�� virtual�� ����� ������ ���Ӱ� �ۼ�
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
