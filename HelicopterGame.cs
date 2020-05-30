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

    private Database database;
    private QueryResult queryResult;
    private Music music;
    private SoundEffect Hcs;
    private SoundEffect Star;
    public string PlayerName{ get; set; }
    public bool MenuOn;
    private SoundEffect Laser;
   
    
    private  Timer GameTimer = new Timer("GameTimer");
    private Timer Cooldown = new Timer("CoolDown");
    private Timer BuffTimer = new Timer("BuffTimer");
    private Timer BuffSpawnTimer = new Timer("BuffSpawnTimer");
    private int Highscore;
    private int CurrentScore;
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

    private void LoadSounds()
    {

         music = new Music("FS16", "FS16.mp3");
         Hcs = new SoundEffect("hcs", "chainsaw.wav");
         Laser = new SoundEffect("laser", "laser.mp3");
         Star = new SoundEffect("star", "star.mp3");

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
    private void AddScore()
    {

  queryResult = database.RunSql($"INSERT INTO scoretable(name, highscore) VALUES ('{PlayerName}', {CurrentScore})");
        if (queryResult.Successful)
        {
            Console.WriteLine("Added Score");
        }
        else
        {
            Console.WriteLine("Score Not added");
        }

    }

        private void DisplayScores()
    {
      

        queryResult = database.RunSql("SELECT rowid, name, highscore FROM scoretable");
        if (queryResult.HasRow)
        {
            do
            {
                Int32 ID = queryResult.QueryColumnForInt(0);
                string  name = queryResult.QueryColumnForString(1);
                Int32 Score = queryResult.QueryColumnForInt(2);
                Console.WriteLine($"ID: {ID}, Name: {name}, Score:{Score}");
            }
            while (queryResult.GetNextRow());
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
                    AddScore();
                    GameTimer.Pause();
                    break;

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
                    SplashKit.PauseMusic();
                    Star.Play();

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
            _Window.DrawText("The Score is " + CurrentScore, Color.White, 0, (_Window.Height / 2) - 100);
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

        CurrentScore = Convert.ToInt32(GameTimer.Ticks / 10);
        if (CurrentScore > Highscore)
        {
            Highscore = CurrentScore;
        }
       
        _Player.HandleImput(_Window);
        if (_Player.ResetLevel == true)
        {
            Restart();
            
        }

        if (!SplashKit.MusicPlaying())
        {
            music.Play();
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
            if (BuffTimer.Ticks > 5000)
            {
                _Player.PowerUp = false;
                BuffTimer.Stop();
                Star.Stop();
                SplashKit.ResumeMusic();
                //SplashKit.PlayMusic();
                //Controls how long the buff goes for
            }
            if (_Player.Shoot)
            {
                //Uses Sprite sheet so width is divided by 5
                Laser laser = new Laser(_Player.X + (_Player.width/5), _Player.Y + _Player.height/2);
                _laser.Add(laser);
                _Player.Shoot = false;
                 Laser.Play();
            }


        }
        if (_Player.DisplayScore == true)
        {
            DisplayScores();
            _Player.DisplayScore = false;
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
    public void StartingScreen()
    {
        _Window.Clear(Color.Black);
        _Window.DrawText("Use Mouse to Fly", Color.Red, 100, 10 + _Window.Height / 2);
        _Window.DrawText("Press Space to Shoot", Color.Red, 100, _Window.Height / 2);
        _Window.DrawText("Press Mouse to Start", Color.Red, 350, 500);
        _Window.DrawText("Press D to display scores", Color.Red, 100, 20 + _Window.Height / 2);
        _Window.DrawText("Collect these Items", Color.Yellow, 600, 200);
        _Window.DrawText("Blue Blocks can be destoryed when Invincible/Shot", Color.Yellow, 580, 330);

        _Window.DrawText("Invincibility", Color.Yellow, 640, 230);
        _Window.FillCircle(Color.AliceBlue, 600, 300, 24);
        _Window.FillCircle(Color.Blue, 600, 300, 24 - 4);
        _Window.FillCircle(Color.DeepSkyBlue, 600, 300, 24 - 8);
        _Window.FillCircle(Color.SkyBlue, 600, 300, 24 - 12);
        _Window.FillCircle(Color.MidnightBlue, 600, 300, 24 - 16);

        _Window.DrawText("Gives Ammo", Color.Yellow, 640, 300);
        _Window.FillCircle(Color.Red, 600, 240, 24);
        _Window.FillCircle(Color.Blue, 600, 240, 24 - 4);
        _Window.FillCircle(Color.Yellow, 600, 240, 24 - 8);
        _Window.FillCircle(Color.Violet, 600, 240, 24 - 12);
        _Window.FillCircle(Color.Green, 600, 240, 24 - 16);


    }

    public HelicopterGame (Window w, Helicopter h)
        {

            _Player = h;
            _Window = w;
        LoadSounds();
        database = new Database("Score","DBScore");
        queryResult = database.RunSql("CREATE TABLE IF NOT EXISTS scoretable(name VARCHAR(255), highscore INT)");
        music.Play();
        MenuOn = true;
        PlayerName = "NoName";

        if (queryResult.Successful)
        {
            Console.WriteLine("Table created");
        }
        else
        {
            Console.WriteLine("Table NOT created");
        }

        
        queryResult = database.RunSql("INSERT INTO scoretable(name, highscore) VALUES ('Test3', 2394)");
        if (queryResult.Successful)
        {
            Console.WriteLine("Added Score");
        }
        else
        {
            Console.WriteLine("Score Not added");
        }


        BuffSpawnTimer.Start();
        GameTimer.Start();
         SpawnStart();
         Cooldown.Start();


    }
}
