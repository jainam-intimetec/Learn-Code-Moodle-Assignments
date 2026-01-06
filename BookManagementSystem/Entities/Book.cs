namespace BookSystem.Entities
{
    public class Book
    {
        public string GetTitle() => "A Great Book";
        public string GetAuthor() => "John Doe";
        public string GetCurrentPage() => "current page content";
        
        public void TurnPage() 
        {
            // Logic for next page
        }
    }
}