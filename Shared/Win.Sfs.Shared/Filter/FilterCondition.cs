namespace Win.Sfs.Shared.Filter
{
    public class FilterCondition
    {
        public FilterCondition()
        {
            Logic = EnumFilterLogic.And;
        }

        public FilterCondition(string column, string value, EnumFilterAction action = EnumFilterAction.Equal,
            EnumFilterLogic logic = EnumFilterLogic.And)
        {
            Column = column;
            Action = action;
            Value = value;
            Logic = logic;
        }

        /// <summary>
        /// 过滤条件之间的逻辑关系:AND和OR
        /// </summary>
        public EnumFilterLogic Logic { get; set; }

        /// <summary>
        /// 过滤条件中使用的数据列
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 过滤条件中的操作:Equal、NotEqual、BiggerThan、SmallThan、BiggerThanOrEqual、SmallThanOrEqual、In、NotIn
        /// </summary>
        public EnumFilterAction Action { get; set; }

        /// <summary>
        /// 过滤条件中的操作的值
        /// </summary>
        public string Value { get; set; }
    }
}