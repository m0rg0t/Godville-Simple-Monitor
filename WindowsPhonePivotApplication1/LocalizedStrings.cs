﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GodvilleSimpleMonitor;


namespace GodvilleSimpleMonitor.Localization
{
    public class LocalizedStrings
    {
        public LocalizedStrings() { }

        private static AppResources localizedresources = new AppResources();
        public AppResources LocalizedResources { get { return localizedresources; } }
    }

}
