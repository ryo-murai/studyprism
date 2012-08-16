* WPF(Windows Presentation Foundation)とPrismで MVVMをやってみるサンプルアプリを置いてます
* 前に書いたBlogは[こちら](http://exceptionblend.wordpress.com/category/net/wpf/) 
* 環境
 * Visual Studio 2010
 * NuGet (このソリューションは NuGet 1.7.3を使用して作成しました)
 * Microsoft Ribbon for WPF ([Download](http://www.microsoft.com/en-us/download/details.aspx?id=11877))
 * Ribbonは WPF4.5にとりこまれる前に試していたソースのなごりです
* 導入手順
 * `git clone` か [ZIPダウンロード](https://github.com/ryo-murai/studyprism/zipball/master)でソースコード取得
 * ソリューションファイル(*.sln)をVisual Studioで開く
 * (設定が悪いせいなのですが)エラーがでて、開けないプロジェクトがあったりしても気にせず次の手順へ
 * ソリューション右クリックで「Enable NuGet Package Restore」を選択
 * 確認画面が出るので「はい」をクリック。するとソリューションフォルダ直下に.nugetフォルダとNuGet.exe、NuGet.targetsができるはず
 * ソリューション右クリックで今度は「Manage NuGet Packages for Solution」を選択
 * 出現する Manage NuGet Packagesウィンドウのヘッダ部に「Some NuGet packages are missing from this solution. Click to restore.」と表示されているところの「Restore」ボタンをクリックする
 * packagesフォルダが作られ、必要なDLLがそこにダウンロードされる。
 * ビルドできるはず
* 参考
 * NuGetについてはこちらの記事がわかりやすいです→ @IT [.NETで開発モジュール導入が楽々に！ NuGet入門](http://www.atmarkit.co.jp/fdotnet/chushin/nuget_01/nuget_01_01.html)
