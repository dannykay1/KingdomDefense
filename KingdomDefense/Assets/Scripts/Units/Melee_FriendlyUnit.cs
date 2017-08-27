using UnityEngine;
using System.Collections;

public class Melee_FriendlyUnit : FriendlyUnit
{
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