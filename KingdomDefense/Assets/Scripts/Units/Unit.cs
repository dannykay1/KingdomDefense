using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Animator ) )]
[RequireComponent( typeof( BoxCollider2D ) )]
public abstract class Unit : MonoBehaviour
{
    [Header( "Stats" )]
    [Range( 1, 10 )]
    public int StartingHealth = 5;
    [Range( 1, 5 )]
    public int Damage = 1;

    [Header( "Animation Delay" )]
    [Range(0.1f, 1f)]
    public float AnimationDelay = 0.5f;
    [Range( 0.1f, 1f )]
    public float NextAttackDelay = 0.5f;

    public int Health { get; protected set; }
    protected Animator Anim;
    protected BoxCollider2D Col2D;

    public bool IsDead { get; protected set; }

    protected virtual void Awake( )
    {
        Health = StartingHealth;
        Anim = GetComponent<Animator>( );
        Col2D = GetComponent<BoxCollider2D>( );
        IsDead = false;
    }

    protected virtual void Start( )
    {
    }

    public virtual void Init( )
    {

    }

    public virtual void PlayAttackAnim( )
    {
        Anim.SetBool( "HasTarget", true );
    }

    protected virtual IEnumerator AttackWithDelay( )
    {
        while ( Health > 0 )
        {
            PlayAttackAnim( );
            yield return new WaitForSeconds( AnimationDelay );
            StopAttackAnim( );
            yield return new WaitForSeconds( NextAttackDelay );
        }
    }

    public virtual void Attack( )
    {

    }

    public virtual void ApplyDamage( int damage )
    {
        if ( IsDead ) return;

        Health -= damage;
        if ( Health <= 0 )
        {
            Die( );
        }
    }

    public virtual void StopAttackAnim( )
    {
        Anim.SetBool( "HasTarget", false );
    }

    public virtual void Die( )
    {
        Col2D.enabled = false;
        IsDead = true;
        PlayDeathAnim( );
        Destroy( gameObject, 3f );
    }

    public virtual void PlayDeathAnim( )
    {
        Anim.SetBool( "IsDead", true );
    }
}