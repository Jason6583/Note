   M S B A M L   `   `   `  ???? ?  ;Note, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Note.ConvertersNote.Converters  -clr-namespace:Note.ControlsNote.Controls      Note.Views.ExportView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4
convertersclr-namespace:Note.Converters     0controlsclr-namespace:Note.Controls       ? 	Resources  <   6Note.Converters.InvertingBooleanToVisibilityConverters *  &InvertingBooleanToVisibilityConverters2   ,Note.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter,   &Note.Converters.InvertCheckedConverter  InvertCheckedConverter??%-   &	        &	      &	 
           .??? $	??254??$	??345??.+???   PopupExport#??[ ? $??20,30,20,30q? ? RowDefinitions ?? $??20 ??? $??34 ??? $??31 ??? $??34 ??? $??49 ??? $??50 ??? $??35 ??? .+??? $??合并导出一个笔记?????#4A4B4C$
??Bold"?$	??Top=??? ???#e8e8e8$	??0.5??$??Bottom=??? $??1??$??导出压缩包名称?????#4A4B4C$??	0,10,0,10q???   Note.TextBox#??[ ???#FFFFFF$??2??$	??184??$??26??$??0,2.5,0,2.5q????? *
FileName+ ?? Mode$ TwoWay=??? $??3??$??导出格式?????#4A4B4C$??	0,10,0,10q??? $??4?? ?? Orientation$ 
Horizontal=?.+?$   Note.Controls.ImageRadioButton  -     Name$ 
ImageButon??$	??108??$??49??  Note.ImageRadioButton#??[   ExportImage   Image# [   ExportImageHover   
HoverImage# [ $
??Left=? d? 	IsChecked ?? *IsImageChecked+  -   $ 	PdfButton??$	??108??$??49??#??[   	pdfNormal# [  	 	ExportPdf# [	 $??Right=? ?? *IsImageChecked+ ?? 	Converter# [ $ TwoWay=?   Note.Controls.ImageButton   
 Note.ImageButton#??[
 $	??210??$??50??  ExportToDesktop	   Image#	 [   ExportDesktopHover
   
HoverImage#
 [ $??5?????? *SaveToDeskTopCommand+    
ImportFile#	 [   ImportFileHover#
 [ $	??210??$??50??$??6?????#4A4B4C???? *ImportCommand+