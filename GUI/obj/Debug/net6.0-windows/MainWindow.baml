   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI  5   	       GUI.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:GUI     5   	   -   5          � Title$  
MainWindow��5   	   $	��566��6   $	��851��6)    � WindowStartupLocation$ CenterScreen=�65   .��5	      � $��0,72,0,0q�6   .+�5   
   �� $��0,-64,0,457q�6    �� Orientation$ 
Horizontal=�6*   .+�5      �� -   5         ��Name$ addTaskButton��5      $��
700 2 2 20q�6,   $	��120��6@   $��28��6j   $��Add new task��6v   $
��Bold"�6�   5      5      �� 5   
   $��5,-21,5,437q�6   �� 5   
   -   5         ��Name$ tasksListView��5      $	��100��6*   $��2��6:   ��6N   �� *Tasks+ � DataContext 6l   �� *	ViewModel+ �� View 5      �� 5       �� AllowsColumnReorder$ False��6   .P�5      ��  �� Width$
 Auto��6%   $��	Task Name��62   	 �� DisplayMemberBinding	 6E   �� *Name+�� 5      $
 Auto��6%   $��
Task State��62   	 6F   �� *State+�� 5      $��Progress��6%   
 �� CellTemplate
 5      �� 5      .R�5   "   � $��ProgressBar��6.   $
��Auto��6A   $	��300��6N    � Maximum$	 1.0Q�6]   $��15��6k    � Value 6w   �� *
Progress+ �� Mode$ OneWay=�6�   5      5      �� 5      $��Controls��6%   
 5      �� 5      .R�5   "   �� $ 
Horizontal=�6-   $
��Auto��6F   $
��Auto��6T   .+�5    &   �� $ startTaskButton��6-   -   5        {�5    F   �� *IsStartable+.��5!   *   �� $��Resources/play32.png��60   $��16��6N   $��16��6Y   6g   5"   '   �� 5#   &   $ pauseTaskBtton��6-   -   5        {�5#   E   �� *
IsPausable+.��5$   *   �� $��Resources/pause32.png��60   $��16��6O   $��16��6Z   6h   5%   '   �� 5&   &   $ stopTaskButton��6-   -   5        {�5&   E   �� *IsStoppable+.��5'   *   �� $��Resources/stop32.png��60   $��16��6N   $��16��6Y   6g   5(   '   �� 5)   &   $ closeTaskButton��6-   -   5        {�5)   F   �� *IsCloseable+.��5*   *   �� $��Resources/close32.png��60   $��16��6O   $��16��6Z   6h   5+   '   5,   #   5-      5/      50      52      53      54      