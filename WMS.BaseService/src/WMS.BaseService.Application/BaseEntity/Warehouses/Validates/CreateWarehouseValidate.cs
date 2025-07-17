using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using WMS.BaseService.BaseEntity.Warehouses.Dtos;
using WMS.BaseService.BusinessEntity;

namespace WMS.BaseService.BaseEntity.Warehouses.Validates
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public class CreateWarehouseValidate : AbstractValidator<CreateWarehouseDto>, ISingletonDependency
    {
        readonly IRepository<Warehouse, Guid> _repository;

        public CreateWarehouseValidate(IRepository<Warehouse, Guid> repository)
        {
            _repository = repository;
            RuleFor(dto => dto.WarehouseCode)
               .NotEmpty().WithMessage("编码不能为空")
               .MustAsync(BeUniqueName).WithMessage("编码已存在");
        }
        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            var existingBrand = await _repository.FirstOrDefaultAsync(b => b.WarehouseCode == name, cancellationToken);
            return existingBrand == null;
        }
    }
}
