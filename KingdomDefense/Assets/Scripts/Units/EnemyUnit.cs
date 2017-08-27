using UnityEngine;
using System.Collections;

public enum E_MOVE_STATE
{
    WALK = 0,
    ATTACK = 1,
    VICOTRY = 2,
    DIE = 3
}

public class EnemyUnit : Unit
{
    private E_MOVE_STATE MoveState { get; set; }

    [Header( "Value on Death" )]
    [Range( 1, 50 )]
    public int RewardValue = 25;

    [Header( "Movement Speed" )]
    [Range( 1f, 20f )]
    public float MoveSpeed = 5f;

    private FriendlyUnit UnitToAttack;

    public Castle CastleToAttack { get; set; }

    protected override void Awake( )
    {
        base.Awake( );
    }

    protected override void Start( )
    {
        base.Start( );
        StartCoroutine( MoveForward( ) );
    }

    public override void PlayAttackAnim( )
    {
        base.PlayAttackAnim( );
        MoveState = E_MOVE_STATE.ATTACK;
    }

    private IEnumerator MoveForward( )
    {
        MoveState = E_MOVE_STATE.WALK;
        Anim.SetFloat( "Speed", 1f );
        while ( MoveState == E_MOVE_STATE.WALK && !IsDead )
        {
            transform.Translate( -Vector2.right * Time.deltaTime * MoveSpeed );
            yield return null;
        }
    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Friendly" && Vector2.Distance( transform.position, other.transform.position ) <= 5f )
        {
            if ( other.GetComponent<FriendlyUnit>( ) != null )
            {
                Anim.SetFloat( "Speed", 0f );
                UnitToAttack = other.GetComponent<FriendlyUnit>( );
                StartCoroutine( "AttackWithDelay" );
            }
        }
        else if ( other.tag == "Castle" )
        {

        }
    }

    private void OnTriggerExit2D( Collider2D other )
    {
        if ( other.tag == "Friendly" && !IsDead )
        {
            UnitToAttack = null;
            StopCoroutine( "AttackWithDelay" );
            StopAttackAnim( );
            StartCoroutine( MoveForward( ) );
        }
    }

    public override void Attack( )
    {
        base.Attack( );

        if ( UnitToAttack != null )
            UnitToAttack.ApplyDamage( Damage );

        if ( CastleToAttack != null )
            CastleToAttack.ApplyDamage( Damage );
    }

    public override void Die( )
    {
        base.Die( );

        GameManager.Instance.AddGold( RewardValue );
        GameManager.OnEnemyUnitDeath.Invoke( this );
    }
}