   M S B A M L   `   `   `  ����W  SRota Creator App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f411e81d611e7e603clr-namespace:Rota_Creator_AppRota_Creator_App  5   	   !    Rota_Creator_App.RotaWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Rota_Creator_App     5   	     � Title$
  Rota��5   	   $	��450��6   $	��800��6#    � SizeToContent$ WidthAndHeight=�6/    � WindowStartupLocation$ CenterOwner=�6N   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��auto �6   .+�5   
   � -   5         �Name$ rotaGrid��5      $��0q�6!   5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      $��* �6#   .+�5      �� -   5         ��Name$ chkPropagateChanges��5      $��Propagate Changes��64    d� 	IsChecked$
 TrueY�6P   $��5q�6a   $��Center=�6l   �� 5      -   5        $ chkLockOnChange��5      $��Lock On Change��60   $
 TrueY�6I   $��
10,8,533,8q�6Z   $��Center=�6n   $��1��6�   �� 5      $��1��6    �� Orientation$ 
Horizontal=�6)   $��RightToLeft=�6B   .+�5      �� -   5        	 ��Name$	 btnClose��5      $��Close��6+   $��Right=�6;   $��5q�6W   $��5,2,5,2q�6b   6t   �� 5      -   5        $	 btnPrint��5      $��Print��6+   $��Right=�6;   $��5q�6W   $��5,2,5,2q�6b   {�.  6�   �� 5      -   5        $	 btnRegenerate��5      $��
Regenerate��60   $��Right=�6E   $��5q�6a   $��5,2,5,2q�6l   {�.  6�   5      5       � 5"   
   -   5        
 �Name$
 progLoading��5"      $��Center=�6+   $��Center=�6H   $��18��6c   $	��105��6o    � IsIndeterminate . 6{   5#      5$      