﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.Xml;
using System.Threading;
using Google.AdMob.Ads.WindowsPhone7;
using System.Globalization;

namespace GodvilleSimpleMonitor
{

    public partial class MainPage : PhoneApplicationPage
    {
        private int phase = 0;
        private Timer t;
        private string Godname;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void MyTimerCallback(object state)
        {
            
            WebClient web = new WebClient();
            web.DownloadStringAsync(new Uri("http://godville.net/gods/api/" + Godname + ".xml"));
            web.DownloadStringCompleted += new DownloadStringCompletedEventHandler(web_DownloadStringCompleted);
        }


        private void btnCheckGod_Click(object sender, RoutedEventArgs e)
        {
            WebClient web = new WebClient();

            web.DownloadStringAsync(new Uri("http://godville.net/gods/api/"+this.GodName.Text+".xml"));
            web.DownloadStringCompleted += new DownloadStringCompletedEventHandler(web_DownloadStringCompleted);
            Godname = this.GodName.Text;
            t = new Timer(MyTimerCallback, null, 60000, 60000); //lets start timer to get updates about hero
        }

        string GetDataFromXML(string type, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = "";
                XmlReader r = XmlReader.Create(new MemoryStream(System.Text.UnicodeEncoding.UTF8.GetBytes(e.Result)));
                while (r.ReadToFollowing(type))
                {
                    data = data + r.ReadElementContentAsString() + "\r\n";
                };
                r.Close();
                return data;
            }
            catch
            {
                return "0";
            };
        }

        void web_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    string s = e.Result;
                    //this.HeroData.Visibility;
                    this.DiaryOutput.Text += "\r\n" + GetDataFromXML("diary_last", e);

                    this.HeroOutput1.Text = "Hero " + GetDataFromXML("name", e);
                    this.HeroOutput2.Text = "God " + GetDataFromXML("godname", e);
                    this.HeroOutput3.Text = "Sex " + GetDataFromXML("gender", e);
                    this.HeroOutput4.Text = "Money " + GetDataFromXML("gold_approx", e);
                    this.HeroOutput5.Text = "Level " + GetDataFromXML("level", e);
                    this.HeroOutput6.Text = "Health "+GetDataFromXML("health", e);
                    this.HealthBar.Maximum = double.Parse(GetDataFromXML("max_health", e));
                    //this.HealthBar.Value = double.Parse(GetDataFromXML("health", e));
                    this.HealthBar.Value = '0';
                    this.HeroOutput8.Text = "Alignment " + GetDataFromXML("alignment", e);
                    this.HeroOutput9.Text = "Quest " + GetDataFromXML("quest", e);
                    //this.HeroOutput10.Text = "Quest progress";
                    //this.QuestBar.Value = double.Parse(GetDataFromXML("quest_progress", e));

                    this.InventaryOutput.Text = GetDataFromXML("item", e);

                    this.OutputAnswer.Text = "Get data about hero" + "\r\n";
                }
                catch (System.Net.WebException)
                {
                    this.OutputAnswer.Text = "Can't get data about hero, sorry";
                }
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            //AboutPrompt about = new AboutPrompt();
            //about.Completed += about_Completed;
            //about.Show("Anton Lenev", "@m0rg0t", "m0rg0t.Anton@gmail.com", @"http://m0rg0t.ru");
            MessageBox.Show("Author - Anton Lenev (m0rg0t) \nhttp://m0rg0t.ru \nm0rg0t.Anton@gmail.com \n\n As background image used: Незавершенный набросок карты Годвилля. Скетч - Зефирка. Наложил на папирус - Floyd311");
        }

        //void about_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        //{
        //    //add some code here
        //}

   
    }
}