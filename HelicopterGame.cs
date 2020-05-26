using System;
using SplashKitSDK;

using System.Collections.Generic;

public class HelicopterGame
{
    private Window _Window;
    private Helicopter _Player;
    private List<Block> _blocks = new List<Block>();
    private List<Buff> _buff = new List<Buff>();
    private List<Laser> _laser = new List<Laser>();


    private  Timer GameTimer = new Timer("GameTimer");
    private Timer Cooldown = new Timer("CoolDown");
    private Timer BuffTimer = new Timer("BuffTimer");
    private Timer BuffSpawnTimer = new Timer("BuffSpawnTimer");
    private long Highscore;
    public bool Quit
    {
        get
        {
            return _Player.Quit;
        }

    }
    public void ClearBlocks()
    {
        List<Block> RemoveBlocks = new List<Block>();
        foreach (Block BLOCK in _blocks)
        {
           
                RemoveBlocks.Add(BLOCK);
           
        }
        foreach (Block BLOCK in RemoveBlocks)
        {
            _blocks.Remove(BLOCK);
        }
     }
    public void ClearBuff()
    {
        List<Buff> RemoveBuff = new List<Buff>();
        foreach (Buff BUFF in _buff)
        {
            RemoveBuff.Add(BUFF);
        }
        foreach (Buff BUFF in RemoveBuff)
        {
            _buff.Remove(BUFF);
        }
    }
    public void Restart()
    {
        GameTimer.Start();
        _Player.ResetHeight(_Window);
        _Player.PowerUp = false;
        _Player.Ammo = 0;
        Cooldown.Start();
        ClearBlocks();
        ClearBuff();
        SpawnStart();
        _Player.ResetLevel = false;
        _Player.LevelEnd = false;
    }
    private void CheckCollions()
    {
        List<Block> RemoveBlocks = new List<Block>();
        List<Laser> RemoveLaser = new List<Laser>();
        List<Buff> RemoveBuff = new List<Buff>();
        foreach (Block BLOCK in _blocks)
        {
            if(_Player.Collidewith(BLOCK))
            {   if (_Player.PowerUp == true)
                {
                    if (!BLOCK.SpawnNext)
                    {
                        RemoveBlocks.Add(BLOCK);
                    }
                }
                else
                {
                    _Player.LevelEnd = true;
                    GameTimer.Pause();
                }
                
                
            }
        
            foreach (Laser LASER in _laser)
            {
                if(LASER.Collidewith(BLOCK))
                {
                    RemoveBlocks.Add(BLOCK);
                    RemoveLaser.Add(LASER);
                }
            }

        }
        foreach (Block BLOCK in RemoveBlocks)
        {
            _blocks.Remove(BLOCK);
        }
        foreach (Laser LASER in RemoveLaser)
        {
            _laser.Remove(LASER);
        }
       
        foreach (Buff BUFF in _buff)
        {
            if (_Player.Collidewith(BUFF))
            {
                if(BUFF.bufftype == Buff.BuffType.Rainbow)
                {
                _Player.PowerUp = true;
                BuffTimer.Start();

                }
                if (BUFF.bufftype == Buff.BuffType.Ammo)
                {
                    _Player.Ammo = _Player.Ammo + 1;
                }

                RemoveBuff.Add(BUFF);
            }
            if (BUFF.IsOffScreen(_Window))
            {
                RemoveBuff.Add(BUFF);
            }
            BUFF.Update();
        }
      
      
        foreach (Buff BUFF in RemoveBuff)
        {
            _buff.Remove(BUFF);
        }
    }


        public void Draw()
    {
        if (_Player.LevelEnd == false)
        {
            _Window.Clear(Color.Black);
            _Window.DrawText("The Score is " + GameTimer.Ticks / 10, Color.White, 0, (_Window.Height / 2) - 100);
            _Window.DrawText("Ammo: " + _Player.Ammo, Color.White, 0, (_Window.Height / 2) );
            _Window.DrawText("The High Score is " + Highscore, Color.White, 0, (_Window.Height / 2) + 100);
            _Player.Draw();

            foreach (Block BLOCK in _blocks)
            {
                BLOCK.Draw(_Window);
            }
            foreach (Buff BUFF in _buff)
            {
                BUFF.Draw(_Window);
            }
            foreach (Laser laser in _laser)
            {
                laser.Draw(_Window);
            }
        }
        
        if (_Player.LevelEnd == true)
        {
            _Window.DrawText("Press Mouse to Restart ", Color.Blue, (_Window.Width / 2), (_Window.Height / 2));


        }
        _Window.Refresh(60);
    }
  


        public void Update()
    {
        double LastBlockHeight = 50;
        if ((GameTimer.Ticks / 10)> Highscore)
        {
            Highscore = GameTimer.Ticks / 10;
        }

        _Player.HandleImput(_Window);
        if (_Player.ResetLevel == true)
        {
            Restart();
            
        }
        Draw();
            if (_Player.LevelEnd == false)
            {
            
            CheckCollions();
            foreach (Laser LASER in _laser)
            {
                LASER.Update();
            }


            List<Block> RemoveBlocks = new List<Block>();
            foreach (Block BLOCK in _blocks)
            {
                BLOCK.Update();
                if (BLOCK.IsOffScreen(_Window))
                {
                    RemoveBlocks.Add(BLOCK);
                }

            }

            foreach (Block BLOCK in RemoveBlocks)
            {
                if (BLOCK.SpawnNext)
                {
                    double MaybeNegative;
                    if (SplashKit.Rnd() > 0.5)
                    {
                        MaybeNegative = 1;
                    }
                    else
                    {
                        MaybeNegative = -1;
                    }
                    MaybeNegative = SplashKit.Rnd(20) * MaybeNegative;
                    double additon = (GameTimer.Ticks / 1000);
                    //Tell if it ceiling or floor block

                    if (BLOCK.Y == 0)
                    {
                        SpawnBlock(_Window.Width, BLOCK.Y, 50, LastBlockHeight + MaybeNegative + additon);
                    }
                    else
                    {
                        SpawnBlock(_Window.Width, _Window.Height - (LastBlockHeight + MaybeNegative + additon), 50, LastBlockHeight + MaybeNegative + additon);
                    }
                    LastBlockHeight = BLOCK.height;
                }
                _blocks.Remove(BLOCK);

            }
          
            // 3 second timer
            if (Cooldown.Ticks > 3000)
            {
                FloatBlock();
                // Cooldown.Reset();
            }
            if (BuffSpawnTimer.Ticks > 10000)
            {
                SpawnBuff();
                // Controls how often Buff Spawns
            }
            if (BuffTimer.Ticks > 4000)
            {
                _Player.PowerUp = false;
                BuffTimer.Stop();
               //Controls how long the buff goes for
            }
            if (_Player.Shoot)
            {
                //Uses Sprite sheet so width is divided by 5
                Laser laser = new Laser(_Player.X + (_Player.width/5), _Player.Y + _Player.height/2);
                _laser.Add(laser);
                _Player.Shoot = false;
            }


        }
    }

    public void SpawnStart()
    {
        //Window is 800 wide, block is 50 wide so 16 blocks cover whole screen

        //double offset = 0; Not needed
        for (double i = 0; i < _Window.Width+ 100; i = i + 50)
        {
            SpawnBlock(i,0,50,50);
            SpawnBlock(i, (_Window.Height - 50), 50, 50);
        }
        //Use Random for height later

    }
    public void FloatBlock()
    {
        double thisY = SplashKit.Rnd(100, _Window.Height - 100);
        
        SpawnBlock(_Window.Width, thisY, 50, SplashKit.Rnd(50,150), Color.Aqua);
        Cooldown.Start();
    }
    public void SpawnBuff()
    {
        double thisY = SplashKit.Rnd(100, _Window.Height - 100);

     //   SpawnBlock(_Window.Width, thisY, 50, SplashKit.Rnd(50, 150), Color.Aqua);


        Buff BuffTemp = new Buff(_Window.Width, thisY);
        _buff.Add(BuffTemp);

        BuffSpawnTimer.Start();
    }

    public void SpawnBlock(double x, double y, double width, double height)
    {
        //  800, 600 window size

        Block blocktest = new Block(x, y, width, height);
        _blocks.Add(blocktest);

    }

    public void SpawnBlock(double x, double y, double width, double height, Color Colour)
    {
        //  800, 600 window size

        Block blocktest = new Block(x, y, width, height, Colour);
        blocktest.SpawnNext = false;
        _blocks.Add(blocktest);

    }
    public void SpawnFloatBlock(double x, double y, double width, double height, Color Colour)
    {
        //  800, 600 window size

        Block blocktest = new Block(x, y, width, height, Colour);
        blocktest.SpawnNext = false;
        _blocks.Add(blocktest);

    }

    public void RainbowBuff(double x, double y, double width, double height, Color Colour)
    {
        //  800, 600 window size

        Block blocktest = new Block(x, y, width, height, Colour);
        _blocks.Add(blocktest);

    }

    public HelicopterGame (Window w, Helicopter h)
        {

            _Player = h;
            _Window = w;


        BuffSpawnTimer.Start();
        GameTimer.Start();
         SpawnStart();
         Cooldown.Start();


    }
}
