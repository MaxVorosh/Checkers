   M S B A M L   `   `   `  ????C  ?Checkers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CheckersCheckers  5   	       Checkers.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Checkers     5   	     ? Title$  Checkers??5   	   $	??200??6   $	??400??6'    ? 
ResizeMode$ NoResize=?63   .??5	      ? -   5         ?Name$ MainGrid??5	      
??????? ? RowDefinitions 5
   
   ?? 5      $
??0.4* ?6   ?? 5      $
??0.1* ?6   ?? 5      $
??0.5* ?6    ? ColumnDefinitions 5   
   ?? 5      ?? 5      ?? 5      .+?5   
   ?? -   5        $??2??5      $??1??6   $??Start??6.   $??22(?6>   $??75??6L   $??50??6W   6c   ?? 5   
   -   5         ??Name$ DifficultStackPanel??5   2   $??0??6   $??1??6"   .+?5      	? -   5         	? 	GroupName$ 	Difficult??5      $
??Easy??60    d? 	IsChecked$
 TrueY?6?   6P   	? 5      -   5        $ 	Difficult??5      $??Medium??60   6A   	? 5      -   5        $ 	Difficult??5      $
??Hard??60   6?   5      ?? 5   
   -   5        $ ModeStackPanel??5   2   $??0??6   $??0??6"   .+?5      	? -   5        $
 Mode??5      $??1 player??6+   $
 TrueY?6>   6O   	? 5      -	   5        $
 Mode??5      $??	2 players??6+   6?   5      ?? 5   
   -
   5        $ GameplayPanel??5   2   $??0??6   $??2??6"   .+?5      	? -   5        $ Gameplay??5      $??Russian??6/   $
 TrueY?6A   6R   	? 5       -   5        $ Gameplay??5       $??Giveaway??6/   6B   	? 5!      -   5        $ Gameplay??5!      $??Pool checkers??6/   6G   5"      ?? 5#   
   -   5        $ Styles??5#   F   $??1??6   $??0??6"   $??3??62    ?? Orientation$ 
Horizontal=?6V   $??Center=?6o   .+?5$      	? -   5        $ Styles??5$      $??90??6-   $??Green Style??6;   $
 TrueY?6Q   6b   	? 5%      -   5        $ Styles??5%      $??90??6-   $??Brown Style??6;   6Q   	? 5&      -   5        $ Styles??5&      $??90??6-   $??
Blue Style??6;   6P   5'      5(      5)      