using System.Collections.Generic;

namespace NewsFeed.Common
{
    public static class Constants
    {
        public static string ProjectName = "NewsFeed";
        public static string ServicesFolderName = "Services";
        public static string ModelsFolderName = "Models";
        public static string CommonFolderName = "Common";
        public static Dictionary<string, string> TableAndServicePath = new Dictionary<string, string>()
        {
            {"News", $"{ProjectName}.{ServicesFolderName}.NewsService" },
            {"Employee", $"{ProjectName}.{ServicesFolderName}.EmployeeService" },
            {"NewsComment", $"{ProjectName}.{ServicesFolderName}.NewsCommentService"  },
            {"Hashtag", $"{ProjectName}.{ServicesFolderName}.HashtagService" },
            {"HashtagNews", $"{ProjectName}.{ServicesFolderName}.HashtagNewsService"  }
        };

        public static Dictionary<string, string> ClassPathByName = new Dictionary<string, string>()
        {
            {"News", $"{ProjectName}.{ModelsFolderName}.News" },
            {"Employee", $"{ProjectName}.{ModelsFolderName}.Employee" },
            {"NewsComment", $"{ProjectName}.{ModelsFolderName}.NewsComment"  },
            {"Hashtag", $"{ProjectName}.{ModelsFolderName}.Hashtag" },
            {"HashtagNews", $"{ProjectName}.{ModelsFolderName}.HashtagNews"  },
            {"Mapping", $"{ProjectName}.{CommonFolderName}.Mapping"  }
        };
    }
}
