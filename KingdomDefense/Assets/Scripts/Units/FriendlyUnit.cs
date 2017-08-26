using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendlyUnit : Unit
{
    [Header( "Cost to Purchase" )]
    [Range( 50, 200 )]
    public int Cost = 50;

    protected List<EnemyUnit> Enemies = new List<EnemyUnit>( );

    protected override void Awake( )
    {
        base.Awake( );
    }

    protected override void Start( )
    {
        base.Start( );
    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( !Enemies.Contains( enemy ) )
            {
                Enemies.Add( enemy );
                PlayAttackAnim( );
            }
        }
    }

    private void OnTriggerExit2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( Enemies.Contains( enemy ) )
            {
                Enemies.Remove( enemy );
            }

            StopAttackAnim( );
        }
    }

    public override void Die( )
    {
        base.Die( );
        GameManager.OnFriendlyUnitDeath.Invoke( );
    }
}