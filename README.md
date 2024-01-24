Do uruchomienia projektu należy mieć zainstalowaną najnowszą wersje Visual Studio oraz .Net8 oraz SQL Server Menagement Studio 19

!!!  w folderze Data w pliku ShopContext.cs należy zmienić ścieżkę optionsBuilder.UseSqlServer("Server=DESKTOP-SMPHPSP\\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;TrustServerCertificate=True;"); na własny server SQL a następnie wybrać z zakładki Tools/Narzędzia NuGet Package Manager i Pckage Manager Console i wpisać komendy dotnet ef migrations add oraz obojetnie jaka nazwa a następnie dotnet ef database update. !!!

Ponieważ mieliśmy problemy z podłączeniem FrontEnd z BackEnd to można wszystkie funkcje przetestować za pomocą Swagger wystarczy w folderze Properties/launchSettings.json usunąć komentarze przed //"launchUrl": "swagger" a przy "launchUrl": "index.html", dodać zakomentowanie.

Aby ustawić urzytkownika jako Admin należy w bazie danych w Tabeli Users zmienić UserRole z 0 na 1 gdzyż domyślna wartoś 0 to Użytkownik a 1 to Admin
Aby przetestować funkcje autoryzacji po zalogowaniu w Swagger w zakładce User/Login należy skopiować Token i za pomocą przycisku do góry strony Authorize należy wpisać komende bearer oraz wkleić token.

Po rejestracji użytkownika ważne jest aby zweryfikować konto za pomocą linku który jest wysyłany na email podany w formularzu oraz W kodzie w linijce 165 w pliku UserController należy zmienić https://localhost:7157/api/User/verify?token={token} ...localhost:(na własne lokalne ip)/api/User...
