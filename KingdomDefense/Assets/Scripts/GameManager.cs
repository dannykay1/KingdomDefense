﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static UnitDropEvent OnUnitDrop = new UnitDropEvent( );
    public static UnitPurchasedEvent OnUnitPurchased = new UnitPurchasedEvent( );
    public static UnityEvent OnGoldUpdated = new UnityEvent( );
    public static CastleHitEvent OnCastleHit = new CastleHitEvent( );
    public static UnityEvent OnWaveStarted = new UnityEvent( );
    public static UnityEvent OnFriendlyUnitDeath = new UnityEvent( );
    public static EnemyUnitDeathEvent OnEnemyUnitDeath = new EnemyUnitDeathEvent( );

    public static GameManager Instance;

    [Header( "Gold" )]
    [Range( 100, 1000 )]
    public int StartingGold = 500;

    [Header( "Wave Info" )]
    [Range( 1, 10 )]
    public int WaveNumber = 1;
    [Range( 10, 20 )]
    public int MaxWaveNum = 10;
    public Vector2 EnemySpawnDelay = new Vector2( 1f, 10f );

    public int Gold { get; set; }
    public bool IsPaused { get; set; }

    public BoardManager Board { get; private set; }

    private void Awake( )
    {
        if ( Instance == null ) Instance = this;
        else Destroy( gameObject );

        Gold = StartingGold;
        Board = GameObject.FindObjectOfType<BoardManager>( );

        GameManager.OnUnitPurchased.AddListener( PurchaseUnit );
        GameManager.OnWaveStarted.AddListener( StartNextWave );
    }

    public void Start( )
    {
        Board.InitBoard( );
        GameManager.OnWaveStarted.Invoke( );
    }

    public bool HasEnoughGoldToPurchase( int amount )
    {
        return Gold - amount >= 0;
    }

    public void AddGold( int goldAmount )
    {
        Gold += goldAmount;
        InGameHUD.Instance.UpdateGoldText( );
    }

    private void PurchaseUnit( FriendlyUnit unit )
    {
        if ( HasEnoughGoldToPurchase( unit.Cost ) )
        {
            Gold -= unit.Cost;
            InGameHUD.Instance.UpdateGoldText( );
        }
    }

    public void StartNextWave( )
    {
        StartCoroutine( SpawnEnemies( ) );
    }

    private IEnumerator SpawnEnemies( )
    {
        for ( int i = 0; i < Board.GetNumEnemiesToSpawn( ); i++ )
        {
            Board.SpawnEnemy( );
            float randSpawnDelay = Random.Range( EnemySpawnDelay.x, EnemySpawnDelay.y );
            yield return new WaitForSeconds( randSpawnDelay );
        }

        yield return null;
    }

    public void PerformWaveCountdown( )
    {
        GameManager.OnWaveStarted.Invoke( );
    }
}

[System.Serializable]
public class UnitDropEvent : UnityEvent<FriendlyUnit>
{

}

[System.Serializable]
public class UnitPurchasedEvent : UnityEvent<FriendlyUnit>
{

}

[System.Serializable]
public class CastleHitEvent : UnityEvent<int>
{

}

[System.Serializable]
public class EnemyUnitDeathEvent : UnityEvent<EnemyUnit>
{

}