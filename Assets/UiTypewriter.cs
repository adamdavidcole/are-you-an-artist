using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTypewriter : MonoBehaviour
{
	public static float timePerChar = 0.1f;

	Text txt;
	string story;

	void OnEnable()
	{
		txt = GetComponent<Text>();
		story = txt.text;
		txt.text = "";

		Debug.Log("TEXT AWAKE!");

		// TODO: add optional delay when to start
		StartCoroutine("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in story)
		{
			txt.text += c;
			yield return new WaitForSeconds(timePerChar);
		}
	}
}
