   M S B A M L   `   `   `  ���� B  >Overlay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:OverlayOverlay      Overlay.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Overlay       � Title$  Visual Spotter Overlay��$��90��$
��1920�� � AllowsTransparency .  � WindowStyle$
 None=�
������  � WindowStartupLocation$ Manual=� � Left$ 0��
 � Top$ 0�� � Topmost .  � ShowInTaskbar .   � Icon$ icon.png��.��� 	 � ColumnDefinitions	 �� -   
 ��Name$
 
LeftMargin��$	��30* ��� -   $
 LeftIndicatorWidth��$	��20* ��� -   $
 CenterWidth��$
��100* ��� -   $
 RightIndicatorWidth��$	��20* ��� -   $
 RightMargin��$	��30* �.+�� $��1��	 �� $��* ��� $��* � � RowDefinitions �� $��* ��� $��* �.+��� -    ��Name$$ FastClassApproachingIndicator1��$��0��
������ � $��3��	 �� $��* ��� $��* � �� $��* ��� $��* �.+��� -   $$ FastClassApproachingIndicator2��$��1��
������ �� -   $ CarLeftIndicator��$��1��
��������� -	   $ CarRightIndicator��$��3��
�������