using System;
using SplashKitSDK;

using System.Collections.Generic;

public class Block
{
    public bool SpawnNext = true;

    public double X { get; private set; }
    public double Y { get; private set; }

    public double width;
    public double height;
    Color Colour = Color.LightGreen;
    public Vector2D MoveDirection;
    public Rectangle HitBox;
    public Block(double posx, double posy, double Width ,double Height)
    {
        X = posx;
        Y = posy;

        width = Width;
        height = Height;
        
        HitBox = new Rectangle();
        HitBox.X = X;
        HitBox.Y = Y;
        HitBox.Width = width;
        HitBox.Height = height;
        //Y Not really needed but added anyway
        MoveDirection.Y = 0;
        MoveDirection.X = 5;
    }
    public Block(double posx, double posy, double Width, double Height, Color colour)
    {
        X = posx;
        Y = posy;

        width = Width;
        height = Height;

        HitBox = new Rectangle();
        HitBox.X = X;
        HitBox.Y = Y;
        HitBox.Width = width;
        HitBox.Height = height;
        //Y Not really needed but added anyway
        MoveDirection.Y = 0;
        MoveDirection.X = 5;
        Colour = colour;
    }

    public void Draw(Window _window )
    {
        _window.FillRectangle(Colour, X, Y, width, height);

    }



    public void Update( )
    {
        //Update Hitbox as it moves


        X = X - MoveDirection.X;
       

        HitBox.X = X;
        HitBox.Y = Y;
        HitBox.Width = width;
        HitBox.Height = height;


    }
    public bool IsOffScreen(Window screen)
    {

        return X < -width  || X > screen.Width || Y < -height || Y > (screen.Height);
    }

}
