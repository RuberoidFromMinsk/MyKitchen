   M S B A M L   `   `   `  ����G  CMyKitchenApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MyKitchenAppMyKitchenApp  5   	   "    MyKitchenApp.AddRecipeWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:MyKitchenApp     5   	     � Title$  AddRecipeWindow��5   	   $��462.829��6!   $��621.6��62   .��5	      � $��0,0,-6.4,3.2q�6   .+�5
   
   �� -   5         ��Name$ RecipeNameBox��5
      $
��Left=�6)   $��23��6D   $��
154,58,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��120��6�   �� 5   
   $
��Name��6   $
��Left=�6   $��
154,32,0,0q�6:   $	��Top=�6N   �� 5   
   $
��Left=�6   $��23��6-   $��154,113,0,0q�69   $
 Wrap=�6N   $	��Top=�6b   $	��120��6z   �� 5   
   -   5         ��Name$ PortionsBox��5      $��Portions��6%   $
��Left=�68   $��
156,87,0,0q�6S   $	��Top=�6g   �� 5   
   -   5         ��Name$ ProductItems��5      $
��Left=�6)   $��
66,168,0,0q�6D   $	��Top=�6X   $	��120��6p   �� 5   
   $��	Ingedient��6   $
��Left=�6$   $��
93,142,0,0q�6?   $	��Top=�6S   �� 5   
   -   5        $ DescriptionBox��5      $
��Left=�6*   $	��106��6E   $��114,264,0,0q�6R   $
 Wrap=�6g   $	��Top=�6{   $	��204��6�   �� 5   
   $1��+Here you can describe how to cook your dish��6   $
��Left=�6F   $��
93,242,0,0q�6a   $	��Top=�6u   $	��275��6�   �� 5   
   -   5        $ 	AmountBox��5      $
��Left=�6%   $��23��6@   $��206,168,0,0q�6L   $
 Wrap=�6a   $	��Top=�6u   $��84��6�   �� 5   
   $��Amount��6   $
��Left=�6!   $��221,142,0,0q�6<   $	��Top=�6Q   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ AvalibleProducts��5   +   $
��Left=�6E   $	��136��6`   $��
384,32,0,0q�6m   $	��Top=�6�   $	��143��6�   ��6   �� 6�   �� 5   
   $��Avalible products��6   $
��Left=�6,   $��	384,6,0,0q�6G   $	��Top=�6Z     5   
   -   5        $ SelectedProducts��5   +   $
��Left=�6E   $	��144��6`   $��384,226,0,0q�6m   $	��Top=�6�   $	��143��6�   ��6   �� 6�   �� 5   
   $��Selected products��6   $
��Left=�6,   $��384,195,0,0q�6G   $	��Top=�6\   �� 5   
   -   5        $��+��5      $
��Left=�6   $��309,155,0,0q�68   $	��Top=�6M   $��36��6e   $��36��6p   6|   �� 5   
   -	   5        $	��Add��5      $
��Left=�6   $��246,389,0,0q�6:   $	��Top=�6O   $	��133��6g   $��31��6s   6   �� 5   
   -
   5        $
��Back��5      $
��Left=�6    $	��Top=�6;   $��44��6S   $��26��6^   6j   5      5      