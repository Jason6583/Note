   M S B A M L   `   `   `  ???? ?  ;Note, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Note.ControlsNote.Controls  1clr-namespace:Note.ConvertersNote.Converters      Note.Views.MoreColorView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     0controlsclr-namespace:Note.Controls     4
convertersclr-namespace:Note.Converters       ? 	Resources  /   )Note.Converters.StringToPenColorConverter   StringToPenColorConverter??%   &	          .??? $	??239??$	??159??.+???   EraserPopup#??[ ? $??27.5,27.5,25,27.5q?.+??? $??	背景色?????#4A4B4C$??4??$??0,3,0,6q??? -    ??Name$ BackColorList??
?????? $??	-8,20,0,0q?$??5??$
??Left=?$??Hidden=?$??0q?$??Hidden=????? *
BackColors+ ?? SelectedItem ?? *SelectedBackColor+ ?? Mode$ TwoWay=????? .R?A?  K? IsItemsHost$
 True?? A? Columns$ 5??$??Stretch=?????  ?? 
TargetType" ??.???  ?? Property ? ?? ?? Value$ 8,5,8,5q???  ? ?? ?? 	 ?? 
TargetType"	 ??.R?%   Note.Controls.ImageToggleButton    Note.PenColorButton#??[ ???? *Color+
 ?? 	Converter#
 [   d? 	IsChecked ??  ?? RelativeSource ??  ?? AncestorType" ?? ?? Path$ 
IsSelected?