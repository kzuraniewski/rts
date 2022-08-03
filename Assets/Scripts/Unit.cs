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
		/// Movement
		if (Vector2.Distance(targetPosition, (Vector2)transform.position) > movementSpeed * Time.deltaTime)
		{
			Vector2 offset = new Vector2(
					targetPosition.x - transform.position.x,
					targetPosition.y - transform.position.y
			).normalized * Time.deltaTime * movementSpeed;

			transform.Translate(offset);
		}
		else
		{
			transform.position = targetPosition;
		}

		/// Rotation
		Vector2 vectorToTarget = targetPosition - (Vector2)transform.position;
		if (vectorToTarget.magnitude > movementSpeed * Time.deltaTime)
		{
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, q, rotationSpeed * Time.deltaTime);
		}
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
