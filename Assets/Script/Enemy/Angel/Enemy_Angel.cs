using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy_Angel :Enemy
{
    public AngelIdleState idleState { get;private set; }
    public AngelMoveState moveState { get;private set; }
    public AngelBattleState battleState { get;private set; }
    public AngelAttackState attackState { get;private set; }
    public AngelStunState stunState { get;private set; }
    protected override void Awake()
    {
        base.Awake();
        battleState = new AngelBattleState(this, stateMachine, "Move", this);
        idleState = new AngelIdleState(this, stateMachine, "Idle", this);
        moveState = new AngelMoveState(this, stateMachine, "Move", this);
        attackState = new AngelAttackState(this, stateMachine, "Attack", this);
        stunState = new AngelStunState(this, stateMachine, "Stun", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            stateMachine.ChangeState(stunState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }

        return false;
    }
}
