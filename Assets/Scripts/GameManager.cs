using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] private GameObject marker;

	private Vector2 startSelectPosition;
	private List<Unit> selectedUnits;

	void Awake()
	{
		selectedUnits = new List<Unit>();
	}

	// Start is called before the first frame update
	void Start()
	{
		instance = this;

		marker.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		/// Marking area
		if (Input.GetMouseButtonDown(0))
		{
			marker.SetActive(true);
			startSelectPosition = GetWorldMousePosition();
		}

		if (Input.GetMouseButtonUp(0))
		{
			marker.SetActive(false);
			UpdateSelectedUnits();
		}

		/// Unit control
		if (Input.GetMouseButtonDown(1))
		{
			foreach (Unit unit in selectedUnits)
			{
				unit.MoveTo(GetWorldMousePosition());
			}
		}
	}

	public Vector2 GetWorldMousePosition() => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

	private void UpdateSelectedUnits()
	{
		Collider2D[] markedColliders = Physics2D.OverlapAreaAll(startSelectPosition, GetWorldMousePosition());
		List<Unit> unitsToDeselect = new List<Unit>(selectedUnits);

		selectedUnits.Clear();
		foreach (Collider2D collider in markedColliders)
		{
			Unit unit = collider.GetComponent<Unit>();
			if (unit != null)
			{
				selectedUnits.Add(unit);
				unitsToDeselect.Remove(unit);
			}
		}

		foreach (Unit unit in selectedUnits)
		{
			unit.SetSelected(true);
		}

		foreach (Unit unit in unitsToDeselect)
		{
			unit.SetSelected(false);
		}
	}
}
