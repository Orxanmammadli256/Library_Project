using Library.Business.Implementations;
using Library.Core.Entities;
using Library.Data;
using System.Globalization;
using System.Transactions;
using System.Xml.Linq;

Database database = new Database();
AuthorService authorService = new AuthorService(database);
GenreService genreService = new GenreService(database);
BookService bookService = new BookService(database);
BookRentalService bookRentalService = new BookRentalService(database);
bool isOptionActive = true; 
bool isOperationActive = true;
while (isOptionActive)
{
    Console.WriteLine("Choose the option: ");
    Console.WriteLine("Genre (genre)\t\tAuthor (author)\t\tBook (book)\t BookRental (bookrental)\tExit Program (exit)");
    string? entity = Console.ReadLine();
    if(string.IsNullOrEmpty(entity))
    {
        Console.WriteLine("Please choose one option");
        continue;
    }
    if(entity.ToLower() == "exit")
    {
        break;
    }
    if(entity.ToLower() != "genre" &&  entity.ToLower() != "author" && entity.ToLower() != "book" && entity.ToLower() != "exit" && entity.ToLower() != "bookrental")
    {
        Console.WriteLine("Please do not choose out of these options");
        continue;
    }
    while (isOperationActive)
    {
        Console.WriteLine("Choose the operation: ");
        Console.WriteLine();
        ChooseOperation(entity);
        string? operation = Console.ReadLine();
        if (string.IsNullOrEmpty(operation))
        {
            Console.WriteLine("Please choose one operation");
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
                        DeleteGenre(genreid, bookService);
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
        case ("author"):
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
                        DeleteAuthor(authorid, bookService);
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
        case ("book"):
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
                Console.WriteLine("Please do not choose out of these operations");
                Iscorrect = false;
            }
            break;
        case "author":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbyname" && operation != "showbysurname")
            {
                Console.WriteLine("Please do not choose out of these operations");
                Iscorrect = false;
            }
            break;
        case "book":
            if (operation != "create" && operation != "update" && operation != "delete" && operation != "showall" && operation != "showbytitle" && operation != "showbydate" && operation != "showbygenre" && operation != "showbyauthor")
            {
                Console.WriteLine("Please do not choose out of these operations");
                Iscorrect = false;
            }
            break;
    }
    return Iscorrect;
}

void ChooseOperation(string entity)
{
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
    }
}

#region Genre
void CreateGenre(string name)
{
    try
    {
        genreService.Create(name);
        Console.WriteLine($"{name} genre is created successfully");
    } 
    catch(Exception ex) 
    {
        Console.WriteLine(ex.Message);
    }
}

void DeleteGenre(int id, BookService bookService)
{
    try
    {
        genreService.Delete(id, bookService);
        Console.WriteLine("Genre is deleted");
    }
    catch(Exception ex )
    {
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
    }
}
void UpdateGenre(int id, string name)
{
    try
    {
        genreService.Update(id, name);
        Console.WriteLine("Genre is updated");
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
        Console.WriteLine($"Author {name} {surname} is created");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
void DeleteAuthor(int id, BookService bookService)
{
    try
    {
        authorService.Delete(id, bookService);
        Console.WriteLine("Author is deleted");
    }
    catch( Exception ex )
    {
       Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
    }
}
void UpdateAuthor(int id, string name, string? surname)
{
    try
    {
        authorService.Update(id, name, surname);
        Console.WriteLine("Author is updated");
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
#endregion
#region Book
void CreateBook(string title, int count, DateTime publicationdate, List<Genre> genres, List<Author> authors)
{
    try
    {
        bookService.Create(title, count, publicationdate, genres, authors);
        Console.WriteLine($"{title} book is created");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
void DeleteBook(Guid id)
{
    try
    {
        bookService.Delete(id);
        Console.WriteLine("Book is deleted");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
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
        Console.WriteLine(ex.Message);
    }
}
void UpdateBook(Guid id, string title, int count, DateTime publicationdate, List<Genre> genres, List<Author> authors)
{
    try
    {
        bookService.Update(id, title, count, publicationdate, genres, authors);
        Console.WriteLine("Book is updated");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
#endregion