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
using ImageProcessing;
using System.IO;

namespace ImageProcessingInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int selectedImageIndex;

        MyImage ModifiedImage;

        public static int SelectedImageIndex
        {
            get { return selectedImageIndex; }
            set
            {
                if (availableBitmaps.Length == 0)
                {
                    selectedImageIndex = 0;
                    return;
                }
                selectedImageIndex = value % availableBitmaps.Length;
                while (selectedImageIndex < 0)
                    selectedImageIndex += availableBitmaps.Length;
            }
        }
        public static string[] availableBitmaps = new string[0];

        public MainWindow()
        {
            InitializeComponent();
            UpdateAvailableBitmaps();
            SelectedImageIndex = 0;
            Reset(null, null);
            if (availableBitmaps.Length > 0)
            {
                SaveTemp();
                ChangeImageSource(availableBitmaps[selectedImageIndex]);
            }
        }

        public void ChangeImageSource(string source)
        {
            BitmapImage bi = new BitmapImage();
            using (var fs = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = fs;
                bi.EndInit();
            }
            FileNameText.Text = source.Split("\\")[source.Split("\\").Length - 1];
            SelectedImage.Source = bi;
        }

        public static string[] UpdateAvailableBitmaps()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/images"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/images");
            }

            List<string> result = new List<string>();
            foreach (string e in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/images"))
            {
                if (e.Substring(e.Length - 4) == ".bmp" && e.Split("\\")[e.Split("\\").Length - 1][0] != '.')
                    result.Add(e);
            }

            availableBitmaps = result.ToArray();
            return result.ToArray();
        }

        public void PreviousImage(object sender, RoutedEventArgs e)
        {
            SelectedImageIndex--;
            Reset(sender, e);
        }

        public void NextImage(object sender, RoutedEventArgs e)
        {
            SelectedImageIndex++;
            Reset(sender, e);
        }

        void SaveTemp()
        {
            ModifiedImage.FromImageToFile(Directory.GetCurrentDirectory() + "\\images\\.temp.bmp");
            ChangeImageSource(Directory.GetCurrentDirectory() + "\\images\\.temp.bmp");
            //File.Delete(Directory.GetCurrentDirectory() + "\\images\\.temp.bmp");
        }

        void Save(object sender, RoutedEventArgs e)
        {
            ModifiedImage.FromImageToFile(Directory.GetCurrentDirectory() + "\\images\\" + ImageNameInput.Text + ".bmp");
            UpdateAvailableBitmaps();
        }
        public void RetourMenu(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        async Task Delay (int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        async void ShowError (string text)
        {
            ErrorText.Text = text;
            await Delay(5000);
            if (ErrorText.Text == text)
                ErrorText.Text = "";
        }

        public void Reset(object sender, RoutedEventArgs e)
        {
            if (availableBitmaps.Length > 0)
            {
                string source = availableBitmaps[selectedImageIndex];
                ImageNameInput.Text = source.Split("\\")[source.Split("\\").Length - 1].Replace(".bmp", "");
                ChangeImageSource(source);
                ModifiedImage = new MyImage(source);
            }
        }

        public void FiltreGris(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.ToShadesOfGrey();
            SaveTemp();
        }

        public void ChangeTint(object sender, RoutedEventArgs e)
        {
            try
            {
                ModifiedImage = ModifiedImage.ChangedTint(TintInput.Text);
                SaveTemp();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void Redimensionner(object sender, RoutedEventArgs e)
        {
            try
            {
                float factor = float.Parse(ResizeInput.Text.Replace(".", ",").Replace("x", ""));
                ModifiedImage = ModifiedImage.Resized(factor);
                SaveTemp();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }
        public void Rotation(object sender, RoutedEventArgs e)
        {
            try
            {
                float factor = float.Parse(RotationInput.Text.Replace(".", ",").Replace("°", ""));
                ModifiedImage = ModifiedImage.Rotation(factor);
                SaveTemp();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void EffetMiroir(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.EffetMiroir();
            SaveTemp();
        }
        public void DetectionContour(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.BorderDetection();
            SaveTemp();
        }
        public void RenforcementBord(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.EdgeReinforcement();
            SaveTemp();
        }
        public void FiltreFlou(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Blur();
            SaveTemp();
        }

        public void Sharpen(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Sharpen();
            SaveTemp();
        }
        public void FiltreRepoussage(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Emboss();
            SaveTemp();
        }

        public void Histogram(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Histogram();
            SaveTemp();
        }

        public void RemoveBackground(object sender, RoutedEventArgs e)
        {
            try
            {
                double threshold = (ThresholdInput.Value * 2f);
                ModifiedImage = ModifiedImage.RemoveBackground(BackgroundColorInput.Text, threshold);
                SaveTemp();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
            
        }

        public void Mandelbrot(object sender, RoutedEventArgs e)
        {
            try
            {
                int width = int.Parse(MandelbrotWidthInput.Text);
                int iterations = int.Parse(MandelbrotIterationsInput.Text);
                double centerX = double.Parse(MandelbrotCenterXInput.Text.Replace(".", ","));
                double centerY = double.Parse(MandelbrotCenterYInput.Text.Replace(".", ","));
                double rectWidth = double.Parse(MandelbrotRectWidth.Text.Replace(".", ","));
                double rectHeight = double.Parse(MandelbrotRectHeight.Text.Replace(".", ","));

                ModifiedImage = MyImage.Mandelbrot(width, iterations, centerX, centerY, rectWidth, rectHeight);
                SaveTemp();
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void CreateQRCode (object sender, RoutedEventArgs e)
        {
            try
            {
                string content = QRCodeInput.Text;
                ModifiedImage = new QRCode(content);
                SaveTemp();
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
            
        }

        public void ReadQRCode (object sender, RoutedEventArgs e)
        {
            try
            {
                string content = (new QRCode(ModifiedImage)).Read();
                QRCodeOutput.Text = content;
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void SelectHidingImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string source = availableBitmaps[selectedImageIndex];
                HidingImageText.Text = source.Split("\\")[source.Split("\\").Length - 1].Replace(".bmp", "");
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void HideImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string sourceHiding = Directory.GetCurrentDirectory() + "\\images\\" + HidingImageText.Text + ".bmp";
                ModifiedImage = ModifiedImage.HideIn(new MyImage(sourceHiding));
                SaveTemp();
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

        public void DiscoverImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string source = availableBitmaps[selectedImageIndex];
                ModifiedImage = ModifiedImage.DiscoverImage().Item1;
                SaveTemp();
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString().Split('\n')[0].Trim());
            }
        }

    }
}
