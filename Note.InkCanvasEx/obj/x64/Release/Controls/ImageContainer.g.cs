﻿#pragma checksum "F:\Note优化\Note重构\Note优化版\Note.InkCanvasEx\Controls\ImageContainer.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E066174C152AAACCCB5B412228F49A7E9EA078A580A81170EA814A5162D683E5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Note.InkCanvasEx.Controls
{
    partial class ImageContainer : 
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
            case 2: // Controls\ImageContainer.xaml line 8
                {
                    this.ContainerGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).PointerEntered += this.ContainerGrid_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).PointerExited += this.ContainerGrid_PointerExited;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).ManipulationStarted += this.Manipulator_OnManipulationStarted;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).ManipulationDelta += this.Manipulator_OnManipulationDelta;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).ManipulationCompleted += this.Manipulator_ManipulationCompleted;
                }
                break;
            case 3: // Controls\ImageContainer.xaml line 21
                {
                    this.toolBar = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // Controls\ImageContainer.xaml line 30
                {
                    this.ElementGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 5: // Controls\ImageContainer.xaml line 32
                {
                    this.ElementTransform = (global::Windows.UI.Xaml.Media.CompositeTransform)(target);
                }
                break;
            case 6: // Controls\ImageContainer.xaml line 34
                {
                    this.border = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 7: // Controls\ImageContainer.xaml line 43
                {
                    this.image = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 8: // Controls\ImageContainer.xaml line 44
                {
                    this.closeImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.closeImage).Tapped += this.CloseImage_Tapped;
                }
                break;
            case 9: // Controls\ImageContainer.xaml line 52
                {
                    this.scaleImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 10: // Controls\ImageContainer.xaml line 25
                {
                    global::Windows.UI.Xaml.Controls.Button element10 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element10).Tapped += this.Recognize_Click;
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

