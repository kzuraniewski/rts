using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] private Sprite normalSprite;
	[SerializeField] private Sprite selectedSprite;
	[SerializeField] private float movementSpeed = 1f;
	[SerializeField] private float rotationSpeed = 1f;

	private GameObject sprite;
	private Vector2 targetPosition;
	private Quaternion targetRotation;

	void Start()
	{
		sprite = transform.Find("Sprite").gameObject;
		targetRotation = sprite.transform.rotation;
		targetPosition = transform.position;
	}

	void Update()
	{
		/// Rotate Sprite towards target
		Vector2 vectorToTarget = targetPosition - (Vector2)transform.position;
		if (vectorToTarget.magnitude > movementSpeed * Time.deltaTime)
		{
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, q, rotationSpeed * Time.deltaTime);
		}
	}

	void FixedUpdate()
	{

	}

	public void SetSelected(bool selected)
	{
		sprite.GetComponent<SpriteRenderer>().sprite = selected ? selectedSprite : normalSprite;
		Debug.Log(selected);
	}

	public void MoveTo(Vector3 position)
	{
		targetPosition = position;
	}
}
