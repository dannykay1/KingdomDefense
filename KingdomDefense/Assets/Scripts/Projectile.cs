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

    [Header( "Imapct" )]
    public bool SpawnImpact = false;
    public GameObject ImpactEffect;

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
            switch ( MoveDirection )
            {
                case E_MOVE_DIRECTION.DOWN:
                    SoundManager.Instance.PlayImpact( true );
                    break;
                case E_MOVE_DIRECTION.RIGHT:
                    SoundManager.Instance.PlayImpact( );
                    break;
            }

            HitTarget = true;
            if ( SpawnImpact && ImpactEffect != null )
            {
                Quaternion randomRot = UnityEngine.Random.rotation;
                randomRot.x = randomRot.y = 0f;
                GameObject effect = Instantiate( ImpactEffect, transform.position, randomRot ) as GameObject;
                Destroy( effect, 2.0f );
            }

            EnemyUnit enemy = other.GetComponent<EnemyUnit>( );
            if ( enemy == null ) return;

            enemy.ApplyDamage( Damage );
            Destroy( gameObject );
        }
    }

    protected virtual IEnumerator Fire( )
    {
        switch ( MoveDirection )
        {
            case E_MOVE_DIRECTION.DOWN:
                //SoundManager.Instance.PlayFire( true );
                break;
            case E_MOVE_DIRECTION.RIGHT:
                SoundManager.Instance.PlayFire( );
                break;
        }

        while ( true )
        {
            transform.Translate( Direction * MoveSpeed * Time.deltaTime );
            yield return null;
        }
    }
}