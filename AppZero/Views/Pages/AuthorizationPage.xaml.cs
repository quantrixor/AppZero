﻿using AppZero.Context;
using AppZero.Model;
using AppZero.Settings;
using AppZero.Views.Pages.AdminPages;
using AppZero.Views.Pages.EmployePages;
using AppZero.Views.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AppZero.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var logger = new Logger();
            try
            {
                var currentUser = AppData.db.SignIn.FirstOrDefault(item => item.Username == txbUsername.Text && item.Password == psbPassword.Password);
                if (currentUser != null)
                {
                    switch (currentUser.IDRole)
                    {
                        case "A":
                            NavigationService.Navigate(new ViewPage(currentUser.User.FirstOrDefault(item => item.IDSignIn == currentUser.ID)));
                            
                            break;
                        case "U":
                            NavigationService.Navigate(new ViewPageEmp(currentUser.User.FirstOrDefault(item => item.IDSignIn == currentUser.ID)));
                            
                            break;
                        default:
                            throw new Exception("Неверный логин или пароль!");
                    }
                }
                else
                {
                    throw new Exception("Неверный логин или пароль!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAbdout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
