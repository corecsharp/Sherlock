using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sherlock.MvcSample.Model
{
    public class NewsModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 快讯时间：录入时间或第三方时间
        /// </summary>
        public DateTime? NewsTime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标题中文
        /// </summary>
        public string TitleChinese { get; set; }
        /// <summary>
        /// 摘要=内容百分比
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 英文翻译
        /// </summary>
        public string ContentChinese { get; set; }
        /// <summary>
        /// 快讯来源主键
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 快讯类型 1：快讯
        /// </summary>
        public int NewsType { get; set; }

        /// <summary>
        /// 快讯地址
        /// </summary>
        public string NewsUrl { get; set; }

        public int AduitStatus { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
    }
}
