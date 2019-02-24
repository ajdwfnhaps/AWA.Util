namespace AWA.Util.Common.Entity
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 成功标识
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg
        {
            get;
            set;
        }

        /// <summary>
        /// 创建泛型返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Result<T> Create<T>(T data, string msg = "", string title = "")
        {
            return new Result<T>
            {
                Data = data,
                Success = true,
                Msg = msg,
                Title = title
            };
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Result Fail(string msg = "", string title = "")
        {
            return new Result { Msg = msg, Title = title };
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Result<T> Fail<T>(string msg = "", string title = "")
        {
            return new Result<T> { Msg = msg, Title = title };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Result OK(string msg = "", string title = "")
        {
            return new Result { Msg = msg, Title = title, Success = true };
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Result<T> OK<T>(T data, string msg = "", string title = "")
        {
            return new Result<T> { Data = data, Msg = msg, Title = title, Success = true };
        }

    }

    /// <summary>
    /// 响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 返回泛型结果
        /// </summary>
        public T Data { get; set; }
    }
}
