using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public Vector3? targetPosition = null;

	[SerializeField] private Sprite normalSprite;
	[SerializeField] private Sprite selectedSprite;
	[SerializeField] private float movementSpeed = 1f;
	[SerializeField] private float rotationSpeed = 1f;

	private GameObject sprite;
	private Quaternion targetRotation;
	private new Rigidbody2D rigidbody;
	private new CircleCollider2D collider;
	private Vector2 repelOffset;
	private float selfRadius;

	void Start()
	{
		sprite = transform.Find("Sprite").gameObject;

		rigidbody = GetComponent<Rigidbody2D>();
		collider = GetComponent<CircleCollider2D>();

		targetRotation = sprite.transform.rotation;
		selfRadius = collider.radius;
	}

	void Update()
	{
		HandleRotation();
	}

	void FixedUpdate()
	{
		HandleMovement();
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		// calculate the additional repel offset vector to move away from other units
		CircleCollider2D otherCollider = collider.gameObject.GetComponent<CircleCollider2D>();
		Vector3 distanceVector = otherCollider.bounds.center - this.collider.bounds.center;
		float overlapDepth = selfRadius + otherCollider.radius - distanceVector.magnitude;
		repelOffset = -distanceVector.normalized * overlapDepth;

		// move targetPosition if occupied
		if ((otherCollider.bounds.center - targetPosition ?? transform.position).magnitude < distanceVector.magnitude)
		{
			targetPosition ??= transform.position;
			targetPosition -= distanceVector.normalized * movementSpeed * Time.deltaTime * 0.5f;
		}
	}

	void OnTriggerExit2D()
	{
		repelOffset = new Vector2();
	}

	public void SetSelected(bool selected)
	{
		sprite.GetComponent<SpriteRenderer>().sprite = selected ? selectedSprite : normalSprite;
	}

	public void MoveTo(Vector3 position)
	{
		targetPosition = position;
	}

	private void HandleMovement()
	{
		Vector3 newPos;

		if (targetPosition != null)
		{
			Vector3 offsetVector = (Vector3)targetPosition - transform.position;
			Vector3 moveOffset = (offsetVector + (Vector3)repelOffset).normalized * movementSpeed;
			newPos = transform.position + moveOffset * Time.deltaTime;

			if (((Vector3)targetPosition - newPos).magnitude < movementSpeed * Time.deltaTime)
			{
				targetPosition = null;
			}
		}
		else
		{
			newPos = transform.position + (Vector3)repelOffset * Time.deltaTime;
		}

		rigidbody.MovePosition(newPos);
	}

	private void HandleRotation()
	{
		if (targetPosition == null) return;

		/// Rotate Sprite towards target
		Vector2 vectorToTarget = (Vector2)targetPosition - (Vector2)transform.position;
		if (vectorToTarget.magnitude > movementSpeed * Time.deltaTime)
		{
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			sprite.transform.rotation = Quaternion.Slerp(sprite.transform.rotation, q, rotationSpeed * Time.deltaTime);
		}
	}
}
