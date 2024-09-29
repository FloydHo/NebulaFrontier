using Godot;
using System;
using static Godot.TextServer;

public partial class Camera2d : Camera2D
{
    private float baseZoomSpeed = 0.1f;
    private float minZoom = 0.1f;
    private float maxZoom = 10.0f;

    private Vector2 _targetZoom;


    private Vector2 dragStart;
    private bool isDragging = false;

    public override void _Ready()
    {
        _targetZoom = Zoom;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.IsPressed())
            {
                if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
                {
                    ZoomIn();
                }
                else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
                {
                    ZoomOut();
                    
                }
            }
        }

        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseEvent.Pressed)
                {
                    dragStart = mouseEvent.Position;
                    isDragging = true;
                }
                else
                {
                    isDragging = false;
                }
            }
        }

        if (isDragging && @event is InputEventMouseMotion motionEvent)
        {
            Vector2 dragDelta = motionEvent.Position - dragStart;
            dragStart = motionEvent.Position;
            Position -= dragDelta / Zoom;
        }
    }

    private void ZoomIn()
    {
        if (Zoom.X < maxZoom)
        {
            var mousePos = GetGlobalMousePosition();
            float zoomSpeed = baseZoomSpeed * Zoom.X;
            Zoom += new Vector2(zoomSpeed, zoomSpeed);

            AdjustPosition(mousePos,1, zoomSpeed);
        }
    }

    private void ZoomOut()
    {
        if (Zoom.X > minZoom)
        {
            var mousePos = GetGlobalMousePosition();
            float zoomSpeed = baseZoomSpeed * Zoom.X;
            Zoom -= new Vector2(zoomSpeed, zoomSpeed);

            AdjustPosition(mousePos,-1, zoomSpeed);
        }
    }

    private void AdjustPosition(Vector2 mousePos, int direction, float zoomSpeed)
    {
        GD.Print($"{direction} | X: {Zoom.X} Y: {Zoom.Y} | speed: {zoomSpeed} ");
        Vector2 delta = mousePos - Position;
        Position += delta * (direction * zoomSpeed / Zoom.X);
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        
    }

}
