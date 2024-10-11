// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using Volo.Abp.AspNetCore.Mvc;
using Win.Sfs.FileStorage.Localization;

namespace Win.Sfs.FileStorage
{
    public abstract class FileStorageController : AbpControllerBase
    {
        protected FileStorageController()
        {
            LocalizationResource = typeof(FileStorageResource);
        }
    }
}
