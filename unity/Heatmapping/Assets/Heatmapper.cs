using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heatmapper : MonoBehaviour
{
    public float savePositionDelay;
    public Gradient gradient;
    public Heatmap heatmap;
    public Image output;
    [Range(0f, 1f)] public float detailFactor;

    private float savePositionTimer;

    private void Start()
    {
        Debug.Log("width: " + Screen.width + "; heihgt; " + Screen.height);
        heatmap = new Heatmap(Screen.width, Screen.height, detailFactor);
    }
    private void Update()
    {
        savePositionTimer += Time.deltaTime;
        if(savePositionTimer >= savePositionDelay)
        {
            heatmap.AddPosition(Input.mousePosition);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            RenderHeatmap();
        }
    }

    public void RenderHeatmap()
    {
        var tex = heatmap.Export(gradient);
        var spr = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f));
        output.sprite = spr;
    }
}