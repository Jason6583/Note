   M S B A M L   `   `   `  ???? ?  ;Note, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Note.ControlsNote.Controls  1clr-namespace:Note.ConvertersNote.Converters  'clr-namespace:Note.Views
Note.Views  h dMicrosoft.Toolkit.Wpf.UI.XamlHost, Version=6.1.0.0, Culture=neutral, PublicKeyToken=4aff67a105548ee2?Zclr-namespace:Microsoft.Toolkit.Wpf.UI.XamlHost;assembly=Microsoft.Toolkit.Wpf.UI.XamlHost!Microsoft.Toolkit.Wpf.UI.XamlHost     Note.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0controlsclr-namespace:Note.Controls     4
convertersclr-namespace:Note.Converters     *viewsclr-namespace:Note.Views     oxamlhostZclr-namespace:Microsoft.Toolkit.Wpf.UI.XamlHost;assembly=Microsoft.Toolkit.Wpf.UI.XamlHost     -     ? WindowState$  	Maximized=? ? WindowStyle$
 None=? ? AllowsTransparency$
 True?? ? Title$ 
MainWindow??$	??800??$
??1280?? ? 	Resources <   6Note.Converters.InvertingBooleanToVisibilityConverters *  &InvertingBooleanToVisibilityConverters2   ,Note.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter??%   &	        &	          .???  ? RowDefinitions ?? $??60 ??? .+?? ???#FCFCFC.+?%   Note.Controls.ImageToggleButton  -     Name$ ShareBtn??  Note.ImageToggleButton#??[   	ShareNote   Image# [   ShareNoteHover   
HoverImage# [ $??	15,7,15,7q?$
??Left=?$??30??$??30??? 	 ? 	StaysOpen$	 False??$??1??$??Bottom=?
 ? AllowsTransparency$

 True??????  ?? ElementName$ ShareBtn?? ?? Path$ 	IsChecked? ?? Mode$ TwoWay=? ? PlacementTarget ?? $ ShareBtn??.??   Note.Views.ExportView     Note.Views.AudioPanel  -     Name$ 
AudioPanel??$??Center=?$??Center=?y??? *+)InkStrokeAudioService.IsAudioPanelVisible+ ?? 	Converter# [ ?? $	??125??$??Right=? ?? Orientation$ 
Horizontal=?.+?   Note.Controls.ImageButton     Note.Controls.ImageButton   Click$ MinimizeButton_Clicked??  MiniminzeWindow   Image# [    
HoverImage# [ $??0,7,5,7q?$
??Left=?$??30??$??30??  $ MaximizeButton_Clicked??  MaximizeWindow# [ # [ $??5,7,5,7q?$
??Left=?$??30??$??30??  $ CloseButton_Clicked??  CloseWindow# [ # [ $??5,7,15,7q?$
??Left=?$??30??$??30??7  1Microsoft.Toolkit.Wpf.UI.XamlHost.WindowsXamlHost  -     Name$ XamlHost??   InitialTypeName$" Note.InkCanvasEx.InkCanvasEx??$??1??$??0q?