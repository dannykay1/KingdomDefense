using UnityEngine;
using System.Collections;

public enum E_MOVE_DIRECTION
{
    DOWN,
    RIGHT
}

public class Projectile : MonoBehaviour
{
    [Header( "Direction" )]
    public E_MOVE_DIRECTION MoveDirection;

    [Header( "Speed" )]
    [Range( 1f, 20f )]
    public float MoveSpeed = 4f;

    private Vector2 Direction;

    protected bool HitTarget = false;

    public int Damage { get; set; }

    protected virtual void Start( )
    {
        switch ( MoveDirection )
        {
            case E_MOVE_DIRECTION.DOWN:
                Direction = -Vector2.up;
                break;
            case E_MOVE_DIRECTION.RIGHT:
                Direction = Vector2.right;
                break;
        }

        Destroy( gameObject, 5f );
        StartCoroutine( Fire( ) );
    }

    protected virtual void OnTriggerEnter2D( Collider2D other )
    {
        if ( other.tag == "Enemy" && !HitTarget )
        {
            HitTarget = true;

            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            enemy.ApplyDamage( Damage );
            Destroy( gameObject );
        }
    }

    protected virtual IEnumerator Fire( )
    {
        while ( true )
        {
            transform.Translate( Direction * MoveSpeed * Time.deltaTime );
            yield return null;
        }
    }
}