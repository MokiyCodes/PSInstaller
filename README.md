# PSInstaller
A PSTools Installer in C#, for installing [PSTools](https://docs.microsoft.com/en-us/sysinternals/downloads/pstools), or, if you want, the whole [SysInternals Suite](https://docs.microsoft.com/en-us/sysinternals/downloads/sysinternals-suite)

**Made by a 3rd party, not created by, nor endorsed by Microsoft!**

## Usage
To use this to install SysInternals' PSTools, or SysInternals' whole suite, follow these steps:
1. Download and run [this](https://github.com/MokiyCodes/PSInstaller/releases/latest/download/FrameworkDependent-Portable.exe) (`FrameworkDependent-Portable.exe`)
2. If this appears:<br/>![image](https://user-images.githubusercontent.com/92001109/154757832-304c7e67-04e6-46e3-84d6-6366f46ede66.png)<br/>press More Info, then Run Anyway.<br/>This appears because I don't have a code-signing certificate.
3. Follow instructions in the command-line window. Accept any administrator prompts that appear.
4. If for some reason the CMD doesn't appear, use [this](https://github.com/MokiyCodes/PSInstaller/releases/latest/download/SelfContained-x86.exe) executable instead.

## Usage in scripts
### PSTools Install
```batch
C:/Path/To/Installer.exe force accepteula
```
### Suite Install
```batch
C:/Path/To/Installer.exe force accepteula full-suite
```
### Description of parameters
| param | desc |
| ----- | ---- |
| force | Replace any existing installation directories, rather than asking first. Also removes some other prompts. |
| accepteula | Skip EULA prompt, and add registry key to prevent sysinternals from asking for EULA shit every time |
| full-suite | Download full suite from [this repo](https://github.com/Mokiy2/SysInternalsSuite) ([archive](https://codeload.github.com/Mokiy2/SysInternalsSuite/zip/refs/heads/main)) instead of PSTools from [this repo](https://github.com/MokiyCodes/PSTools) ([archive](https://codeload.github.com/MokiyCodes/PSTools/zip/refs/heads/main)) |
