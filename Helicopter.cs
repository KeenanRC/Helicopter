using System;
using SplashKitSDK;

using System.Collections.Generic;

public class Helicopter
{

    DrawingOptions Animator;
    public double X { get; private set; }
    public double Y { get; private set; }

    public Bitmap HelicopterBM;
    public Bitmap HelicopterBM2;
    public bool Quit { get;  set; }
    public bool LevelEnd { get; set; }
    public bool ResetLevel { get; set; }
    public double speed = 0;
    public bool PowerUp { get; set; }
    public int Ammo { get; set; }
    public bool Shoot { get; set; }
    public int width
    {
        get
        {
            return HelicopterBM.Width;
        }
    }
    public int height
    {
        get
        {
            return HelicopterBM.Height;
        }
    }

    public Helicopter(Bitmap bitmap, Bitmap bitmap2, Window GameWindow, DrawingOptions ani)
    {
        HelicopterBM2 = bitmap2;
        HelicopterBM = bitmap;
        X = 135;

        Y = (GameWindow.Height - height) / 2;
        /*

              X = (GameWindow.Width + width);

              Y = (GameWindow.Height - height) / 2;
         */
        Quit = true;
        LevelEnd = false;
        ResetLevel = false;
         Animator = ani;
        Shoot = false;
        Ammo = 0;
        PowerUp = false;

    }
    public void Draw()
    {
        
        if (PowerUp == true)
        {
            HelicopterBM2.Draw(X, Y, Animator);
        }
        else
        {
            HelicopterBM.Draw(X, Y, Animator);

        }
        
         
           
    }
    public void ResetHeight(Window GameWindow)
    {

        Y = (GameWindow.Height - height) / 2;

    }

    public void HandleImput(Window _window)
    {
        SplashKit.ProcessEvents();
        //Speed is used to handle accerlartion rather than velocity
        if (SplashKit.MouseDown(MouseButton.LeftButton))
        {
            speed = speed - 1;
            if (speed < -10)
            {
                speed = -10;
            }
        }
        if (!SplashKit.MouseDown(MouseButton.LeftButton))
        {
            speed = speed + 1;
            if (speed > 10)
            {
                speed = 10;
            }
        }
        if (IsOffScreen(_window))
        {
            if (Y > _window.Height)
            {
                Y = _window.Height;
            }
            if (Y < 0)
            {
                Y = 0;
                
            }
        }
        Y = Y + speed;

        if (SplashKit.KeyDown(KeyCode.EscapeKey))
        {
            Quit = false;
        }
        if (SplashKit.KeyTyped(KeyCode.SpaceKey))
        {
            if(Ammo > 0)
            {
             Shoot = true;
                Ammo = Ammo-1;
            }
          
        }
        if (SplashKit.MouseClicked(MouseButton.LeftButton) & LevelEnd == true)
        {
            ResetLevel = true;

        }

    }
    public bool IsOffScreen(Window screen)
    {

        return X < -width || X > screen.Width || Y < -height || Y > (screen.Height);
    }

    public void Update(Window _window)
    {
        HandleImput(_window);
    

    }

    public bool Collidewith(Buff buff)
    {

        return HelicopterBM.CircleCollision(X, Y, buff.HitBox);
    }
    public bool Collidewith(Block block)
    {
      
        return HelicopterBM.RectangleCollision(X, Y, block.HitBox);
    }

}
