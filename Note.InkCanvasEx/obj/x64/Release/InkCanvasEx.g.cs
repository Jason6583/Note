﻿#pragma checksum "F:\Note优化\Note重构\Note优化版\Note.InkCanvasEx\InkCanvasEx.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "272656426F01C1574D7AB9D1B40C9E0002064894B43FC987E708184F4FE49808"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Note.InkCanvasEx
{
    partial class InkCanvasEx : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // InkCanvasEx.xaml line 12
                {
                    this.mainView = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // InkCanvasEx.xaml line 13
                {
                    this.scrollViewer = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                }
                break;
            case 4: // InkCanvasEx.xaml line 19
                {
                    this.imageCanvas = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl)(target);
                }
                break;
            case 5: // InkCanvasEx.xaml line 22
                {
                    this.inkCanvas = (global::Windows.UI.Xaml.Controls.InkCanvas)(target);
                }
                break;
            case 6: // InkCanvasEx.xaml line 26
                {
                    this.shapeCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            case 7: // InkCanvasEx.xaml line 29
                {
                    this.selectionCanvas = (global::Windows.UI.Xaml.Controls.Canvas)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
