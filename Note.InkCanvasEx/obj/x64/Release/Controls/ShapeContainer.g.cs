﻿#pragma checksum "F:\Note优化\Note重构\Note优化版\Note.InkCanvasEx\Controls\ShapeContainer.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C4F76474E2395E45879623DDB866F0C7AA1139FBA9D102EFFA6E7C3A7271673E"
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
    partial class ShapeContainer : 
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
            case 2: // Controls\ShapeContainer.xaml line 6
                {
                    this.ContainerGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).PointerEntered += this.ContainerGrid_PointerEntered;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).PointerExited += this.ContainerGrid_PointerExited;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).ManipulationStarted += this.Manipulator_OnManipulationStarted;
                    ((global::Windows.UI.Xaml.Controls.Grid)this.ContainerGrid).ManipulationDelta += this.Manipulator_OnManipulationDelta;
                }
                break;
            case 3: // Controls\ShapeContainer.xaml line 13
                {
                    this.ElementGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4: // Controls\ShapeContainer.xaml line 14
                {
                    this.border = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 5: // Controls\ShapeContainer.xaml line 22
                {
                    this.closer = (global::Windows.UI.Xaml.Controls.Image)(target);
                    ((global::Windows.UI.Xaml.Controls.Image)this.closer).Tapped += this.CloseImage_Tapped;
                }
                break;
            case 6: // Controls\ShapeContainer.xaml line 29
                {
                    this.resizer = (global::Windows.UI.Xaml.Controls.Image)(target);
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

