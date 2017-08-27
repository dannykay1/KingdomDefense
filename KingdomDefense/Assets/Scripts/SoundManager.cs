using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header( "Attack Clips" )]
    public AudioClip[ ] Attacks;
    public AudioClip GunFire;
    public AudioClip ArrowFire;

    [Header( "Impacts" )]
    public AudioClip BulletImpact;
    public AudioClip[ ] ArrowImpacts;

    [Header( "Vicotry" )]
    public AudioClip Victory;

    [Header( "Death Clips" )]
    public AudioClip[ ] HumanDeaths;
    public AudioClip[ ] OrcDeaths;

    private AudioSource Source;

    private void Awake( )
    {
        if ( Instance == null ) Instance = this;
        else Destroy( gameObject );

        Source = GetComponent<AudioSource>( );
    }

    public AudioClip GetRandomAttackClip( )
    {
        int idx = GetRandomIndex( Attacks );
        return Attacks[ idx ];
    }

    public AudioClip GetRandomDeathClip( bool isHuman = true )
    {
        if ( isHuman )
        {
            int idx = GetRandomIndex( HumanDeaths );
            return HumanDeaths[ idx ];
        }
        else
        {
            int idx = GetRandomIndex( OrcDeaths );
            return OrcDeaths[ idx ];
        }
    }

    private int GetRandomIndex( AudioClip[ ] source )
    {
        return Random.Range( 0, source.Length );
    }

    public void PlayRandomAttack( )
    {
        Source.PlayOneShot( GetRandomAttackClip( ) );
    }

    public void PlayRandomDeath( bool isHuman = true )
    {
        Source.PlayOneShot( GetRandomDeathClip( isHuman ) );
    }

    public void PlayFire( bool isArrow = false )
    {
        Source.PlayOneShot( GunFire );
    }

    public void PlayImpact( bool isArrow = false )
    {
        if ( isArrow )
        {
            int idx = GetRandomIndex( ArrowImpacts );
            Source.PlayOneShot( ArrowImpacts[ idx ] );
        }
        else
            Source.PlayOneShot( BulletImpact );
    }
}