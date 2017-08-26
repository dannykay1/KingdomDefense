using UnityEngine;
using System.Collections;

public class Ranged_FriendlyUnit : FriendlyUnit
{
    [Header( "Projectile" )]
    public Transform ProjectileSpawn;
    public Projectile ProjectilePrefab;

    [Header( "Attack Layer" )]
    public LayerMask AttackLayer;

    [Header( "Attack Range" )]
    [Range( 10f, 50f )]
    public float AttackRange = 20.0f;

    [Header( "Attack Delay" )]
    [Range(0.5f, 2.0f)]
    public float Delay = 1f;

    public override void Init( )
    {
        base.Init( );
        StartCoroutine( ShootWithDelay( ) );
    }

    private IEnumerator ShootWithDelay( )
    {
        while ( Health > 0 )
        {
            RaycastHit2D hit = Physics2D.Raycast( ProjectileSpawn.position, Vector2.right, AttackRange, AttackLayer );
            if ( hit && Col2D.enabled )
            {
                Unit unit = hit.collider.GetComponent<Unit>( );
                if ( unit != null && unit.Health > 0 )
                {
                    PlayAttackAnim( );
                    yield return new WaitForSeconds( AnimationDelay );
                    StopAttackAnim( );
                }
            }

            yield return new WaitForSeconds( Delay );
        }
    }

    private void Shoot( )
    {
        Projectile proj = Instantiate( ProjectilePrefab, ProjectileSpawn.position, Quaternion.identity ) as Projectile;
        proj.Damage = Damage;
    }
}