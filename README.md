Do uruchomienia projektu należy mieć zainstalowaną najnowszą wersje Visual Studio oraz .Net8


Aby uruchomić bazę danych należy zainstalować SQLServer a następnie wpisać komendy dotnet ef migrations add oraz obojetnie jaka nazwa a następnie dotnet ef database update.
Ponieważ mieliśmy problemy z podłączeniem HTML z Api to można wszystkie funkcje przetestować za pomocą Swagger wystarczy w folderze Properties/launchSettings.json usunąć komentarze przed //"launchUrl": "swagger".