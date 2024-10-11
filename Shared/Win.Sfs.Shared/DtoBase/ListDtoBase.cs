using Volo.Abp.Application.Dtos;

namespace Win.Sfs.Shared.DtoBase
{
    /// <summary>
    /// List entity DTO base
    /// </summary>
    /// <typeparam name="TKey">TKey</typeparam>
    public abstract class ListDtoBase<TKey> : EntityDto<TKey>
    {
    }
}