   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089 !  XpsDocumentViewerAnimatedBase  �� 
TargetType �� Property �� Value �� 
TargetType � RowDefinitions  HyperLinkCheckBoxImage  image/32x32/close_window � ToolTip d� 	IsChecked �� Mode �� RelativeSource	 �� Mode
 �� AncestorType �� Padding J� Effect3   -System.Windows.Media.Effects.DropShadowEffect    
BlurRadius    ShadowDepth    Opacity ��Name �� EndPoint �� 
StartPoint  ControlLightColor � Color � Offset  ControlMediumColor �� Triggers P� Property P� Value N� EnterActions g� Duration �� TargetProperty W� From	 W� To W� EasingFunction0  *System.Windows.Media.Animation.ElasticEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode   Springiness    Oscillations! N� ExitActions  XpsDocumentViewerAnimated" �� BasedOn# �� Path  BooleanVisibilityConverter$ �� 	Converter  XpsDocument2ViewerAnimated  XpsDocumentViewer3Animated�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %r  &	        7  7  &	 2    7   7  &	 .	    7   7  &	 ,
    7   7  �� 5      "  Z�62   .�5   
   ��  � ��# � ��6'   �� 5   
    � ��# � ��6'   �� 5   
    �@�FocusVisualStyle 6-   Z� �� 5   
    �@Z�ShowPageBorders .  6,   �� 5	   
    � ��$ 0��6"   �� 5
   
    � ��$ 0��6#   �� 5   
    � {� .  6&   �� 5   
    �@Z�Zoom$	 100Q�6!   �� 5   
    � ��$ Center=�60   �� 5   
    � ��$ Center=�6.   �� 5   
    � ��$
 Left=�67   �� 5   
    � ��$	 Top=�65   �� 5   
    � �� 5      �� 5      " Z�6"   .R�5      �  5      �� 5      $��32 �6,   �� 5      $��* �6,   .+�5      �� 8��  8�� $��Right=�5   #   $��32��6?   $��32��6J   $ 
Schließen��5   #    5   #   �� *	IsEnabled+$ TwoWay=� �� $	 FindAncestor=�$
 DocumentViewerL�5   #   �� 5      $��1��6!   $��2q�6.   
���   �|�.  
�������$ 0q�6{   $��0,0,20,0q�6�    5         5    "   $ 15Q�63   $ 10Q�6C   $	 0.5Q�6T   ._�5"      �� $ PART_ContentHost��6l   ��. $��Disabled=�6C   ��. $��0q�6�   ��5#   "   �� 5$   &   $ 0.5,1&�6:   $ 0.5,0&�6K   .��5%   *   � # �  67   $ 0Q�6c   � 5&   *   # �  67   $ 1Q�6d   5'   '   5)      5*      5+      5,      5.       5/   
   P� 50       � {� .  51      �� 52      .��53      �� $ 0:0:2.0D�6%   .��54      W� $ Width�6.   $ 0X�6P   $
 1050X�6Y    55   "     56   &   $ EaseOut=�62   $ 3Q�6G   $  2��6W   58      W� 59      $ Height�6.   $ 0X�6Q   $	 500X�6Z    5:   "     5;   &   $ EaseOut=�62   $ 3Q�6G   $  2��6W   5=      5>      5?      ! 5A      �� 5B      .��5C      �� $ 0:0:2.0D�6%   .��5D      W� $ Width�6.   $
 1050X�6P   $ 0X�6\    5E   "     5F   &   $ EaseIn=�62   $ 1Q�6F   $  1��6V   5H      W� 5I      $ Height�6.   $	 500X�6Q   $ 0X�6\    5J   "     5K   &   $ EaseIn=�62   $ 1Q�6F   $  1��6V   5M      5N      5O      5Q      5S      �� 5T      8"   "  Z�6g   .�5U   
   ��  � {� 5V      �� 5W      $# XpsDocumentVisible�6   $ TwoWay=�64   5Y      �� 5Z   
    � y� 5[      �� 5\      $# XpsDocumentVisibleEnabled�6   8$  6;   5^      5_      �� 5`      8"   "  Z�6h   .�5a   
   ��  � {� 5b      �� 5c      $# XpsDocument2Visible�6   $ TwoWay=�65   5e      �� 5f   
    � y� 5g      �� 5h      $ # XpsDocument2VisibleEnabled�6   8$  6<   5j      5k      �� 5l      8"   "  Z�6h   .�5m   
   ��  � {� 5n      �� 5o      $# XpsDocument3Visible�6   $ TwoWay=�65   5q      �� 5r   
    � y� 5s      �� 5t      $ # XpsDocument3VisibleEnabled�6   8$  6<   5v      5w      5y      