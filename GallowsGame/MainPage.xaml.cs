using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace GallowsGame
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        int symbolSize = 50;
        int questionNumber = 0;
        byte numberOfMistakes = 0;
        Random rand = new Random();
        
        string[] questionArray;
        
        string[] answerArray;
        public MainPage()
        {
            this.InitializeComponent();
            GenerateAlphabetField();
        }

        void GenerateQuestion()
        {
            questionNumber = rand.Next(questionArray.Length);
            questionText.Text = questionArray[questionNumber];
            answerText.Text = new string('*', answerArray[questionNumber].Length);
        }

        bool StringContainsThisSymbol(string strAnswer, char symbol) => strAnswer.Contains(symbol);
 
        void InsertSymbolIntoString(string currentAnswer, char symbol)
        {
            string newAnswer = "";
            for (int i = 0; i <answerArray[questionNumber].Length; i++)
            {
                if (answerArray[questionNumber][i] == symbol)
                    newAnswer += symbol;
                else
                    newAnswer += currentAnswer[i];
            }
            answerText.Text = newAnswer;
        }
        void GenerateAlphabetField() 
        {
            int borders = (int)Math.Sqrt(alphabet.Length);
            int symbol=0;
            for (int i = 0; i < borders+1; i++)
            {
                for (int j = 0; j < borders+1; j++)
                {
                    if (symbol<alphabet.Length)
                    {
                        Button btn = new Button();
                        btn.Content = alphabet[symbol++];
                        btn.Width = symbolSize;
                        btn.Height = symbolSize;
                        btn.Tapped += Btn_Tapped;
                        btn.Margin = new Thickness(j * btn.Width, i * btn.Height, 0, 0);
                        alphabetCanvas.Children.Add(btn);
                    }
                }
            }
        }

        private void Btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (numberOfMistakes <= 6)
            {

                char symbol = (sender as Button).Content.ToString()[0];
                if (StringContainsThisSymbol(answerArray[questionNumber], symbol))
                {
                    InsertSymbolIntoString(answerText.Text, symbol);
                }
                else
                {
                    UpdateImage(numberOfMistakes++);
                }
            (sender as Button).IsEnabled = false;
            }
        }
        
        private void UpdateImage(byte numOfMistakes) 
        {
            BitmapImage gallowsImg = new BitmapImage();
            gallowsImg.UriSource = new Uri(gallowsImage.BaseUri, $"Gallows_{numOfMistakes}.png");
            gallowsImage.Source = gallowsImg;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Database transmittedDb = (e.Parameter as Database);
            questionArray = transmittedDb.questionCollection;
            answerArray = transmittedDb.answerCollection;
            GenerateQuestion();
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
