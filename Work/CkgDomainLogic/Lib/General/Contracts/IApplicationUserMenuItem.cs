﻿using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IApplicationUserMenuItem 
    {
        int AppID { get; set; }

        string AppName { get; set; }

        string AppFriendlyName { get; set; }

        string AppURL { get; set; }

        int AppRank { get; set; }

        bool AppIsMvcFavorite { get; set; }
        string NewLevel { get; set; }

        Dictionary<string, string> BerechtigungsLevel { get; }
        
        #region Menu Group
        
        string AppType { get; set; }

        int AppTypeRank { get; set; }

        string AppTypeFriendlyName { get; set; }

        #endregion
    }
}
