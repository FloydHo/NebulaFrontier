using Godot;
using System;
using System.Diagnostics;

public partial class ClickableColorRect : Sprite2D
{
    private bool hasFocus = false;

    public override void _Ready()
    {
        SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {



        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
        {
            Vector2 localMousePosition = GetLocalMousePosition();

            if (GetRect().HasPoint(localMousePosition))
            {
                Debug.WriteLine("FokusOn");
                hasFocus = true;
            }
            else
            {
                Debug.WriteLine("FokusOff");
                hasFocus = false;
            }
        }
    }

    public override void _Process(double delta)
    {
        if (hasFocus && Input.IsActionJustPressed("ui_cancel"))
        {
            hasFocus = false;
        }
    }
}
