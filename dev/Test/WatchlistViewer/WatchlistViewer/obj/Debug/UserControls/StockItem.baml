   M S B A M L   `   `   `  ����  WatchlistViewer,    &WatchlistViewer.UserControls.StockItem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      ��   �� Orientation$  
Horizontal=�6   $��0,0,0,0q�6*    � Cursor$
 Hand��5   F    � ToolTip 5   ;   �� *	ToolTip+
 � Tag 5      ��  �� RelativeSource �� *Self+ J� InputBindings 5
   
   d� 5       d� MouseAction$ 	LeftClicke�5       �� Command 5      �� *'%DataContext.ShowWknAtComdirectCommand+ ��  �� Mode$ FindAncestor=�	 �� AncestorType"	 >�5      
 � ContextMenu
 5   
   �� 5      .@�5      m�    image/16x16/comdirect m� Icon# [  5   '   $��Show Chart ...��5   '    m� Command 5      �� *'%DataContext.ShowWknAtComdirectCommand+ �� $ FindAncestor=�"	 ��5   '   5      .+�5   
   �� $��11(�6   
������� �� TextAlignment$ Right=�6O   $��5,0,0,0q�6e   ��6"   �� *	ShortName+6e   �� 5   
   $��15��65    J� Opacity$	 0.2Q�6@   $��Center=�6|   ��6   �� *	ForeColor+��6N   �� *PixelAbsPercentChangeForUI+ 6�   �� *ToolTipChangePercent+�� 5   
   $��11(�6   $
��Bold"�6"   
���   �$��2,0,0,0q�6g   $��60��6x   $
 Left=�6�   ��64   �� *ValueFormatted+6�   5      5      