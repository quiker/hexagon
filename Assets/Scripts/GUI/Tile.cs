using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
	public Transform ButtonPrefub = null;
	public GameObject Controller;
	public string FunctionName = ""; 
	private List<Transform> levelList;
	private List<Transform> gridList;
	public UIGrid gridWrapper = null;
	public int countElementsPerGrid = 20;
	public int gridCellWidth = 120;
	public int gridCellHeight = 120;
	public int gridMaxPerLine = 4;
	public Vector3 gridPosition; 
	private int currentGridIndex = 0;
	private int currentDirection = -1; // 1 - right, -1 - left
	public float slideSpeed = 12.56f;
	public Vector3 initWrapperPosition; 
	
	
	public void ActivateGrid(int gridIndex) {
		currentGridIndex = gridIndex;
	}
	
	public int GetCountGrids() {
		return gridList.Count;
	}
	
	public int GetCurrentGridIndex() {
		return currentGridIndex;
	}
	
	public void UpdateLevels() {
		for (int i = 0; i < levelList.Count; i++) {
			Controller.SendMessage(FunctionName, levelList[i], SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void Update() {
		float reqPosition = initWrapperPosition.x - (GetCurrentGridIndex() * gridWrapper.cellWidth);
				
		if (currentDirection < 0 && gridWrapper.transform.position.x <= reqPosition) {
			gridWrapper.transform.localPosition = new Vector3(reqPosition, initWrapperPosition.y, 0);
		}else if (currentDirection > 0 && gridWrapper.transform.position.x >= reqPosition){
			gridWrapper.transform.localPosition = new Vector3(reqPosition, initWrapperPosition.y, 0);
		}
		
		if (gridWrapper.transform.position.x != reqPosition) {
			gridWrapper.transform.Translate(new Vector3(currentDirection * slideSpeed/* * Time.deltaTime*/, 0, 0));
		}
	}
	
	public void NextGrid() {
		currentDirection = -1;
		ActivateGrid(++currentGridIndex);
	}
	
	public void BackGrid() {
		currentDirection = 1;
		ActivateGrid(--currentGridIndex);
	}
	
	public bool isGenerated() {
		return levelList.Count > 0;
	}
	
	public void DestroyLevels() {
		foreach (Transform childTransform in gridWrapper.transform) {
		    Destroy(childTransform.gameObject);
		}
	}
	
	private Transform GetLastGrid() {
		if (gridList.Count > 0) {
			if (gridList[gridList.Count - 1].transform.childCount < countElementsPerGrid) {
				return gridList[gridList.Count - 1];
			}
		}
		
		GameObject gridGO = new GameObject("Grid");
		gridGO.transform.parent = gridWrapper.transform;
		gridGO.transform.localPosition = gridPosition;
		UIGrid tempGrid = gridGO.AddComponent<UIGrid>();
		tempGrid.cellHeight = gridCellHeight;
		tempGrid.cellWidth = gridCellWidth;
		tempGrid.maxPerLine = gridMaxPerLine;
		tempGrid.arrangement = UIGrid.Arrangement.Horizontal;
		
		gridList.Add(gridGO.transform);
		
		return gridGO.transform;
	}
	
	public void Generate(int count) {
		DestroyLevels();
		
		for (int i = 0; i < count; i++) {
			Transform button = Instantiate(ButtonPrefub, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			button.transform.parent = GetLastGrid();			
			button.name = (i + 1).ToString();
			
			levelList.Add(button);
			
			if (Controller != null) {
				Controller.SendMessage(FunctionName, button, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		gridWrapper.repositionNow = true;
	}
	
	void OnEnable() {
		levelList = new List<Transform>();
		gridList  = new List<Transform>();
		gridWrapper.transform.localPosition = initWrapperPosition;
	}
}