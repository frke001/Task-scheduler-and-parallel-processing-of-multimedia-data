   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI  5   	       GUI.StartupWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:GUI     5   	   -   5          � Title$  Startup settings��5   	   $	��398��6"   $	��531��6/   
������� � WindowStartupLocation$ CenterScreen=�6N   .��5
      � ���#555555 � RowDefinitions 5   
   �� 5      $
��0.5* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��5* �6   �� 5      $��5* �6   �� 5      $��1* �6   .+�5   
   �� $��0��6   $��0��6"   $��4��62   $��Stretch=�6F   $��Center=�6d   .+�5      �� $
��Auto��6   $��Task Scheduler Settings��6"   $��Center=�6D      CustomLabel#��[  6a   $��18(�6�   �� 5      $
��Auto��6   $��Stretch=�6&   5      �� 5   
   $��1��6   $��1��6   $��Concurent tasks number:��6-   #��[  6O   $
��Auto��6u   $
��Bold"�6�   $��Center=�6�   $
��Left=�6�   �� 5    
   -   5         ��Name$&  maxNumberOfConcurentTasksTextBox��5    /   $��1��6   $��2��6     CustomInput#��[ 6Y   $��Center=�6~   $
��Auto��6�   $
��Auto��6�   $��Center=�6�   $	��200��6�   $��24��6�   �� 5!   
   $��2��6   $��1��6   $��Scheduling algorithm:��6-   #��[  6N   $
��Auto��6t   $
��Auto��6�   $��Center=�6�   $
��Left=�6�   $
��Bold"�6�   �� 5"   
   -   5         ��Name$! schedulingAlgorithmComboBox��5"   1   $��2��6   $��2��6    #��[ 6W   $	��200��6|   $��24��6�   $
��Auto��6�   $
��Auto��6�   $��Center=�6�   $
��Bold"�6�   �� 5#   
   $��3��6   $��1��6   $��Time slice [ms]:��6-   #��[  6H   $
��Auto��6m   $
��Auto��6z   $��Center=�6�   $
��Left=�6�   $
��Bold"�6�   �� 5$   
   -   5        $ timeSliceTextBox��5$   /   $��3��6   $��2��6   $	��200��6I   #��[ 6X   $��Center=�6}   $��24��6�   $
��1000��6�   $
��Auto��6�   $
��Auto��6�   $��Center=�6�   �� 5%   
   -   5         ��Name$ startButton��5%   B   $��4��6   $��0��6   $��4��6.   $	��161��6W   $��29��6f   $��Center=�6u   $��Center=�6�   $��Start��6�     CustomButton#��[ 6�    � Cursor$
 Hand��6�   5(      5+      