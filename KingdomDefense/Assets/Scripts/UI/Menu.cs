using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    RectTransform rectTransform;

    public Animator Anim { get; private set; }

    void Awake( )
    {
        rectTransform=GetComponent<RectTransform>( );
        rectTransform.offsetMax=rectTransform.offsetMin=Vector2.zero;

        Anim=GetComponentInChildren<Animator>( );
    }

    public float GetTransitionOutLength( )
    {
        return Anim.runtimeAnimatorController.animationClips[ 0 ].length;
    }

    public void TransitionIn( )
    {
        Anim.SetBool( "MoveIn", true );
    }

    public void TransitionOut( )
    {
        Anim.SetBool( "MoveIn", false );
    }
}