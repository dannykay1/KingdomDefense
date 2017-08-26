using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
	public Menu startMenu;
	public Menu optionsMenu;
	public Menu howToPlayMenu;
	public Menu creditsMenu;

	[Range(0.1f, 1f)]
	public float transitionDelay = 0.1f;
	public Image[] howToPlayImages;

	private int currentIndex = 0;

	void Start ()
	{
		startMenu.TransitionIn ();

		for (int i = 0; i < howToPlayImages.Length; i++)
		{
			howToPlayImages [i].gameObject.SetActive (false);
		}

		howToPlayImages [0].gameObject.SetActive (true);
	}

	public void OpenScene (string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public void OpenStartMenu ()
	{
		if (creditsMenu.Anim.GetBool("MoveIn"))
		{
			creditsMenu.TransitionOut ();
			StartCoroutine (MenuTransition (startMenu, creditsMenu));
		}

		if (howToPlayMenu.Anim.GetBool("MoveIn"))
		{
			howToPlayMenu.TransitionOut ();
			StartCoroutine (MenuTransition (startMenu, howToPlayMenu));
		}
	}

	public void OpenOptionsMenu ()
	{
		optionsMenu.TransitionIn ();
	}

	public void OpenHowToPlayMenu ()
	{
		startMenu.TransitionOut ();
		optionsMenu.TransitionOut ();

		for (int i = 0; i < howToPlayImages.Length; i++)
		{
			howToPlayImages [i].gameObject.SetActive (false);
		}

		howToPlayImages [0].gameObject.SetActive (true);

		StartCoroutine (MenuTransition (howToPlayMenu, startMenu));
	}

	public void OpenCreditsMenu ()
	{
		startMenu.TransitionOut ();
		optionsMenu.TransitionOut ();

		StartCoroutine (MenuTransition (creditsMenu, startMenu));
	}

	IEnumerator MenuTransition (Menu menuIn, Menu menuOut)
	{
		yield return new WaitForSeconds (transitionDelay);
		menuIn.TransitionIn ();
	}

	public void MoveToNextInstruction(int direction)
	{
		howToPlayImages [currentIndex].gameObject.SetActive (false);
		currentIndex = (direction == 1) ? currentIndex + 1 : currentIndex - 1;

		if (currentIndex == -1)
		{
			currentIndex = howToPlayImages.Length - 1;
		}
		else if (currentIndex == howToPlayImages.Length)
		{
			currentIndex = 0;
		}

		howToPlayImages [currentIndex].gameObject.SetActive (true);
	}
}