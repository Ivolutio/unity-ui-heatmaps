using System;
using System.Collections.Generic;
using UnityEngine;

public class Heatmap
{
    private int width, height;
    private Dictionary<Vector2Int, float> heatData;
    private float biggestValue;
    private float factor;

    public Heatmap(int width, int height, float factor)
    {
        this.width = (int)(width * factor);
        this.height = (int)(height * factor);
        this.factor = factor;
        this.heatData = new Dictionary<Vector2Int, float>();
    }

    public void AddPosition(Vector2 position)
    {
        position *= factor;
        // Lock position within bounds
        var pos = new Vector2Int(Mathf.RoundToInt(Mathf.Clamp(position.x, 0f, width)), Mathf.RoundToInt(Mathf.Clamp(position.y, 0f, height)));

        // save data
        if (heatData.ContainsKey(pos))
        {
            heatData[pos] += .1f;
        }
        else
        {
            heatData.Add(pos, .1f);
        }

        biggestValue = Math.Max(biggestValue, heatData[pos]);
    }

    public Texture2D Export(Gradient colorGradient)
    {
        var data = new Dictionary<Vector2Int, float>();
        foreach (var key in heatData.Keys)
        {
            //Debug.Log(heatData[key] + " / " + biggestValue + " = " + (heatData[key] / biggestValue));
            data.Add(key, heatData[key] / biggestValue);
        }

        var tex = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                var pos = new Vector2Int(x, y);
                Color c = Color.white;
                if (data.ContainsKey(pos))
                {
                    c = colorGradient.Evaluate(data[pos]);
                }
                else
                {
                    //c.a = .5f;
                }
                tex.SetPixel(x, y, c);

            }
        }
        tex.Apply();
        return tex;
    }
}
