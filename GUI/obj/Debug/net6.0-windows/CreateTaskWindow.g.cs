﻿#pragma checksum "..\..\..\CreateTaskWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1F28B6CE727834792CE0D6C7D30C1421C28B7E02"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI;
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


namespace GUI {
    
    
    /// <summary>
    /// CreateTaskWindow
    /// </summary>
    public partial class CreateTaskWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox nameTextBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox priorityTextBox;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox executionTimeTextBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox deadlineCheckBox;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker deadlineDatePicker;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox deadlineTextBox;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox startTimeCheckBox;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker startTimeDatePicker;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox startTimeTextBox;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox maxDegreeOfParallelismComboBox;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox taskTypeListBox;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\CreateTaskWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox userTaskJsontextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.13.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUI;component/createtaskwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CreateTaskWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.13.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.nameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.priorityTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.executionTimeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.deadlineCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.deadlineDatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.deadlineTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.startTimeCheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.startTimeDatePicker = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 9:
            this.startTimeTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.maxDegreeOfParallelismComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.taskTypeListBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 66 "..\..\..\CreateTaskWindow.xaml"
            this.taskTypeListBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TaskTypeListBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.userTaskJsontextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            
            #line 72 "..\..\..\CreateTaskWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FinishButton_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 75 "..\..\..\CreateTaskWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

