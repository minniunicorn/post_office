   M S B A M L   `   `   `  ����F  Bpost_office, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:post_officepost_office  5   	       post_office.Workers   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:post_office     5   	     � Title$  Workers��5   	    � WindowState$ 	Maximized=�6    � Icon$ /Image/logo1.png��61   $
��1000��6I   $
��1920��6Z   .��5	      � ��5
   
   �� 5       �� ImageSource$ /Image/back2.jpg��6    �� Opacity$
 0.35Q�68    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ employeeDataGrid��5      $��1��6-   $��18(�6:      AutoGenerateColumns .     CanUserAddRows .  	   
IsReadOnly	 . 
   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 * 5      $y�Visible=�6#    � 	Resources 5         CenteredColumnHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value��%V   &	        �� 5      "  6:   .�5      ��  � ��$ Center=�6C   5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width$
 0.5* 6H      HeaderStyle# [  6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *ID+6U     5      $ Фамилия��6%   # [  6R    66   �� *	Surname+6R     5      $ Имя��6%   # [  6K    62   �� *Name+6K     5      $ Отчество��6%   # [  6T    67   �� *
Lastname+6T     5      $ Должность��6%   # [  6U    68   �� *
Position+6U     5      $ Телефон��6%   # [  6P    66   �� *Phone+6P     5      $ 
Логин��6%   # [  6N    64   �� *Login+6N     5       $ Пароль��6%   # [  6R    65   �� *
Password+6R     5!      $
 Root��6%   $
 0.5* 6L   # [  6Y    63   �� *Root+6Y   5#        5%   
   -   5        $ logsDataGrid��5%      $��1��6)   $��18(�66    .   .  	 . $
 * 5&      $y�Hidden=�6#    5'      ��%V   &	        �� 5(      "  6:   .�5)      ��  � ��$ Center=�6C   5*      5+       5,        5-      $ ID��6%   $
 0.5* 6H   # [  6U    61   �� *id+6U     5.      $ Дата и время��6%   # [  6X    6;   �� *
datetime+6X     5/      $ Действие��6%   # [  6R    67   �� *action+6R     50      $ ФИО работника��6%   # [  6\    6<   �� *worker_name+6\   52      �� 53   
   -   5        $��gotomain��53      $9��3Вернуться к выбору операций��6!   $��10q�6G   $��3q�6S   $
��Left=�6_   $��Center=�6z   $��0��54      $��2��6   $��20(�62   $��Montserrat Alternates)�6@   ���	#FF2B92CD
����������	#FF2B92CD55   <   �� 56   
   -   5        $��openLogButton��56      $!��Открыть журнал��6&   $��10q�6?   $��3q�6K   $��Right=�6W   $��Center=�6s   $��0��57      $��20(�6   $��Montserrat Alternates)�6,   ���	#FF2B92CD
����������	#FF2B92CD58   <   5:      5;      