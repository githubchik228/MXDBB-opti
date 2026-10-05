using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace MXDBBOpti
{
    class MonitorSnapshot
    {
        public int CpuPercent, RamUsedGb, RamTotalGb, DiskFreeGb, PingMs;
        public string GpuName, GpuVram, Network, TopProcesses;
    }

    static class Advanced
    {
        static PerformanceCounter cpu;
        static bool cpuReady;
        static string savedGameKey = @"Software\MXDBB Opti";

        public static string GetSavedGamePath()
        {
            try { using(var k=Registry.CurrentUser.OpenSubKey(savedGameKey)) return k==null?"":(k.GetValue("GamePath") as string ?? ""); } catch { return ""; }
        }
        public static void SaveGamePath(string path)
        { try { using(var k=Registry.CurrentUser.CreateSubKey(savedGameKey)) k.SetValue("GamePath",path??"",RegistryValueKind.String); } catch {} }

        static int CpuLoad()
        {
            try { if(!cpuReady){cpu=new PerformanceCounter("Processor","% Processor Time","_Total",true);cpu.NextValue();cpuReady=true;return 0;}return Math.Max(0,Math.Min(100,(int)Math.Round(cpu.NextValue()))); } catch{return 0;}
        }
        static int RamUsed(out int total)
        {
            total=(int)Math.Max(1,Math.Round(Sys.TotalRamMb()/1024.0));
            try{var psi=new ProcessStartInfo("wmic","OS get FreePhysicalMemory,TotalVisibleMemorySize /Value"){UseShellExecute=false,CreateNoWindow=true,RedirectStandardOutput=true};using(var p=Process.Start(psi)){string o=p.StandardOutput.ReadToEnd();p.WaitForExit();long free=0,t=0;foreach(var line in o.Split(new[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries)){var z=line.Split('=');if(z.Length==2){if(z[0].IndexOf("Free",StringComparison.OrdinalIgnoreCase)>=0)long.TryParse(z[1],out free);if(z[0].IndexOf("Total",StringComparison.OrdinalIgnoreCase)>=0)long.TryParse(z[1],out t);}}if(t>0)return (int)Math.Round((t-free)/1024.0/1024.0);}}catch{}return 0;
        }
        static string Nvidia(out string vram)
        {
            vram="—";
            try{string o=Sys.RunOut("nvidia-smi","--query-gpu=name,memory.used,memory.total,temperature.gpu --format=csv,noheader,nounits");if(!string.IsNullOrWhiteSpace(o)){var p=o.Trim().Split(',');if(p.Length>=4){vram=p[1].Trim()+" / "+p[2].Trim()+" MB";return p[0].Trim()+" · "+p[3].Trim()+" °C";}}}catch{}
            try{using(var q=new ManagementObjectSearcher("select Name from Win32_VideoController")){foreach(ManagementObject m in q.Get())return (m["Name"] as string)??"GPU";}}catch{}
            return "GPU не определён";
        }
        static string Net()
        {try{var n=NetworkInterface.GetAllNetworkInterfaces().Where(x=>x.OperationalStatus==OperationalStatus.Up&&x.NetworkInterfaceType!=NetworkInterfaceType.Loopback).OrderByDescending(x=>x.Speed).FirstOrDefault();return n==null?"нет сети":n.Name+" · "+(n.Speed/1000000)+" Mbps";}catch{return "нет сети";}}
        public static int Ping(string host){try{using(var p=new Ping()){var r=p.Send(host,1200);return r.Status==IPStatus.Success?(int)r.RoundtripTime:-1;}}catch{return -1;}}
        static string Top(){try{return string.Join(" · ",Process.GetProcesses().OrderByDescending(p=>{try{return p.WorkingSet64;}catch{return 0L;}}).Take(4).Select(p=>{try{return p.ProcessName+" "+(p.WorkingSet64/1024/1024)+"MB";}catch{return p.ProcessName;}}));}catch{return "—";}}
        public static MonitorSnapshot Snapshot(){int total;int used=RamUsed(out total);string vr;string gpu=Nvidia(out vr);int disk=0;try{var d=new DriveInfo("C");disk=(int)(d.AvailableFreeSpace/1073741824);}catch{}return new MonitorSnapshot{CpuPercent=CpuLoad(),RamUsedGb=used,RamTotalGb=total,GpuName=gpu,GpuVram=vr,DiskFreeGb=disk,PingMs=Ping("1.1.1.1"),Network=Net(),TopProcesses=Top()};}
        public static void LaunchGame(string exe,Action<string> log)
        {
            if(string.IsNullOrEmpty(exe)||!File.Exists(exe))throw new FileNotFoundException("Игра не найдена",exe);
            try{Sys.Run("powercfg","-setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",log);}catch{}
            var psi=new ProcessStartInfo(exe){WorkingDirectory=Path.GetDirectoryName(exe),UseShellExecute=true};
            var p=Process.Start(psi);if(p==null)throw new InvalidOperationException("Windows не запустила процесс");
            try{p.PriorityClass=ProcessPriorityClass.AboveNormal;}catch{}
            log("    Game Boost: "+p.ProcessName+" → AboveNormal");
        }
    }
}
