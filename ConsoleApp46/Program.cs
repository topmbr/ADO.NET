using System;
using System.Data;
using System.Data.SQLite;
namespace ConsoleApp46
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Data Source=LibraryDB.sqlite;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание таблицы Books
                using (SQLiteCommand createTableCmd = new SQLiteCommand(
                    "CREATE TABLE IF NOT EXISTS Books (" +
                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Title TEXT, " +
                    "Author TEXT, " +
                    "PublicationYear INTEGER, " +
                    "Genre TEXT, " +
                    "ISBN TEXT);", connection))
                {
                    createTableCmd.ExecuteNonQuery();
                }

                while (true)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Добавить новую книгу");
                    Console.WriteLine("2. Просмотреть список всех книг");
                    Console.WriteLine("3. Поиск книги");
                    Console.WriteLine("4. Обновить информацию о книге");
                    Console.WriteLine("5. Удалить книгу");
                    Console.WriteLine("0. Выход");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            CreateBook(connection);
                            break;
                        case 2:
                            ReadAllBooks(connection);
                            break;
                        case 3:
                            SearchBook(connection);
                            break;
                        case 4:
                            UpdateBook(connection);
                            break;
                        case 5:
                            DeleteBook(connection);
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("Некорректный выбор.");
                            break;
                    }
                }
            }
        }

        static void CreateBook(SQLiteConnection connection)
        {
            Console.WriteLine("Введите название книги:");
            string title = Console.ReadLine();

            Console.WriteLine("Введите автора книги:");
            string author = Console.ReadLine();

            Console.WriteLine("Введите год публикации:");
            int publicationYear = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите жанр книги:");
            string genre = Console.ReadLine();

            Console.WriteLine("Введите ISBN книги:");
            string isbn = Console.ReadLine();

            using (SQLiteCommand insertCmd = new SQLiteCommand(
                "INSERT INTO Books (Title, Author, PublicationYear, Genre, ISBN) " +
                "VALUES (@Title, @Author, @PublicationYear, @Genre, @ISBN);", connection))
            {
                insertCmd.Parameters.AddWithValue("@Title", title);
                insertCmd.Parameters.AddWithValue("@Author", author);
                insertCmd.Parameters.AddWithValue("@PublicationYear", publicationYear);
                insertCmd.Parameters.AddWithValue("@Genre", genre);
                insertCmd.Parameters.AddWithValue("@ISBN", isbn);
                insertCmd.ExecuteNonQuery();
                Console.WriteLine("Книга успешно добавлена.");
            }
        }

        static void ReadAllBooks(SQLiteConnection connection)
        {
            using (SQLiteCommand selectCmd = new SQLiteCommand(
                "SELECT * FROM Books;", connection))
            {
                using (SQLiteDataReader reader = selectCmd.ExecuteReader())
                {
                    Console.WriteLine("Список всех книг:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, Название: {reader["Title"]}, Автор: {reader["Author"]}, Год: {reader["PublicationYear"]}, Жанр: {reader["Genre"]}, ISBN: {reader["ISBN"]}");
                    }
                }
            }
        }

        static void SearchBook(SQLiteConnection connection)
        {
            Console.WriteLine("Введите критерий поиска (название, автор, ID и т. д.):");
            string searchTerm = Console.ReadLine();

            using (SQLiteCommand searchCmd = new SQLiteCommand(
                "SELECT * FROM Books WHERE Title LIKE @SearchTerm OR Author LIKE @SearchTerm OR ID = @SearchTerm;", connection))
            {
                searchCmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                using (SQLiteDataReader reader = searchCmd.ExecuteReader())
                {
                    Console.WriteLine("Результаты поиска:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, Название: {reader["Title"]}, Автор: {reader["Author"]}, Год: {reader["PublicationYear"]}, Жанр: {reader["Genre"]}, ISBN: {reader["ISBN"]}");
                    }
                }
            }
        }

        static void UpdateBook(SQLiteConnection connection)
        {
            Console.WriteLine("Введите ID книги, которую вы хотите обновить:");
            int bookId = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите новое название книги:");
            string newTitle = Console.ReadLine();

            Console.WriteLine("Введите нового автора книги:");
            string newAuthor = Console.ReadLine();

            using (SQLiteCommand updateCmd = new SQLiteCommand(
                "UPDATE Books SET Title = @NewTitle, Author = @NewAuthor WHERE ID = @BookId;", connection))
            {
                updateCmd.Parameters.AddWithValue("@NewTitle", newTitle);
                updateCmd.Parameters.AddWithValue("@NewAuthor", newAuthor);
                updateCmd.Parameters.AddWithValue("@BookId", bookId);
                int rowsUpdated = updateCmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
                {
                    Console.WriteLine("Информация о книге обновлена.");
                }
                else
                {
                    Console.WriteLine("Книга с указанным ID не найдена.");
                }
            }
        }

        static void DeleteBook(SQLiteConnection connection)
        {
            Console.WriteLine("Введите ID книги, которую вы хотите удалить:");
            int bookId = int.Parse(Console.ReadLine());

            using (SQLiteCommand deleteCmd = new SQLiteCommand(
                "DELETE FROM Books WHERE ID = @BookId;", connection))
            {
                deleteCmd.Parameters.AddWithValue("@BookId", bookId);
                int rowsDeleted = deleteCmd.ExecuteNonQuery();
                if (rowsDeleted > 0)
                {
                    Console.WriteLine("Книга успешно удалена.");
                }
                else
                {
                    Console.WriteLine("Книга с указанным ID не найдена.");
                }
            }
        }
    }
}