using UnityEditor;
using System.Collections;

[CustomEditor( typeof( SpriteSetup ) )]
public class SpriteSetupEditor : Editor
{
    public override void OnInspectorGUI( )
    {
        DrawDefaultInspector( );

        SpriteSetup unit = ( SpriteSetup )target;
        unit.SetupSprite( );
    }
}