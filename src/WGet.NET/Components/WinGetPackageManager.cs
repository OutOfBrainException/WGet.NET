﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetPackageManager"/> class offers methods to manage packages with winget.
    /// </summary>
    public class WinGetPackageManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        public WinGetPackageManager()
        {
        }

        //---Search------------------------------------------------------------------------------------
        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name
        /// </summary>
        /// <param name="packageName">The name of the package that you want to search</param>
        /// <returns>
        /// A List of packages that mached the search criteria
        /// </returns>
        public List<WinGetPackage> SearchPackage(string packageName)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.SearchCmd, packageName);

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                using (Process searchProc = new Process() { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    searchProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = searchProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    searchProc.WaitForExit();
                }

                return ToPackageList(output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception)
            {
                return new List<WinGetPackage>();
            }
        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name
        /// </summary>
        /// <param name="packageName">The name of the package that you want to search</param>
        /// <returns>
        /// A Task object of the search task
        /// </returns>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageName)
        {
            return await Task.Run(() => SearchPackage(packageName));
        }
        //---------------------------------------------------------------------------------------------

        //---Install-----------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all installed packages
        /// </summary>
        /// <returns>
        /// A List of packages that are installed on the system
        /// </returns>
        public List<WinGetPackage> GetInstalledPackages()
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = ExecutionInfo.ListCmd;

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                using (Process searchProc = new Process() { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    searchProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = searchProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    searchProc.WaitForExit();
                }

                return ToPackageList(output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages
        /// </summary>
        /// <returns>
        /// A List of packages that are installed on the system
        /// </returns>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync()
        {
            return await Task.Run(() => GetInstalledPackages());
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be installed</param>
        /// <returns>
        /// True if the installation was successfull
        /// </returns>
        public bool InstallPackage(string packageId)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.InstallCmd, packageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process{ StartInfo = ExecutionInfo.WinGetStartInfo }) 
                {
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be installed</param>
        /// <returns>
        /// True if the installation was successfull
        /// </returns>
        public bool InstallPackage(WinGetPackage package)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.InstallCmd, package.PackageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process{ StartInfo = ExecutionInfo.WinGetStartInfo }) 
                { 
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be installed</param>
        /// <returns>
        /// A Task object of the install task
        /// </returns>
        public async Task<bool> InstallPackageAsync(string packageId)
        {
            return await Task.Run(() => InstallPackage(packageId));
        }

        /// <summary>
        /// Insatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be installed</param>
        /// <returns>
        /// A Task object of the install task
        /// </returns>
        public async Task<bool> InstallPackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => InstallPackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Uninstall---------------------------------------------------------------------------------
        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be uninstalled</param>
        /// <returns>
        /// True if the uninstallation was successfull
        /// </returns>
        public bool UninstallPackage(string packageId)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.UninstallCmd, packageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process uninstallProc = new Process{ StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    uninstallProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = uninstallProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    uninstallProc.WaitForExit();
                    exitCode = uninstallProc.ExitCode;
                }

                //Check if uninstallation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be uninstalled</param>
        /// <returns>
        /// True if the uninstallation was successfull
        /// </returns>
        public bool UninstallPackage(WinGetPackage package)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.UninstallCmd, package.PackageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process uninstallProc = new Process{ StartInfo = ExecutionInfo.WinGetStartInfo }) 
                {
                    uninstallProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = uninstallProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    uninstallProc.WaitForExit();
                    exitCode = uninstallProc.ExitCode;
                }

                //Check if uninstallation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be uninstalled</param>
        /// <returns>
        /// A Task object of the uninstall task
        /// </returns>
        public async Task<bool> UninstallPackageAsync(string packageId)
        {
            return await Task.Run(() => UninstallPackage(packageId));
        }

        /// <summary>
        /// Uninsatll a package using winget
        /// </summary>
        /// <param name="package">The package that should be uninstalled</param>
        /// <returns>
        /// A Task object of the uninstall task
        /// </returns>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => UninstallPackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Upgrade-----------------------------------------------------------------------------------
        /// <summary>
        /// Uses the winget upgrade function to get all upgradeable packages
        /// </summary>
        /// <returns>
        /// A List of upgradeable packages
        /// </returns>
        public List<WinGetPackage> GetUpgradeablePackages()
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = ExecutionInfo.GetUpgradeableCmd;

                //Output List
                List<string> output = new List<string>();

                //Create and run process
                using (Process searchProc = new Process { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    searchProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = searchProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    searchProc.WaitForExit();
                }
                return ToPackageList(output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", e);
            }
        }

        /// <summary>
        /// Uses the winget upgrade function to get all upgradeable packages
        /// </summary>
        /// <returns>
        /// A List of upgradeable packages
        /// </returns>
        public async Task<List<WinGetPackage>> GetUpgradeablePackagesAsync()
        {
            return await Task.Run(() => GetUpgradeablePackages());
        }

        /// <summary>
        /// Upgrades a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgradet</param>
        /// <returns>
        /// True if the upgrade was successfull
        /// </returns>
        public bool UpgradePackage(string packageId)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.UpgradeCmd, packageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget
        /// </summary>
        /// <param name="package">The package that should be upgradet</param>
        /// <returns>
        /// True if the upgrade was successfull
        /// </returns>
        public bool UpgradePackage(WinGetPackage package)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.UpgradeCmd, package.PackageId);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget
        /// </summary>
        /// <param name="packageId">The id or name of the package that should be upgradet</param>
        /// <returns>
        /// True if the upgrade was successfull
        /// </returns>
        public async Task<bool> UpgradePackageAsync(string packageId)
        {
            return await Task.Run(() => UpgradePackage(packageId));
        }

        /// <summary>
        /// Upgrades a package using winget
        /// </summary>
        /// <param name="package">The package that should be upgradet</param>
        /// <returns>
        /// True if the upgrade was successfull
        /// </returns>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => UpgradePackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Other------------------------------------------------------------------------------------
        /// <summary>
        /// Exports a list of all installed winget packages as json to the given file
        /// </summary>
        /// <param name="file">The file for the export</param>
        /// <returns>
        /// True if the export was successfull
        /// </returns>
        public bool ExportPackageList(string file)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.ExportCmd, file);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process() { StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", e);
            }
        }

        /// <summary>
        /// Exports a list of all installed winget packages as json to the given file
        /// </summary>
        /// <param name="file">The file for the export</param>
        /// <returns>
        /// True if the export was successfull
        /// </returns>
        public async Task<bool> ExportPackageListAsync(string file)
        {
            return await Task.Run(() => ExportPackageList(file));
        }

        /// <summary>
        /// Imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// This may take some time and winget might not install/upgrade all packages.
        /// </summary>
        /// <param name="file">The file with the package data for the import</param>
        /// <returns>
        /// True if the import was compleatly successfull or False if some or all packages failed to install.
        /// </returns>
        public bool ImportPackages(string file)
        {
            try
            {
                //Set Arguments
                ExecutionInfo.WinGetStartInfo.Arguments = String.Format(ExecutionInfo.ImportCmd, file);

                //Output List
                List<string> output = new List<string>();

                int exitCode = -1;

                //Create and run process
                using (Process installProc = new Process(){ StartInfo = ExecutionInfo.WinGetStartInfo })
                {
                    installProc.Start();

                    //Read output to list
                    using StreamReader procOutputStream = installProc.StandardOutput;
                    while (!procOutputStream.EndOfStream)
                    {
                        output.Add(procOutputStream.ReadLine());
                    }

                    //Wait till end and get exit code
                    installProc.WaitForExit();
                    exitCode = installProc.ExitCode;
                }

                //Check if installation was succsessfull
                if (exitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", e);
            }
        }

        /// <summary>
        /// Imports packages and installes/updates all pakages, if possible.
        /// This may take some time.
        /// </summary>
        /// <param name="file">The file with the package data for the import</param>
        /// <returns>
        /// True if the import was compleatly successfull or False if some or all packages failed to install.
        /// </returns>
        public async Task<bool> ImportPackagesAsync(string file)
        {
            return await Task.Run(() => ImportPackages(file));
        }
        //---------------------------------------------------------------------------------------------

        private List<WinGetPackage> ToPackageList(List<string> output)
        {
            //Get top line index
            int topLineIndex = 0;
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("------"))
                {
                    topLineIndex = i;
                    break;
                }
            }

            //Get start indexes of each tabel colum
            int nameStartIndex = 0;

            int idStartIndex = 0;
            bool idStartIndexSet = false;

            int versionStartIndex = 0;
            bool versionStartIndexSet = false;

            int extraInfoStartIndex = 0;
            bool extraInfoStartIndexSet = false;

            int labelLine = topLineIndex - 1;
            bool checkForChar = false;
            for (int i = 0; i < output[labelLine].Length; i++)
            {
                if (output[labelLine][i] != ' ' && checkForChar)
                {
                    if (!idStartIndexSet)
                    {
                        idStartIndex = i;
                        idStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (!versionStartIndexSet)
                    {
                        versionStartIndex = i;
                        versionStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (!extraInfoStartIndexSet)
                    {
                        extraInfoStartIndex = i;
                        extraInfoStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (idStartIndexSet && versionStartIndexSet && extraInfoStartIndexSet)
                    {
                        //Breake the loop if all indexes are set
                        break;
                    }
                }
                else if (output[labelLine][i] == ' ')
                {
                    checkForChar = true;
                }
            }

            //Remove unneeded output Lines
            output.RemoveRange(0, topLineIndex + 1);

            List<WinGetPackage> resultList = new List<WinGetPackage>();

            foreach (string line in output)
            {
                string name = line
                    .Substring(nameStartIndex, idStartIndex - 1)
                    .Trim();
                string winGetId = line
                    .Substring(idStartIndex, (versionStartIndex - idStartIndex) - 1)
                    .Trim();
                string version = line
                    .Substring(versionStartIndex, (extraInfoStartIndex - versionStartIndex) - 1)
                    .Trim();

                resultList.Add(new WinGetPackage() { PackageName = name, PackageId = winGetId, PackageVersion = version });
            }

            return resultList;
        }
    }
}