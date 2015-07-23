using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UVa_ASP.NET_MVC.Utils
{
    public static class Util
    {
        static string baseurl = "http://uhunt.felix-halim.net/api/";
        static string problem_baseurl = "http://uva.onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=24&page=show_problem&problem=";
        static string username_baseurl = "http://uhunt.felix-halim.net/id/";
        static int[] userid = {
            1498,  
            271633,
            230537,
            230534,
            272593,
            327438,
            324273,
            230526,
            329831,
            329838,
            329848,
            326241,
            322566,
            331044,
            327396,
            280440,
            327400,
            230542,
            348674,
            78850, 
            348679};

        static Dictionary<dynamic, String> verdict = new Dictionary<dynamic, String>()
        {
           {10, "Submission error"}, 
           {15, "Can't be judge"}, 
           {20, "In queue"}, 
           {30, "Compile error"}, 
           {35, "Restricted function"}, 
           {40, "Runtime error"}, 
           {45, "Output limit"}, 
           {50, "Time limit"}, 
           {60, "Memory limit"}, 
           {70, "Wrong answer"}, 
           {80, "PresentationE"}, 
           {90, "Accepted"}
        };

        private static dynamic fetch(string url)
        {
            using (var w = new NoKeepAlivesWebClient())
            {
                var json_data = string.Empty; ;
                json_data = w.DownloadString(url);
                return JsonConvert.DeserializeObject(json_data);
            }
        }

        public static string getProblemName(dynamic pid)
        {
            string url = baseurl + "p/id/" + pid.ToString();
            dynamic p = fetch(url);
            return p.num + " - " + p.title;
        }

        public static String getVerdict(dynamic vid)
        {
            int id = vid;
            String s = verdict[id];
            return s;
        }

        //http://stackoverflow.com/questions/3354893/how-can-i-convert-a-datetime-to-the-number-of-seconds-since-1970
        public static DateTime ConvertFromUnixTimestamp(dynamic timestamp)
        {
            timestamp += 25200; //7 hours local time
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds((double)timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }


        public static List<dynamic> ranklist()
        {
            var data = new List<dynamic>();
            var ranklist_baseurl = baseurl + "ranklist/";
            var lastsubmission_baseurl = baseurl + "subs-user-last/";

            foreach (int uid in userid)
            {
                string url;
                dynamic u, detail = new ExpandoObject();

                // user basic info
                url = ranklist_baseurl + uid.ToString() + "/0/0";
                u = fetch(url);

                detail.rank = u[0].rank;
                detail.name = u[0].name;
                detail.username = u[0].username;
                detail.AC = u[0].ac;
                detail.NOS = u[0].nos;
                detail.usernameLink = username_baseurl + uid.ToString();

                //user last submission
                url = lastsubmission_baseurl + uid.ToString() + "/1";
                u = fetch(url);
                if (u.subs.Count > 0)
                {
                    dynamic subs = u.subs[0];
                    detail.lastProblem = getProblemName(subs[1]);
                    detail.lastVerdict = getVerdict(subs[2]);
                    detail.lastDate = ConvertFromUnixTimestamp(subs[4]);
                    detail.problemLink = problem_baseurl + subs[1].ToString();
                }

                data.Add(detail);
            }

            return data;
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
}