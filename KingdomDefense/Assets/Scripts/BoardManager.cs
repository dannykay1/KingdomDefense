using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header( "Board Dimensions" )]
    [Range( 5, 25 )]
    public int Width = 10;
    [Range( 3, 10 )]
    public int Height = 5;
    public Vector2 Offset = Vector2.one;
    public Vector2 TileSize = new Vector2( 1.07f, 2.15f );

    [Header( "Tile" )]
    public Tile TilePrefab;

    [Header( "Enemies" )]
    public EnemyUnit[ ] EnemyPrefabs;
    public Vector2 EnemyCount = new Vector2( 2f, 8f );

    [Header( "Coin" )]
    public Coin CoinPrefab;
    public Transform Spawn;
    public Transform Destination;
    public Vector2 SpawnDelayRange = new Vector2( 1.0f, 5.0f );

    public Tile[ , ] Tiles { get; private set; }

    private Transform BoardHolder;
    private List<EnemyUnit> Enemies = new List<EnemyUnit>( );

    private void Awake( )
    {
        GameManager.OnEnemyUnitDeath.AddListener( EnemyDeath );
    }

    private void Start( )
    {
        StartCoroutine( SpawnCoinsOnRepeat( ) );
    }

    private Vector2 GetRandomSpawnPos( )
    {
        int randomCol = Random.Range( 0, Tiles.GetLength( 1 ) );
        Vector2 spawnPos = Tiles[ Width - 1, randomCol ].UnitSpawn.position;
        spawnPos.x += TileSize.x * 5f;
        return spawnPos;
    }

    public Tile GetRandomTile( )
    {

        int randomRow = Random.Range( 0, Tiles.GetLength( 0 ) );
        int randomCol = Random.Range( 0, Tiles.GetLength( 1 ) );
        return Tiles[ randomRow, randomCol ];
    }

    public int GetNumEnemiesToSpawn( )
    {
        int min = ( int )EnemyCount.x * GameManager.Instance.WaveNumber;
        int max = ( int )EnemyCount.y * GameManager.Instance.WaveNumber;

        return Random.Range( min, max + 1 );
    }

    public void InitBoard( )
    {
        if ( TilePrefab == null )
        {
            Debug.LogError( "The TilePrefab is null" );
            return;
        }

        Tiles = new Tile[ Width, Height ];

        BoardHolder = transform.Find( "Tile Holder" );
        if ( BoardHolder != null )
        {
            DestroyImmediate( BoardHolder.gameObject );
        }

        BoardHolder = new GameObject( "Tile Holder" ).transform;
        BoardHolder.SetParent( gameObject.transform );

        for ( int y = 0; y < Height; y++ )
        {
            for ( int x = 0; x < Width; x++ )
            {
                Vector3 spawnPos = transform.position + new Vector3( x * ( TileSize.x * 2f + Offset.x ), y * ( TileSize.y * 2f + Offset.y ), 0f );
                Tile tile = Instantiate( TilePrefab, spawnPos, Quaternion.identity ) as Tile;
                tile.transform.localScale = TileSize;
                tile.transform.SetParent( BoardHolder );
                Tiles[ x, y ] = tile;
            }
        }
    }

    public void SpawnEnemy( )
    {
        int enemyCount = ( int )Random.Range( EnemyCount.x, EnemyCount.y );
        //for ( int i = 0; i < enemyCount; i++ )
        {
            EnemyUnit enemy = EnemyPrefabs[ Random.Range( 0, EnemyPrefabs.Length ) ];

            Vector2 spawnPos = GetRandomSpawnPos( );

            EnemyUnit enemyObject = Instantiate( enemy, spawnPos, Quaternion.identity ) as EnemyUnit;
            enemyObject.transform.SetParent( BoardHolder );

            Enemies.Add( enemyObject );
        }
    }

    private void EnemyDeath( EnemyUnit enemy )
    {
        if ( Enemies.Contains( enemy ) )
        {
            Enemies.Remove( enemy );
            GameManager.Instance.Stats.EnemiesKilled++;
        }

        if ( Enemies.Count <= 0 && !GameManager.Instance.IsSpawningEnemies )
        {
            Enemies.Clear( );
            GameManager.Instance.ProcessEndOfWave( );
        }
    }

    private IEnumerator SpawnCoinsOnRepeat( )
    {
        while ( true )//!GameManager.Instance.IsPaused )
        {
            float delay = Random.Range( SpawnDelayRange.x, SpawnDelayRange.y );
            yield return new WaitForSeconds( delay );

            SpawnCoin( );

            yield return null;
        }
    }

    public void SpawnCoin( )
    {
        if ( !GameManager.Instance.IsSpawningEnemies ) return;

        Vector2 spawnPos = new Vector2( GetRandomTile( ).GetRandomPointInTile( ).x, Spawn.position.y );
        Instantiate( CoinPrefab, spawnPos, Quaternion.identity );
    }
}