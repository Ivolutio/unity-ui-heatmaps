using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Heatmapping : MonoBehaviour
{
    public float interval;
    private float intervalTimer;
    public int width, height;
    public HeatMap heatMap;
    public Gradient heatmapGradient;
    public int radius;
    public Image exportTarget;
    public float detail;
    public Canvas canvas;

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;
        heatMap = new HeatMap(width, height, detail);
    }

    private void Update()
    {
        intervalTimer += Time.deltaTime;
        if (intervalTimer >= interval)
        {
            intervalTimer = 0f;
            heatMap.AddPosition(Mathf.RoundToInt(Input.mousePosition.x), Mathf.RoundToInt(Input.mousePosition.y));
        }

        if (Input.GetKey(KeyCode.Space))
        {
            var tex = heatMap.Export(heatmapGradient, radius);
            exportTarget.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f));
        }
    }

    public class HeatMap
    {
        public int width, height;
        public float detail;
        public int[,] data;
        public int hightestValue;

        public HeatMap(int width, int height, float detail)
        {
            this.detail = detail;
            this.width = Mathf.RoundToInt(width * detail);
            this.height = Mathf.RoundToInt(height * detail);

            data = new int[this.width, this.height];
        }

        public void AddPosition(int x, int y)
        {
            x = Mathf.RoundToInt(x * detail);
            y = Mathf.RoundToInt(y * detail);
            //Debug.Log(x + ", " + y);
            if (x < width && y < height)
            {
                if (x >= 0 && y >= 0)
                {
                    data[x, y] += 1;
                    if (data[x, y] > hightestValue)
                        hightestValue = data[x, y];
                }
            }
        }

        public Texture2D Export(Gradient gradient, int radius)
        {
            //float[,] map = new float[width, height];

            ////Write texture
            //var tex = new Texture2D(width, height);
            //for (int x = 0; x < width; x++)
            //{
            //    for (int y = 0; y < height; y++)
            //    {
            //        Color c = Color.white;
            //        float d = Mathf.Sqrt(x * x + y * y);
            //        float alpha = Mathf.Min(Mathf.Max(d - radius, 0), 1);
            //        if (alpha != 0)
            //        {
            //            c = gradient.Evaluate(alpha);
            //        }
            //        else
            //        {
            //            c.a = 0f;
            //        }
            //        tex.SetPixel(x, y, c);
            //    }
            //}
            //tex.Apply();
            //return tex;

            float[,] map = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = (float)data[x, y] / hightestValue;
                    map[x, y] += value;
                    for (int i = 0; i < radius; i++)
                    {
                        float nValue = (value / radius) * i;
                        if (x - i >= 0)
                            map[x - i, y] += nValue;
                        if (x + i < width)
                            map[x + i, y] += nValue;
                        if (y - i >= 0)
                            map[x, y - i] += nValue;
                        if (y + i < height)
                            map[x, y + i] += nValue;
                        if (x - i >= 0 && y + i < height)
                            map[x - i, y + i] += nValue;
                        if (x + i < width && y + i < height)
                            map[x + i, y + i] += nValue;
                        if (x - i >= 0 && y - i >= 0)
                            map[x - i, y - i] += nValue;
                        if (x + i < width && y - i >= 0)
                            map[x + i, y - i] += nValue;
                    }
                }
            }

            //Write texture
            var tex = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = Color.white;
                    if (map[x, y] != 0)
                    {
                        //Debug.Log(map[x, y]);
                        c = gradient.Evaluate(map[x, y]);
                        //c.a = .5f;
                    }
                    else
                    {
                        c.a = 0f;
                    }
                    tex.SetPixel(x, y, c);
                }
            }
            tex.Apply();
            return tex;
        }
    }

    public Vector3 ScreenToCanvasPosition(Vector3 screenPosition)
    {
        return ViewportToCanvasPosition(new Vector3(screenPosition.x / Screen.width, screenPosition.y / Screen.height, 0));
    }

    public Vector3 ViewportToCanvasPosition(Vector3 viewportPosition)
    {
        return Vector3.Scale(viewportPosition - new Vector3(0.5f, 0.5f, 0), canvas.GetComponent<RectTransform>().sizeDelta);
    }

    public Vector3 CanvasToScreenPosition(Vector3 canvasPosition)
    {
        return CanvasToViewportPosition(new Vector3(canvasPosition.x * Screen.width, canvasPosition.y * Screen.height, 0)) + new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
    }

    public Vector3 CanvasToViewportPosition(Vector3 canvasPosition)
    {
        return (canvasPosition + new Vector3(0.5f, 0.5f, 0)) / canvas.GetComponent<RectTransform>().sizeDelta;
    }
}
