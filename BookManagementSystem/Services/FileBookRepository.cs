using System;
using System.IO;
using BookSystem.Entities;
using BookSystem.Interfaces;

namespace BookSystem.Services
{
    public class FileBookRepository : IBookRepository
    {
        public void Save(Book book)
        {
           string filename = $"/documents/{book.Title} - {book.Author}.txt";
           // Logic to serialize and save the book
            File.WriteAllText(filename, book.ToString());
        }
    }
}