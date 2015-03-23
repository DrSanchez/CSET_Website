//using System.Configuration;
//using System.Text;

//namespace System.Web.Mvc
//{
//    public static partial class HtmlHelpers
//    {
//        /// <summary>
//        /// Insert GA with an Urchin and domain name
//        /// </summary>
//        /// <param name="htmlHelper"></param>
//        /// <param name="urchin"></param>
//        /// <param name="domainName"></param>
//        /// <returns></returns>
//        public static HtmlString Analytics(this HtmlHelper htmlHelper, string urchin, string domainName)
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.Append("<script type='text/javascript'>");
//            sb.Append("  var _gaq = _gaq || [];");
//            sb.Append(" _gaq.push(['_setAccount', '" + urchin + "']);");
//            sb.Append(" _gaq.push(['_setDomainName', '" + domainName + "']);");
//            sb.Append(" _gaq.push(['_trackPageview']);");
//            sb.Append("  (function() {");
//            sb.Append("   var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
//            sb.Append("    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
//            sb.Append("   var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
//            sb.Append("  })();");
//            sb.Append("</script>");

//            return new HtmlString(sb.ToString());
//        }

//        /// <summary>
//        /// Pull the urchin and domain name from Web.Config
//        /// </summary>
//        /// <param name="htmlHelper"></param>
//        /// <returns></returns>
//        public static HtmlString Analytics(this HtmlHelper htmlHelper)
//        {
//            //pull values from Config
//            string urchin = ConfigurationManager.AppSettings["ga-urchin"];
//            string domainName = ConfigurationManager.AppSettings["ga-domainName"];
//            return Analytics(htmlHelper, urchin, domainName);
//        }
//    }
//}
