﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using tft_cosmetics_manager.Models;

namespace tft_cosmetics_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string port;
        private string token;

        private readonly List<int> companionIds = new();
        private readonly List<int> mapSkinIds = new();
        private readonly List<int> damageSkinIds = new();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowOverlay();
            GetKeys();
        }

        private void GetKeys()
        {
            string command = "wmic";
            string arguments = "PROCESS WHERE name='LeagueClientUx.exe' GET commandline";

            Process process = new();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C{command} {arguments}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine("Saída: " + output);
            Console.WriteLine("Erro: " + error);

            string appPortPattern = "--app-port=([0-9]*)";
            string authTokenPattern = "--remoting-auth-token=([\\w-]*)";

            Match appPortMatch = Regex.Match(output, appPortPattern);
            Match authTokenMatch = Regex.Match(output, authTokenPattern);

            if (appPortMatch.Success && authTokenMatch.Success)
            {
                port = appPortMatch.Groups[1].Value;
                token = authTokenMatch.Groups[1].Value;
                GetData();
            }
        }
        private void GetData()
        {
            GetCompanions();
            GetMapSkins();
            GetDamageSkins();
            HideOverlay();
        }
        private void GetCompanions()
        {
            string url = $"https://127.0.0.1:{port}/lol-inventory/v1/inventory?inventoryTypes=%5B%22COMPANION%22%5D";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add($"Authorization", $"Basic {auth}");
            request.ContentType = "application/json";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("statusCode=" + response.StatusCode);
                }

                var body = new StringBuilder();
                using (var responseStream = response.GetResponseStream())
                {
                    using var reader = new StreamReader(responseStream);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        body.AppendLine(line);
                    }
                }

                string responseBody = body.ToString();

                var items = JsonConvert.DeserializeObject<List<Response>>(responseBody);
                foreach (var item in items)
                {
                    companionIds.Add(item.ItemId);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using var errorResponse = (HttpWebResponse)ex.Response;
                    Console.WriteLine("Error: " + errorResponse.StatusCode);
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        private void GetMapSkins()
        {
            string url = $"https://127.0.0.1:{port}/lol-inventory/v1/inventory?inventoryTypes=%5B%22TFT_MAP_SKIN%22%5D";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add($"Authorization", $"Basic {auth}");
            request.ContentType = "application/json";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("statusCode=" + response.StatusCode);
                }

                var body = new StringBuilder();
                using (var responseStream = response.GetResponseStream())
                {
                    using var reader = new StreamReader(responseStream);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        body.AppendLine(line);
                    }
                }

                string responseBody = body.ToString();

                var items = JsonConvert.DeserializeObject<List<Response>>(responseBody);
                foreach (var item in items)
                {
                    mapSkinIds.Add(item.ItemId);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using var errorResponse = (HttpWebResponse)ex.Response;
                    Console.WriteLine("Error: " + errorResponse.StatusCode);
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        private void GetDamageSkins()
        {
            string url = $"https://127.0.0.1:{port}/lol-inventory/v1/inventory?inventoryTypes=%5B%22TFT_DAMAGE_SKIN%22%5D";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add($"Authorization", $"Basic {auth}");
            request.ContentType = "application/json";

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("statusCode=" + response.StatusCode);
                }

                var body = new StringBuilder();
                using (var responseStream = response.GetResponseStream())
                {
                    using var reader = new StreamReader(responseStream);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        body.AppendLine(line);
                    }
                }

                string responseBody = body.ToString();

                var items = JsonConvert.DeserializeObject<List<Response>>(responseBody);
                foreach (var item in items)
                {
                    damageSkinIds.Add(item.ItemId);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using var errorResponse = (HttpWebResponse)ex.Response;
                    Console.WriteLine("Error: " + errorResponse.StatusCode);
                }
                else
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        private async Task<bool> SetCompanion()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, companionIds.Count);
            string randomId = companionIds[randomIndex].ToString();

            string url = $"https://127.0.0.1:{port}/lol-cosmetics/v1/selection/companion";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(randomId);
                    using (var content = new ByteArrayContent(data))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.PutAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Request successful.");
                        return true; // Retorna true para indicar sucesso
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false; // Retorna false para indicar erro
                }
            }

        }
        private async Task<bool> SetMapSkin()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, mapSkinIds.Count);
            string randomId = mapSkinIds[randomIndex].ToString();

            string url = $"https://127.0.0.1:{port}/lol-cosmetics/v1/selection/tft-map-skin";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));

            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(randomId);
                    using (var content = new ByteArrayContent(data))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.PutAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Request successful.");
                        return true; // Retorna true para indicar sucesso
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false; // Retorna false para indicar erro
                }
            }
        }
        private async Task<bool> SetDamageSkin()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, damageSkinIds.Count);
            string randomId = damageSkinIds[randomIndex].ToString();

            string url = $"https://127.0.0.1:{port}/lol-cosmetics/v1/selection/tft-damage-skin";
            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"riot:{token}"));



            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + auth);

                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(randomId);
                    using (var content = new ByteArrayContent(data))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.PutAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Request successful.");
                        return true; // Retorna true para indicar sucesso
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false; // Retorna false para indicar erro
                }
            }
        }

        private async void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = FindName("randomizeButton") as Button;


            if (button != null)
            {
                button.IsEnabled = false;
                button.Cursor = Cursors.No;
                button.Content = "Loading...";
            }


            Task<bool> task1 = SetCompanion();
            Task<bool> task2 = SetMapSkin();
            Task<bool> task3 = SetDamageSkin();
            await Task.WhenAll(task1, task2, task3);


            if (button != null)
            {
                button.IsEnabled = true;
                button.Cursor = Cursors.Hand;
                button.Content = "Randomize!";
            }
        }
        private void ShowOverlay()
        {
            Overlay.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;
            Application.Current.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
        }
        private void HideOverlay()
        {
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}
