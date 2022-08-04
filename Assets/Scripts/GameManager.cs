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
			List<Vector2> offsets = UnitPlacement.GetGridOffsets(selectedUnits.Count, 0.2f);
			Vector2 mousePos = GetWorldMousePosition();

			for (int i = 0; i < selectedUnits.Count; i++)
			{
				selectedUnits[i].MoveTo(mousePos + offsets[i]);
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
