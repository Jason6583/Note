#pragma checksum "F:\Note优化\Note重构\Note优化版\Note.InkCanvasEx\Controls\EraserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4C76D68BF61EEBD140387FCC749CD4C6B743AFFB7068C3610B88D597A6B60ADA"
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
    partial class EraserControl : 
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
            case 2: // Controls\EraserControl.xaml line 19
                {
                    this.Stroke1Btn = (global::Note.InkCanvasEx.Controls.ImageRadioButton)(target);
                    ((global::Note.InkCanvasEx.Controls.ImageRadioButton)this.Stroke1Btn).Checked += this.EraserSize_Checked;
                }
                break;
            case 3: // Controls\EraserControl.xaml line 20
                {
                    this.Stroke2Btn = (global::Note.InkCanvasEx.Controls.ImageRadioButton)(target);
                    ((global::Note.InkCanvasEx.Controls.ImageRadioButton)this.Stroke2Btn).Checked += this.EraserSize_Checked;
                }
                break;
            case 4: // Controls\EraserControl.xaml line 21
                {
                    this.Stroke3Btn = (global::Note.InkCanvasEx.Controls.ImageRadioButton)(target);
                    ((global::Note.InkCanvasEx.Controls.ImageRadioButton)this.Stroke3Btn).Checked += this.EraserSize_Checked;
                }
                break;
            case 5: // Controls\EraserControl.xaml line 22
                {
                    this.Stroke4Btn = (global::Note.InkCanvasEx.Controls.ImageRadioButton)(target);
                    ((global::Note.InkCanvasEx.Controls.ImageRadioButton)this.Stroke4Btn).Checked += this.EraserSize_Checked;
                }
                break;
            case 6: // Controls\EraserControl.xaml line 23
                {
                    this.Stroke5Btn = (global::Note.InkCanvasEx.Controls.ImageRadioButton)(target);
                    ((global::Note.InkCanvasEx.Controls.ImageRadioButton)this.Stroke5Btn).Checked += this.EraserSize_Checked;
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

