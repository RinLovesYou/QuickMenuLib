using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;
using NewQuickMenu = VRC.UI.Elements.QuickMenu;

namespace QuickMenuLib.UI
{
    public static class QuickMenuExtensions
    {
        private static NewQuickMenu _quickMenuInstance;

        public static NewQuickMenu GetQuickMenu
        {
            get
            {
                if(_quickMenuInstance == null)
                {
                    _quickMenuInstance = Utils.FindInactive("UserInterface/Canvas_QuickMenu(Clone)").GetComponent<NewQuickMenu>();
                }
                return _quickMenuInstance;
            }
        }
        public static MenuStateController MenuStateController => GetQuickMenu.prop_MenuStateController_0;

        private static SelectedUserMenuQM _selectedUserMenuQm;

        public static SelectedUserMenuQM SelectedUserMenu
        {
            get
            {
                if(_selectedUserMenuQm == null)
                {
                    _selectedUserMenuQm = GetQuickMenu.field_Public_Transform_0.Find("Window/QMParent/Menu_SelectedUser_Local").GetComponent<SelectedUserMenuQM>();
                }

                return _selectedUserMenuQm;
            }
        }

        private static Wing[] _wings;
        private static Wing _leftWing;
        private static Wing _rightWing;

        public static Wing[] Wings
        {
            get
            {
                if (_wings == null || _wings.Length == 0)
                {
                    _wings = GameObject.Find("UserInterface").GetComponentsInChildren<Wing>(true);
                }

                return _wings;
            }
        }

        public static Wing LeftWing
        {
            get
            {
                _leftWing = Wings.FirstOrDefault(w => w.field_Public_WingPanel_0 == Wing.WingPanel.Left);
                return _leftWing;
            }
        }

        public static Wing RightWing
        {
            get
            {
                _rightWing = Wings.FirstOrDefault(w => w.field_Public_WingPanel_0 == Wing.WingPanel.Right);
                return _rightWing;
            }
        }
    }

    internal class QuickMenuTemplates
    {
        private static GameObject _pageButtonReference;
        private static GameObject _headerReference;
        private static GameObject _buttonRowReference;
        private static GameObject _singleButtonReference;
        private static GameObject _toggleButtonReference;
        private static GameObject _menuReference;
        private static GameObject _modalReference;
        private static GameObject _wingMenuReference;
        private static GameObject _wingButtonReference;
        private static Sprite _toggleOnReference;
        private static Sprite _toggleOffReference;
        private static GameObject _sliderReference;
        private static GameObject _foldoutHeaderReference;
        private static GameObject _foldoutContainerReference;
        private static GameObject _infoPanelReference;
        private static GameObject _imageButtonReference;

        internal static GameObject GetSliderPrefab()
        {
            if (_sliderReference == null)
            {
                _sliderReference =
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master");
            }
            return _sliderReference;
        }

        internal static GameObject GetWingMenuTemplate()
        {
            if (_wingMenuReference == null)
            {
                _wingMenuReference = QuickMenuExtensions.LeftWing.field_Public_RectTransform_0.Find("WingMenu").gameObject;
            }
            return _wingMenuReference;
        }

        internal static GameObject GetWingButtonTemplate()
        {
            if (_wingButtonReference == null)
            {
                _wingButtonReference = QuickMenuExtensions.LeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile").gameObject;
            }
            return _wingButtonReference;
        }

        internal static GameObject GetPageButtonTemplate()
        {
            if (_pageButtonReference == null)
            {
                _pageButtonReference =
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings");
            }

            return _pageButtonReference;
        }

        internal static GameObject GetHeaderTemplate()
        {
            if (_headerReference == null)
            {
                _headerReference =
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions");
            }

            return _headerReference;
        }
        
        internal static GameObject GetFoldoutHeaderTemplate()
        {
            if (_foldoutHeaderReference == null)
            {
                _foldoutHeaderReference =
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/QM_Foldout_WorldActions");
            }

            return _foldoutHeaderReference;
        }

        internal static GameObject GetButtonRowTemplate()
        {
            if (_buttonRowReference == null)
            {
                _buttonRowReference = 
                    Utils.FindInactive(
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions");
            }

            return _buttonRowReference;
        }
        
        internal static GameObject GetFoldoutContainerTemplate()
        {
            if (_foldoutContainerReference == null)
            {
                _foldoutContainerReference = 
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_Debug");
            }

            return _foldoutContainerReference;
        }

        internal static GameObject GetSingleButtonTemplate()
        {
            if (_singleButtonReference == null)
            {
                _singleButtonReference = Utils.FindInactive(
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn");
            }

            return _singleButtonReference;
        }

        internal static GameObject GetImageButtonTemplate()
        {
            if (_imageButtonReference == null)
            {
                _imageButtonReference = Utils.FindInactive(
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/QM_Grid_UsersInWorld/Cell_QM_User(Clone)");
            }

            return _imageButtonReference;
            
        }

        internal static GameObject GetToggleButtonTemplate()
        {
            if (_toggleButtonReference == null)
            {
                _toggleButtonReference =
                    Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo");
            }

            return _toggleButtonReference;
        }

        internal static Sprite GetToggleOnIconTemplate()
        {
            if (_toggleOnReference == null)
            {
                _toggleOnReference = Utils.FindInactive(
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<Image>().sprite;
            }

            return _toggleOnReference;
        }

        internal static Sprite GetToggleOffIconTemplate()
        {
            if (_toggleOffReference == null)
            {
                _toggleOffReference = Utils.FindInactive(
                        "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_2/Button_ToggleTooltips/Icon_Off")
                    .GetComponent<Image>().sprite;
            }

            return _toggleOffReference;
        }

        internal static GameObject GetMenuTemplate()
        {
            if (_menuReference == null)
            {
                _menuReference = Utils.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_DevTools");
            }

            return _menuReference;
        }

        internal static GameObject GetModalTemplate()
        {
            if (_modalReference == null)
            {
                _modalReference = Utils.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Modal_AddMessage");
            }

            return _modalReference;
        }
        
        internal static GameObject GetInfoPanelTemplate()
        {
            if (_infoPanelReference == null)
            {
                _infoPanelReference = Utils.FindInactive("MenuContent/Popups/PerformanceSettingsPopup/Popup/Pages/Page_LimitAvatarPerformance/Tooltip_Details");
            }

            return _infoPanelReference;
        }
    }
}
