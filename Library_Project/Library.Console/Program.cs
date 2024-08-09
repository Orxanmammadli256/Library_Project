using Library.Business.Implementations;
using Library.Core.Entities;
using Library.Data;
using System.Globalization;
using System.Transactions;
using System.Xml.Linq;

Database database = new Database();
BookService bookService = new BookService(database);
AuthorService authorService = new AuthorService(database,bookService);
GenreService genreService = new GenreService(database, bookService);
BookRentalService bookRentalService = new BookRentalService(database);
MemberService memberService = new MemberService(database,bookRentalService);
bool isOptionActive = true; 
bool isOperationActive = true;
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("Welcome to the Library");
Console.ResetColor();
while (isOptionActive)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("Choose the item to operate with: ");
    Console.ResetColor();
    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("Genre (genre)\t   Author (author)\t   Book (book)\t   Member (member)\t   RentalRecord (rentalrecord)\t   Exit Program (exit)");
    Console.WriteLine();
    Console.ResetColor();
    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine();
    string? entity = Console.ReadLine();
    if(string.IsNullOrEmpty(entity))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Please choose one option");
        Console.WriteLine();
        Console.ResetColor();
        continue;
    }
    if(entity.ToLower() == "exit")
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine();
        Console.WriteLine("Program is terminated");
        Console.WriteLine();
        Thread.Sleep(3000);
        Console.ResetColor();
        break;
    }
    if(entity.ToLower() != "genre" &&  entity.ToLower() != "author" && entity.ToLower() != "book" && entity.ToLower() != "exit" && entity.ToLower() != "rentalrecord" && entity.ToLower() != "member")
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Please do not choose out of these options");
        Console.WriteLine();
        Console.ResetColor();
        continue;
    }
    Console.WriteLine();
    while (isOperationActive)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Choose the operation: ");
        Console.WriteLine();
        Console.WriteLine("--------------------------------");
        Console.WriteLine();
        Console.WriteLine("--------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        ChooseOperation(entity);
        Console.ResetColor();
        Console.WriteLine("--------------------------------");
        Console.ResetColor();
        string? operation = Console.ReadLine();
        Console.WriteLine("--------------------------------");
        if (string.IsNullOrEmpty(operation))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please choose one operation");
            Console.WriteLine();
            Console.ResetColor();
            continue;
        }
        if (operation.ToLower() == "exit")
        {
            break;
        }
        if (!ValidateOperation(entity.ToLower(), operation.ToLower()))
        {
            continue;
        }
        ProcessOperation(entity.ToLower(),operation.ToLower());

    }
}

void ProcessOperation(string entity, string operation)
{
    start:
    switch (entity)
    {
        case "genre":
            switch (operation)
            {
                case ("create"):
                    Console.WriteLine("Enter genre name: ");
                    string genrename = Console.ReadLine() ?? string.Empty;
                    CreateGenre(genrename);
                    break;
                case ("delete"):
                    Console.WriteLine("Enter genre name to search: ");
                    genrename = Console.ReadLine() ?? string.Empty;
                    try
                    {
                        ShowAllGenreByName(genrename);
                        Console.WriteLine("Enter genre id to delete: ");
                        int genreid = int.Parse(Console.ReadLine() ?? string.Empty);
                        DeleteGenre(genreid);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case ("showall"):
                    ShowAllGenre();
                    break;
                case ("showbyname"):
                    Console.WriteLine("Enter genre name to search: ");
                    genrename = Console.ReadLine() ?? string.Empty;
                    ShowAllGenreByName(genrename);
                    break;
                case ("update"):
                    Console.WriteLine("Enter genre name to search: ");
                    genrename = Console.ReadLine() ?? string.Empty;
                    ShowAllGenreByName(genrename);
                    Console.WriteLine("Enter genre id to update: ");
                    try
                    {
                        int genreid = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter new genre name: ");
                        genrename = Console.ReadLine() ?? string.Empty;
                        UpdateGenre(genreid, genrename);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
            break;
        case "author":
            switch (operation)
            {
                case ("create"):
                    Console.WriteLine("Enter author name: ");
                    string authorname = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine("Enter author surname: ");
                    string? authorsurname = Console.ReadLine() ?? string.Empty;
                    CreateAuthor(authorname, authorsurname);
                    break;
                case ("delete"):
                    Console.WriteLine("Enter author name to search: ");
                    authorname = Console.ReadLine() ?? string.Empty;
                    ShowAllAuthorByName(authorname);
                    Console.WriteLine("Enter author id to delete: ");
                    try
                    {
                        int authorid = int.Parse(Console.ReadLine() ?? string.Empty);
                        DeleteAuthor(authorid);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case ("showall"):
                    ShowAllAuthor();
                    break;
                case ("showbyname"):
                    Console.WriteLine("Enter author name to search: ");
                    authorname = Console.ReadLine() ?? string.Empty;
                    ShowAllAuthorByName(authorname);
                    break;
                case ("showbysurname"):
                    Console.WriteLine("Enter author surname to search: ");
                    authorsurname = Console.ReadLine();
                    ShowAllAuthorBySurname(authorsurname);
                    break;
                case ("update"):
                    Console.WriteLine("Enter author name to search: ");
                    authorname = Console.ReadLine() ?? string.Empty;
                    ShowAllAuthorByName(authorname);
                    Console.WriteLine("Enter author id to update: ");
                    try
                    {
                        int authorid = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter new author name: ");
                        authorname = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Enter new author surname: ");
                        authorsurname = Console.ReadLine();
                        UpdateAuthor(authorid, authorname, authorsurname);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
            break;
        case "book":
            switch (operation)
            {
                case "create":
                    if (authorService.GetAll().Count == 0)
                    {
                        Console.WriteLine("Firstly, The creation of the author is needed");
                        entity = "author";
                        operation = "create";
                        goto start;
                    }
                    if(genreService.GetAll().Count == 0)
                    {
                        Console.WriteLine("Firstly, The creation of the genre is needed");
                        entity = "genre";
                        operation = "create";
                        goto start;
                    }
                    Console.WriteLine("Enter book title: ");
                    string title = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine("Enter book count: ");
                    try
                    {
                        int count = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter publication date (dd.MM.yyyy): ");
                        string date = Console.ReadLine() ?? string.Empty;
                        DateTime publicationdate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        ShowAllGenre();
                        Console.WriteLine("Enter genre ids with the space between them: ");
                        HashSet<int> genreidset = new HashSet<int>(Console.ReadLine().Split(" ").Select(int.Parse));
                        List<Genre> genres = new List<Genre>();
                        foreach (int id in genreidset)
                        {
                            var genre = genreService.GetById(id);
                            genres.Add(genre);
                        }
                        ShowAllAuthor();
                        Console.WriteLine("Enter author ids with the space between them: ");
                        HashSet<int> authoridset = new HashSet<int>(Console.ReadLine().Split(" ").Select(int.Parse));
                        List<Author> authors = new List<Author>();
                        foreach (int id in authoridset)
                        {
                            var author = authorService.GetById(id);
                            authors.Add(author);
                        }
                        CreateBook(title,count,publicationdate,genres,authors);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "delete":
                    Console.WriteLine("Enter title to search: ");
                    string booktitle = Console.ReadLine() ?? string.Empty;
                    ShowAllBookByTitle(booktitle);
                    Console.WriteLine("Enter book id to delete: ");
                    try
                    {
                        Guid bookid = Guid.Parse(Console.ReadLine() ?? string.Empty);
                        DeleteBook(bookid);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "showall":
                    ShowAllBook();
                    break;
                case "showbytitle":
                    Console.WriteLine("Enter title to search: ");
                    title = Console.ReadLine() ?? string.Empty;
                    ShowAllBookByTitle(title);
                    break;
                case "showbygenre":
                    ShowAllGenre();
                    Console.WriteLine("Enter genre id to search in the books: ");
                    try
                    {
                        int genreid = int.Parse(Console.ReadLine() ?? string.Empty);
                        var wantedgenre = genreService.GetById(genreid);
                        ShowAllBookByGenre(wantedgenre);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "showbyauthor":
                    ShowAllAuthor();
                    Console.WriteLine("Enter author id to search in the books: ");
                    try
                    {
                        int authorid = int.Parse(Console.ReadLine() ?? string.Empty);
                        var wantedauthor = authorService.GetById(authorid);
                        ShowAllBookByAuthor(wantedauthor);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "showbydate":
                    Console.WriteLine("Enter publication date (dd.MM.yyyy) to search: ");
                    try
                    {
                        string wanteddate = Console.ReadLine() ?? string.Empty;
                        DateTime wantedpublicationdate = DateTime.ParseExact(wanteddate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        ShowAllBookByPublicationDate(wantedpublicationdate);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "update":
                    ShowAllBook();
                    try
                    {
                        Console.WriteLine("Enter id to update: ");
                        Guid bookid = Guid.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter new title: ");
                        string newtitle = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Enter new count: ");
                        int newcount = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter new publication date (dd.MM.yyyy): ");
                        string newdate = Console.ReadLine() ?? string.Empty;
                        DateTime newpublicationdate = DateTime.ParseExact(newdate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        ShowAllGenre();
                        Console.WriteLine("Enter new genre ids with the space between them: ");
                        HashSet<int> genreidset = new HashSet<int>(Console.ReadLine().Split(" ").Select(int.Parse));
                        List<Genre> genres = new List<Genre>();
                        foreach (int id in genreidset)
                        {
                            var genre = genreService.GetById(id);
                            genres.Add(genre);
                        }
                        ShowAllAuthor();
                        Console.WriteLine("Enter new author ids with the space between them: ");
                        HashSet<int> authoridset = new HashSet<int>(Console.ReadLine().Split(" ").Select(int.Parse));
                        List<Author> authors = new List<Author>();
                        foreach (int id in authoridset)
                        {
                            var author = authorService.GetById(id);
                            authors.Add(author);
                        }
                        UpdateBook(bookid, newtitle, newcount, newpublicationdate, genres, authors);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
            break;
        case "member":
            switch (operation)
            {
                case "create":
                    Console.WriteLine("Enter member name: ");
                    string membername = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine("Enter member surname: ");
                    string? membersurname = Console.ReadLine();
                    Console.WriteLine("Enter member pin: ");
                    string memberpin = Console.ReadLine() ?? string.Empty;
                    CreateMember(membername,memberpin,membersurname);
                    break;
                case "delete":
                    try
                    {
                        Console.WriteLine("Enter member name to search: ");
                        membername = Console.ReadLine() ?? string.Empty;
                        ShowAllMemberByName(membername);
                        Console.WriteLine("Enter member id to delete: ");
                        int memberid = int.Parse(Console.ReadLine() ?? string.Empty);
                        DeleteMember(memberid);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "showall":
                    ShowAllMember();
                    break;
                case "showbyname":
                    Console.WriteLine("Enter member name to search: ");
                    membername = Console.ReadLine() ?? string.Empty;
                    ShowAllMemberByName(membername);
                    break;
                case "showbysurname":
                    Console.WriteLine("Enter member surname to search: ");
                    membersurname = Console.ReadLine() ?? string.Empty;
                    ShowAllMemberBySurname(membersurname);
                    break;
                case "update":
                    try
                    {
                        Console.WriteLine("Enter member name to search: ");
                        membername = Console.ReadLine() ?? string.Empty;
                        ShowAllMemberByName(membername);
                        Console.WriteLine("Enter member id to update: ");
                        int memberid = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter new member name: ");
                        membername = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Enter new member surname: ");
                        membersurname = Console.ReadLine() ?? string.Empty;
                        UpdateMember(memberid, membername, membersurname);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "loan":
                    if(bookService.GetAll().Count == 0)
                    {
                        Console.WriteLine("Creation of book is needed");
                        entity = "book";
                        operation = "create";
                        goto start;
                    }
                    if(memberService.GetAll().Count == 0)
                    {
                        Console.WriteLine("Creation of member is needed");
                        goto case "create";
                    }
                    try
                    {
                        Console.WriteLine("Enter member name to search: ");
                        membername = Console.ReadLine() ?? string.Empty;
                        ShowAllMemberByName(membername);
                        Console.WriteLine("Enter member pin: ");
                        memberpin = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Enter book title to search: ");
                        string booktitle = Console.ReadLine() ?? string.Empty;
                        ShowAllBookByTitle(booktitle);
                        Console.WriteLine("Enter book id to rent: ");
                        Guid bookid = Guid.Parse(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine("Enter borrow date (dd.MM.yyyy): ");
                        DateTime borrowdate = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        Console.WriteLine("Enter return date (dd.MM.yyyy): ");
                        DateTime returndate = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        LoanBook(memberpin,bookid,borrowdate,returndate);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "return":
                    if(bookRentalService.GetAll().Count == 0)
                    {
                        Console.WriteLine("There is no book rental record");
                        break;
                    }
                    try
                    {
                        Console.WriteLine("Enter member name to search: ");
                        membername = Console.ReadLine() ?? string.Empty;
                        ShowAllMemberByName(membername);
                        Console.WriteLine("Enter member pin: ");
                        memberpin = Console.ReadLine() ?? string.Empty;
                        ReturnBook(memberpin);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }
            break;
        case "rentalrecord":
            switch (operation)
            {
                case "update":
                    ShowAllRentalRecord();
                    Console.WriteLine("Enter rental record id: ");
                    int id = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter member name to search: ");
                    string membername = Console.ReadLine() ?? string.Empty;
                    ShowAllMemberByName(membername);
                    Console.WriteLine("Enter new member id: ");
                    int memberid = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter book title to search: ");
                    string title = Console.ReadLine() ?? string.Empty;
                    ShowAllBookByTitle(title);
                    Console.WriteLine("Enter new book id: ");
                    Guid bookid = Guid.Parse(Console.ReadLine() ?? string.Empty);
                    Console.WriteLine("Enter new borrow date (dd.MM.yyyy): ");
                    DateTime borrowdate = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    Console.WriteLine("Enter new return date (dd.MM.yyyy): ");
                    DateTime returndate = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    UpdateRentalRecord(id, memberid, bookid, borrowdate, returndate);
                    break;
                case "showall":
                    ShowAllRentalRecord();
                    break;
                case "showexpired":
                    ShowAllExpiredRentalRecord();
                    break;
            }
            break;
    }
}

bool ValidateOperation(string entity, string operation)
{
    bool Iscorrect = true;
    switch (entity)
    {
        case "genre":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbyname")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please do not choose out of these operations");
                Console.WriteLine();
                Console.ResetColor();
                Iscorrect = false;
            }
            break;
        case "author":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbyname" && operation != "showbysurname")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please do not choose out of these operations");
                Console.WriteLine();
                Console.ResetColor();
                Iscorrect = false;
            }
            break;
        case "book":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbytitle" && operation != "showbydate" && operation != "showbygenre" && operation != "showbyauthor")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please do not choose out of these operations");
                Console.WriteLine();
                Console.ResetColor();
                Iscorrect = false;
            }
            break;
        case "member":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbyname" && operation != "showbysurname" && operation != "loan" && operation != "return")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please do not choose out of these operations");
                Console.WriteLine();
                Console.ResetColor();
                Iscorrect = false;
            }
            break;
        case "rentalrecord":
            if (operation != "update" && operation != "showall" && operation != "showexpired")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please do not choose out of these operations");
                Console.WriteLine();
                Console.ResetColor();
                Iscorrect = false;
            }
            break;
    }
    return Iscorrect;
}

void ChooseOperation(string entity)
{
    Console.WriteLine();
    switch (entity.ToLower())
    {
        case "genre":
            Console.WriteLine("Create genre (create)");
            Console.WriteLine("Update genre (update)");
            Console.WriteLine("Delete genre (delete)");
            Console.WriteLine("Show genres (showall)");
            Console.WriteLine("Show genres by name (showbyname)");
            Console.WriteLine("Exit (exit)");
            break;
        case "author":
            Console.WriteLine("Create author (create)");
            Console.WriteLine("Update author (update)");
            Console.WriteLine("Delete author (delete)");
            Console.WriteLine("Show authors (showall)");
            Console.WriteLine("Show authors by name (showbyname)");
            Console.WriteLine("Show authors by surname (showbysurname)");
            Console.WriteLine("Exit (exit)");
            break;
        case "book":
            Console.WriteLine("Create book (create)");
            Console.WriteLine("Update book (update)");
            Console.WriteLine("Delete book (delete)");
            Console.WriteLine("Show books (showall)");
            Console.WriteLine("Show books by title (showbytitle)");
            Console.WriteLine("Show books by publication date (showbydate)");
            Console.WriteLine("Show books by author (showbyauthor)");
            Console.WriteLine("Show books by genre (showbygenre)");
            Console.WriteLine("Exit (exit)");
            break;
        case "member":
            Console.WriteLine("Create member (create)");
            Console.WriteLine("Update member (update)");
            Console.WriteLine("Delete member (delete)");
            Console.WriteLine("Show members (showall)");
            Console.WriteLine("Show members by name (showbyname)");
            Console.WriteLine("Show members by surname (showbysurname)");
            Console.WriteLine("Loan book (loan)");
            Console.WriteLine("Return book (return)");
            Console.WriteLine("Exit (exit)");
            break;
        case "rentalrecord":
            Console.WriteLine("Update rental record (update)");
            Console.WriteLine("Show rentall records (showall)");
            Console.WriteLine("Show expired rentall records (showexpired)");
            Console.WriteLine("Exit (exit)");
            break;
        }
    Console.WriteLine();
}

#region Genre
void CreateGenre(string name)
{
    try
    {
        genreService.Create(name);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{name} genre is created successfully");
        Console.ResetColor();
    } 
    catch(Exception ex) 
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}

void DeleteGenre(int id)
{
    try
    {
        genreService.Delete(id);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Genre with {id} Id is successfully deleted");
        Console.ResetColor();
    }
    catch(Exception ex )
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}

void ShowAllGenre()
{
   var genres = genreService.GetAll();
   Console.WriteLine("Genres");
    foreach (var genre in genres)
    {
        Console.WriteLine(genre);
    }
}
void ShowAllGenreByName(string name)
{
    try
    {
        var genres = genreService.SearchByName(name);
        Console.WriteLine("Genres");
        foreach (var genre in genres)
        {
            Console.WriteLine(genre);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
    }
}
void UpdateGenre(int id, string name)
{
    try
    {
        genreService.Update(id, name);
        Console.WriteLine($"Genre with {id} Id is updated");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
#endregion
#region Author
void CreateAuthor(string name, string? surname)
{
    try
    {
        authorService.Create(name,surname);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Author {name} {surname} is successfully created");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void DeleteAuthor(int id)
{
    try
    {
        authorService.Delete(id);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Author with {id} Id is  successfully deleted");
        Console.ResetColor();
    }
    catch( Exception ex )
    {
       Console.ForegroundColor = ConsoleColor.Red;
       Console.WriteLine(ex.Message);
       Console.ResetColor();
    }

}
void ShowAllAuthor()
{
    var authors = authorService.GetAll();
    Console.WriteLine("Authors");
    foreach (var author in authors)
    {
        Console.WriteLine(author);
    }
}
void ShowAllAuthorByName(string name)
{
    try
    {
        var authors = authorService.SearchByName(name);
        Console.WriteLine("Authors");
        foreach (var author in authors)
        {
            Console.WriteLine(author);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllAuthorBySurname(string? surname)
{
    try
    {
        var authors = authorService.SearchBySurname(surname);
        Console.WriteLine("Authors");
        foreach (var author in authors)
        {
            Console.WriteLine(author);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void UpdateAuthor(int id, string name, string? surname)
{
    try
    {
        authorService.Update(id, name, surname);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Author with {id} Id is successfully updated");
        Console.ResetColor();
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
#endregion
#region Book
void CreateBook(string title, int count, DateTime publicationdate, List<Genre> genres, List<Author> authors)
{
    try
    {
        bookService.Create(title, count, publicationdate, genres, authors);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{title} book is successfully created");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void DeleteBook(Guid id)
{
    try
    {
        bookService.Delete(id);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Book with {id} Id is deleted");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllBook()
{
    var books = bookService.GetAll();
    Console.WriteLine("Books");
    foreach (var book in books)
    {
        Console.WriteLine(book);
    }
}
void ShowAllBookByTitle(string title)
{
    try
    {
        var books = bookService.SearchByTitle(title);
        Console.WriteLine("Books");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllBookByPublicationDate(DateTime publicationdate)
{
    try
    {
        var books = bookService.SearchByPublicationDate(publicationdate);
        Console.WriteLine("Books");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllBookByAuthor(Author author)
{
    try
    {
        var books = bookService.SearchByAuthor(author);
        Console.WriteLine("Books");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}

void ShowAllBookByGenre(Genre genre)
{
    try
    {
        var books = bookService.SearchByGenre(genre);
        Console.WriteLine("Books");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void UpdateBook(Guid id, string title, int count, DateTime publicationdate, List<Genre> genres, List<Author> authors)
{
    try
    {
        bookService.Update(id, title, count, publicationdate, genres, authors);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Book with {id} Id is updated");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
#endregion
#region Member
void CreateMember(string name, string pin, string? surname = null)
{
    try
    {
        memberService.Create(name, pin, surname);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Member {name} {surname} is created successfully");
        Console.ResetColor();
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void DeleteMember(int id)
{
    try
    {
        memberService.Delete(id);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Member with {id} Id is successfully deleted");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void UpdateMember(int id, string name, string? surname = null)
{
    try
    {
        memberService.Update(id, name, surname);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Member with {id} Id is successfully updated");
        Console.ResetColor();
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllMember()
{
    var members = memberService.GetAll();
    Console.WriteLine("Members");
    foreach (var member in members)
    {
        Console.WriteLine(member);
    }
}
void ShowAllMemberByName(string name)
{
    try
    {
        var members = memberService.SearchByName(name);
        Console.WriteLine("Members");
        foreach(var member in members)
        {
            Console.WriteLine(member);
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllMemberBySurname(string? surname)
{
    try
    {
        var members = memberService.SearchBySurname(surname);
        Console.WriteLine("Members");
        foreach( var member in members)
        {
            Console.WriteLine(member);
        }
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void LoanBook(string pin, Guid bookid, DateTime borrowdate, DateTime returndate)
{
    try
    {
        memberService.LoanBook(pin, bookid, borrowdate, returndate);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The book is successfully rented");
        Console.ResetColor();
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ReturnBook(string pin)
{
    try
    {
        memberService.ReturnBook(pin);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The book is returned successfully");
        Console.ResetColor();
    }
    catch(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
#endregion
#region BookRental
void UpdateRentalRecord(int id, int memberid, Guid bookid, DateTime borrowdate, DateTime returndate)
{
    try
    {
        bookRentalService.Update(id, memberid, bookid, borrowdate, returndate);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Rental record with {id} Id is updated");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }
}
void ShowAllRentalRecord()
{
    var rentalrecords = bookRentalService.GetAll();
    Console.WriteLine("Rental Records");
    foreach(var record in rentalrecords)
    {
        Console.WriteLine(record);  
    }
}
void ShowAllExpiredRentalRecord()
{
    var rentalrecords = bookRentalService.ExpiredGetAll();
    Console.WriteLine("Expired Rental Records");
    foreach (var record in rentalrecords)
    {
        Console.WriteLine(record);
    }
}
#endregion