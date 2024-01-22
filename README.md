Do uruchomienia projektu należy mieć zainstalowaną najnowszą wersje Visual Studio oraz .Net8 oraz SQL Server Menagement Studio 19

!!!  w folderze Data w pliku ShopContext.cs należy zmienić ścieżkę optionsBuilder.UseSqlServer("Server=DESKTOP-SMPHPSP\\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;TrustServerCertificate=True;"); na własny server SQL a następnie wybrać z zakładki Tools/Narzędzia NuGet Package Manager i Pckage Manager Console i wpisać komendy dotnet ef migrations add oraz obojetnie jaka nazwa a następnie dotnet ef database update. !!!

Ponieważ mieliśmy problemy z podłączeniem FrontEnd z BackEnd to można wszystkie funkcje przetestować za pomocą Swagger wystarczy w folderze Properties/launchSettings.json usunąć komentarze przed //"launchUrl": "swagger".

