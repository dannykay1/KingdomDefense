using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof( BoardManager ) )]
public class BoardManagerEditor : Editor
{
    public override void OnInspectorGUI( )
    {
        BoardManager boardManager = ( BoardManager )target;
        if ( DrawDefaultInspector( ) )
        {            
            boardManager.InitBoard( );
        }

        if ( GUILayout.Button( "Spawn Enemies" ) )
        {
            boardManager.SpawnEnemy( );
        }
    }
}