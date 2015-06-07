using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class TeamMember : MonoBehaviour
{
    public GameObject PrefabDest;
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool IsSelected;

    public float Speed;

    public State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator.SetInteger("State", (int)_state);
        }
    }

    public string Name;

    public Animator Animator;
    private GameObject _destObject;
    private State _state;

    void Update()
    {
        if (IsSelected)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 200, 1 << 10))
                {
                    TurnTo(hitInfo.point);
                }
            }
        }

        switch (State)
        {
            case State.Idle:
                //查找周围的敌人
                Enemy enemy = FindObjectOfType<Enemy>();
                if (enemy)
                {
                    TurnTo(enemy.transform.position);
                }
                break;
            case State.Move:
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                break;
            case State.Attack:
                break;
        }
    }

    void TurnTo(Vector3 dest)
    {
        if (_destObject)
        {
            Destroy(_destObject);
        }
        _destObject = Instantiate(PrefabDest, dest, Quaternion.identity) as GameObject;
        iTween.LookTo(gameObject, dest, 0.5f);
        State = State.Move;
    }

    void Top()
    {
        State = State.Idle;
        Destroy(_destObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _destObject)
        {
            Top();
        }
        else if (other.tag == "Enemy")
        {
            Destroy(_destObject);
            State = State.Attack;
        }
    }
}

public enum State
{
    Idle,
    Move,
    Attack,
}