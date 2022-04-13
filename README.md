<p align="center">
  <img alt="CertName Logo" src="Images/app.ico" width="100px" />
  <h1 align="center">ShreddersBundleManager</h1>
</p>

## What is ShreddersBundleManager?
Shredders Bundle Manager is a simple tool made in hope that managing the bundles can be done more easily. With this tool, you don't have to rename files to match the target bundle name. This will also automatically backup the original bundle file as you apply the bundle through the Bundle Manager. 

With Shredders Bundle Manager, after you import your custom bundle and choose the target bundle it will be saved to the imported list. And let's say if you want to switch between the custom bundle you've imported that replace the same bundle, you can just restore the old bundle and re-apply the other one. 

No need to worry about backing up, renaming, and copy-paste the bundles.

## Installation Instruction
1. Download the release build Shredders Bundle Manager from the Release page (Coming soon).
2. Extract the files using [WinRAR](https://www.win-rar.com/) or [WinZIP](https://www.winzip.com/).
3. Open the `ShreddersBundleManager.exe`

## Usage Instruction
### Short instruction:

`Import Bundle -> Target Bundle -> Apply Bundle`

### Detailed Instruction:
1. If it's your first time, it will notice you with a warning that the Shredders directory is not defined. Click the Bundle Manager menu on the top left and choose Shredders Folder. Locate where the `Shredders.exe` file. After that, the middle text should shows the directory.
2. Import the bundle by clicking Import Bundle button on the bottom left corner, or by clicking the Bundle Manager menu on the top left and choose Import Bundle.
3. Select the bundle you've just imported and Click Target Bundle. A window will show and you need to choose the bundle you would like to replace. 
4. Select the bundle you want to apply and then click on the Apply Bundle button. The Bundle Manager will create a backup folder and automatically backup and replace the target bundle.
5. Done!

### Extra Steps:
You can also open the UABEA through the Bundle Manager. If it's the first time you open the Bundle Manager:

6. Click the Bundle Manager menu on the top left and choose UABEA Folder.
7. Locate the `UABEAvalonia.exe`
8. Click UABEA button on the top right corner next to the Exit button.

## FAQ
Coming soon!

## Building from source
### 1. Prerequisites
- [Git](https://git-scm.com)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
### 2. Clone or Download this repo. This will create a local copy of the repo.
```ps
git clone https://github.com/bagiosw/ShreddersBundleManager
```
### 3. Build the project
- Open the `ShreddersBundleManager.sln` file.
- Debug the project.

## Screenshots
![ShreddersBundleManager](https://i.imgur.com/GI0VOmP.png)