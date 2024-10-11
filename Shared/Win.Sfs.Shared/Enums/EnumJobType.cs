namespace Win.Sfs.Shared.Enums
{
    public enum EnumJobType
    {
        /// <summary>
        /// 收货
        /// </summary>
        RECEIPT=1,

        /// <summary>
        /// 质检
        /// </summary>
        INSPECT=2,

        /// <summary>
        /// 合格上架
        /// </summary>
        PUTAWAY=3,
        
        /// <summary>
        /// 拣料
        /// </summary>
        PICKUP=4,

        /// <summary>
        /// 发货
        /// </summary>
        ISSUE=5,

        /// <summary>
        /// 移库
        /// </summary>
        TRANSFER=6,

        /// <summary>
        /// 盘点
        /// </summary>
        COUNT=7,
    }
}