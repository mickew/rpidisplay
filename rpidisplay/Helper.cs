using System.Diagnostics;

namespace rpidisplay;

public static class Helper
{
    public static string GetIpAddress()
    {
        var fileName = "/bin/bash"; //linux command i want to execute
        var arguments = "-c \"hostname -I |cut -f 1 -d ' '\"";  //the argument to return ip address
        using Process proc = CreateProcess(fileName, arguments);
        string ss = string.Empty;

        proc.Start();  //start the process
        while (!proc.StandardOutput.EndOfStream)  //wait until entire stream from output read in
        {
            ss = $"{ss}{proc.StandardOutput.ReadLine()}";  //this contains the output                    
        }
        return $"IP: {ss}";
    }

    public static string GetCpuLoad()
    {
        var fileName = "/bin/bash"; //linux command i want to execute
        var arguments = "-c \"top -bn1\"";  //the argument to return cpu load
        using Process proc = CreateProcess(fileName, arguments);
        var list = new List<string>();
        string ss = string.Empty;

        proc.Start();  //start the process
        while (!proc.StandardOutput.EndOfStream)  //wait until entire stream from output read in
        {
            list.Add(proc.StandardOutput.ReadLine()!);
            //ss = $"{ss}{proc.StandardOutput.ReadLine()}";  //this contains the output                    
        }
        ss = list.First().ToString();
        //top - 00:34:19 up  6:20,  1 user,  load average: 0.21, 0.24, 0.23
        var i = ss.LastIndexOf(':');
        ss = ss.Substring(i + 1).Split(',')[0].Trim();
        return $"CPU Load: {ss}";
    }

    public static string GetMemoryUsage()
    {
        var fileName = "/bin/bash"; //linux command i want to execute
        var arguments = "-c \"free -m\" | awk 'NR==2{printf \"Mem: %s/%sMB %.2f%%\", $3,$2,$3*100/$2 }'";  //the argument to return memory usage
        using Process proc = CreateProcess(fileName, arguments);
        var list = new List<string>();
        string ss = string.Empty;

        proc.Start();  //start the process
        while (!proc.StandardOutput.EndOfStream)  //wait until entire stream from output read in
        {
            list.Add(proc.StandardOutput.ReadLine()!);
            //ss = $"{ss}{proc.StandardOutput.ReadLine()}";  //this contains the ip output                    
        }
        var sTotal = list[1].Split("       ")[1].Trim();
        var sUsed = list[1].Split("       ")[2].Trim();
        var percentageused = double.Parse(sUsed) / double.Parse(sTotal) * 100;
        ss = $"Mem: {sUsed}/{sTotal}MB {percentageused.ToString("#.0")}%";
        return ss;
    }

    public static string GetDiskUsage()
    {
        var fileName = "/bin/bash"; //linux command i want to execute
        var arguments = "-c \"df -h\" | awk '$NF==\"/\"{printf \"Disk: %d/%d GB %s\", $3,$2,$5}'";  //the argument to return disk usage
        using Process proc = CreateProcess(fileName, arguments);
        var list = new List<string>();
        string ss = string.Empty;

        proc.Start();  //start the process
        while (!proc.StandardOutput.EndOfStream)  //wait until entire stream from output read in
        {
            list.Add(proc.StandardOutput.ReadLine()!);
            //ss = $"{ss}{proc.StandardOutput.ReadLine()}";  //this contains the ip output                    
        }
        var sZise = list[1].Split("  ")[3].Trim().TrimEnd('G');
        var sUsed = list[1].Split("  ")[4].Trim().TrimEnd('G');
        var sPercentageUsed = list[1].Split("  ")[6].Trim().TrimEnd('/');
        ss = $"Disk: {sUsed}/{sZise}GB {sPercentageUsed}";
        return ss;
    }

    private static Process CreateProcess(string fileName, string arguments)
    {
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,  //redirect output to my code here
                CreateNoWindow = true //do not show a window
            }
        };
        return proc;
    }
}