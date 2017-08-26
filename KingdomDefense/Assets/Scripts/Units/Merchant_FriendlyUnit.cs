using UnityEngine;
using System.Collections;

public class Merchant_FriendlyUnit : FriendlyUnit
{
    [Header( "Coin Spawn" )]
    public Vector2 CoinSpawnDelay = new Vector2( 10.0f, 30.0f );

    public override void Init( )
    {
        base.Init( );
        StartCoroutine( SpawnCoinWithDelay( ) );
    }

    private IEnumerator SpawnCoinWithDelay( )
    {
        while ( Health > 0 )
        {
            float randTime = Random.Range( CoinSpawnDelay.x, CoinSpawnDelay.y );
            yield return new WaitForSeconds( randTime );
            PlayAttackAnim( );
            yield return new WaitForSeconds( AnimationDelay );
            StopAttackAnim( );
            yield return null;
        }
    }

    public void SpawnCoin( )
    {
        if ( GameManager.Instance == null ) return;
        GameManager.Instance.Board.SpawnCoin( );
    }
}