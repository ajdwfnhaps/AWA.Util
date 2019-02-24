
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// ModelState扩展
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 获取ViewModel验证第一个错误信息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="onlyFirst">是否只取第一个错误信息</param>
        /// <param name="spiltString"></param>
        /// <returns></returns>
        public static string GetFirstErrors(this ModelStateDictionary modelState)
        {
            var firstErr = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => x.Value.Errors).FirstOrDefault();
            if (firstErr != null)
            {
                return firstErr[0].ErrorMessage;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取ViewModel验证所有错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetAllErrors(this ModelStateDictionary modelState)
        {
            var result = new List<KeyValuePair<string, string>>();

            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            foreach (var item in errorFieldsAndMsgs)
            {
                //获取键
                var fieldKey = item.Key;

                //获取键对应的错误信息
                var fieldErrors = item.Errors.Select(e => new KeyValuePair<string, string>(fieldKey, e.ErrorMessage));

                result.AddRange(fieldErrors);
            }

            return result;
        }

    }

}


namespace Microsoft.AspNetCore.ModelBinding
{
    /// <summary>
    /// ModelState扩展
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 获取ViewModel验证第一个错误信息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="onlyFirst">是否只取第一个错误信息</param>
        /// <param name="spiltString"></param>
        /// <returns></returns>
        public static string GetFirstErrors(this ModelStateDictionary modelState)
        {
            var firstErr = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => x.Value.Errors).FirstOrDefault();
            if (firstErr != null)
            {
                return firstErr[0].ErrorMessage;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取ViewModel验证所有错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>> GetAllErrors(this ModelStateDictionary modelState)
        {
            var result = new List<KeyValuePair<string, string>>();

            //找到出错的字段以及出错信息
            var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            foreach (var item in errorFieldsAndMsgs)
            {
                //获取键
                var fieldKey = item.Key;

                //获取键对应的错误信息
                var fieldErrors = item.Errors.Select(e => new KeyValuePair<string, string>(fieldKey, e.ErrorMessage));

                result.AddRange(fieldErrors);
            }

            return result;
        }

    }

}