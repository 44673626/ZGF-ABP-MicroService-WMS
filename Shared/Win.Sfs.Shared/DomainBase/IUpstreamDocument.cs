using System.Collections.Generic;

namespace Win.Sfs.Shared.DomainBase
{
    public interface IUpstreamDocument
    {
        //TKey UpstreamDocumentId { get; set; }


        List<UpstreamDocument> UpstreamDocuments { get; set; }

    }
}