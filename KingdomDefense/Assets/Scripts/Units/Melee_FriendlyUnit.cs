using UnityEngine;
using System.Collections;

public class Melee_FriendlyUnit : FriendlyUnit
{
    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( !Enemies.Contains( enemy ) )
                Enemies.Add( enemy );

            StartCoroutine( "AttackWithDelay" );
        }
    }

    private void OnTriggerExit2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( Enemies.Contains( enemy ) )
                Enemies.Remove( enemy );

            if ( Enemies.Count <= 0 )
            {
                StopCoroutine( "AttackWithDelay" );
                StopAttackAnim( );
            }
        }
    }
    public override void Attack( )
    {
        base.Attack( );
        SoundManager.Instance.PlayRandomAttack( );
        ApplyDamageToEnemies( );
    }

    public void ApplyDamageToEnemies( )
    {
        for ( int i = 0; i < Enemies.Count; i++ )
        {
            Enemies[ i ].ApplyDamage( Damage );
        }
    }
}