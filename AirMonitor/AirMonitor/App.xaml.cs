﻿using AirMonitor.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TabPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
