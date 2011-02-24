using System;
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

namespace WindowsPhonePivotApplication1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        private void btnCheckGod_Click(object sender, RoutedEventArgs e)
        {
            //string websiteURL;
            //websiteURL = "http://www.usps.com/shipping/trackandconfirm.htm";
            //websiteURL = "http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do";

            //this.OutputAnswer.Text = "Results:\n";

            WebClient web = new WebClient();
            //string str = "strOrigTrackNum=CJ301795892US&Go+to+Track+%26+Confirm.x=20&Go+to+Track+%26+Confirm.y=7&Go+to+Track+%26+Confirm=Go";
            //WebClient rest = new WebClient();
            web.DownloadStringAsync(new Uri("http://godville.net/gods/api/"+this.GodName.Text+".xml"));
            web.DownloadStringCompleted += new DownloadStringCompletedEventHandler(web_DownloadStringCompleted);

            //web.OpenReadAsync(new Uri(websiteURL + "?strOrigTrackNum=" + this.trackcode.Text));
            //web.OpenReadCompleted += new OpenReadCompletedEventHandler(web_OpenReadCompleted);
        }

        string GetDataFromXML(string type, DownloadStringCompletedEventArgs e)
        {
            string data = "";
            XmlReader r = XmlReader.Create(new MemoryStream(System.Text.UnicodeEncoding.UTF8.GetBytes(e.Result)));
            while (r.ReadToFollowing(type))
            {
                data = data + r.ReadElementContentAsString() + "\r\n"; ;
            };
            r.Close();
            return data;
        }

        void web_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            lock (this)
            {
                try
                {
                    string s = e.Result;

                    this.DiaryOutput.Text = GetDataFromXML("diary_last", e);

                    this.HeroOutput.Text = "Герой " + GetDataFromXML("name", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Бог " + GetDataFromXML("godname", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Пол " + GetDataFromXML("gender", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Денег " + GetDataFromXML("gold_approx", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Уровень " + GetDataFromXML("level", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Здоровье " + GetDataFromXML("health", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Максимум здоровья " + GetDataFromXML("max_health", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Мировозрение " + GetDataFromXML("alignment", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Задание " + GetDataFromXML("quest", e);
                    this.HeroOutput.Text = this.HeroOutput.Text + "Процент выполнения " + GetDataFromXML("quest_progress", e);

                    this.InventaryOutput.Text = this.InventaryOutput.Text + GetDataFromXML("item", e);

                    this.OutputAnswer.Text = "Данные о герое получены";
                }
                catch (System.Net.WebException)
                {
                    this.OutputAnswer.Text = "Информация о герое недоступна";
                }
            }
        }

   
    }
}