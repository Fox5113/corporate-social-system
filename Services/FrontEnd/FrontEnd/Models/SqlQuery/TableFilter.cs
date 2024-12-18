﻿using System.Collections.Generic;

namespace SqlQuery
{
    public class TableFilter
    {
        public ICollection<FieldFilter> FieldsFilter { get; set; }
        public ICollection<TableFilter> InnerTableFilter { get; set; }
        public int Operation { get; set; }
    }
}
