# CapriciousUI

本リポジトリは、Liberfy等で開発していたUI周りの実装のライブラリ化したものです。  
現時点ではWPF用のみ用意していますが、将来的には他のUIフレームワーク(Uno Platform等)にも対応していきたいと思っています。

## テーマ
現時点で`Light`のみですが、`Dark`も実装する予定です。

## WPFでの使用方法
1. 任意のWPFプロジェクトに`CapriciousUI.Wpf`への参照を追加する。
2. App.xamlで`ThemeManager.Theme`を設定する。
3. プロジェクトを再ビルドする。

以上の手順を実施後、WPFアプリケーションを起動するとUIの外観が変更されているかと思います。

App.xamlの変更例:
```xml:App.xaml
<Application x:Class="SampleProject.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:SampleProject"
             StartupUri="MainWindow.xaml"
             xmlns:theme="clr-namespace:CapriciousUI.Wpf;assembly=CapriciousUI.Wpf"
             theme:ThemeManager.Theme="Light">>
..省略..
</Application>

```
