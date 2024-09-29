using Godot;
using System;
using System.Diagnostics;
using System.Drawing;

public partial class Map : Node2D
{
	public float mapSize = 100.0f;
	public int gridSize = 30;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //Vector2 size = GetViewportRect().Size;
        //Position = new Vector2(size.X / 2, size.Y / 2);

		ColorRect colorRect = GetNode<ColorRect>("ColorRect");
		colorRect.Color = new Godot.Color(0, 0, 0);
		colorRect.Size = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_pressed()
	{
		Debug.WriteLine("Pressed!");

	}

    public override void _Draw()
    {
        Vector2 VPsize = GetViewportRect().Size;
		for (int x = 0; x < VPsize.Y+gridSize; x += gridSize)
		{
			DrawLine(new Vector2(x, 0), new Vector2(x, VPsize.Y), new Godot.Color(1, 1, 1, 0.2f));
		}

        for (int y = 0; y < VPsize.Y; y += gridSize)
        {
            DrawLine(new Vector2(0, y), new Vector2(VPsize.Y, y), new Godot.Color(1, 1, 1, 0.2f));
        }

		DrawCircle(new Vector2(VPsize.Y / 2, VPsize.Y / 2),1, new Godot.Color(1, 1, 1, 1));
    }
}
