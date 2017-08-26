using UnityEngine;
using System.Linq;
using System.Collections;

public class SpriteSetup : MonoBehaviour
{
    public Sprite BodySprite;
    public Sprite HeadSprite;
    public Sprite ShoulderSprite;
    public Sprite Hand1;
    public Sprite RightHandWeaponSprite;
    public Sprite Hand2;
    public Sprite LeftHandWeaponSprite;
    public Sprite FootSprite;

    public void SetupSprite( )
    {
        ChangeSpriteTo( "Body", BodySprite );
        ChangeSpriteTo( "Head", HeadSprite );
        ChangeSpriteTo( "Shoulder", ShoulderSprite );
        ChangeSpriteTo( "Hand1", Hand1 );
        ChangeSpriteTo( "Shield", RightHandWeaponSprite );
        ChangeSpriteTo( "Hand2", Hand2 );
        ChangeSpriteTo( "Weapon", LeftHandWeaponSprite );
        ChangeSpriteTo( "Foot1", FootSprite );
        ChangeSpriteTo( "Foot2", FootSprite );
    }

    private void ChangeSpriteTo( string name, Sprite sprite )
    {
        SpriteRenderer[ ] children = GetComponentsInChildren<SpriteRenderer>( );

        SpriteRenderer spr = children.ToList<SpriteRenderer>( ).SingleOrDefault<SpriteRenderer>( x => x.name.Equals( name ) );

        if ( spr != null )
        {
            spr.sortingLayerName = "Unit";
            spr.sprite = sprite;
        }
    }
}