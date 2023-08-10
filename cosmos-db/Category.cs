namespace cosmos_db
{
    public record Category(
     string Id,
     string CategoryId
 ) : Item(
     Id,
     CategoryId,
     nameof(Category)
 );
}
