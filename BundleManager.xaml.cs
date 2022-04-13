using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using Bundles.Models;
using AppliedBundles.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace ShreddersBundleManager
{
    /// <summary>
    /// Interaction logic for BundleManager.xaml
    /// </summary>
    public partial class BundleManager : Window
    {
        public string shreddersDir = Properties.Settings.Default.GameDir;
        public string uabeaDir = Properties.Settings.Default.UabeaDir;
        public string appVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        ObservableCollection<Bundle> observableImported = new ObservableCollection<Bundle>(); //use this as itemssource
        ObservableCollection<AppliedBundle> observableApplied = new ObservableCollection<AppliedBundle>();


        public BundleManager()
        {
            InitializeComponent();
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ShreddersBundleManager launched. Application version " + appVersion);
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Loading configuration..");
            ReadImportedJson();
            ReadAppliedJson();
            if (Properties.Settings.Default.GameDir != string.Empty)
            {
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Game directory defined.");
                ImportButtonMenu.IsEnabled = true;
                ImportButton.IsEnabled = true;
                ApplyButton.IsEnabled = true;
                DeleteAppliedButton.IsEnabled = true;
                ShredderDirText.Text = "Shredders Dir : " + shreddersDir;
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Shredders game found on path " + shreddersDir);
            }
            else
            {
                MessageBox.Show("Shredders directory not defined. Please define it from \n'Bundle Manager -> Shredders Folder'", "Game Directory Not Defined", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Game directory not defined.");
            }
        }

        public void ReadImportedJson()
        {
            string datafolder = @".\Data\";
            string filename = @"ImportedBundles.json";
            if (File.Exists(datafolder + filename))
            {
                string json = File.ReadAllText(datafolder + filename);
                observableImported = JsonConvert.DeserializeObject<ObservableCollection<Bundle>>(json);
                LoadObsToList(); //load to list
                Debug.WriteLine("Imported JSON loaded.");
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Imported bundle JSON loaded from " + datafolder + filename);
            }
            else
            {
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Imported bundle JSON not loaded.");
            }
        }

        public void ReadAppliedJson()
        {
            string datafolder = @".\Data\";
            string filename = @"AppliedBundles.json";
            if (File.Exists(datafolder + filename))
            {
                string json = File.ReadAllText(datafolder + filename);
                observableApplied = JsonConvert.DeserializeObject<ObservableCollection<AppliedBundle>>(json);
                LoadObsToList();
                Debug.WriteLine("Applied JSON loaded.");
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Applied bundle JSON loaded from" + datafolder + filename);
            }
            else
            {
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Applied bundle JSON not loaded.");
            }
        }

        public void LoadObsToList()
        {
            ImportedList.ItemsSource = observableImported;
            AppliedList.ItemsSource = observableApplied;
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | JSON loaded to the lists.");
        }

        public void WriteImportedJsonLine()
        {
            string datafolder = @".\Data\";
            string filename = @"ImportedBundles.json";
            System.Diagnostics.Debug.WriteLine("json not exist");
            using var tw = new StreamWriter(datafolder + filename, true);
            System.Diagnostics.Debug.WriteLine("json created");
            tw.WriteLine("[");
            foreach (var item in ImportedList.Items)
            {
                string JSONres = JsonConvert.SerializeObject(item);
                tw.WriteLine(JSONres.ToString() + ",");
                Debug.WriteLine(JSONres.ToString());
            }
            tw.WriteLine("]");
            tw.Close();
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ImportedBundles JSON updated.");
        }

        public void WriteAppliedJsonLine()
        {
            string datafolder = @".\Data\";
            string filename = @"AppliedBundles.json";
            using var tw = new StreamWriter(datafolder + filename, true);
            tw.WriteLine("[");
            foreach (var item in AppliedList.Items)
            {
                string JSONres = JsonConvert.SerializeObject(item);
                tw.WriteLine(JSONres.ToString() + ",");
            }
            tw.WriteLine("]");
            tw.Close();
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | AppliedBundles JSON updated.");
        }

        public void WriteImportedJson()
        {
            string datafolder = @".\Data\";
            string filename = @"ImportedBundles.json";

            if (!Directory.Exists(datafolder))
            {
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Data folder not found, creating the folder.");
                Debug.WriteLine("data not exist");
                Directory.CreateDirectory(datafolder);
                Debug.WriteLine("data created");
                if (!File.Exists(filename))
                {
                    WriteImportedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ImportedBundles created for the first time.");
                }
                else
                {
                    File.Delete(datafolder + filename);
                    WriteImportedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ImportedBundles replaced.");
                }
            }
            else
            {
                if (!File.Exists(datafolder + filename))
                {
                    WriteImportedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ImportedBundles created for the first time.");
                }
                else
                {
                    File.Delete(datafolder + filename);
                    WriteImportedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ImportedBundles replaced.");
                }
            }
        }

        public void WriteAppliedJson()
        {
            string datafolder = @".\Data\";
            string filename = @"AppliedBundles.json";

            if (!Directory.Exists(datafolder))
            {
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Data folder not found, creating the folder.");
                Debug.WriteLine("data not exist");
                Directory.CreateDirectory(datafolder);
                Debug.WriteLine("data created");
                if (!File.Exists(filename))
                {
                    WriteAppliedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | AppliedBundles created for the first time.");
                }
                else
                {
                    File.Delete(datafolder + filename);
                    WriteAppliedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | AppliedBundles replaced.");
                }
            }
            else
            {
                if (!File.Exists(datafolder + filename))
                {
                    WriteAppliedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | AppliedBundles created for the first time.");
                }
                else
                {
                    File.Delete(datafolder + filename);
                    WriteAppliedJsonLine();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | AppliedBundles replaced.");
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = ".bundle";
            dialog.Filter = "Bundle |*.bundle";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.SafeFileName;
                string filepath = dialog.FileName;
                Debug.WriteLine(filename);
                if (!ImportedList.Items.Cast<Bundle>().Any(b => b.Name == filename))
                {
                    observableImported.Add(new Bundle { Name = filename, Directory = filepath, Target = "no-target", TargetFile = "no-target" });
                    LoadObsToList();
                    Debug.WriteLine("obs : " + observableImported.Count);
                    Debug.WriteLine("list : " + ImportedList.Items.Count);
                    WriteImportedJson();
                    MessageBox.Show("The " + filename + " has been imported. Don't forget to select the target bundle.", "Import Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | New bundle imported from" + filepath);
                }
                else
                {
                    MessageBox.Show("Failed to import " + filename + ". \nDuplicated file name on the list. Please choose a different file, or rename it to a different file name if it's a different bunlde", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Imported bundle has the same name. Import cancelled. Filename: " + filename);
                }

            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImportedList.SelectedItem != null)
            {
                observableImported.RemoveAt(ImportedList.Items.IndexOf(ImportedList.SelectedItem));
                Debug.WriteLine("DELETED!");
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | A bundle has been removed from the list.");
            }
            else
            {
                MessageBox.Show("No bundle selected.", "Error Deleting", MessageBoxButton.OK, MessageBoxImage.Error);
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Unable to delete unselected bundle.");
                Debug.WriteLine("implist : " + ImportedList.Items.Count);
                Debug.WriteLine("impobs : " + observableImported.Count);
                Debug.WriteLine("apllist : " + AppliedList.Items.Count);
                Debug.WriteLine("aplobs : " + observableApplied.Count);
            }
        }
        private void TargetButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImportedList.SelectedItem != null)
            {
                Bundle SelectedItem = (Bundle)ImportedList.SelectedItem;
                string selectedName = SelectedItem.Name;
                string selectedDirectory = SelectedItem.Directory;
                int selectedIndex = ImportedList.Items.IndexOf(SelectedItem);
                Debug.WriteLine(selectedName);
                Debug.WriteLine(selectedDirectory);
                Debug.WriteLine(selectedIndex);

                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.FileName = "";
                dialog.DefaultExt = ".bundle";
                dialog.Filter = "Bundle |*.bundle";

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // Open document
                    string filename = dialog.SafeFileName;
                    string filepath = dialog.FileName;
                    Debug.WriteLine(selectedIndex);
                    observableImported.RemoveAt(ImportedList.Items.IndexOf(ImportedList.SelectedItem));
                    observableImported.Insert(selectedIndex, new Bundle { Name = selectedName, Directory = selectedDirectory, Target = filepath, TargetFile = filename });
                    MessageBox.Show("Target selected.", "Targetting Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | A bundle updated with new target.");
                    WriteImportedJson();
                }
            }
            else
            {
                MessageBox.Show("No bundle selected. Please select a bundle above and then clik 'Target Bundle' again", "Error Targetting", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImportedList.SelectedItem != null)
            {
                Bundle SelectedItem = (Bundle)ImportedList.SelectedItem;
                string selectedTarget = SelectedItem.Target;
                string selectedName = Path.GetFileName(selectedTarget);
                string backupFolder = shreddersDir + "\\Shredders_Data\\StreamingAssets\\aa\\StandaloneWindows64\\backup\\";
                Debug.WriteLine("name : " + selectedName);
                Debug.WriteLine("target: " + selectedTarget);
                if (selectedTarget == "no-target")
                {
                    MessageBox.Show("The selected bundle has no target. Please select the correct target for the bundle.\nSelect bundle from the list above -> Target Bundle -> Browse", "Apply Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Unable to apply unselected Bundle.");
                }
                else
                {
                    if (!AppliedList.Items.Cast<AppliedBundle>().Any(b => b.Target == selectedTarget))
                    {
                        Debug.WriteLine("not exist");
                        if (!Directory.Exists(backupFolder))
                        {
                            CreateBackupFolder();
                            Debug.WriteLine("backup folder created!");
                            if (!File.Exists(backupFolder + selectedName)) //if target file not exists in backup folder
                            {
                                Debug.WriteLine("backup file not exist!");
                                Backup();
                                ApplyBundle();
                            }
                            else if (File.Exists(backupFolder + selectedName))
                            {
                                Debug.WriteLine("backup file exist!");
                                ApplyBundle();
                            }
                        }
                        else
                        {
                            Debug.WriteLine("backup folder existed!");
                            if (!File.Exists(backupFolder + selectedName)) //if target file not exists in backup folder
                            {
                                Debug.WriteLine("backup file not exist!");
                                Backup();
                                ApplyBundle();
                            }
                            else
                            {
                                Debug.WriteLine("backup file exist!");
                                ApplyBundle();
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("exist");
                        MessageBox.Show("The selected target is already modified. Please 'Restore Applied Bundle' on already applied bundle and then re-apply the new bundle.", "Apply Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Unable to apply new bundle to modified one.");
                    }
                }
            }
            else
            {
                MessageBox.Show("No file selected.", "Apply Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Unable to apply unselected bundle.");
            }
        }

        private void DeleteAppliedButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppliedList.SelectedItem != null)
            {
                RestoreBundle();
            }
            else
            {
                MessageBox.Show("No bundle selected.", "Error Restoring", MessageBoxButton.OK, MessageBoxImage.Error);
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | ERROR | Unable to restore unselected bundle.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Delay(1000).Wait();
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ShreddersBundleManager Exited.\n.\n.\n.");
            Application.Current.Shutdown();
        }

        private void MenuImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = ".bundle";
            dialog.Filter = "Bundle |*.bundle";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Open document
                string filename = dialog.SafeFileName;
                string filepath = dialog.FileName;
                ImportedList.Items.Add(new Bundle { Name = filename, Directory = filepath, Target = "no-target" });
                MessageBox.Show("Import success, please specify the target bundle before applying the bundle", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MenuFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please select your Shredders.exe", "Game Folder", MessageBoxButton.OK, MessageBoxImage.Information);
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Shredders.exe";
            dialog.DefaultExt = ".exe";
            dialog.Filter = "Application |*.exe";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string shreddersApp = dialog.FileName;
                string shreddersDirTemp = shreddersApp.Substring(0, shreddersApp.LastIndexOf('\\'));
                string modRoot = shreddersDirTemp + "\\Shredders_Data\\StreamingAssets\\aa\\StandaloneWindows64";
                if (Directory.Exists(modRoot))
                {
                    shreddersDir = shreddersDirTemp;
                    ShredderDirText.Text = "Shredders Dir : " + shreddersDir;
                    ImportButton.IsEnabled = true;
                    ApplyButton.IsEnabled = true;
                    DeleteAppliedButton.IsEnabled = true;
                    Properties.Settings.Default["GameDir"] = shreddersDir;
                    Properties.Settings.Default.Save();
                    AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Shredders folder defined on " + shreddersDir);
                    MessageBox.Show("Game folder located. Happy modding!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Wrong directory, please choose proper directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ShredderDirText.Text = "Shredders directory not defined. Please define it from 'Bundle Manager -> Shredders Folder'";
                }
            }
        }

        private void UabeaFolder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please select your UABEAvalonia.exe", "UABEA Folder", MessageBoxButton.OK, MessageBoxImage.Information);
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "UABEAvalonia.exe";
            dialog.DefaultExt = ".exe";
            dialog.Filter = "Application |*.exe";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string uabeaDir = dialog.FileName;
                uabeaDir = uabeaDir.Replace(@"\", "\\\\");
                Properties.Settings.Default["UabeaDir"] = uabeaDir;
                Properties.Settings.Default.Save();
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | UABEA folder defined on " + shreddersDir);
                MessageBox.Show("UABEA folder located. Happy modding!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | ShreddersBundleManager Exited.\n.\n.\n.");
            Task.Delay(1000).Wait();
            Application.Current.Shutdown();
        }

        public void CreateBackupFolder()
        {
            string backupFolder = shreddersDir + "\\Shredders_Data\\StreamingAssets\\aa\\StandaloneWindows64\\backup";
            Directory.CreateDirectory(backupFolder);
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Backup folder not found. Creating the folder on: " + backupFolder);
        }

        public void Backup()
        {
            Bundle SelectedItem = (Bundle)ImportedList.SelectedItem;
            string selectedTarget = SelectedItem.Target;
            string selectedName = Path.GetFileName(selectedTarget);
            string backuppath = shreddersDir + "\\Shredders_Data\\StreamingAssets\\aa\\StandaloneWindows64\\backup\\" + selectedName;
            Console.WriteLine("backup file not exists. backing up file..");
            File.Move(selectedTarget, backuppath);
            Console.WriteLine("File has been backed up");
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Bundle backup success.");
        }
        public void ApplyBundle()
        {
            Bundle SelectedItem = (Bundle)ImportedList.SelectedItem;
            string selectedTarget = SelectedItem.Target;
            string selectedBundle = SelectedItem.Name;
            string selectedName = Path.GetFileName(selectedTarget);
            string selectedDirectory = SelectedItem.Directory;
            string selectedTargetName = SelectedItem.TargetFile;
            File.Delete(selectedTarget);
            File.Copy(selectedDirectory, selectedTarget);
            Debug.WriteLine("Applying!");
            Debug.WriteLine(selectedTarget);
            observableApplied.Add(new AppliedBundle { Name = selectedBundle, Target = selectedTarget, TargetFile = selectedTargetName });
            LoadObsToList();
            WriteAppliedJson();
            MessageBox.Show(selectedName + " successfully applied. A backup file has been created in 'backup' folder", "Apply successfull", MessageBoxButton.OK, MessageBoxImage.Information);
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Bundle applied.");
        }

        public void RestoreBundle()
        {
            AppliedBundle SelectedItem = (AppliedBundle)AppliedList.SelectedItem;
            string appliedBundle = SelectedItem.Target;
            //string selectedTarget = System.IO.Path.GetFileName(appliedBundle);
            //string selectedTarget = SelectedItem.TargetFile;
            string selectedBackup = shreddersDir + "\\Shredders_Data\\StreamingAssets\\aa\\StandaloneWindows64\\backup\\" + SelectedItem.TargetFile;
            File.Delete(appliedBundle);
            File.Copy(selectedBackup, appliedBundle);
            Debug.WriteLine("File has been restored");
            //AppliedList.Items.RemoveAt(AppliedList.Items.IndexOf(AppliedList.SelectedItem));
            observableApplied.RemoveAt(AppliedList.Items.IndexOf(AppliedList.SelectedItem));
            LoadObsToList();
            WriteAppliedJson();
            MessageBox.Show("Bundle has been restored to original file.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | Bundle restored.");
        }

        private void CustomDraggable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void UabeaButton_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.UabeaDir == string.Empty)
            {
                MessageBox.Show("UABEA directory not defined. Please define it from \n'Bundle Manager -> UABEA Folder'", "UABEA Directory Not Defined", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | WARNING | Unable to launch UABEA with undefined directory.");
            }
            else
            {
                LaunchUabea();
            }
        }

        private void LaunchUabea()
        {
            Process.Start(uabeaDir);
            AppendLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " | INFO | UABEA launched.");
            Debug.WriteLine("uabeaDir = " + uabeaDir);
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }
        public void AppendLog(string log)
        {
            string logFolder = @".\Logs\";
            string logFormat = DateTime.Now.ToString("dd-MM-yyyy");
            string logName = "ShreddersBundleManager Log" + logFormat + ".log";

            if (Directory.Exists(logFolder))
            {
                using StreamWriter file = new(logFolder + logName, append: true);
                Debug.WriteLine("log added");
                file.WriteLineAsync(log);
            }
            else
            {
                Directory.CreateDirectory(logFolder);
                using StreamWriter file = new(logFolder + logName, append: true);
                Debug.WriteLine("log added");
                file.WriteLineAsync(log);
            }
        }
    }
}
