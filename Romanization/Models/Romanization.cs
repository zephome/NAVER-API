using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romanization.Models
{
    public class Romanization
    {
        //! JSON RESULT (홍길동 검색)
        //{
        //  "aResult": [
        //    {
        //      "sFirstName": "홍",
        //      "aItems": [
        //        {
        //          "name": "Hong Gildong",
        //          "score": "99"
        //        },
        //        {
        //          "name": "Hong Kildong",
        //          "score": "96"
        //        },
        //        {
        //          "name": "Hong Gildoung",
        //          "score": "21"
        //        },
        //        {
        //          "name": "Hong Kildoung",
        //          "score": "20"
        //        }
        //      ]
        //    }
        //  ]
        //}

        /// <summary>
        /// 로마자 변환 클래스
        /// </summary>
        public class Roman
        {
            public List<AResult> aResult { get; set; }
        }

        /// <summary>
        /// 로마자 변환 결과 셋
        /// </summary>
        public class AResult
        {
            public string sFirstName { get; set; }
            public List<AItem> aItems { get; set; }
        }

        /// <summary>
        /// 로마자 변환 결과 목록
        /// 우선 순위가 높은 순서대로 나열
        /// </summary>
        public class AItem
        {
            public string name { get; set; }
            public string score { get; set; }
        }

        /// <summary>
        /// 로자마 변환 결과
        /// </summary>
        public class RomanizationResult
        {
            public int statusCode { get; set; }
            public string errorMessage { get; set; }
            public string errorCode { get; set; }
            public string name { get; set; }
        }

        /// <summary>
        /// 로마자 변환 에러 결과
        /// </summary>
        public class RomanizationError
        {
            public string errorMessage { get; set; }
            public string errorCode { get; set; }
        }
    }
}