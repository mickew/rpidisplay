using System.Device.I2c;
using System.Drawing;
using System.Reflection;
using Iot.Device.Graphics;
using Iot.Device.Graphics.SkiaSharpAdapter;
using Iot.Device.Ssd13xx;

namespace rpidisplay;

internal class Program
{
    private const string VersionArgs = "--version";
    private const string Font = "DejaVu Sans";
    private const int FontSize = 8;
    private const int DisplayInfoTimeout = 1000;
    private const int DisplayLogoTimeout = 15000;
    private static bool keepRunning = true;

    static int Main(string[] args)
    {
        var printVersion = args.Any(x => x == VersionArgs);
        if (printVersion)
        {
            Console.WriteLine(GetVersion());
            return 0;
        }
        Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            keepRunning = false;
        };
        Console.WriteLine("Starting...");

        SkiaSharpAdapter.Register();
        I2cConnectionSettings connectionSettings = new(1, 0x3C);
        I2cDevice? i2cDevice = I2cDevice.Create(connectionSettings);
        Ssd13xx device = new Ssd1306(i2cDevice, 128, 32);

        device.ClearScreen();
        DisplayImage(device);
        device.ClearScreen();

        while (keepRunning)
        {
            var ipAddress = Helper.GetIpAddress();
            var cpuLoad = Helper.GetCpuLoad();
            var memoryUsage = Helper.GetMemoryUsage();
            var diskUsage = Helper.GetDiskUsage();
            DisplayInfo(device, Font, FontSize, ipAddress, cpuLoad, memoryUsage, diskUsage);
        }

        DisplayImage(device, false);
        device.Dispose();
        Console.WriteLine("Shuting down...");
        return 0;
    }

    private static void DisplayInfo(GraphicDisplay ssd1306, string font, int fontSize, string ipAddress, string cpuLoad, string memoryUsage, string diskUsage)
    {
        var y = 0;
        using (var image = BitmapImage.CreateBitmap(128, 32, PixelFormat.Format32bppArgb))
        {
            image.Clear(Color.Black);
            var g = image.GetDrawingApi();
            g.DrawText(ipAddress, font, fontSize, Color.White, new Point(0, y));
            y += (fontSize - 1);
            g.DrawText(cpuLoad, font, fontSize, Color.White, new Point(0, y));
            y += (fontSize - 1);
            g.DrawText(memoryUsage, font, fontSize, Color.White, new Point(0, y));
            y += (fontSize - 1);
            g.DrawText(diskUsage, font, fontSize, Color.White, new Point(0, y));
            ssd1306.DrawBitmap(image);
        }
    }

    private static void DisplayImage(GraphicDisplay ssd1306, bool waitForTimeout = true)
    {
        Console.WriteLine("Display Image");
        var image_name = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "logo.bmp");
        using BitmapImage image = BitmapImage.CreateFromFile(image_name);
        ssd1306.DrawBitmap(image);
        if (waitForTimeout)
        {
            Thread.Sleep(DisplayLogoTimeout);
        }
    }

    private static string GetVersion()
    {
        Assembly currentAssembly = typeof(Program).Assembly;
        if (currentAssembly == null)
        {
            currentAssembly = Assembly.GetCallingAssembly();
        }
        var version = $"{currentAssembly.GetName().Version!.Major}.{currentAssembly.GetName().Version!.Minor}.{currentAssembly.GetName().Version!.Build}";
        return version ?? "?.?.?";
    }
}
