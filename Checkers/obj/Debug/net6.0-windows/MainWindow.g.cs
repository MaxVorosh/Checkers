﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "522CF8996F938F79DBAD0A8F44DB5EAC816C3F58"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Checkers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Checkers {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel DifficultStackPanel;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ModeStackPanel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Checkers;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 19 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DifficultStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            
            #line 21 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonChecked);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 22 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonChecked);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 23 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonChecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ModeStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            
            #line 26 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonChecked);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 27 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.RadioButtonChecked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

