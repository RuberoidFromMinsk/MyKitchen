   M S B A M L   `   `   `  ����G  CMyKitchenApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MyKitchenAppMyKitchenApp  5   	   #    MyKitchenApp.AddProductWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:MyKitchenApp     5   	     � Title$  Add product��5   	   $��483.214��6   $��317.857��6.   
�������.��5	      � $��	0,0,3,1.6q�6    � ColumnDefinitions 5
   
   �� 5      �� 5      $��0* �6   .+�5   
   �� -   5         ��Name$ ProductNameBox��5      $
��Left=�6*   $��34��6E   $��
98,129,0,0q�6Q    �� TextWrapping$
 Wrap=�6e   $�� ��6y   $	��Top=�6�   $	��120��6�   $��14(�6�   �� 5   
   -   5        $ 	AmountBox��5      $
��Left=�6%   $��35��6@   $��
98,186,0,0q�6L   $
 Wrap=�6`   $	��Top=�6t   $	��120��6�   $��14(�6�   �� 5   
   -   5        $ CalorificBox��5      $
��Left=�6(   $��31��6C   $��
98,251,0,0q�6O   $
 Wrap=�6c   $	��Top=�6w   $	��120��6�   $��14(�6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ PurchaseDateBox��5      $
��Left=�6.   $��
97,318,0,0q�6I   $	��Top=�6]   $��26��6u   $	��120��6�   5      �� 5   
   $��Product name��6   $
��Left=�6'   $��
110,98,0,0q�6B   $	��Top=�6V   $��26��6n   $	��104��6z   $��14(�6�   �� 5   
   $��Amount��6   $
��Left=�6!   $��125,160,0,0q�6<   $	��Top=�6Q   $��26��6i   $��68��6u   $��14(�6�   �� 5   
   $��	Calorific��6   $
��Left=�6$   $��123,224,0,0q�6?   $	��Top=�6T   $��26��6l   $��70��6x   $��14(�6�   �� 5   
   $��Purchase date��6   $
��Left=�6(   $��106,287,0,0q�6C   $	��Top=�6X   $��26��6p   $	��104��6|   $��14(�6�   �� 5   
   $��You can add your product!��6   $
��Left=�64   $��	46,55,0,0q�6O   $	��Top=�6b   $��43��6z   $	��228��6�   $��DemiBold"�6�   $��18(�6�   �� 5   
   -   5        $	��Add��5      $
��Left=�61   $��
97,387,0,0q�6L   $	��Top=�6`   $	��120��6x   $��41��6�   $��18(�6�   
�������
���"�"�$��Century Gothic)�6�   �� 5   
   -   5        $�� ��5      $
��Left=�6   $	��Top=�67   $��54��6O   $��42��6Z   $��2��6y   
���������5      �� 5      .R�5      �� $��Img\back.png��6   5      5       5"      5#      