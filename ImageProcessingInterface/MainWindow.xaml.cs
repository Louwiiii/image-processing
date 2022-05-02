using ImageProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageProcessingInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The currently modified and showed image index.
        /// </summary>
        static int selectedImageIndex;

        /// <summary>
        /// The source of the currently modified image
        /// </summary>
        static string currentSource;
        
        MyImage modifiedImage;
        /// <summary>
        /// The currently modified MyImage. When modified, the function showing the image in the application will be called.
        /// </summary>
        MyImage ModifiedImage
        {
            get { return modifiedImage; }
            set
            {
                modifiedImage = value;
                LoadModifiedImage();
            }
        }

        /// <summary>
        /// The currently modified and showed image index. The index will cycle through the number of images of the "/images" folder.
        /// </summary>
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
                
                currentSource = availableBitmaps[selectedImageIndex];
            }
        }

        /// <summary>
        /// The filepaths of the available .bmp images in the "/images" folder
        /// </summary>
        public static string[] availableBitmaps = new string[0];

        /// <summary>
        /// The constructor of the MainWindow, called when the application starts.
        /// Checks which bitmap files are available and selects the first image of the folder.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            UpdateAvailableBitmaps();
            SelectedImageIndex = 0;
            Reset(null, null);
            if (availableBitmaps.Length > 0)
            {
                ChangeImageSource(currentSource);
            }
        }

        /// <summary>
        /// Show the modified image in the app
        /// </summary>
        public void LoadModifiedImage()
        {
            BitmapImage bi = new BitmapImage();
            using (var fs = new MemoryStream(ModifiedImage.ToFileStream()))
            {
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = fs;
                bi.EndInit();
            }
            SelectedImage.Source = bi;
        }

        /// <summary>
        /// Change the modified image to the given source and load it into the application. Change the content of the textbox under the image to the name of the image.
        /// </summary>
        /// <param name="source"></param>
        public void ChangeImageSource(string source)
        {
            ModifiedImage = new MyImage(source, true);
            ImageNameInput.Text = source.Split("\\")[source.Split("\\").Length - 1].Replace(".bmp", ""); ;
        }

        /// <summary>
        /// Load all the available images of the "/images" folder
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Save the image in the "/images" folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Save(object sender, RoutedEventArgs e)
        {
            ModifiedImage.FromImageToFile(Directory.GetCurrentDirectory() + "\\images\\" + ImageNameInput.Text + ".bmp");
            UpdateAvailableBitmaps();
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

        /// <summary>
        /// Import an image from the computer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Import(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".bmp";
            dialog.Filter = "BMP Image Files|*.bmp";

            if (dialog.ShowDialog() == true)
            {
                currentSource = dialog.FileName;
                ChangeImageSource(currentSource);
            }
        }

        /// <summary>
        /// Reset the modified image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Reset(object sender, RoutedEventArgs e)
        {
            if (availableBitmaps.Length > 0)
            {
                string source = currentSource;
                ImageNameInput.Text = source.Split("\\")[source.Split("\\").Length - 1].Replace(".bmp", "");
                ChangeImageSource(source);
                ModifiedImage = new MyImage(source);
            }
        }
        public void FiltreGris(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.ToShadesOfGrey();
        }

        public void ChangeTint(object sender, RoutedEventArgs e)
        {
            try
            {
                ModifiedImage = ModifiedImage.ChangedTint(TintInput.Text);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void Redimensionner(object sender, RoutedEventArgs e)
        {
            try
            {
                float factor = float.Parse(ResizeInput.Text.Replace(".", ",").Replace("x", ""));
                MyImage temp = ModifiedImage.Resized(factor);
                ModifiedImage = temp;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        public void Rotation(object sender, RoutedEventArgs e)
        {
            try
            {
                float factor = float.Parse(RotationInput.Text.Replace(".", ",").Replace("°", ""));
                MyImage temp = ModifiedImage.Rotation(factor);
                ModifiedImage = temp;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void Mirror(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Mirror();
        }
        public void DetectionContour(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.BorderDetection();
        }
        public void RenforcementBord(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.EdgeReinforcement();
        }
        public void FiltreFlou(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Blur();
        }

        public void Sharpen(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Sharpen();
        }
        public void FiltreRepoussage(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Emboss();
        }

        public void Histogram(object sender, RoutedEventArgs e)
        {
            ModifiedImage = ModifiedImage.Histogram();
        }

        public void RemoveBackground(object sender, RoutedEventArgs e)
        {
            try
            {
                double threshold = (ThresholdInput.Value * 2f);
                MyImage temp = ModifiedImage.RemoveBackground(BackgroundColorInput.Text, threshold);
                ModifiedImage = temp;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
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

                MyImage temp = MyImage.Mandelbrot(width, iterations, centerX, centerY, rectWidth, rectHeight);
                
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void CreateQRCode (object sender, RoutedEventArgs e)
        {
            try
            {
                string content = QRCodeInput.Text;
                MyImage temp = new QRCode(content);
                ModifiedImage = temp;
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
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
                ShowError(ex.Message);
            }
        }

        public void SelectHidingImage(object sender, RoutedEventArgs e)
        {
            try
            {
                HidingImageText.Text = currentSource;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void HideImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string sourceHiding = HidingImageText.Text;
                MyImage temp = ModifiedImage.HideIn(new MyImage(sourceHiding));
                ModifiedImage = temp;
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void DiscoverHiddenImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string source = currentSource;
                MyImage temp = ModifiedImage.DiscoverImage().Item1;
                ModifiedImage = temp;
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void DiscoverHidingImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string source = currentSource;
                MyImage temp = ModifiedImage.DiscoverImage().Item2;
                ModifiedImage = temp;
                ImageNameInput.Text = "NewImage";
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

    }
}
