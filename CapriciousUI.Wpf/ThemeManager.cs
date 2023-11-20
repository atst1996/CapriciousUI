using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;

namespace CapriciousUI.Wpf;

/// <summary>
/// テーママネージャ
/// </summary>
public static class ThemeManager
{
    private static readonly ConcurrentDictionary<Application, ThemeResource> _resources = new();
    /// <summary>EditorContextMenuの型(type)</summary>
    private static Type EditorContextMenuType { get; }
        = Type.GetType($"System.Windows.Documents.TextEditorContextMenu+EditorContextMenu,{typeof(Application).Assembly.FullName}")!;

    /// <summary>
    /// アプリケーションに適用しているテーマ情報
    /// </summary>
    private class ThemeResource(Theme Theme, ResourceDictionary ResourceDictionary)
    {
        /// <summary>テーマ</summary>
        public Theme Theme { get; } = Theme;

        /// <summary>ResourceDictionary</summary>
        public ResourceDictionary Resources { get; } = ResourceDictionary;
    }

    /// <summary>
    /// 現在アプリケーションに適用中のリソースを取得する。
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static Theme? GetTheme(Application app)
    {
        _ = GetCurrentThemeResource(app, out var resource);
        return resource?.Theme;
    }

    /// <summary>
    /// アプリケーションにリソースを適用する。
    /// </summary>
    /// <param name="app"></param>
    /// <param name="theme"></param>
    public static void SetTheme(Application app, Theme? theme)
    {
        bool isThemed = GetCurrentThemeResource(app, out var prevTheme);

        var dictionaries = app.Resources.MergedDictionaries;
        bool removePrevTheme = false;

        // MEMO: 
        // テーマの変更は、意図せずデフォルトテーマに戻るのを防ぐため、
        // 新しいテーマのリソースを追加したあとに古いテーマのリソースを削除する。

        if (theme is { } newTheme)
        {
            // テーマが未適用または適用中のテーマと異なる場合は、新しいテーマを適用する
            if (!isThemed || prevTheme.Theme != theme)
            {
                var themeResource = GetThemeResource(newTheme);

                app.Resources.MergedDictionaries.Add(themeResource.Resources);
                _resources.AddOrUpdate(app, themeResource, (arg1, arg2) => themeResource);

                // 適用テーマが変更される場合は、現在適用中のテーマを削除する
                removePrevTheme = prevTheme != null && prevTheme.Theme != theme;
            }
        }
        else
        {
            // 次に適用するテーマが未指定の場合は、現在適用中のテーマを解除する
            removePrevTheme = isThemed;
        }

        // 適用中のリソースを削除する
        if (removePrevTheme)
        {
            foreach (var resource in dictionaries)
            {
                if (ReferenceEquals(resource, prevTheme!.Resources))
                {
                    dictionaries.Remove(resource);
                    _ = _resources.TryRemove(app, out _);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 現在適用中のテーマ情報を取得する。
    /// </summary>
    /// <param name="app">適用先アプリケーション</param>
    /// <param name="themeResource">テーマ情報</param>
    /// <returns>テーマの有無</returns>
    private static bool GetCurrentThemeResource(Application app, out ThemeResource themeResource)
        => _resources.TryGetValue(app, out themeResource!);

    /// <summary>
    /// テーマ情報を取得する。
    /// </summary>
    /// <param name="theme">テーマ</param>
    /// <returns>テーマ情報</returns>
    private static ThemeResource GetThemeResource(Theme theme)
    {
        // ResourceDictionaryを生成
        var resDict = new ResourceDictionary();

        // テーマリソースを追加
        resDict.MergedDictionaries.Add(new() { Source = GetResourceUri("Themes/Light.xaml") });

        // 個別のリソースを追加
        // TextBoxのコンテキストメニューにテーマを適用する
        resDict[EditorContextMenuType] = resDict[typeof(ContextMenu)];

        return new(theme, resDict);
    }

    /// <summary>
    /// 内部リソースのURIを取得する。
    /// </summary>
    /// <param name="path">相対パス</param>
    /// <returns>リソースのURI</returns>
    private static Uri GetResourceUri(string path)
        => new($"pack://application:,,,/CapriciousUI.Wpf;component/{path}");
}
