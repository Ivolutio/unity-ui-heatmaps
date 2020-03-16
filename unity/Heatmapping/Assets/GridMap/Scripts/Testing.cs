/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour {

    [SerializeField] private HeatMapVisual heatMapVisual;
    private Grid grid;

    private void Start() {
        grid = new Grid(Screen.width, Screen.height, 2f, Vector3.zero);

        heatMapVisual.SetGrid(grid);
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            //Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(Input.mousePosition, 50, 0, 5);
        }
    }
}
