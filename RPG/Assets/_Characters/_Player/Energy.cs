using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
	public class Energy : MonoBehaviour {

		[SerializeField] RawImage energyBar;
		[SerializeField] float maxEnergyPoints = 100f;
		[SerializeField] float pointsPerHit = 10f;

		float currentEnergyPoints;
		CameraUI.CameraRaycaster cameraRaycaster;
		
		// Use this for initialization
		void Start () {
			currentEnergyPoints = maxEnergyPoints;
			cameraRaycaster = Camera.main.GetComponent<CameraUI.CameraRaycaster>();
			cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;	
		}

        void OnMouseOverEnemy(Enemy enemy)
		{
			if (Input.GetMouseButtonDown(1))
            {
                UpdatgeEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void UpdatgeEnergyPoints()
 
       {
            float newEnergyPoints = currentEnergyPoints - pointsPerHit;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
        }

        private void UpdateEnergyBar()
        {
            float energyAsPercent = currentEnergyPoints / maxEnergyPoints;
            float xValue = -(energyAsPercent / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }
}