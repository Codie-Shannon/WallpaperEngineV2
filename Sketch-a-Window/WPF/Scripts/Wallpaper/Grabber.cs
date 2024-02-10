using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;
using MediaInfo.DotNetWrapper.Enumerations;

namespace WPF.Scripts
{
    public class Grabber
    {
        // Variables
        // ======================================================================
        // ======================================================================
        private static readonly MediaInfo.DotNetWrapper.MediaInfo mediainfo = new MediaInfo.DotNetWrapper.MediaInfo();



        #region Methods
        // Configuration
        // ======================================================================
        // ======================================================================
        private static void ConfigureGrabber(string filepath)
        {
            //Configure MediaInfo Grabber
            mediainfo.Open(filepath);
            mediainfo.Option("ParseSpeed", "0");
        }


        // Resolution
        // ======================================================================
        // ======================================================================
        private static string GetVideoWidth()
        {
            //Get and Return Width of Video
            return mediainfo.Get(StreamKind.Video, 0, "Width");
        }

        private static string GetVideoHeight()
        {
            //Get and Return Height of Video
            return mediainfo.Get(StreamKind.Video, 0, "Height");
        }


        // Video Preview
        // ======================================================================
        // ======================================================================
        public static string GetVideoPreview(string path)
        {
            //Configure Grabber
            ConfigureGrabber(path);

            //Get Width and Height of Video
            string _width = GetVideoWidth(), _height = GetVideoHeight();

            //Initialize Save Locations
            string tempLocation = $"{LocalSettings.GetValue("ImagesFolder")}\\{LocalSettings.GetValue("WallpaperIndex")}_temp.png";
            string saveLocation = $"{LocalSettings.GetValue("ImagesFolder")}\\{LocalSettings.GetValue("WallpaperIndex")}.png";

            //Convert Width and Height to Integers
            int.TryParse(_width, out int width);
            int.TryParse(_height, out int height);

            //Create Media Player Object and Set Volume to 0 and ScrubbingEnabled to true
            MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };

            //Open Media Player Object with filepath Variable
            player.Open(new Uri(path));

            //Pause Media Player Object
            player.Pause();

            //Set Position of Media Player Object to 0 Seconds
            player.Position = TimeSpan.FromSeconds(0);

            //Wait for Media Player Object to Load
            System.Threading.Thread.Sleep(1 * 1000);

            //Draw Visual
            DrawingVisual drawVisual = new DrawingVisual();

            //Set Size of Media Player Object
            DrawingContext drawingContext = drawVisual.RenderOpen();
            drawingContext.DrawVideo(player, new Rect(0, 0, width, height));
            drawingContext.Close();

            //Render Video Frame as Bitmap
            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 1 / width, 1 / height, PixelFormats.Pbgra32);
            bmp.Render(drawVisual);

            //Create PngBitmapEncodeer Object
            PngBitmapEncoder png = new PngBitmapEncoder();

            //Add bmp to the PngBitmapEncoder Frames Collection
            png.Frames.Add(BitmapFrame.Create(bmp));

            //Try Saving Video Preview
            try
            {
                //Open Stream
                using (Stream stm = File.Create(tempLocation))
                {
                    //Save Temporary Image
                    png.Save(stm);
                }

                //Close Media Player Object
                player.Close();

                //Resize Image
                GetImage(tempLocation, saveLocation, new System.Drawing.Size(width, height), true, new System.Drawing.Size(401, 230));

                //Return Save Location
                return saveLocation;
            }
            catch (Exception e)
            {
                //Return String.Empty
                return string.Empty;
            }
        }

        private static void GetImage(string templocation, string savelocation, System.Drawing.Size size, bool isdesiredsize, System.Drawing.Size desiredsize)
        {
            //Initialize Bitmap Image
            Bitmap image = new Bitmap(templocation);

            //Resize Image
            image = ResizeImage(image, size, isdesiredsize, desiredsize);

            //Save Image
            image.Save(savelocation);

            //Dispose of Bitmap Image
            image.Dispose();
        }

        // Source 1: https://stackoverflow.com/a/10839428 || ================= || Source 2: https://stackoverflow.com/a/2001692
        private static Bitmap ResizeImage(Bitmap image, System.Drawing.Size size, bool isdesiredsize, System.Drawing.Size desiredsize)
        {
            //Check if Desired Size has been Set
            if (isdesiredsize == true)
            {
                //Get Ratio
                decimal wratio = (decimal)size.Width / desiredsize.Width;
                decimal hratio = (decimal)size.Height / desiredsize.Height;

                //Get Multiplier
                decimal ratio = wratio > hratio ? wratio : hratio;

                //Get New Size
                size = new System.Drawing.Size((int)(size.Width / ratio), (int)(size.Height / ratio));
            }

            //Initialize Bitmap with Set Size
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            //Open Drawing Interface
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //Set Interpolation Mode to High Quality
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //Draw Image to Size
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }

            //Return Result
            return bmp;
        }
        #endregion Methods
    }
}