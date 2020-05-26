using System;
using SplashKitSDK;

public class Program
{
    public static void Main()
    {
        Window ShapesWindow = new Window("window", 800, 600);
        Bitmap BmPlayer = new Bitmap("Helicopter", "HCS1.png");
        Bitmap BmPlayer2 = new Bitmap("Helicopter2", "HCS2.png");
        BmPlayer.SetCellDetails(132, 36, 5, 1, 5);
        BmPlayer2.SetCellDetails(132, 36, 5, 1, 5);
        // BmPlayer.SetCellDetails(73,105, 3, 3, 16);
        AnimationScript FlyScript = SplashKit.LoadAnimationScript("Animation", "Animation.txt");

        Animation Test = FlyScript.CreateAnimation("Fly");

        DrawingOptions opt;
        opt = SplashKit.OptionWithAnimation(Test);

        Helicopter helicopter = new Helicopter(BmPlayer, BmPlayer2,ShapesWindow, opt);
        HelicopterGame HC = new HelicopterGame(ShapesWindow, helicopter);
        Test.Assign("Fly");
        
        bool start = false;
        while (!start)
        {
            SplashKit.ProcessEvents();
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                start = true;
               
            }
            ShapesWindow.Clear(Color.Black);
            ShapesWindow.DrawText("Use Mouse to Fly", Color.Red, 100, 10  + ShapesWindow.Height / 2);
            ShapesWindow.DrawText("Press Space to Shoot", Color.Red, 100,ShapesWindow.Height / 2);
            ShapesWindow.DrawText("Press Mouse to Start", Color.Red, 350, 500);

            ShapesWindow.DrawText("Collect these Items", Color.Yellow, 600, 200);
            ShapesWindow.DrawText("Blue Blocks can be destoryed when Invincible/Shot", Color.Yellow, 580, 330);

            ShapesWindow.DrawText("Invincibility", Color.Yellow, 640, 230);
            ShapesWindow.FillCircle(Color.AliceBlue, 600, 300, 24);
            ShapesWindow.FillCircle(Color.Blue, 600, 300, 24 - 4);
            ShapesWindow.FillCircle(Color.DeepSkyBlue, 600, 300, 24 - 8);
            ShapesWindow.FillCircle(Color.SkyBlue, 600, 300, 24 - 12);
            ShapesWindow.FillCircle(Color.MidnightBlue, 600, 300, 24 - 16);

            ShapesWindow.DrawText("Gives Ammo", Color.Yellow, 640, 300);
            ShapesWindow.FillCircle(Color.Red, 600, 240, 24);
            ShapesWindow.FillCircle(Color.Blue, 600, 240, 24 - 4);
            ShapesWindow.FillCircle(Color.Yellow, 600, 240, 24 - 8);
            ShapesWindow.FillCircle(Color.Violet, 600, 240, 24 - 12);
            ShapesWindow.FillCircle(Color.Green, 600, 240, 24 - 16);

    
            ShapesWindow.Refresh(60);
           
        }

        while (HC.Quit)
        {
            SplashKit.UpdateAnimation(Test);
            HC.Update();
           
        }

        ShapesWindow.Close();

        
        helicopter.Draw();

        ShapesWindow.Refresh(60);
        SplashKit.Delay(4000);


    }
}
