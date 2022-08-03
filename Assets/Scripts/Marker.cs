using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
	[SerializeField] private GameManager gm;

	private Vector2 startPosition;

	// Update is called once per frame
	void Update()
	{
		Vector2 offset = gm.GetWorldMousePosition() - startPosition;
		transform.localScale = new Vector3(offset.x, offset.y, 1f);
	}

	void OnEnable()
	{
		transform.position = startPosition = gm.GetWorldMousePosition();
		transform.localScale = Vector3.zero;
	}
}
