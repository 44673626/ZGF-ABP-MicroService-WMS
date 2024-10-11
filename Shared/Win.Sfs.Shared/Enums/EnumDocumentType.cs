namespace Win.Sfs.Shared.Enums
{
    public enum EnumDocumentType
    {
        /// <summary>
        /// 预到货
        /// </summary>
        AdvanceShippingNotice = 201,

        /// <summary>
        /// 到货
        /// </summary>
        ArrivalNotice = 202,

        /// <summary>
        /// 收货
        /// </summary>
        Receipt = 203,

        /// <summary>
        /// 不合格上架
        /// </summary>
        NPA=4,
        
        /// <summary>
        /// 拣料
        /// </summary>
        PICK=5,

        /// <summary>
        /// 发货
        /// </summary>
        ISS=6,

        /// <summary>
        /// 移库
        /// </summary>
        TR=7,

        /// <summary>
        /// 盘点
        /// </summary>
        CYC=8,
    }
}