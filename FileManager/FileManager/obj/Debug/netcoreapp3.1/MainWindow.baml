   M S B A M L   `   `   `  ����F  BFileManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:FileManagerFileManager  5   	       FileManager.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:FileManager     5   	     � Title$  
MainWindow��5   	   $	��360��6   $	��640��6)   .��5	      ]� $��False��6   $��0,0,0,0q�6&   .+�5
   
   � $	��Top=�6    � RowDefinitions 5      �� 5      $��1* �6    �� 5      $��6* �6     � ColumnDefinitions 5      �� 5      $��5* �6#   �� 5      $��3* �6#   .+�5      �� -   5         ��Name$ 
SearchMenu��5       �� TextWrapping$
 Wrap=�6*    �� TextAlignment$ Justify=�6>   $��1��6�   �� 5      -   5         ��Name$ DirectoryMenu��5      $y�Visible=�6_   
���������5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6%   $��Stretch=�6>   $��Stretch=�6Z   $
��Auto��6x   $
��Auto��6�   5      5      5      �� 5   
   -   5        $ DirectoryDisplay��5      $
��Left=�6,   
���������5      �� 5      .R�5      �� $ Vertical=�6!   $��Stretch=�68   $��Stretch=�6T   $
��Auto��6r   $
��Auto��6�   5       5"      �� 5#   
   -   5        $ InfoDisplay��5#      $
��Left=�6'   $y�Visible=�6=   ${�False��6R   ��5$      �� 5%      .R�5&      �� $ Vertical=�6!   $��Stretch=�68   $��Stretch=�6T   $
��Auto��6r   $
��Auto��6�   5'      5)      5*      5+      