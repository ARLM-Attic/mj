   M S B A M L   `   `   `  ����J  FWatchlistViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:WatchlistViewer.UserControlsWatchlistViewer.UserControls  5   	        WatchlistViewer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   CuserControls*clr-namespace:WatchlistViewer.UserControls     5   	   -   5          � WindowStartupLocation$  Manual=�5   	   
 � Top$ 17��5   	    � Left$	 500��6    � Topmost .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  $��0q�63   
������  � AllowsTransparency .  � Title$ Watchlist Viewer��5   	    � SizeToContent$ WidthAndHeight=�6"   ��5	      �� 5
   
   	 �� Opacity$		 0.0Q�6   
 �� Color$
 White��6(   .��5      �� 
�������$��0q�6!    �� CornerRadius$ 8��65   $��Center=�6F   $��Center=�6a   ��5   
   �� 5      $		 0.4Q�6   $
 	AliceBlue��6,    J� BitmapEffect 5   
   F� 5       F� Color$ White��6%    F� Opacity$	 0.4Q�63    F� 	Direction$	 270Q�6A    F� ShadowDepth$	 0.7Q�6Q   ._�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��2,0,0,0q�68      HyperLinkSmallImage#��[  6I     image/12x12/proxy-on#��[ 6v    � ToolTip$ Proxy einschalten��6�    J� Opacity$	 0.4Q�6�   ��6   �� *ProxyOnCommand+6�   �� 5      $��2,0,0,0q�69   #��[  6J     image/12x12/proxy-off#��[ 6w   $ Proxy ausschalten��6�   $	 0.4Q�6�   ��6   �� *ProxyOffCommand+6�   �� 5      $��15,0,0,0q�6@   #��[  6R     image/12x12/toggle#��[ 6   $ Watchlist-Anzeige toggeln��6�   $	 0.4Q�6�   ��6   �� *WatchlistToggleCommand+6�   �� 5      $��15,0,5,0q�65   #��[  6G     image/12x12/delete_black#��[ 6t   $ Applikation beenden!��6�   ��6   �� *QuitCommand+6�   �� 5        ItemsControlHorizontal#��[ 6�   $��	10,0,15,0q�6�   y�6   �� *StockItemsVisible+  BooleanVisibilityConverter �� 	Converter# [ ��6{   �� *
StockItems+��5      �� 5      .R�5      ,   &WatchlistViewer.UserControls.StockItem  5      5!      5#      5$      5%      