using System;
using SplashKitSDK;

using System.Collections.Generic;

public class Buff
{
   public  enum BuffType
    {
        Ammo,
        Rainbow     
    }
    public bool SpawnNext;

    public double X { get; private set; }
    public double Y { get; private set; }

    public double radius;

    public BuffType bufftype;
    Color Colour = Color.LightYellow;
    public Vector2D MoveDirection;
    public Circle HitBox;
    public Buff(double posx, double posy)
    {
        X = posx;
        Y = posy;

        radius = 12;


        
        HitBox = new Circle();
        HitBox.Center.X = X;
        HitBox.Center.Y = Y;
        
        HitBox.Radius = radius;
        //Y Not really needed but added anyway
        MoveDirection.Y = 0;
        MoveDirection.X = 5;


       if (SplashKit.Rnd() < 0.5)
        {
            bufftype = BuffType.Ammo;
        }
       else
        {
            bufftype = BuffType.Rainbow;
        }


    }
   
    public void Draw(Window _window )
    {
        switch (bufftype)
        {
            case BuffType.Rainbow:
                {
                    _window.FillCircle(Color.Red, X, Y, radius);
                    _window.FillCircle(Color.Blue, X, Y, radius - 2);
                    _window.FillCircle(Color.Yellow, X, Y, radius - 4);
                    _window.FillCircle(Color.Violet, X, Y, radius - 6);
                    _window.FillCircle(Color.Green, X, Y, radius - 8);
                    break;
                }
            case BuffType.Ammo:
                {
                    _window.FillCircle(Color.AliceBlue, X, Y, radius);
                    _window.FillCircle(Color.Blue, X, Y, radius - 2);
                    _window.FillCircle(Color.DeepSkyBlue, X, Y, radius - 4);
                    _window.FillCircle(Color.SkyBlue, X, Y, radius - 6);
                    _window.FillCircle(Color.MidnightBlue, X, Y, radius - 8);
                    break;

                }
          
        }

    }



    public void Update( )
    {
        //Update Hitbox as it moves


        X = X - MoveDirection.X;


        HitBox.Center.X = X;
        HitBox.Center.Y = Y;
        HitBox.Radius = radius;
  


    }
    public bool IsOffScreen(Window screen)
    {

        return X < -radius || X > screen.Width || Y < -radius || Y > (screen.Height);
    }

}
