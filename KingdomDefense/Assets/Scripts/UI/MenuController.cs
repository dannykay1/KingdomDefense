using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Header( "Menus" )]
    public Menu StartMenu;
    public Menu HowToPlayMenu;
    public Menu CreditsMenu;

    [Header( "Menu Config" )]
    [Range( 0.1f, 1f )]
    public float TransitionDelay = 0.1f;
    public Image[ ] HowToPlayImages;

    private int currentIndex = 0;

    void Start( )
    {
        StartMenu.TransitionIn( );

        for ( int i = 0; i < HowToPlayImages.Length; i++ )
            HowToPlayImages[ i ].gameObject.SetActive( false );

        HowToPlayImages[ 0 ].gameObject.SetActive( true );
    }

    public void OpenScene( string sceneName )
    {
        SceneManager.LoadScene( sceneName );
    }

    public void OpenStartMenu( )
    {
        if ( CreditsMenu.Anim.GetBool( "MoveIn" ) )
        {
            CreditsMenu.TransitionOut( );
            StartCoroutine( MenuTransition( StartMenu, CreditsMenu ) );
        }

        if ( HowToPlayMenu.Anim.GetBool( "MoveIn" ) )
        {
            HowToPlayMenu.TransitionOut( );
            StartCoroutine( MenuTransition( StartMenu, HowToPlayMenu ) );
        }
    }

    public void OpenHowToPlayMenu( )
    {
        StartMenu.TransitionOut( );

        for ( int i = 0; i < HowToPlayImages.Length; i++ )
            HowToPlayImages[ i ].gameObject.SetActive( false );

        HowToPlayImages[ 0 ].gameObject.SetActive( true );
        StartCoroutine( MenuTransition( HowToPlayMenu, StartMenu ) );
    }

    public void OpenCreditsMenu( )
    {
        StartMenu.TransitionOut( );
        StartCoroutine( MenuTransition( CreditsMenu, StartMenu ) );
    }

    IEnumerator MenuTransition( Menu menuIn, Menu menuOut )
    {
        yield return new WaitForSeconds( TransitionDelay );
        menuIn.TransitionIn( );
    }

    public void MoveToNextInstruction( int direction )
    {
        HowToPlayImages[ currentIndex ].gameObject.SetActive( false );
        currentIndex = ( direction == 1 ) ? currentIndex + 1 : currentIndex - 1;

        if ( currentIndex == -1 )
            currentIndex = HowToPlayImages.Length - 1;
        else if ( currentIndex == HowToPlayImages.Length )
            currentIndex = 0;

        HowToPlayImages[ currentIndex ].gameObject.SetActive( true );
    }
}