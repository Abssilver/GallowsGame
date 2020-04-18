using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace GallowsGame
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Menu : Page
    {
        Database informaticsDb, cartoonsDb;
        string[,] language = new string[2, 2]
        {
            {"Информатика", "Мультфильмы" },
            {"Computer Science", "Cartoons"}
        };
        int languageIndex = 0;
        ApplicationDataContainer localSettings; 
        public Menu()
        {
            this.InitializeComponent();

            localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("languageIndex"))
                languageIndex = (int)localSettings.Values["languageIndex"];
            if (localSettings.Values.ContainsKey("languageCheckBox"))
                languageCheckBox.IsChecked = (bool)localSettings.Values["languageCheckBox"];
            //MessageDialog msg = new MessageDialog("Debugging");
            //msg.ShowAsync();

            ChangeLanguage();
            
#region Informatics database initialization
            informaticsDb = new Database
            {
                questionCollection = new string[] { 
                    "Функция или процедура",
                    "while, do, for",
                    "if-else",
                    "(s, e)=>{...}" },
                answerCollection = new string[] { 
                    "метод",
                    "цикл",
                    "ветвление",
                    "лямбда"}
            };
#endregion

#region Cartoons database initialization
            cartoonsDb = new Database
            {
                questionCollection = new string[] { 
                    "Что лечит от всех болезней того, кто живет на крыше",
                    "С помощью чего Нильс избавил город от мышей",
                    "Что несла Красная Шапочка своей бабушке",
                    "На чем играл крокодил Гена" },
                answerCollection = new string[] { 
                    "варенье",
                    "дудочка",
                    "пирожки",
                    "гармошка"}
            };
#endregion
        }
        private void ChangeLanguage()
        {
            InformaticsButton.Content = language[languageIndex, 0];
            CartoonsButton.Content = language[languageIndex, 1];
        }

        private void button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), informaticsDb);
        }

        private void CartoonsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), cartoonsDb);

        }

        private void languageCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            languageIndex = 1;
            SaveSetting();
            ChangeLanguage();
        }

        private void languageCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            languageIndex = 0;
            SaveSetting();
            ChangeLanguage();
        }

        private void SaveSetting()
        {
            localSettings.Values["languageIndex"] = languageIndex;
            localSettings.Values["languageCheckBox"] = languageCheckBox.IsChecked;
        }
    }
}
