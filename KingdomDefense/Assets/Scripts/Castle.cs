using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castle : MonoBehaviour
{
    [Header( "Health" )]
    [Range( 1, 100 )]
    public int StartingHealth = 100;

    [Header( "Trigger" )]
    public BoxCollider2D DefendTrigger;

    [Header("Arrow Properties")]
    public Transform ArrowSpawnTransform;
    public Projectile ArrowPrefab;
    [Range( 1, 20 )]
    public int ArrowDamage = 20;
    [Range( 0.1f, 5f )]
    public float ArrowFireDelay = 0.5f;

    public bool IsDefending { get; set; }

    private List<EnemyUnit> EnemiesAttacking = new List<EnemyUnit>( );
    private int Health;

    private void Awake( )
    {
        Health = StartingHealth;
        GameManager.OnCastleHit.Invoke( Health );
    }

    public void Defend( )
    {
        IsDefending = true;
        StartCoroutine( FireArrow( ) );
    }

    public void StopDefending( )
    {
        IsDefending = false;
    }

    private IEnumerator FireArrow( )
    {
        while ( IsDefending )
        {
            Projectile arrow = Instantiate( ArrowPrefab, GetArrowSpawnPosition( ), Quaternion.identity ) as Projectile;
            arrow.Damage = ArrowDamage;
            yield return new WaitForSeconds( ArrowFireDelay );
        }
    }

    private Vector2 GetArrowSpawnPosition( )
    {
        float ranXPos = DefendTrigger.bounds.max.x + Random.Range( -DefendTrigger.size.x, DefendTrigger.size.x );
        Vector2 destination = new Vector2( ranXPos, ArrowSpawnTransform.position.y );
        return destination;
    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( !EnemiesAttacking.Contains( enemy ) )
                EnemiesAttacking.Add( enemy );

            if ( !IsDefending )
                Defend( );
        }
    }

    private void OnTriggerExit2D( Collider2D other )
    {
        if ( other.tag == "Enemy" )
        {
            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            if ( EnemiesAttacking.Contains( enemy ) )
                EnemiesAttacking.Remove( enemy );
        }

        if ( EnemiesAttacking.Count == 0 )
        {
            StopDefending( );
        }
    }

    public void ApplyDamage( int damage )
    {
        Health -= damage;
        GameManager.OnCastleHit.Invoke( Health );
        if ( Health <= 0 )
        {
            // TODO:
            // Do gameover stuff in gamemanger.
            // GameManager.Instance.GameOver();
        }
    }
}