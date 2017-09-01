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

    public override void Die( )
    {
        base.Die( );
        SoundManager.Instance.PlayRandomDeath( );
        GameManager.OnFriendlyUnitDeath.Invoke( );
    }
}