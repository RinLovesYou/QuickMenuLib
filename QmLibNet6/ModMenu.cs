using QuickMenuLib.UI;
using QuickMenuLib.UI.Elements;
using UnityEngine;
using VRC.Core;
using VRC.DataModel;

namespace QuickMenuLib
{
    /// <summary>
    /// The ModMenu Class.
    /// </summary>
    public class ModMenu
    {
        /// <summary>
        /// The name of your Menu. This has to be changed.
        /// </summary>
        public virtual string MenuName => "Default Menu";

        public Sprite Logo = null;

        public QuickMenuPage MyModMenu = null;
        public QuickMenuWingMenu MyLeftWingMenu = null;
        public QuickMenuWingMenu MyRightWingMenu = null;
        public QuickMenuPage MyTargetMenu = null;

        private IUser _lastSelectedUser;

        /// <summary>
        /// Gets the currently selected user. Caches last selected user. Returns null if the menu is hidden and there is no cache.
        /// </summary>
        public IUser SelectedUser
        {
            get
            {
                _lastSelectedUser = QuickMenuExtensions.SelectedUserMenu.field_Private_IUser_0;

                return _lastSelectedUser;
            }
        }

        public virtual void OnQuickMenuInitialized() { }
        public virtual void OnWingMenuLeftInitialized() { }
        public virtual void OnWingMenuRightInitialized() { }
        public virtual void OnTargetMenuInitialized() { }
        
    }
}
