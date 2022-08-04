using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
	private Vector2 startPosition;

	// Update is called once per frame
	void Update()
	{
		Vector2 offset = GameManager.instance.GetWorldMousePosition() - startPosition;
		transform.localScale = new Vector3(offset.x, offset.y, 1f);
	}

	void OnEnable()
	{
		transform.position = startPosition = GameManager.instance.GetWorldMousePosition();
		transform.localScale = Vector3.zero;
	}
}
