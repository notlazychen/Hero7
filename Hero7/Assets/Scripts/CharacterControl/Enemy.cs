using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
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
    private TeamMember _destObject;
    private State _state;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (State != State.Attack && _destObject == null)
        {
            TeamMember member = FindObjectOfType<TeamMember>();
            if (member)
            {
                _destObject = member;
                State = State.Move;
            }
        }

        if (_destObject!= null && State == State.Move)
        {
            var dir = (_destObject.transform.position - transform.position).normalized;
            transform.Translate(dir * Speed * Time.deltaTime, Space.World);
            transform.LookAt(_destObject.transform);
        }
    }

    void Top()
    {
        State = State.Idle;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _destObject)
        {
            Top();
        }
        else if (other.tag == "Member")
        {
            Destroy(_destObject);
            State = State.Attack;
        }
    }
}
