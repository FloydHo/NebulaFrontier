using Godot;
using System;






[Tool]
public partial class HexagonScript : Polygon2D
{
    BP_Sector _parentNode;

    [Export] public float _hexRadius = 1000f;

    public override void _Ready()
    {
        UpdateHexagon();
    }

    public override void _Draw()
    {
        UpdateHexagon();
    }


    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            UpdateHexagon();
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationProcess)
        {
            UpdateHexagon();
        }
        else if (what == NotificationDraw)
        {
            UpdateHexagon();
        }
    }

    private void UpdateHexagon()
    {
        Vector2[] points = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            float angle = (Mathf.Pi / 3) * i;
            points[i] = new Vector2(
                _hexRadius * Mathf.Cos(angle),
                _hexRadius * Mathf.Sin(angle)
            );
        }
        Polygon = points;
        QueueRedraw();
    }

   public float GetHexRadius() => _hexRadius;
}
