
public class Author : BaseEntity
{
    public int AuthorId { get; set; }
    public string FullName { get; set; }
    public string Bio { get; set; }
    public string BirthCountry { get; set; }
    public string BirthCity { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public byte[]? AuthorImage { get; set; }
    public virtual ICollection<AuthorGenre> AuthorGenres { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}