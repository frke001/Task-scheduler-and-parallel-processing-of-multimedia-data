   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI  5   	       GUI.StartupWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:GUI     5   	   -   5          � Title$  StartupWindow��5   	   $	��466��6   $	��653��6,   
������� � WindowStartupLocation$ CenterScreen=�6K   .��5	      � .+�5
   
   ��  �� Orientation$ 
Horizontal=�6   $��2,338,-3,21q�6.   .+�5      �� -   5         ��Name$ startButton��5      $	��161��6*   $��29��66   $��Start��6B   $
��Bold"�6R   $��	240 5 5 5q�6d   6w   5      �� 5   
   $ 
Horizontal=�6   $��
5,64,0,274q�6.   .+�5      �� $'��!Maximum Number of Concurent Tasks��6   $	��236��6@   $
��Bold"�6L   $��35��6^   $��	100 5 5 5q�6j   �� 5      -   5         ��Name$&  maxNumberOfConcurentTasksTextBox��5      $��73��6@   $��10 5 5 5q�6K   $��20��6]   5      �� 5   
   $ 
Horizontal=�6   $��	5,0,5,365q�6.   .+�5      �� $��Task Scheduler Settings��6   $��	220 5 5 5q�66   $
��Bold"�6I   $��14(�6[   5      �� 5   
   $ 
Horizontal=�6   $��0,164,0,191q�6.   .+�5      �� $��Scheduling Algorithm��6   $��	105 5 5 5q�63   $	��177��6F   $
��Bold"�6R   $��43��6d   �� 5      -   5         ��Name$! schedulingAlgorithmComboBox��5      $	��244��6=   $��70 5 5 5q�6I   $��27��6[   $
��Bold"�6g   5      �� 5   
   $ 
Horizontal=�6   $��0,259,5,117q�6.   .+�5      �� $��Time Slice [ms]��6   $	��250��6.   $��	108 5 5 5q�6:   $��34��6M   $
��Bold"�6Y   �� 5      -   5        $ timeSliceTextBox��5      $��73��61   $��20��6<   $
��1000��6H   5      �� 5   
   $��	5,0,5,347q�6   5      5       