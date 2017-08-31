using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InGameHUD : MonoBehaviour
{
    public static InGameHUD Instance;

    [Header( "Health and Money" )]
    public Image HealthSlider;
    public Text GoldText;

    [Header( "Wave Indicator" )]
    public GameObject WaveIndicator;

    [Header( "Pause" )]
    public GameObject PauseText;

    [Header( "Wave Info" )]
    public Animator WaveInfoMenu;
    public Text WaveInfoText;

    private HorizontalLayoutGroup WaveIndicatorGrid;

    private void Awake( )
    {
        if ( Instance == null ) Instance = this;
        else Destroy( gameObject );

        WaveIndicatorGrid = GameObject.Find( "Wave Indicators" ).GetComponent<HorizontalLayoutGroup>( );
        UpdateGoldText( );
        PauseText.SetActive( false );

        GameManager.OnCastleHit.AddListener( UpdateCastleHealth );
        GameManager.OnWaveStarted.AddListener( SpawnWaveIndicator );
    }

    private void SpawnWaveIndicator( )
    {
        Vector3 tempScale = WaveIndicator.transform.lossyScale;
        GameObject go = Instantiate( WaveIndicator, WaveIndicatorGrid.transform.position, Quaternion.identity ) as GameObject;
        go.transform.SetParent( WaveIndicatorGrid.transform, false );
        go.transform.localScale = tempScale;
    }

    public void UpdateGoldText( )
    {
        GoldText.text = GameManager.Instance.Gold.ToString( );
        GameManager.OnGoldUpdated.Invoke( );
    }

    public void UpdateCastleHealth( int health )
    {
        health = ( health <= 0 ) ? 0 : health;
        float newHealth = health / 100.0f;
        HealthSlider.fillAmount = newHealth;
    }

    public void TogglePause( )
    {
        GameManager.Instance.TogglePause( );
        PauseText.SetActive( GameManager.Instance.IsPaused );
        EventSystem.current.SetSelectedGameObject( null );
    }

    public void ToggleWaveInfoMenu( bool moveIn )
    {
        string waveNum = GameManager.Instance.WaveNumber + "/" + GameManager.Instance.MaxWaveNum;
        string enemiesKilled = GameManager.Instance.Stats.EnemiesKilled.ToString( );
        string goldEarned = GameManager.Instance.Stats.GoldEarned.ToString( );
        string goldSpent = GameManager.Instance.Stats.GoldSpent.ToString( );

        WaveInfoText.text = string.Format( "{0}\n{1}\n${2}\n${3}", waveNum, enemiesKilled, goldEarned, goldSpent );

        WaveInfoMenu.SetBool( "MoveIn", moveIn );
    }

    public void GoToNextWave( )
    {
        ToggleWaveInfoMenu( false );
        GameManager.OnWaveStarted.Invoke( );
    }
}