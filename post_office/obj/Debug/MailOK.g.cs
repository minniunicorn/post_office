﻿#pragma checksum "..\..\MailOK.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "47221FBB5C3CE6E1901036B12A737B697B75A1D68B77E31BEA983EB7024EAA9C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using post_office;


namespace post_office {
    
    
    /// <summary>
    /// MailOK
    /// </summary>
    public partial class MailOK : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 88 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button gotomain;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label num_pack;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label type_rank;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label package_inf;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label client_inf;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label fio;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\MailOK.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label time;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/post_office;component/mailok.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MailOK.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.gotomain = ((System.Windows.Controls.Button)(target));
            
            #line 88 "..\..\MailOK.xaml"
            this.gotomain.Click += new System.Windows.RoutedEventHandler(this.Gotomain_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.num_pack = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.type_rank = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.package_inf = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.client_inf = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            
            #line 104 "..\..\MailOK.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NewPack_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.fio = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.time = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

