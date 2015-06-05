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
                    //var distance = (hitInfo.point - this.transform.position).magnitude;
                    //iTween.MoveTo(this.gameObject, hitInfo.point, distance / Speed);
                    if (_destObject)
                    {
                        Destroy(_destObject);
                    }
                    _destObject = Instantiate(PrefabDest, hitInfo.point, Quaternion.identity) as GameObject;
                    //dest.name = "Dest" + Name;
                    iTween.LookTo(gameObject, hitInfo.point, 0.5f);
                    State = State.Move;
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
                    State = State.Attack;
                }
                break;
            case State.Move:
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                break;
            case State.Attack:
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _destObject)
        {
            State = State.Idle;
            Destroy(_destObject);
        }
    }
}

public enum State
{
    Idle,
    Move,
    Attack,
}