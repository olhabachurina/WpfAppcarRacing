using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace WpfAppcarRacing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); 
        List<Rectangle> itemRemover = new List<Rectangle>(); 

        Random rand = new Random(); 

        ImageBrush playerImage = new ImageBrush(); 
        ImageBrush starImage = new ImageBrush(); 
        Rect playerHitBox; 
      
        int speed = 15;
        int playerSpeed = 10;
        int carNum;
        int starCounter = 30;
        int powerModeCounter = 200;

       
        double score;
        double i;

         
        bool moveLeft, moveRight, gameOver, powerMode;
        System.Windows.Controls.Image player = new System.Windows.Controls.Image(); 
        public MainWindow()
        {
            InitializeComponent();
            myCanvas.Focus(); 
            gameTimer.Tick += GameLoop; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 

            StartGame(); 
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            score += 0.05; 
            starCounter --;
            scoreText.Content = $"Survived {score.ToString("#.#")} Seconds";
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
           
            if (starCounter < 1)
            {
                MakeStar();
                starCounter = rand.Next(600, 900);
            }

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                
                if ((string)x.Tag == "roadMarks")
                {
                   

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed); 
                   
                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }

                } 

                if ((string)x.Tag == "Car")
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed); 

                    
                    if (Canvas.GetTop(x) > 500)
                    {
                        ChangeCars(x);
                    }

                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(carHitBox) && powerMode == true)
                    {
                     
                        ChangeCars(x);
                    }
                    else if (playerHitBox.IntersectsWith(carHitBox) && powerMode == false)
                    {
                        gameTimer.Stop(); 
                        scoreText.Content += " Press Enter to replay"; 
                        gameOver = true; 
                    }

                } 
               
                if ((string)x.Tag == "star")
                {
                    
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);

                   
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                   
                    if (playerHitBox.IntersectsWith(starHitBox))
                    {
                       
                        itemRemover.Add(x);

                       
                        powerMode = true;

                     
                        powerModeCounter = 200;

                    }

                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x);
                    }

                } 
            }
           
            if (powerMode == true)
            {
                powerModeCounter -= 1; 
                
                PowerUp();
               
                if (powerModeCounter < 1)
                {
                   
                    powerMode = false;
                }
            }
            else
            {

                
                myCanvas.Background = Brushes.Gray;
            }

          
            foreach (Rectangle y in itemRemover)
            {
                myCanvas.Children.Remove(y);
            }

            

            if (score >= 10 && score < 20)
            {
                speed = 12;
            }

            if (score >= 20 && score < 30)
            {
                speed = 14;
            }
            if (score >= 30 && score < 40)
            {
                speed = 16;
            }
            if (score >= 40 && score < 50)
            {
                speed = 18;
            }
            if (score >= 50 && score < 80)
            {
                speed = 22;
            }


        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUP(object sender, KeyEventArgs e)
        {
          

            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

           
            if (e.Key == Key.Enter && gameOver == true)
            {
               
                StartGame();
            }
        }

        private void StartGame()
        {
           

            speed = 8; 
            gameTimer.Start(); 

           
            moveLeft = false;
            moveRight = false;
            gameOver = false;
            powerMode = false;

           
            score = 0;
            
            scoreText.Content = "Survived: 0 Seconds";
            
            
                 
           
           
            myCanvas.Background = Brushes.Gray;

           

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                
                if ((string)x.Tag == "Car")
                {
                    
                    Canvas.SetTop(x, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(x, rand.Next(0, 430));
                    
                    ChangeCars(x);
                }

                
                if ((string)x.Tag == "star")
                {
                    itemRemover.Add(x);
                }

            }
                itemRemover.Clear();
        }

        private void ChangeCars(Rectangle car)
        {

            

            carNum = rand.Next(1, 6); 

                                            
           
            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(car, rand.Next(0, 430));
        }

        private void PowerUp()
        {
            

            i += 5; 
            if (i > 4)
            {
                i = 1;
            }

            
            myCanvas.Background = Brushes.LightCoral;


        }

        private void MakeStar()
        {
           
            Rectangle newStar = new Rectangle
            {
                Height = 50,
                Width = 50,
                Tag = "star",
                Fill = starImage
            };

            
            Canvas.SetLeft(newStar, rand.Next(0, 430));
            Canvas.SetTop(newStar, (rand.Next(100, 400) * -1));

            
            myCanvas.Children.Add(newStar);

        }
    }
}
    
    

