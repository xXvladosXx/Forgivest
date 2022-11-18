using System;
using System.Collections.Generic;
using System.Linq;
using UI.Core;
using UI.Warning;
using UnityEngine;

namespace UI.Menu.Core
{
    public class MenuSwitcher : UIElement
    {
        protected static MenuSwitcher Instance;
        
        [SerializeField] private Menu _startingMenu;
        [SerializeField] private Menu[] _menus;
        [SerializeField] private BaseWarning _warning;
        
        private Menu _currentMenu;
        private readonly Stack<Menu> _history = new Stack<Menu>();
        
        
        private void Awake()
        {
            Instance = this;
            
            Hide();
        }

        private void Start()
        {
            _currentMenu = _startingMenu;
        }

        public static void Show<T>(bool remember = true) where  T : Menu
        {
            foreach (var menu in Instance._menus)
            {
                if (menu is T)
                {
                    if (Instance._currentMenu != null)
                    {
                        if (remember)
                        {
                            Instance._history.Push(Instance._currentMenu);
                        }
                        
                        Instance._currentMenu.HideMenu();
                    }
                    
                    menu.ShowMenu();
                    Instance._currentMenu = menu;
                }
            }
        }
        
        private static void Show(Menu menu, bool remember = true)
        {
            if (Instance._currentMenu != null)
            {
                if (remember)
                {
                    Instance._history.Push(Instance._currentMenu);
                }
                
                Instance._currentMenu.HideMenu();
            }
            
            if (remember && Instance._currentMenu == null)
            {
                Instance._history.Push(Instance._currentMenu);
            }
            menu.ShowMenu();
            Instance._currentMenu = menu;
        }

        public static void ShowLast()
        {
            if (Instance._history.Count != 0)
            {
                Show(Instance._history.Pop(), false);
            }
        }

        public Menu Find<T>() where T : Menu
        {
            foreach (var menu in _menus)
            {
                if (menu is T)
                {
                    return menu;
                }
            }

            return null;
        }

        private void OnDisable()
        {
            if (_startingMenu != null)
            {
                Show(_startingMenu, true);
            }
        }
    }
}