﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WGetNET.HelperClasses;

namespace WGetNET
{
    /// <summary>
    /// Represents WinGet related information.
    /// </summary>
    public class WinGetInfo: IWinGetObject
    {
        /// <summary>
        /// Gets the version number of the winget installation as a <see cref="System.String"/>.
        /// </summary>
        public string VersionString
        {
            get
            {
                return _versionString;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        public Version Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Gets a collection of the winget direcories.
        /// </summary>
        public ReadOnlyCollection<WinGetInfoEntry> Directories
        {
            get
            {
                return _directories;
            }
        }

        /// <summary>
        /// Gets a collection of the winget related links.
        /// </summary>
        public ReadOnlyCollection<WinGetInfoEntry> Links
        {
            get
            {
                return _links;
            }
        }

        /// <summary>
        /// Gets a collection of the winget admin setting states.
        /// </summary>
        public ReadOnlyCollection<WinGetAdminOption> AdminSetting
        {
            get
            {
                return _adminSetting;
            }
        }

        /// <summary>
        /// Gets if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_versionString) &&
                    (_directories == null || _directories.Count <= 0) &&
                    (_links == null || _links.Count <= 0) &&
                    (_adminSetting == null || _adminSetting.Count <= 0))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets an empty instance of this object.
        /// </summary>
        internal static WinGetInfo Empty
        {
            get
            {
                return new WinGetInfo("", new List<WinGetInfoEntry>(), new List<WinGetInfoEntry>(), new List<WinGetAdminOption>());
            }
        }

        private readonly string _versionString;
        private readonly Version _version;
        private readonly ReadOnlyCollection<WinGetInfoEntry> _directories;
        private readonly ReadOnlyCollection<WinGetInfoEntry> _links;
        private readonly ReadOnlyCollection<WinGetAdminOption> _adminSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        /// <param name="version">The installed winget version.</param>
        /// <param name="directories">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet directories.
        /// </param>
        /// <param name="links">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing WinGet related links.
        /// </param>
        /// <param name="adminSetting">
        /// <see cref="System.Collections.Generic.List{T}"/> of info entries containing the WinGet admin setting states.
        /// </param>
        internal WinGetInfo(string version, List<WinGetInfoEntry> directories, List<WinGetInfoEntry> links, List<WinGetAdminOption> adminSetting)
        {
            _versionString = version;
            _version = VersionParser.Parse(version);
            _directories = new ReadOnlyCollection<WinGetInfoEntry>(directories);
            _links = new ReadOnlyCollection<WinGetInfoEntry>(links);
            _adminSetting = new ReadOnlyCollection<WinGetAdminOption>(adminSetting);
        }
    }
}
