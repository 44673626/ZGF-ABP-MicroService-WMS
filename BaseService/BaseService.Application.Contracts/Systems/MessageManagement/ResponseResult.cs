using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseService.Systems.MessageManagement
{
    /// <summary>
    /// 响应对象
    /// </summary>

    public class ResponseResult
    {
        private int _code = 0;
        private string _message = "";
        public ResponseResult()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult([Range(0, 1)] int code, string message)
        {
            _code = code;
            _message = message;
        }
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="ex"></param>
        public ResponseResult(Exception ex)
        {
            _code = 0;
            _message = ex.Message;
        }
        /// <summary>
        ///返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseResult Success(string message = "ok")
        {
            return new ResponseResult(1, message);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseResult Fail(string message)
        {
            return new ResponseResult(0, message);
        }

        /// <summary>
        /// 状态码(0:失败，1：成功)
        /// </summary>
        [JsonPropertyName("code")]
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    /// <summary>
    /// 响应对象
    /// </summary>
    /// <typeparam name="T">数据</typeparam>
    public class ResponseResult<T> : ResponseResult where T : class
    {
        private T _result = default;

        public ResponseResult()
        {

        }
        /// <summary>
        /// 返回 带result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        public ResponseResult([Range(0, 1)] int code, string message, T result) : base(code, message)
        {
            _result = result;
        }

        /// <summary>
        /// 返回 不带result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseResult([Range(0, 1)] int code, string message) : base(code, message)
        {
        }

        /// <summary>
        /// 带数据返回成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static ResponseResult<T> Success(string message, T result)
        {
            return new ResponseResult<T>(1, message, result);
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        new public static ResponseResult<T> Fail(string message)
        {
            return new ResponseResult<T>(0, message);
        }
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="ex"></param>
        public ResponseResult(Exception ex) : base(ex)
        {

        }
        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonPropertyName("message")]
        public T Result
        {
            get { return _result; }
            set { _result = value; }
        }
    }


    /// <summary>
    /// 响应对象 带分页
    /// </summary>
    /// <typeparam name="T">数据</typeparam>

    public class PageResponseResult<T> : ResponseResult where T : class
    {

        /// <summary>
        /// 返回 带result
        /// </summary>
        public PageResponseResult([Range(0, 1)] int code, string message, List<T> result, int pageSize, int pageIndex, int totalCount, int pageCount) : base(code, message)
        {
            Result = result;
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalCount = totalCount;
        }


        public PageResponseResult([Range(0, 1)] int code, string message, List<T> result, int pageSize, int pageIndex, int totalCount, int pageCount, decimal sumCount, decimal sumPrice) : base(code, message)
        {
            Result = result;
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalCount = totalCount;
            SumCount = sumCount;
            SumPrice = sumPrice;
        }


        public PageResponseResult([Range(0, 1)] int code, string message, List<T> result, int pageSize, int pageIndex, int totalCount, int pageCount, int allNum, int waitSubmitNum, int notAddedNum) : base(code, message)
        {
            Result = result;
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalCount = totalCount;
            AllNum = allNum;
            WaitSubmitNum = waitSubmitNum;
            NotAddedNum = notAddedNum;
        }

        /// <summary>
        /// 返回 不带result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public PageResponseResult([Range(0, 1)] int code, string message) : base(code, message)
        {
        }


        public PageResponseResult()
        {
        }

        /// <summary>
        /// 带数据返回成功
        /// </summary>
        /// <returns></returns>
        public static PageResponseResult<T> Success(string message, List<T> result, int pageSize, int pageIndex, int totalCount)
        {
            var pageCount = 0;
            if (totalCount > 0)
            {
                pageCount = (totalCount + pageSize - 1) / pageSize;
            }

            return new PageResponseResult<T>(1, message, result, pageSize, pageIndex, totalCount, pageCount);
        }
        /// <summary>
        /// 报表专用 多返回一个汇总
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static PageResponseResult<T> Success(string message, List<T> result, int pageSize, int pageIndex, int totalCount, decimal SumCount, decimal SumPrice)
        {
            var pageCount = 0;
            if (totalCount > 0)
            {
                pageCount = (totalCount + pageSize - 1) / pageSize;
            }

            return new PageResponseResult<T>(1, message, result, pageSize, pageIndex, totalCount, pageCount, SumCount, SumPrice);
        }
        /// <summary>
        /// 资质证照提交页面使用-携带不同状态数量返回
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static PageResponseResult<T> Success(string message, List<T> result, int pageSize, int pageIndex, int totalCount, int allNum, int waitSubmitNum, int notAddedNum)
        {
            var pageCount = 0;
            if (totalCount > 0)
            {
                pageCount = (totalCount + pageSize - 1) / pageSize;
            }

            return new PageResponseResult<T>(1, message, result, pageSize, pageIndex, totalCount, pageCount, allNum, waitSubmitNum, notAddedNum);
        }

        /// <summary>
        /// 带数据返回成功
        /// </summary>
        /// <returns></returns>
        public static PageResponseResult<T> Empty(int pageSize, int pageIndex)
        {
            return new PageResponseResult<T>(1, "暂无数据", new List<T>(), 0, 0, 0, 0);
        }

        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        new public static PageResponseResult<T> Fail(string message)
        {
            return new PageResponseResult<T>(0, message);
        }
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="ex"></param>
        public PageResponseResult(Exception ex) : base(ex)
        {

        }
        /// <summary>
        /// 返回数据列表
        /// </summary>

        public List<T> Result { set; get; }
        /// <summary>
        /// 当前页码
        /// </summary>

        public int PageIndex { set; get; }
        /// <summary>
        ///每页数量
        /// </summary>

        public int PageSize { set; get; }
        /// <summary>
        /// 总数
        /// </summary>

        public int TotalCount { set; get; }
        /// <summary>
        /// 总页数
        /// </summary>

        public int PageCount { set; get; }


        public decimal SumCount { set; get; }

        public decimal SumPrice { set; get; }

        /// <summary>
        /// 全部数量
        /// </summary>
        public int AllNum { get; set; }
        /// <summary>
        /// 待提交数量
        /// </summary>
        public int WaitSubmitNum { get; set; }
        /// <summary>
        /// 待上传数量
        /// </summary>
        public int NotAddedNum { get; set; }





    }  /// <summary>
       /// 响应对象 不分页
       /// </summary>
       /// <typeparam name="T">数据</typeparam>

    public class ListResponseResult<T> : ResponseResult where T : class
    {

        public ListResponseResult() { }

        /// <summary>
        /// 返回 带result
        /// </summary>
        public ListResponseResult([Range(0, 1)] int code, string message, List<T> result) : base(code, message)
        {
            Result = result;
        }
        /// <summary>
        /// 返回 不带result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ListResponseResult([Range(0, 1)] int code, string message) : base(code, message)
        {
        }
        /// <summary>
        /// 带数据返回成功
        /// </summary>
        /// <returns></returns>
        public static ListResponseResult<T> Success(string message, List<T> result)
        {
            return new ListResponseResult<T>(1, message, result);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        new public static ListResponseResult<T> Fail(string message)
        {
            return new ListResponseResult<T>(0, message);
        }
        /// <summary>
        /// 异常返回
        /// </summary>
        /// <param name="ex"></param>
        public ListResponseResult(Exception ex) : base(ex)
        {

        }
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public List<T> Result { set; get; }
    }
}
