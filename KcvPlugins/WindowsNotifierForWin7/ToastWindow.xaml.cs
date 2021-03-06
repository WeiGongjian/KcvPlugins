﻿using AMing.WindowsNotifierEx;
using MetroRadiance.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace AMing.WindowsNotifierEx
{
    /// <summary>
    /// ToastWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ToastWindow : Window
    {
        public ToastWindow()
        {
            InitializeComponent();
            this.InitToast();
            this.Loaded += ToastWindow_Loaded;
            this.StateChanged += ToastWindow_StateChanged;
        }

        void ToastWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState != System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        void ToastWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AMing.Plugins.Core.Helper.ToolWindowHelper.SetToolWindow(this);
            Dpi dpi = this.GetSystemDpi() ?? Dpi.Default;
            this.Top = 0;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width / dpi.ScaleX - this.Width;
        }

        #region Toasts

        private List<ToastItemControl> ToastItems;

        private void InitToast()
        {
            var list = sp_toasts.Children.OfType<ToastItemControl>();
            ToastItems = list.ToList();

            ToastItems.ForEach(item =>
            {
                item.AnimationHideComplete += item_AnimationHideComplete;
                item.ToastClick += item_ToastClick;
            });
        }

        void item_ToastClick(object sender, EventArgs e)
        {
            if (ToastClickAction != null)
                ToastClickAction();
        }

        private void item_AnimationHideComplete(object sender, EventArgs e)
        {
            ShowLastMessage();
        }

        private ToastItemControl GetIdleToast()
        {
            return ToastItems.FirstOrDefault(item => !item.IsPlaying);
        }
        #endregion

        #region method

        private Queue _msgList = new Queue();

        public Queue MessageList
        {
            get { return _msgList; }
            private set { _msgList = value; }
        }

        private void ShowLastMessage()
        {
            var toast = GetIdleToast();
            if (toast != null && MessageList.Count > 0)
            {
                var msg = (ToastMessage)MessageList.Dequeue();
                toast.Show(msg);
            }
        }

        public void ShowToast(Enums.ToastType type, string title, string content)
        {
            MessageList.Enqueue(new ToastMessage
            {
                Type = type,
                Title = title,
                Content = content
            });

            ShowLastMessage();

            this.ActivateWindow();
        }

        /// <summary>
        /// 重新让窗体显示
        /// </summary>
        private void ActivateWindow()
        {
            if (this.WindowState != System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
            if (this.Visibility != System.Windows.Visibility.Visible)
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }
            if (!this.Topmost)
            {
                this.Topmost = true;
            }
            this.Show();
        }
        #endregion

        #region event

        public Action ToastClickAction;

        #endregion
    }
}
