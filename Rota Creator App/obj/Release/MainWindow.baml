   M S B A M L   `   `   `  ���� W  SRota Creator App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f411e81d611e7e603clr-namespace:Rota_Creator_AppRota_Creator_App  !    Rota_Creator_App.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:Rota_Creator_App     -     � Title$  Rota Creator��$��530.825��$��1002.262�� � Icon$&  Resources/icons8-schedule-96.ico�� � WindowState$ 	Maximized=�.��� .+��� -   $��Stretch=�$��	0,18,0,22q�$��Stretch=�.@��� $
��Rota��.��� ���	#FFE5E5E5 � RowDefinitions �� $��* ��� $
��auto ��� $
��Auto � � ColumnDefinitions �� $��* ��� $
��auto ��� $��* �.+��� -    ��Name$ lstAvailableOfficers��$��0��$��0��$��Stretch=�$��5q�$��Stretch=� �� SelectionMode$ Extended=����� .R��� -   $��5q�$��Stretch=�$	��Top=� �� Orientation$ 
Horizontal=�.+��� $	��200������  �� Path$
 Name��� $��Stretch=����� $
 Team��� -   	 ��Name$	 txtAvailableOfficerSearch��$��1��$��Stretch=�$�� ��$��Bottom=�$��5,0,5,0q��� $��1��.+��� -   $��>��$��5q�$��5,2,5,2q�$��50���� -   $��>>��$��5q�$��5,2,5,2q�$��50���� -   $��<��$��5q�$��5,2,5,2q�$��50���� -	   $��<<��$��5q�$��5,2,5,2q�$��50���� -
   $ lstActiveOfficers��$��2��$��0��$��Stretch=�$��5q�$��Stretch=�$ Extended=����� .R��� -   $��5q�$��Stretch=�$	��Top=�$ 
Horizontal=�.+��� $	��200������ $
 Name��� ���� $
 Team��� -   $	 txtActiveOfficerSearch��$��1��$��Stretch=�$�� ��$��Bottom=�$��2��$��5,0,5,0q�� $��2��$��3��$��Stretch=�$��Bottom=�$��2��.+��� -   
 ��Name$
 cmbStartTime��$
��Left=�$��5,5,0,0q�$��1��$	��Top=�$	��120���� $��to��$
��Left=�$��	130,1,0,0q�$	��Top=� J� RenderTransformOrigin$ -0.143,0.423&��� -   $
 cmbFinishTime��$
��Left=�$��	156,5,0,0q�$	��Top=�$	��120���� -   $
 cmbRotaSite��$��Right=�$��Bottom=�$��0,5,70,5q�$��5,2,5,2q�$	��132������ .R��� ���� $
 Name��� -   $��Generate��$��Right=�$��5q�$��5,2,5,2q��� $��Officers��.��� ���	#FFE5E5E5 �� $��3* ��� $��* � �� $��* ��� $
��auto �.+��� -   $ lstOfficers��$ Single=�$��2��$��5q����� .R��� $ 
Horizontal=�$��5q�.+��� $	��200������ $
 Name��� $��50������ $ Abbreviation��� $	��100������ $
 Team��� $��1��$��Stretch=�$��Stretch=�.+��� $
��Name��$��5,5,5,0q��� -   $	 txtOfficerName��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $��Abbreviation��$��5,5,5,0q��� -   $	 txtOfficerAbbr��$�� ��$	��Top=�$��5,0,5,5q� �� 	MaxLength$ 12��{�.  �� $
��Team��$��5,5,5,0q��� -   $	 txtOfficerTeam��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $��	Positions��$��5,5,5,0q��  �� $��* ��� $
��auto �.+��� -   $ lstOfficerPositions��{�.  $	��200������ .R��� ���� $
 Name��� $��1��.+��� -    ��Name$ btnAddOfficerPosition��$��2q�$��2q�.���� $'��!Resources/icons8-plus-math-50.png��$��24���� -   $ btnDeleteOfficerPosition��$��2q�$��2q�.���� $$��Resources/icons8-delete-64.png��$��24���� $��1��$��1��$ 
Horizontal=�.+��� -   $ btnAddOfficer��$��Right=�$	��New��$��5q�$��5,2,5,2q��� -   $ btnUpdateOfficer��$��Right=�$��Update��$��5q�$��5,2,5,2q�{�.  �� -   $ btnDeleteOfficer��$��Right=�$��Delete��$��5q�$��5,2,5,2q�{�.  �� $��	Positions��.��� ���	#FFE5E5E5 �� $��3* ��� $��* � �� $��* ��� $
��auto �.+��� -   $ lstPositions��$ Single=�$��2��$��5q����� .R��� $ 
Horizontal=�$��5q�.+��� $��20������ $ Index��� $	��200������ $
 Name��� $	��200������ $ Duration��� ���� $ 	Site.Name��� $��1��$��Stretch=�$��Stretch=�.+��� $��Index��$��5,5,5,0q��� -   $	 txtPositionIndex��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $
��Name��$��5,5,5,0q��� -   $	 txtPositionName��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $��Duration (Hours)��$��5,5,5,0q��� -   $	 txtPositionDuration��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $
��Site��$��5,5,5,0q��� -   $
 cmbPositionSite�� �� Text$  ��$	��Top=�$��5,0,5,5q�{�.  ���� .R��� ���� $
 Name��� $��1��$��1��$ 
Horizontal=�.+��� -    $ btnAddPosition��$��Right=�$	��New��$��5q�$��5,2,5,2q��� -!   $ btnUpdatePosition��$��Right=�$��Update��$��5q�$��5,2,5,2q�{�.  �� -"   $ btnDeletePosition��$��Right=�$��Delete��$��5q�$��5,2,5,2q�{�.  �� $��Sites��.��� ���	#FFE5E5E5 �� $��3* ��� $��* � �� $��* ��� $
��auto �.+��� -#   $ lstSites��$��2��$��5q����� .R��� $��5q����� $
 Name��� $��1��$��Stretch=�$��Stretch=�.+��� $
��Name��$��5,5,5,0q��� -$   $	 txtSiteName��$�� ��$	��Top=�$��5,0,5,5q�{�.  �� $��1��$��1��$ 
Horizontal=�.+��� -%   $ 
btnAddSite��$��Right=�$	��New��$��5q�$��5,2,5,2q��� -&   $ btnUpdateSite��$��Right=�$��Update��$��5q�$��5,2,5,2q�{�.  �� -'   $ btnDeleteSite��$��Right=�$��Delete��$��5q�$��5,2,5,2q�{�.  �� $��Stretch=�$��Bottom=�.@��� -(    ��Name$ 
statusText��$
��Wrap=�$
��Okay��o� $��Stretch=�$	��Top=�$��0,0,0,0q�.@�m� $
��File��.@�m� $	��New��.@�m� $��Officer��m� $��Position��m� $
��Site���� m� $��Close��m� $
��Edit��.@�m� $
��Undo��m� $
��Redo���� m� $	��Cut��m� $
��Copy��m� $��Paste���� m� $��
Select All��