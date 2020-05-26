using System;
using SplashKitSDK;

using System.Collections.Generic;

public class Laser
{
    

    public double X { get; private set; }
    public double Y { get; private set; }


    public double width;
    public double height;

    public Vector2D MoveDirection;
    public Rectangle HitBox;
    public Laser(double posx, double posy)
    {
        X = posx;
        Y = posy;
        width = 10;
        height = 4;

        HitBox = new Rectangle();
        HitBox.X = X;
        HitBox.Y = Y;
        HitBox.Width = width;
        HitBox.Height = height;

        //Y Not really needed but added anyway
        MoveDirection.Y = 0;
        MoveDirection.X = 15;
    }
   
    public void Draw(Window _window )
    {
        _window.FillRectangle(Color.Blue, X, Y, width, height);
        _window.DrawRectangle(Color.LightBlue, X, Y, width+1, height+1);
    }



    public void Update( )
    {
        //Update Hitbox as it moves


        X = X + MoveDirection.X;


        HitBox.X = X;
        HitBox.Y = Y;
        HitBox.Width = width;
        HitBox.Height = height;


    }
    public bool IsOffScreen(Window screen)
    {


        return X < -width || X > screen.Width || Y < -height || Y > (screen.Height);
    }
    public bool Collidewith(Block block)
    {

        return SplashKit.RectanglesIntersect(HitBox, block.HitBox);
    }
}
