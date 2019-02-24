using System;
using System.Collections.Generic;

namespace AWA.Util.Common
{
    /// <summary>
    /// 返回图片上传结果
    /// </summary>
    public class ImageResult
    {
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public List<ThumbResult> Thumbs { get; set; }
    }

    /// <summary>
    /// 缩略图
    /// </summary>
    public class ThumbResult
    {
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }
    }
}
