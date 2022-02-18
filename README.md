# PSInstaller
A PSTools Installer in C#, for installing [PSTools](https://docs.microsoft.com/en-us/sysinternals/downloads/pstools), or, if you want, the whole [SysInternals Suite](https://docs.microsoft.com/en-us/sysinternals/downloads/sysinternals-suite)

**Made by a 3rd party, not created by, nor endorsed by Microsoft!**

## Installation

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
