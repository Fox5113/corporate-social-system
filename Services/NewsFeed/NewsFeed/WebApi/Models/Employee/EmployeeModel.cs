﻿using System.Collections.Generic;
using System;
using WebApi.Models.News;
using WebApi.Models.NewsComment;

namespace WebApi.Models.Employee
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public List<NewsModel> NewsList { get; set; }
        public List<NewsCommentModel> NewsCommentList { get; set; }
    }
}
