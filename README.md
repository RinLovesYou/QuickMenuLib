# QuickMenuLib
A QuickMenu API inspired by UiExpansionKit, and partially based on ReModCE's first API

## Table of Contents
* [QuickStart](#QuickStart)
* [Planned](#Planned)
* [Credits](#Credits)

## QuickStart

Download the latest [Release](https://github.com/RinLovesYou/QuickMenuLib/releases) and place it in your `Mods` folder
then reference the dll in your project

Create a Class that Inherits from ModMenu.
```cs
using MelonLoader;
using QuickMenuLib;

namespace QuickMenuLibTestMod
{
    public class MyModMenu : ModMenu
    {
        public override string MenuName => "TestModMenu"; //Has to be set: Conflicts will arise if you don't
        
        public MyModMenu()
        {
            Logo = // You can put a sprite here and QuickMenuLib will automatically add it.
        }
        
        public override void OnQuickMenuInitialized()
        {   
            var category = MyModMenu.AddMenuCategory("TestModMenuCat", "Testing Stuff");
            category.AddButton("Test", "This is a test using QuickMenuLib!", () =>
            {
                MelonLogger.Msg("Test Button!");
            });

            MyModMenu.AddSlider("FunnySlider", "Test", (num) => MelonLogger.Msg(num));
        }
        public override void OnWingMenuLeftInitialized()
        {
            MyLeftWingMenu.AddButton("Test", "Test using PepsiLib!", () =>
            {
                MelonLogger.Msg("Test Wing Button!");
            });
        } 
        public override void OnWingMenuRightInitialized()
        {
            MyRightWingMenu.AddButton("Test", "Test using PepsiLib!", () =>
            {
                MelonLogger.Msg("Test Wing Button!");
            });
        }
        public override void OnTargetMenuInitialized()
        {
            MyTargetMenu.AddButton("Test", "Test using PepsiLib!", () =>
            {
                MelonLogger.Msg(SelectedUser.prop_String_0);
            });
        }
    }
}
```

Then simply register your custom Mod Menu Class:
```cs
        public override void OnApplicationStart()
        {
            QuickMenuLib.QuickMenuLibMod.RegisterModMenu(new MyModMenu());
        }
```

A Look at what PepsiLib provides your Menu with, by looking at the ModMenu Class.
```cs
using PepsiLib.UI.Elements;
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
        
        public IUser SelectedUser = null;

        public virtual void OnQuickMenuInitialized() { }
        public virtual void OnWingMenuLeftInitialized() { }
        public virtual void OnWingMenuRightInitialized() { }
        public virtual void OnTargetMenuInitialized() { }  
    }
}
```

The Pages QuickMenuLib creates for itself are exposed to your Mod as well. 
```cs
        //We expose these to the Mods in case they want to add content to these pages. Unlikely but it can't hurt.
        public static QuickMenuPage MainMenu = null;
        public static QuickMenuPage TargetMenu = null;
        public static QuickMenuWingMenu LeftWingMenu = null;
        public static QuickMenuWingMenu RightWingMenu = null;
```

## Planned
* Settings
* Expose every page
* UI 2.0: integrate with big menu when it comes out
* Proper Documentation Page

## Credits
The way we add controls is based on how RequiDev does it in [ReModCE](https://github.com/RequiDev/ReModCE) <br>
This Project is inspired by Knah's [UiExpansionKit](https://github.com/knah/vrcmods)
