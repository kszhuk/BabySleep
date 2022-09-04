using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleepWeb.Models
{
    public enum LanguageType
    {
        en,
        uk,
        ru
    }

    /// <summary>
    /// Language settings (edit language page)
    /// </summary>
    public class LanguageModel
    {
        public LanguageType Id { get; set; }

        public string Title { get; set; }
    }
}
