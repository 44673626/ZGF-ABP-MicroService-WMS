namespace Win.Sfs.Shared.DomainBase
{
    public interface IItem<TKey>
    {
        TKey ItemId { get; set; }
        string ItemCode { get; set; }
    }
}