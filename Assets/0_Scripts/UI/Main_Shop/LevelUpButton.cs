using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
using DG.Tweening;
public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Main_Secne_Shop_Panel Panel;
	private bool isPush =false;
	private float timer = 0.0f;
	Coroutine coroutine = null;
	private void Start()
	{
	}

	private void Update()
	{
		if (isPush)
		{
			timer += Time.deltaTime;
			if (timer > 0.03f)
			{
				LevelUp();
				timer = 0.0f;
			}
		}
	}
	public void LevelUp()
	{
		if (Panel.LevelUp() == false) return;
		Debug.Log("Punch");
		transform.DORewind();
		transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f),0.25f);
	}
		
	public void OnPointerDown(PointerEventData eventData)
	{
		LevelUp();
		coroutine = StartCoroutine(Push_coroutine());
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isPush =false;
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		timer = 0.0f;
	}

	private void InitEXP()
	{
	}
	IEnumerator Push_coroutine()
	{
		yield return new WaitForSeconds(1.0f);
		isPush = true;
	}
}
