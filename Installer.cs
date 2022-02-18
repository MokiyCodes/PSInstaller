// SysInternals PSTools/Suite Installer
// MokiyCodes
// 18.02.2022

// Build Instructions:
// 1. Create an empty console .NET app in visual studio
// 2. Paste this file into Program.cs
// 3. In the project's properties, set the .NET Version to .NET 6.0
// 4. In the project's properties, set the Target OS to Windows, and Startup Objcect to Installer
// 5. Go to Build, then Build Solution

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Net.Http;
using System.Linq;
using System.IO.Compression;
using System.Net;

class Installer
{
  private static string PSURL = "https://codeload.github.com/MokiyCodes/PSTools/zip/refs/heads/main";
  private static string InstallLocation = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles%"), "PSTools");
  private static string PSZipLocation = Path.Combine(InstallLocation, "PSToolsArchive.zip");
  private static string ZipSubdir = "PSTools-main";
  private static string AppName = "PSTools";
  private static string FullAppName = "PSTools";
  private static void die()
  {
    Process.GetCurrentProcess().Kill();
  }
  public static void Title()
  {
    Console.Clear();
    Console.WriteLine($@"
   {AppName} Installer
        0.1.0
");
  }
  private static void UseFullSysInternalsSuite()
  {
    PSURL = "https://codeload.github.com/Mokiy2/SysInternalsSuite/zip/refs/heads/main";
    InstallLocation = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles%"), "SysinternalsSuite");
    ZipSubdir = "SysInternalsSuite-main";
    AppName = "SysInt.";
    FullAppName = "SysInternals Suite";
    Console.WriteLine("Installing the full SysInternals Suite instead of just PSTools");
  }
  private static string[] args = Array.Empty<string>();
  private static bool EnsureDir()
  {
    if (Directory.Exists(InstallLocation))
    {
      if (!args.Contains("force"))
      {
        Title();
        Console.Title = "Existing Installation Found!";
        Console.Write("Existing installation found at " + InstallLocation + ".\nNot removing this will cause the installation to abort.\nDo you wish to remove this? [y/N] ");
        string? input = Console.ReadLine();
        Console.WriteLine("");
        if (input == null || !input.ToLower().StartsWith("y"))
        {
          Log("Aborting...");
          die();
          return false;
        }
      }
      Log("Removing old installation...");
      Directory.Delete(InstallLocation, true);
    };
    Directory.CreateDirectory(InstallLocation);
    Log("Created Empty Directory: " + InstallLocation, "Created Application Directory");
    return true;
  }
  private static bool Elevate()
  {
    if (IsAdministrator() == false)
    {
      // Restart program and run as admin
      string? exeName = Environment.ProcessPath;
      if (exeName == null) throw new FileNotFoundException("Cannot find own executable.");
      Log("Relaunching as administrator...");
      ProcessStartInfo startInfo = new(exeName);
      startInfo.UseShellExecute = true;
      startInfo.Verb = "runas";
      startInfo.Arguments = "rra " + String.Join(" ", args).ToLower() + (AppName == "SysInt." ? "full-suite" : "");
      Process? proc = Process.Start(startInfo);
      if (proc != null)
      {
        Log("Waiting for administrator process to exit...\nPlease complete installation in other process...", "Waiting for other process");
        proc.WaitForExit();
      }
      die();
      return false;
    }
    return true;
  }
  private static bool IsAdministrator()
  {
    if (System.OperatingSystem.IsOSPlatform("windows"))
    {
      WindowsIdentity identity = WindowsIdentity.GetCurrent();
      WindowsPrincipal principal = new WindowsPrincipal(identity);
      return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    else
    {
      throw new PlatformNotSupportedException("Cannot run on non-windows platforms!");
    }
  }
  public static void Log(string text)
  {
    Console.WriteLine(text);
    Console.Title = "PSTI | " + text;
  }
  public static void Log(string text, string title)
  {
    Console.WriteLine(text);
    Console.Title = "PSTI | " + title;
  }

  // https://stackoverflow.com/a/9053614
  public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
  {
    if (source.FullName.ToLower() == target.FullName.ToLower())
    {
      return;
    }

    // Check if the target directory exists, if not, create it.
    if (!Directory.Exists(target.FullName))
    {
      Directory.CreateDirectory(target.FullName);
    }

    // Copy each file into it's new directory.
    foreach (FileInfo fi in source.GetFiles())
    {
      //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
      fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
    }

    // Copy each subdirectory using recursion.
    foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
    {
      DirectoryInfo nextTargetSubDir =
          target.CreateSubdirectory(diSourceSubDir.Name);
      CopyAll(diSourceSubDir, nextTargetSubDir);
    }
  }
  public static void Run()
  {
    Title();
    if (!args.Contains("force") && !args.Contains("rra"))
    {
      Log("This program is not created by, nor endorsed by Microsoft.\nPressing s will install the whole SysInternals Suite, pressing any other key will only install PSUtils.\nPress any key to start...", "Waiting for key to be pressed...");
      if (Console.ReadKey().Key.ToString().ToLower() == "s") UseFullSysInternalsSuite();
    };
    if (!OperatingSystem.IsOSPlatform("windows"))
    {
      Log("PSTI (PowerShell Tools Installer) only works on Windows!\nPlease use your platform's alternatives to PSTools instead.", "Unsupported OS");
      return;
    }
    if (!OperatingSystem.IsOSPlatformVersionAtLeast("Windows", 4))
    {
      throw new PlatformNotSupportedException("Please update your windows version!");
    }
    if (!OperatingSystem.IsOSPlatformVersionAtLeast("Windows", 6, 1))
    {
      Console.Error.WriteLine("[WARN] Not running Windows 7 or above! Bugs are almost guaranteed to occur.");
    }
    Title();
    Log("Checking Pemrissions and elevating if necessary...", "Checking Permissions");
    if (Elevate())
    {
      Log("Running at correct permission level");
      EnsureDir(); // Ensure directory exists and is empty
      Log($"Downloading {FullAppName} from " + PSURL, $"Downloading {FullAppName}");

      /*
      HttpResponseMessage downloadResponse = await httpClient.GetAsync(PSURL); // "https://via.placeholder.com/300.png"
      Log("Writing archive to " + PSZipLocation, "Preparing PSTools for installation");
      FileStream zipStream = new(PSZipLocation, FileMode.CreateNew);
      await downloadResponse.Content.ReadAsStream().CopyToAsync(zipStream);
      */
#pragma warning disable SYSLIB0014 // Type or member is obsolete
      new WebClient().DownloadFile(PSURL, PSZipLocation);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

      Log($"Extracting {FullAppName}");
      ZipFile.ExtractToDirectory(PSZipLocation, InstallLocation);
      Log("Copying Files to Installation Directory", "Copying Files");
      CopyAll(new DirectoryInfo(Path.Combine(InstallLocation, ZipSubdir)), new DirectoryInfo(InstallLocation));

      File.Delete(Path.Combine(InstallLocation, "bin", "README.md"));
      File.WriteAllText(Path.Combine(InstallLocation, "README.txt"), $@"
/// {FullAppName} | Installed from {PSURL} by PSTI (PowerShell Tools Installer) ///
By using {FullAppName} you agree to the Windows Sysinternals EULA. It can be found in bin/EULA.txt or at https://docs.microsoft.com/en-us/sysinternals/license-terms");

      if (!Directory.Exists(Path.Combine(InstallLocation, "bin"))) throw new Exception("Cannot find binary files in extracted archive");

      Log("Adding to PATH");
      string EnvVariableName = "PATH";
      EnvironmentVariableTarget scope = EnvironmentVariableTarget.Machine; // or User
      string? oldValue = Environment.GetEnvironmentVariable(EnvVariableName, scope);
      string? newValue = oldValue + @";" + Path.Combine(InstallLocation, "bin");
      Environment.SetEnvironmentVariable(EnvVariableName, newValue, scope);

      Log("Cleaning Up");
      Directory.Delete(Path.Combine(InstallLocation, ZipSubdir), true);
      File.Delete(PSZipLocation);

      if (!args.Contains("accepteula"))
      {
        Title();
        Log($@"{FullAppName} is now installed! However, we need to ask you one thing...

By using {FullAppName}, you agree to the SysInternals EULA found at https://docs.microsoft.com/en-us/sysinternals/license-terms
By default, Microsoft asks you to accept these every time you run any SysInternals application." + @"
If you accept the SysInternals EULA and press Y here, we will remove said prompt.", "EULA Notice");
        Console.Write("Do you agree to the SysInternals EULA (End-users License Agreement)? [y/N] ");
        ConsoleKeyInfo? Input = Console.ReadKey();
        if (Input == null || Input.Value.Key.ToString().ToLower() != "y") return;
      };
      Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Sysinternals");
      key.SetValue("EulaAccepted", 1);
      Log(@"
Finished Installation!

~ Mokiy#0001", "Done!");
    }
  }
  public static void Main(string[] _args)
  {
    args = _args;
    if (string.Join(' ', args).ToLower().Contains("full-suite")) UseFullSysInternalsSuite();
    Run();
  }
}
