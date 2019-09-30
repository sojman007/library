﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Dto.ResponseModel
{
    public class SearchResponse <T>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
