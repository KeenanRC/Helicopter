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

        AnimationScript FlyScript = SplashKit.LoadAnimationScript("Animation", "Animation.txt");

        Animation Test = FlyScript.CreateAnimation("Fly");

        DrawingOptions opt;
        opt = SplashKit.OptionWithAnimation(Test);

        Helicopter helicopter = new Helicopter(BmPlayer, BmPlayer2,ShapesWindow, opt);
        HelicopterGame HC = new HelicopterGame(ShapesWindow, helicopter);
        Test.Assign("Fly");
        HC.StartingScreen();
        Console.WriteLine("Enter current player name");
       HC.PlayerName = Console.ReadLine();
        while (HC.MenuOn)
        {
            SplashKit.ProcessEvents();
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                HC.MenuOn = false;
               
            }
            HC.StartingScreen();

    
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
