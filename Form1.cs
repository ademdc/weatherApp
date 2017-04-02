using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using System.Diagnostics;

namespace Assignment_2
{
    public partial class Form1 : Form
    {
        private void Form1_Load(object sender, System.EventArgs e)
        {
            textBox1.Text = "Sarajevo";
            setMap("Sarajevo");
        }
        
        public Form1()
        {
            //innitialy it loads Sarajevo, both the map and weather
            InitializeComponent();
            setMap("Sarajevo");
            textBox1.Text = "Sarajevo";    
            getWeather();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getWeather();
        }

        private void setMap(string city)
        {
            //taken from http://www.independent-software.com
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.SetPositionByKeywords(city);

            gmap.DragButton = MouseButtons.Left;
            gmap.CanDragMap = true;
            gmap.MinZoom = 0;
            gmap.MaxZoom = 24;
            gmap.Zoom = 12;
            gmap.AutoScroll = true;
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }
        
        private void getWeather()
        {
            String city = textBox1.Text;

            WebClient client = new WebClient();
            JObject json = null;
            string temp = "";
            string humidity = "";
            string pressure = "";
            string wind = "";
            double sunrise = 0;
            double sunset = 0;
            string country = "";
            string conditions = "";
            try
            {
                string newString = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&appid=85e922e0be7ea333b6cd50859d135eaf");
               
                json = JObject.Parse(newString);
                temp = (String)json["main"]["temp"];
                humidity = (String)json["main"]["humidity"];
                pressure = (String)json["main"]["pressure"];
                wind = (String)json["wind"]["speed"];
                sunrise = (Double)json["sys"]["sunrise"]; 
                sunset = (Double)json["sys"]["sunset"];
                conditions = (String)json["weather"][0]["main"];
                country = (String)json["sys"]["country"];

                //Flag of country
                string filenameCountryIcon = @"C:\Users\Adem Dinarević\Documents\Visual Studio 2017\Projects\Assignment 2\Assignment 2\" + country + ".png";

                Image imageCountry = Image.FromFile(@filenameCountryIcon);
                pictureBox1.BackgroundImage = imageCountry;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            //Displaying weather info in labels
            label1.Text = textBox1.Text + ": " + temp + " C";
            label2.Text = "Humidity: " + humidity + "%";
            label3.Text = "Pressure: " + pressure + "bar";
            label4.Text = "Wind: " + wind + "km/h";
            label5.Text = "Sunrise: " + UnixTimeStampToDateTime(sunrise);
            label6.Text = "Sunset: " + UnixTimeStampToDateTime(sunset);
            label7.Text = textBox1.Text + ", " + country;
            label8.Text = "Desription: "+ conditions;
            
            //decription of weather
            string description = "In " + textBox1.Text + " there is/are " + conditions +
                ". The temperature is " + temp + " C with a humidity of " +
                "." + humidity + "%. The pressure is " + pressure + " bar. The wind speed is" + wind + "km/h";
            setMap(textBox1.Text);
            textBox3.Text = description;

            //Icon of weather
            string filenameWeatherIcon = @"C:\Users\Adem Dinarević\Documents\Visual Studio 2017\Projects\Assignment 2\Assignment 2\Resources\";
            Image imageWeather;
            if (conditions == "Clear")
            {
                filenameWeatherIcon = filenameWeatherIcon + "clear.png";
                imageWeather = Image.FromFile(@filenameWeatherIcon);
                pictureBox2.BackgroundImage = imageWeather;
            }
            else if (conditions == "Clouds")
            {
                filenameWeatherIcon = filenameWeatherIcon + "cloudy.png";
                imageWeather = Image.FromFile(@filenameWeatherIcon);
                pictureBox2.BackgroundImage = imageWeather;
            }
            else if (conditions == "Rain")
            {
                filenameWeatherIcon = filenameWeatherIcon + "rainy.png";
                imageWeather = Image.FromFile(@filenameWeatherIcon);
                pictureBox2.BackgroundImage = imageWeather;
            }
            else if (conditions == "Haze" || conditions =="Fog" || conditions == "Mist")
            {
                filenameWeatherIcon = filenameWeatherIcon + "fog.png";
                imageWeather = Image.FromFile(@filenameWeatherIcon);
                pictureBox2.BackgroundImage = imageWeather;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //Convert seconds for sunrise and sunset
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            string dateWithFormat = dtDateTime.ToLongDateString();
            return dtDateTime;
        }
    }
}
