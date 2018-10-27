using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;

namespace RPG.CameraUI
{
	public class CameraRaycaster : MonoBehaviour
	{
		[SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

		[SerializeField] int[] layerPriorities = null;

		const int WALKABLE_LAYER = 8;
		float maxRaycastDepth = 100f; // Hard coded value
		int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

		//New Delegates 
		public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverTerrain;
		
		public delegate void OnMouseOverEnemy(Enemy enemy);
		public event OnMouseOverEnemy onMouseOverEnemy;

		// Setup delegates for broadcasting layer changes to other classes
		public delegate void OnCursorLayerChange(int newLayer); // declare new delegate type
		public event OnCursorLayerChange notifyLayerChangeObservers; // instantiate an observer set

		public delegate void OnClickPriorityLayer(RaycastHit raycastHit, int layerHit); // declare new delegate type
		public event OnClickPriorityLayer notifyMouseClickObservers; // instantiate an observer set

		public delegate void OnRightClick(RaycastHit raycastHit, int layerHit); // declare new delegate type
		public event OnRightClick notifyRightClickObservers; // instantiate an observer set


		void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Implement UI interaction
            } 
			else 
			{
				PerformRaycasts();
			}
        }

		void PerformRaycasts()
		{
			// Specify layer priorities here
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (RaycastForEnemy(ray)) { return; }
			if (RaycastForWalkable(ray)) { return; }
		}

		private bool RaycastForEnemy(Ray ray)
        {
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
			var enemyHit = gameObjectHit.GetComponent<Enemy>();
			if (enemyHit)
			{
				Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverEnemy(enemyHit);
				return true;
			}
			return false;
        }
        
		private bool RaycastForWalkable(Ray ray)
        {
			RaycastHit hitInfo;
			LayerMask WalkableLayer = 1 << WALKABLE_LAYER;
			bool walkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, WalkableLayer);
			if(walkableHit)
			{
            	Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverTerrain(hitInfo.point);
				return true;
			}
			return false;
		}
	}
}